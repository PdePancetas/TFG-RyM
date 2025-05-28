using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DRCars.Utils
{
    public class SimpleImageService
    {
        /// <summary>
        /// Convierte imagen a múltiples tamaños usando System.Drawing con manejo mejorado de memoria
        /// </summary>
        public async Task<Dictionary<string, byte[]>> CreateImageVersionsAsync(Image originalImage)
        {
            var results = new Dictionary<string, byte[]>();

            try
            {
                // Validar que la imagen original no sea null
                if (originalImage == null)
                {
                    throw new ArgumentNullException(nameof(originalImage), "La imagen original no puede ser null");
                }

                Console.WriteLine($"🖼️ Procesando imagen original: {originalImage.Width}x{originalImage.Height}, Formato: {originalImage.RawFormat}");

                // Crear diferentes tamaños
                var sizes = new Dictionary<string, Size>
                {
                    { "thumbnail", new Size(150, 100) },
                    { "medium", new Size(400, 300) },
                    { "large", new Size(800, 600) }
                };

                // Crear una copia de la imagen original en memoria para evitar problemas de acceso
                using (var originalCopy = new Bitmap(originalImage))
                {
                    foreach (var sizeInfo in sizes)
                    {
                        try
                        {
                            Console.WriteLine($"  - Creando versión {sizeInfo.Key}: {sizeInfo.Value.Width}x{sizeInfo.Value.Height}");

                            using (var resizedImage = ResizeImageSafe(originalCopy, sizeInfo.Value))
                            {
                                var imageBytes = ImageToByteArraySafe(resizedImage, ImageFormat.Jpeg, 85);
                                results[sizeInfo.Key] = imageBytes;

                                Console.WriteLine($"    ✅ {sizeInfo.Key}: {imageBytes.Length:N0} bytes ({imageBytes.Length / 1024.0:F1} KB)");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"    ❌ Error creando {sizeInfo.Key}: {ex.Message}");
                            throw new Exception($"Error al crear versión {sizeInfo.Key}: {ex.Message}", ex);
                        }
                    }

                    // Original optimizado
                    try
                    {
                        Console.WriteLine($"  - Creando versión original optimizada");
                        var originalBytes = ImageToByteArraySafe(originalCopy, ImageFormat.Jpeg, 90);
                        results["original"] = originalBytes;
                        Console.WriteLine($"    ✅ original: {originalBytes.Length:N0} bytes ({originalBytes.Length / 1024.0:F1} KB)");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ❌ Error creando original: {ex.Message}");
                        throw new Exception($"Error al crear versión original: {ex.Message}", ex);
                    }
                }

                Console.WriteLine($"✅ Procesamiento de imagen completado. Total versiones: {results.Count}");
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general en CreateImageVersionsAsync: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                throw new Exception($"Error al procesar imagen: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Redimensiona imagen manteniendo proporción
        /// </summary>
        private Image ResizeImage(Image originalImage, Size targetSize)
        {
            // Calcular nuevo tamaño manteniendo proporción
            float ratioX = (float)targetSize.Width / originalImage.Width;
            float ratioY = (float)targetSize.Height / originalImage.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalImage.Width * ratio);
            int newHeight = (int)(originalImage.Height * ratio);

            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                // Configuración de alta calidad
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }

        /// <summary>
        /// Convierte imagen a byte array con calidad específica
        /// </summary>
        private byte[] ImageToByteArray(Image image, ImageFormat format, long quality = 85)
        {
            using (var ms = new MemoryStream())
            {
                // Configurar encoder con calidad
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                var codec = GetEncoderInfo(format);
                if (codec != null)
                {
                    image.Save(ms, codec, encoderParameters);
                }
                else
                {
                    image.Save(ms, format);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene codec para formato específico
        /// </summary>
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// Detecta si imagen es candidata para vectorización
        /// </summary>
        public bool IsVectorizationCandidate(Image image)
        {
            // Criterios simples: imagen pequeña con pocos colores
            bool isSmall = image.Width <= 300 && image.Height <= 300;
            bool hasSimpleContent = CountUniqueColors(image) < 10;

            return isSmall && hasSimpleContent;
        }

        /// <summary>
        /// Cuenta colores únicos aproximados
        /// </summary>
        private int CountUniqueColors(Image image)
        {
            var colors = new HashSet<int>();
            var bitmap = new Bitmap(image);

            // Muestrear cada 10 píxeles para eficiencia
            for (int x = 0; x < bitmap.Width; x += 10)
            {
                for (int y = 0; y < bitmap.Height; y += 10)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    // Simplificar color (reducir precisión)
                    int simplifiedColor = (pixel.R / 32) << 16 | (pixel.G / 32) << 8 | (pixel.B / 32);
                    colors.Add(simplifiedColor);

                    if (colors.Count > 20) break; // Optimización
                }
                if (colors.Count > 20) break;
            }

            bitmap.Dispose();
            return colors.Count;
        }

        /// <summary>
        /// Obtiene información de la imagen
        /// </summary>
        public VehicleImageInfo GetImageInfo(Image image)
        {
            return new VehicleImageInfo
            {
                Width = image.Width,
                Height = image.Height,
                Format = "JPEG", // Simplificado
                SizeBytes = 0, // Se calculará después
                AspectRatio = (float)image.Width / image.Height,
                IsVectorizationCandidate = IsVectorizationCandidate(image)
            };
        }

        /// <summary>
        /// Crea SVG simple
        /// </summary>
        public string CreateSimpleSVG(VehicleImageInfo imageInfo, string vehicleId)
        {
            var svg = $@"<svg width=""{imageInfo.Width}"" height=""{imageInfo.Height}"" xmlns=""http://www.w3.org/2000/svg"">
  <defs>
    <linearGradient id=""grad1"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""#00A09D""/>
      <stop offset=""100%"" stop-color=""#006B69""/>
    </linearGradient>
  </defs>
  <rect width=""100%"" height=""100%"" fill=""url(#grad1)"" rx=""8""/>
  <text x=""50%"" y=""40%"" text-anchor=""middle"" font-family=""Arial"" font-size=""{Math.Min(imageInfo.Width, imageInfo.Height) / 6}"" fill=""white"" font-weight=""bold"">🚗</text>
  <text x=""50%"" y=""70%"" text-anchor=""middle"" font-family=""Arial"" font-size=""{Math.Min(imageInfo.Width, imageInfo.Height) / 10}"" fill=""white"">ID: {vehicleId}</text>
</svg>";

            return svg.Trim();
        }

        /// <summary>
        /// Redimensiona imagen de forma segura manteniendo proporción
        /// </summary>
        private Bitmap ResizeImageSafe(Image originalImage, Size targetSize)
        {
            try
            {
                // Calcular nuevo tamaño manteniendo proporción
                float ratioX = (float)targetSize.Width / originalImage.Width;
                float ratioY = (float)targetSize.Height / originalImage.Height;
                float ratio = Math.Min(ratioX, ratioY);

                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);

                // Asegurar que las dimensiones sean válidas
                newWidth = Math.Max(1, newWidth);
                newHeight = Math.Max(1, newHeight);

                Console.WriteLine($"    - Redimensionando de {originalImage.Width}x{originalImage.Height} a {newWidth}x{newHeight} (ratio: {ratio:F3})");

                var resizedImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    // Configuración de alta calidad
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    // Limpiar el fondo
                    graphics.Clear(Color.White);

                    // Dibujar la imagen redimensionada
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                return resizedImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ❌ Error en ResizeImageSafe: {ex.Message}");
                throw new Exception($"Error al redimensionar imagen: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convierte imagen a byte array de forma segura con calidad específica
        /// </summary>
        private byte[] ImageToByteArraySafe(Image image, ImageFormat format, long quality = 85)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    // Configurar encoder con calidad
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    var codec = GetEncoderInfo(format);
                    if (codec != null)
                    {
                        Console.WriteLine($"      - Usando codec {codec.CodecName} con calidad {quality}");
                        image.Save(ms, codec, encoderParameters);
                    }
                    else
                    {
                        Console.WriteLine($"      - Usando formato por defecto {format}");
                        image.Save(ms, format);
                    }

                    var bytes = ms.ToArray();
                    Console.WriteLine($"      - Imagen convertida a {bytes.Length:N0} bytes");
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ❌ Error en ImageToByteArraySafe: {ex.Message}");
                throw new Exception($"Error al convertir imagen a bytes: {ex.Message}", ex);
            }
        }
    }

    // Clase de información de imagen simplificada
    public class VehicleImageInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
        public long SizeBytes { get; set; }
        public float AspectRatio { get; set; }
        public bool IsVectorizationCandidate { get; set; }

        public string GetSizeFormatted()
        {
            if (SizeBytes < 1024) return $"{SizeBytes} B";
            if (SizeBytes < 1024 * 1024) return $"{SizeBytes / 1024:F1} KB";
            return $"{SizeBytes / (1024 * 1024):F1} MB";
        }
    }
}
