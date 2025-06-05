using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DRCars.Models;
using System.IO;
using System.IdentityModel.Protocols.WSTrust;

namespace DRCars.Utils
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly Random _random;

        public ApiClient()
        {
            _baseUrl = AppConfig.ApiBaseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

            // Añadir el encabezado requerido para ngrok
            _random = new Random();
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", _random.Next(1000, 9999).ToString());
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/catalogo/app");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta JSON
                var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(content);

                // Asignar valores predeterminados a propiedades que no vienen en el JSON
                foreach (var vehicle in vehicles)
                {
                    // Asignar una imagen de placeholder basada en la marca y modelo
                    vehicle.ImageUrl = $"https://via.placeholder.com/300x200?text={vehicle.Brand}+{vehicle.Model}";

                    // Asignar el precio de costo igual al precio de compra por ahora
                    vehicle.CostPrice = vehicle.Price;

                    // Asignar fechas si no están establecidas
                    if (vehicle.CreatedAt == DateTime.MinValue)
                        vehicle.CreatedAt = DateTime.Now.AddMonths(-1);

                    if (vehicle.ImportDate == DateTime.MinValue)
                        vehicle.ImportDate = DateTime.Now.AddMonths(-1);
                }

                return vehicles;
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                Console.WriteLine($"Error al obtener vehículos: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");

                throw new Exception($"Error al cargar vehículos: {ex.Message}", ex);
            }
        }

        public async Task<Vehicle> GetVehicleAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(content);
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle, new StringEnumConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/catalogo/crear", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle, new StringEnumConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/catalogo/act", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/users");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(content);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user, new StringEnumConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/act", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent);
        }
        public async Task<User> AddUserAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user, new StringEnumConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/crear", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent);
        }
        public async Task<User> DeleteUserAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user, new StringEnumConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/delete", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent);
        }


        public async Task<List<Request>> GetRequestsAsync()
        {
            var response = await _httpClient.GetAsync("/solicitudes");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Request>>(content);
        }

        public async Task<String> UpdateSaleRequestAsync(Request request)
        {
            var respuesta = new
            {
                idReserva = request.Id,
                aceptada = request.Vehicle!=null,
                fechaVenta = request.RequestDate,
                precioVenta = request.Budget
            };
            var json = JsonConvert.SerializeObject(respuesta);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/solicitudes/procesar", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var response = await _httpClient.GetAsync("/ventas");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Sale>>(content);
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            var response = await _httpClient.GetAsync("/reservas");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Appointment>>(content);
        }

        public async Task<(bool Success, string UserType, string Message)> LoginAsync(string email, string password)
        {
            try
            {
                // Crear el objeto de datos para el login
                var loginData = new
                {
                    usuario = email,
                    contraseña = password,
                    ultimo_acceso = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                // Convertir a JSON
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la petición POST
                var response = await _httpClient.PostAsync("/auth/login", content);

                // Leer el contenido de la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();

                // Si la respuesta es exitosa (200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Analizar la respuesta para determinar el tipo de usuario
                    // Asumimos que la respuesta es "Autenticacion Exitosa USER" o "Autenticacion Exitosa ADMIN"
                    if (responseContent.Contains("ADMIN"))
                    {
                        return (true, "ADMIN", "Autenticación exitosa");
                    }
                    else if (responseContent.Contains("USER"))
                    {
                        return (false, "USER", "No tienes los permisos necesarios para iniciar sesión");
                    }
                    else
                    {
                        return (false, "", "Respuesta de autenticación no reconocida");
                    }
                }
                // Si la respuesta es 401 Unauthorized
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return (false, "", "Credenciales incorrectas");
                }
                // Cualquier otro error
                else
                {
                    return (false, "", $"Error del servidor: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return (false, "", "Error de conexión. Verifique su conexión a internet.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "", "Error al procesar la solicitud de inicio de sesión.");
            }
        }

        public async Task<String> CompleteAppointmentAsync(Appointment appointment)
        {
            var respuesta = new
            {
                idReserva = appointment.Id,
                cliente = appointment.client,
                vehiculo = appointment.Vehicle,
                fechaReserva = appointment.AppointmentDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                precioReserva = appointment.AppointmentPrice
            };
            var json = JsonConvert.SerializeObject(respuesta, new StringEnumConverter());

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/reservas/convertir", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<String>(responseContent);
        }

        internal async Task<String> DeleteAppointmentAsync(Appointment appointment)
        {
            var respuesta = new
            {
                idReserva = appointment.Id
            }; 
            var json = JsonConvert.SerializeObject(respuesta);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/reservas/delete", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<String>(responseContent);
        }
    }
}
