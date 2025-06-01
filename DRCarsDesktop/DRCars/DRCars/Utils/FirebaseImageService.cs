using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace DRCars.Utils
{
    public class FirebaseImageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseStorageUrl;
        private readonly string _firebaseApiKey;
        private readonly SimpleImageService _imageProcessor;

        public FirebaseImageService()
        {
            _httpClient = new HttpClient();
            _firebaseStorageUrl = AppConfig.FirebaseStorageUrl;
            _firebaseApiKey = AppConfig.FirebaseApiKey;
            _imageProcessor = new SimpleImageService();
        }

        /// <summary>
        /// Sube imagen con optimización usando SimpleImageService
        /// </summary>
        public async Task<VehicleImageUrls> UploadOptimizedVehicleImagesAsync(Image originalImage, string vehicleId)
        {
            try
            {
                // 1. Obtener información de la imagen
                var imageInfo = _imageProcessor.GetImageInfo(originalImage);

                // 2. Crear versiones optimizadas
                var imageVersions = _imageProcessor.CreateImageVersionsAsync(originalImage);

                // 3. Subir todas las versiones a Firebase
                var imageUrls = new VehicleImageUrls();

                imageUrls.ThumbnailUrl = await UploadToFirebaseStorage(
                    imageVersions["thumbnail"],
                    $"vehicles/{vehicleId}/thumbnail.jpg",
                    "image/jpeg"
                );

                imageUrls.MediumUrl = await UploadToFirebaseStorage(
                    imageVersions["medium"],
                    $"vehicles/{vehicleId}/medium.jpg",
                    "image/jpeg"
                );

                imageUrls.LargeUrl = await UploadToFirebaseStorage(
                    imageVersions["large"],
                    $"vehicles/{vehicleId}/large.jpg",
                    "image/jpeg"
                );

                imageUrls.OriginalUrl = await UploadToFirebaseStorage(
                    imageVersions["original"],
                    $"vehicles/{vehicleId}/original.jpg",
                    "image/jpeg"
                );

                // 4. Si es candidata para vectorización, crear SVG
                if (imageInfo.IsVectorizationCandidate)
                {
                    var svgData = _imageProcessor.CreateSimpleSVG(imageInfo, vehicleId);
                    if (!string.IsNullOrEmpty(svgData))
                    {
                        imageUrls.VectorUrl = await UploadSVGToFirebaseStorage(
                            svgData,
                            $"vehicles/{vehicleId}/vector.svg"
                        );
                    }
                }

                // 5. Guardar metadatos de la imagen
                await SaveImageMetadata(vehicleId, imageInfo, imageUrls);

                return imageUrls;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar y subir imágenes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sube datos binarios a Firebase Storage
        /// </summary>
        private async Task<string> UploadToFirebaseStorage(byte[] data, string path, string contentType)
        {
            try
            {
                var uploadUrl = $"{_firebaseStorageUrl}/{Uri.EscapeDataString(path)}?uploadType=media&name={Uri.EscapeDataString(path)}";

                var content = new ByteArrayContent(data);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                var response = await _httpClient.PostAsync($"{uploadUrl}&key={_firebaseApiKey}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var uploadResult = JsonConvert.DeserializeObject<FirebaseUploadResult>(responseContent);

                return $"{_firebaseStorageUrl}/{Uri.EscapeDataString(path)}?alt=media&token={uploadResult.DownloadTokens}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir a Firebase Storage: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sube SVG a Firebase Storage
        /// </summary>
        private async Task<string> UploadSVGToFirebaseStorage(string svgData, string path)
        {
            var svgBytes = Encoding.UTF8.GetBytes(svgData);
            return await UploadToFirebaseStorage(svgBytes, path, "image/svg+xml");
        }

        /// <summary>
        /// Guarda metadatos de imagen en Firebase
        /// </summary>
        private async Task SaveImageMetadata(string vehicleId, VehicleImageInfo imageInfo, VehicleImageUrls urls)
        {
            var metadata = new
            {
                vehicleId = vehicleId,
                originalWidth = imageInfo.Width,
                originalHeight = imageInfo.Height,
                originalFormat = imageInfo.Format,
                originalSizeBytes = imageInfo.SizeBytes,
                aspectRatio = imageInfo.AspectRatio,
                isVectorized = !string.IsNullOrEmpty(urls.VectorUrl),
                uploadDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                urls = urls
            };

            var json = JsonConvert.SerializeObject(metadata);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var metadataUrl = $"{AppConfig.FirebaseUrl}/imageMetadata/{vehicleId}.json";
            await _httpClient.PutAsync(metadataUrl, content);
        }
    }

    // Clases de apoyo actualizadas
    public class VehicleImageUrls
    {
        public string ThumbnailUrl { get; set; }    // 150x100 JPEG
        public string MediumUrl { get; set; }       // 400x300 JPEG  
        public string LargeUrl { get; set; }        // 800x600 JPEG
        public string OriginalUrl { get; set; }     // Optimizado JPEG
        public string VectorUrl { get; set; }       // SVG vectorial (si aplica)
    }

    public class FirebaseUploadResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        [JsonProperty("downloadTokens")]
        public string DownloadTokens { get; set; }
    }
}
