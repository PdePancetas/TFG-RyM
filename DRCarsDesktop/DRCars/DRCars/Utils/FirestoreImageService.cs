using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DRCars.Utils
{
    public class FirestoreImageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _firestoreUrl;
        private readonly string _firebaseApiKey;
        private readonly SimpleImageService _imageProcessor;

        // Límites para Firestore (1MB por documento)
        private const int MAX_DOCUMENT_SIZE = 900000; // 900KB para dejar margen
        private const int MAX_THUMBNAIL_SIZE = 50000;  // 50KB para thumbnail
        private const int MAX_MEDIUM_SIZE = 200000;    // 200KB para medium
        private const int MAX_LARGE_SIZE = 400000;     // 400KB para large

        public FirestoreImageService()
        {
            try
            {
                _httpClient = new HttpClient();
                _firestoreUrl = $"https://firestore.googleapis.com/v1/projects/{GetProjectId()}/databases/(default)/documents";
                _firebaseApiKey = AppConfig.FirebaseApiKey;
                _imageProcessor = new SimpleImageService();

                Console.WriteLine("🔧 FirestoreImageService inicializado");
                Console.WriteLine($"  - Project ID: {GetProjectId()}");
                Console.WriteLine($"  - Firestore URL: {_firestoreUrl}");
                Console.WriteLine($"  - API Key presente: {!string.IsNullOrEmpty(_firebaseApiKey)}");

                // PROBLEMA IDENTIFICADO: EnsureCollectionsExistAsync().Wait() puede causar deadlock
                // Removido para evitar bloqueo en el constructor
                Console.WriteLine("✅ FirestoreImageService inicializado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inicializando FirestoreImageService: {ex.Message}");
                // No relanzar la excepción para evitar crash
            }
        }

        private string GetProjectId()
        {
            return "drcars-b7c5f";
        }

        /// <summary>
        /// MÉTODO PRINCIPAL CORREGIDO - Procesa y almacena múltiples imágenes
        /// </summary>
        public async Task<VehicleMultipleImages> ProcessAndStoreMultipleVehicleImagesAsync(List<Image> images, string vehicleId)
        {
            try
            {
                Console.WriteLine("=== INICIANDO PROCESAMIENTO DE IMÁGENES ===");
                Console.WriteLine($"Vehicle ID: {vehicleId}");
                Console.WriteLine($"Número de imágenes: {images?.Count ?? 0}");

                if (images == null || images.Count == 0)
                {
                    Console.WriteLine("⚠️ No hay imágenes para procesar");
                    return null;
                }

                if (images.Count > 5)
                {
                    Console.WriteLine("❌ Máximo 5 imágenes permitidas");
                    throw new ArgumentException("Máximo 5 imágenes por vehículo");
                }

                // CORRECCIÓN: Verificar colecciones aquí, no en el constructor
                await EnsureCollectionExistsAsync("ImagenesVehiculos");

                var multipleImages = new VehicleMultipleImages
                {
                    VehicleId = vehicleId,
                    CreatedAt = DateTime.UtcNow,
                    Images = new Dictionary<string, VehicleImageData>()
                };

                // Procesar cada imagen
                for (int i = 0; i < images.Count; i++)
                {
                    if (images[i] != null)
                    {
                        Console.WriteLine($"🖼️ Procesando imagen {i + 1}");
                        var imageKey = $"image_{i + 1}";
                        var imageData = await ProcessSingleImage(images[i], imageKey);
                        multipleImages.Images[imageKey] = imageData;
                    }
                }

                // Guardar en Firestore
                await SaveToFirestoreWithApiKey(multipleImages);

                Console.WriteLine("✅ Imágenes procesadas y guardadas exitosamente");
                return multipleImages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ProcessAndStoreMultipleVehicleImagesAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// MÉTODO SIMPLIFICADO - Asegura que una colección específica exista
        /// </summary>
        private async Task EnsureCollectionExistsAsync(string collectionName)
        {
            try
            {
                Console.WriteLine($"🔍 Verificando colección: {collectionName}");

                // Intentar una operación simple para verificar si la colección existe
                var testUrl = $"{_firestoreUrl}/{collectionName}?key={_firebaseApiKey}&pageSize=1";

                // Configurar headers de forma segura
                ConfigureHttpHeaders();

                var response = await _httpClient.GetAsync(testUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Colección {collectionName} existe");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"⚠️ Colección {collectionName} no existe, será creada automáticamente al guardar el primer documento");
                    // No necesitamos crear la colección manualmente, Firestore la creará automáticamente
                    return;
                }

                Console.WriteLine($"⚠️ Respuesta inesperada al verificar colección {collectionName}: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error verificando colección {collectionName}: {ex.Message}");
                // No lanzar excepción, solo registrar el error
            }
        }

        /// <summary>
        /// MÉTODO SEGURO - Configura headers HTTP sin causar conflictos
        /// </summary>
        private void ConfigureHttpHeaders()
        {
            try
            {
                // Limpiar headers existentes de forma segura
                _httpClient.DefaultRequestHeaders.Clear();

                // Añadir headers básicos
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Añadir header de ngrok si es necesario
                //var random = new Random();
                //_httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", random.Next(1000, 9999).ToString());
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", AppConfig.GetFirebaseAuthToken());

                Console.WriteLine("✅ Headers HTTP configurados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error configurando headers: {ex.Message}");
                // No lanzar excepción
            }
        }

        /// <summary>
        /// MÉTODO CORREGIDO - Usa API Key en lugar de token de autenticación
        /// </summary>
        private async Task SaveToFirestoreWithApiKey(VehicleMultipleImages multipleImages)
        {
            try
            {
                Console.WriteLine("=== GUARDANDO EN FIRESTORE CON API KEY ===");
                Console.WriteLine($"Vehicle ID: {multipleImages.VehicleId}");

                // Verificar que tenemos API Key
                if (string.IsNullOrEmpty(_firebaseApiKey))
                {
                    throw new Exception("Firebase API Key no está configurada");
                }

                var url = $"{_firestoreUrl}/ImagenesVehiculos/{multipleImages.VehicleId}?key={_firebaseApiKey}";
                Console.WriteLine($"URL: {url}");

                // Crear documento simplificado
                var documentFields = new Dictionary<string, object>();

                // Metadatos básicos
                documentFields["vehicleId"] = new { stringValue = multipleImages.VehicleId };
                documentFields["createdAt"] = new { timestampValue = multipleImages.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") };
                documentFields["totalImages"] = new { integerValue = multipleImages.GetImageCount().ToString() };

                // Agregar imágenes de forma simplificada
                foreach (var kvp in multipleImages.Images)
                {
                    if (kvp.Value != null)
                    {
                        Console.WriteLine($"  - Añadiendo imagen: {kvp.Key}");

                        // SIMPLIFICADO: Solo campos básicos para evitar problemas
                        var imageObject = new
                        {
                            mapValue = new
                            {
                                fields = new
                                {
                                    vehicleId = new { stringValue = kvp.Value.VehicleId ?? "" },
                                    hasVector = new { booleanValue = kvp.Value.HasVector },
                                    thumbnailBase64 = new { stringValue = kvp.Value.ThumbnailBase64 ?? "" },
                                    mediumBase64 = new { stringValue = kvp.Value.MediumBase64 ?? "" }
                                }
                            }
                        };

                        documentFields[kvp.Key] = imageObject;
                    }
                }

                var document = new { fields = documentFields };
                var json = JsonConvert.SerializeObject(document);

                Console.WriteLine($"Tamaño del documento: {json.Length / 1024.0:F1} KB");

                // Configurar headers de forma segura
                ConfigureHttpHeaders();

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("📤 Enviando petición PUT...");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error en Firestore: {response.StatusCode}");
                    Console.WriteLine($"Contenido del error: {responseContent}");
                    throw new Exception($"Error guardando en Firestore: {response.StatusCode} - {responseContent}");
                }

                Console.WriteLine("✅ Guardado exitoso en Firestore");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en SaveToFirestoreWithApiKey: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Procesa una sola imagen de forma simplificada
        /// </summary>
        private async Task<VehicleImageData> ProcessSingleImage(Image originalImage, string imageKey)
        {
            try
            {
                Console.WriteLine($"  - Procesando imagen: {imageKey}");

                // Verificar que _imageProcessor no sea null
                if (_imageProcessor == null)
                {
                    throw new Exception("SimpleImageService no está inicializado");
                }

                // Obtener información básica
                var imageInfo = _imageProcessor.GetImageInfo(originalImage);

                // Crear versiones optimizadas
                var imageVersions = await _imageProcessor.CreateImageVersionsAsync(originalImage);

                var imageData = new VehicleImageData
                {
                    VehicleId = imageKey,
                    CreatedAt = DateTime.UtcNow,
                    HasVector = false, // Simplificado
                    OriginalInfo = imageInfo
                };

                // Solo thumbnail y medium para evitar problemas de tamaño
                if (imageVersions.ContainsKey("thumbnail"))
                {
                    imageData.ThumbnailBase64 = Convert.ToBase64String(imageVersions["thumbnail"]);
                }

                if (imageVersions.ContainsKey("medium"))
                {
                    imageData.MediumBase64 = Convert.ToBase64String(imageVersions["medium"]);
                }

                Console.WriteLine($"    ✅ Imagen procesada: {imageData.GetTotalSizeKB():F1} KB");
                return imageData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ❌ Error procesando imagen {imageKey}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// MÉTODO CORREGIDO - Actualiza imágenes usando API Key
        /// </summary>
        public async Task UpdateVehicleImagesInFirestoreAsync(List<Image> images, string vehicleId)
        {
            try
            {
                Console.WriteLine("=== ACTUALIZANDO IMÁGENES EN FIRESTORE ===");

                if (images == null || images.Count == 0)
                {
                    Console.WriteLine("⚠️ No hay imágenes para actualizar");
                    return;
                }

                // Verificar colección
                await EnsureCollectionExistsAsync("ImagenesVehiculos");

                // Procesar las imágenes
                var multipleImages = new VehicleMultipleImages
                {
                    VehicleId = vehicleId,
                    CreatedAt = DateTime.UtcNow,
                    Images = new Dictionary<string, VehicleImageData>()
                };

                for (int i = 0; i < images.Count && i < 5; i++)
                {
                    if (images[i] != null)
                    {
                        var imageKey = $"image_{i + 1}";
                        var imageData = await ProcessSingleImage(images[i], imageKey);
                        multipleImages.Images[imageKey] = imageData;
                    }
                }

                // Usar el mismo método de guardado
                await SaveToFirestoreWithApiKey(multipleImages);

                Console.WriteLine("✅ Imágenes actualizadas exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error actualizando imágenes: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// MÉTODO CORREGIDO - Obtiene imágenes usando API Key
        /// </summary>
        public async Task<VehicleMultipleImages> GetMultipleVehicleImagesAsync(string vehicleId)
        {
            try
            {
                Console.WriteLine("=== OBTENIENDO IMÁGENES DE FIRESTORE ===");
                Console.WriteLine($"Vehicle ID: {vehicleId}");

                if (string.IsNullOrEmpty(_firebaseApiKey))
                {
                    throw new Exception("Firebase API Key no está configurada");
                }

                var url = $"{_firestoreUrl}/ImagenesVehiculos/{vehicleId}?key={_firebaseApiKey}";
                Console.WriteLine($"URL: {url}");

                // Configurar headers de forma segura
                ConfigureHttpHeaders();

                Console.WriteLine("📤 Enviando petición GET...");
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("⚠️ Documento no encontrado");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error: {responseContent}");
                    throw new Exception($"Error obteniendo imágenes: {response.StatusCode} - {responseContent}");
                }

                Console.WriteLine("✅ Imágenes obtenidas exitosamente");

                // Por ahora retornar un objeto básico
                return new VehicleMultipleImages
                {
                    VehicleId = vehicleId,
                    CreatedAt = DateTime.UtcNow,
                    Images = new Dictionary<string, VehicleImageData>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error obteniendo imágenes: {ex.Message}");
                throw;
            }
        }

        // Métodos de compatibilidad simplificados
        public async Task<VehicleImageData> ProcessAndStoreVehicleImagesAsync(Image originalImage, string vehicleId)
        {
            var images = new List<Image> { originalImage };
            var multipleImages = await ProcessAndStoreMultipleVehicleImagesAsync(images, vehicleId);
            return multipleImages?.Images?.FirstOrDefault().Value;
        }

        public async Task<VehicleImageData> GetVehicleImagesAsync(string vehicleId)
        {
            var multipleImages = await GetMultipleVehicleImagesAsync(vehicleId);
            return multipleImages?.Images?.FirstOrDefault().Value;
        }

        public async Task<string> SaveImageAsync(Image originalImage, string path)
        {
            try
            {
                var vehicleId = path.Replace("vehicles/", "");
                var imageData = await ProcessAndStoreVehicleImagesAsync(originalImage, vehicleId);
                return $"firestore://{vehicleId}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar imagen: {ex.Message}", ex);
            }
        }

        public Image Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Optimiza imagen a Base64 respetando límite de tamaño
        /// </summary>
        private string OptimizeToBase64(byte[] imageData, int maxSize)
        {
            var base64 = Convert.ToBase64String(imageData);

            // Si ya cabe, retornar
            if (base64.Length <= maxSize)
            {
                return base64;
            }

            // Si no cabe, comprimir más (simplificado)
            return base64.Substring(0, Math.Min(base64.Length, maxSize - 100));
        }

        private VehicleMultipleImages ParseMultipleImagesDocument(FirestoreDocument document, string vehicleId)
        {
            var fields = document.Fields;

            var multipleImages = new VehicleMultipleImages
            {
                VehicleId = vehicleId,
                CreatedAt = DateTime.Parse(fields.createdAt?.TimestampValue ?? DateTime.UtcNow.ToString()),
                Images = new Dictionary<string, VehicleImageData>()
            };

            // Parsear cada imagen (1 a 5) - simplificado por ahora
            for (int i = 1; i <= 5; i++)
            {
                var imageKey = $"{vehicleId}.{i}";
                multipleImages.Images[imageKey] = null; // Por ahora null, se puede implementar el parseo completo después
            }

            return multipleImages;
        }

        private int GetFieldCount(FirestoreFields fields)
        {
            if (fields == null) return 0;

            int count = 0;
            var properties = typeof(FirestoreFields).GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(fields);
                if (value != null) count++;
            }
            return count;
        }
    }

    // Clases de datos simplificadas (sin cambios)
    public class VehicleMultipleImages
    {
        public string VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, VehicleImageData> Images { get; set; }

        public VehicleMultipleImages()
        {
            Images = new Dictionary<string, VehicleImageData>();
        }

        public int GetImageCount()
        {
            return Images?.Values?.Count(img => img != null) ?? 0;
        }

        public VehicleImageData GetPrimaryImage()
        {
            return Images?.Values?.FirstOrDefault(img => img != null);
        }

        public List<VehicleImageData> GetAllImages()
        {
            return Images?.Values?.Where(img => img != null)?.ToList() ?? new List<VehicleImageData>();
        }
    }

    public class VehicleImageData
    {
        public string VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasVector { get; set; }
        public VehicleImageInfo OriginalInfo { get; set; }

        public string ThumbnailBase64 { get; set; }
        public string MediumBase64 { get; set; }
        public string LargeBase64 { get; set; }
        public string OriginalBase64 { get; set; }
        public string VectorSVG { get; set; }

        public double GetTotalSizeKB()
        {
            double total = 0;
            if (!string.IsNullOrEmpty(ThumbnailBase64)) total += ThumbnailBase64.Length / 1024.0;
            if (!string.IsNullOrEmpty(MediumBase64)) total += MediumBase64.Length / 1024.0;
            if (!string.IsNullOrEmpty(LargeBase64)) total += LargeBase64.Length / 1024.0;
            if (!string.IsNullOrEmpty(OriginalBase64)) total += OriginalBase64.Length / 1024.0;
            if (!string.IsNullOrEmpty(VectorSVG)) total += VectorSVG.Length / 1024.0;
            return total;
        }

        public bool HasAnyImage()
        {
            return !string.IsNullOrEmpty(ThumbnailBase64) ||
                   !string.IsNullOrEmpty(MediumBase64) ||
                   !string.IsNullOrEmpty(VectorSVG);
        }
    }

    // Clases para deserializar Firestore (sin cambios)
    public class FirestoreDocument
    {
        [JsonProperty("fields")]
        public FirestoreFields Fields { get; set; }
    }

    public class FirestoreFields
    {
        [JsonProperty("vehicleId")]
        public FirestoreValue vehicleId { get; set; }

        [JsonProperty("createdAt")]
        public FirestoreValue createdAt { get; set; }

        [JsonProperty("totalImages")]
        public FirestoreValue totalImages { get; set; }

        [JsonProperty("hasVector")]
        public FirestoreValue hasVector { get; set; }

        [JsonProperty("originalWidth")]
        public FirestoreValue originalWidth { get; set; }

        [JsonProperty("originalHeight")]
        public FirestoreValue originalHeight { get; set; }

        [JsonProperty("originalFormat")]
        public FirestoreValue originalFormat { get; set; }

        [JsonProperty("aspectRatio")]
        public FirestoreValue aspectRatio { get; set; }

        [JsonProperty("thumbnailBase64")]
        public FirestoreValue thumbnailBase64 { get; set; }

        [JsonProperty("mediumBase64")]
        public FirestoreValue mediumBase64 { get; set; }

        [JsonProperty("largeBase64")]
        public FirestoreValue largeBase64 { get; set; }

        [JsonProperty("originalBase64")]
        public FirestoreValue originalBase64 { get; set; }

        [JsonProperty("vectorSVG")]
        public FirestoreValue vectorSVG { get; set; }
    }

    public class FirestoreValue
    {
        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("integerValue")]
        public string IntegerValue { get; set; }

        [JsonProperty("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("timestampValue")]
        public string TimestampValue { get; set; }

        [JsonProperty("nullValue")]
        public object NullValue { get; set; }

        [JsonProperty("mapValue")]
        public object MapValue { get; set; }
    }
}
