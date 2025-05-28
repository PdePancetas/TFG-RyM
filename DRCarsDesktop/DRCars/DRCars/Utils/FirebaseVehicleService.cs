using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DRCars.Models;

namespace DRCars.Utils
{
    public class FirebaseVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseUrl;
        private readonly FirebaseImageService _imageService;

        public FirebaseVehicleService()
        {
            _httpClient = new HttpClient();
            _firebaseUrl = "https://tu-proyecto-firebase-default-rtdb.firebaseio.com";
            _imageService = new FirebaseImageService();
        }

        /// <summary>
        /// Guarda vehículo con imágenes optimizadas en Firebase
        /// </summary>
        public async Task<Vehicle> SaveVehicleWithImagesAsync(Vehicle vehicle, System.Drawing.Image vehicleImage = null)
        {
            try
            {
                // 1. Si hay imagen, subirla primero a Firebase Storage
                if (vehicleImage != null)
                {
                    var imageUrls = await _imageService.UploadOptimizedVehicleImagesAsync(vehicleImage, vehicle.Id.ToString());

                    // Asignar URLs de las imágenes al vehículo
                    vehicle.ImageUrl = imageUrls.MediumUrl; // URL principal
                    vehicle.AdditionalImages = new List<string>
                    {
                        imageUrls.ThumbnailUrl,
                        imageUrls.LargeUrl,
                        imageUrls.OriginalUrl
                    };

                    // Si hay versión vectorial, añadirla
                    if (!string.IsNullOrEmpty(imageUrls.VectorUrl))
                    {
                        vehicle.AdditionalImages.Add(imageUrls.VectorUrl);
                    }
                }

                // 2. Guardar vehículo en Firestore
                var vehicleData = new
                {
                    id = vehicle.Id,
                    marca = vehicle.Brand,
                    modelo = vehicle.Model,
                    año = vehicle.Year,
                    precio = vehicle.Price,
                    kilometraje = vehicle.Kilometers,
                    color = vehicle.Color,
                    estado = vehicle.StatusString,
                    categoria = vehicle.Category,
                    tipoCombustible = vehicle.FuelType,
                    transmision = vehicle.Transmission,
                    imagenPrincipal = vehicle.ImageUrl,
                    imagenesAdicionales = vehicle.AdditionalImages,
                    fechaCreacion = vehicle.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    fechaActualizacion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                var json = JsonConvert.SerializeObject(vehicleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_firebaseUrl}/vehiculos/{vehicle.Id}.json", content);
                response.EnsureSuccessStatusCode();

                return vehicle;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar vehículo en Firebase: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los vehículos de Firebase
        /// </summary>
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_firebaseUrl}/vehiculos.json");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content) || content == "null")
                {
                    return new List<Vehicle>();
                }

                var firebaseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content);
                var vehicles = new List<Vehicle>();

                foreach (var item in firebaseData)
                {
                    var vehicleData = JsonConvert.DeserializeObject<dynamic>(item.Value.ToString());

                    var vehicle = new Vehicle
                    {
                        Id = (int)vehicleData.id,
                        Brand = vehicleData.marca,
                        Model = vehicleData.modelo,
                        Year = (int)vehicleData.año,
                        Price = (decimal)vehicleData.precio,
                        Kilometers = (int)vehicleData.kilometraje,
                        Color = vehicleData.color,
                        StatusString = vehicleData.estado,
                        Category = vehicleData.categoria,
                        FuelType = vehicleData.tipoCombustible,
                        Transmission = vehicleData.transmision,
                        ImageUrl = vehicleData.imagenPrincipal,
                        AdditionalImages = vehicleData.imagenesAdicionales?.ToObject<List<string>>() ?? new List<string>(),
                        CreatedAt = DateTime.Parse(vehicleData.fechaCreacion.ToString())
                    };

                    vehicles.Add(vehicle);
                }

                return vehicles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener vehículos de Firebase: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza vehículo en Firebase
        /// </summary>
        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var updates = new
                {
                    marca = vehicle.Brand,
                    modelo = vehicle.Model,
                    año = vehicle.Year,
                    precio = vehicle.Price,
                    kilometraje = vehicle.Kilometers,
                    color = vehicle.Color,
                    estado = vehicle.StatusString,
                    categoria = vehicle.Category,
                    tipoCombustible = vehicle.FuelType,
                    transmision = vehicle.Transmission,
                    fechaActualizacion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                var json = JsonConvert.SerializeObject(updates);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_firebaseUrl}/vehiculos/{vehicle.Id}.json", content);
                response.EnsureSuccessStatusCode();

                return vehicle;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar vehículo en Firebase: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina vehículo de Firebase
        /// </summary>
        public async Task<bool> DeleteVehicleAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_firebaseUrl}/vehiculos/{vehicleId}.json");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar vehículo de Firebase: {ex.Message}", ex);
            }
        }
    }
}
