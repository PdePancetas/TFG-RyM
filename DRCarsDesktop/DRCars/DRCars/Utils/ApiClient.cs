using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DRCars.Models;

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
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);

            // Añadir el encabezado requerido para ngrok
            _random = new Random();
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", _random.Next(1000, 9999).ToString());
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/catalogo");
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
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/catalogo/crear", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/catalogo/act", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<Vehicle> UpdateVehicleToApiAsync(Vehicle vehicle)
        {
            try
            {
                // Crear objeto del vehículo sin imágenes para la API
                var vehicleData = new
                {
                    idVehiculo = vehicle.Id,
                    marca = vehicle.Brand,
                    modelo = vehicle.Model,
                    annoFabricacion = vehicle.Year,
                    color = vehicle.Color,
                    kilometraje = vehicle.Kilometers,
                    precioCompra = vehicle.Price,
                    matricula = vehicle.LicensePlate,
                    numeroChasis = vehicle.VIN,
                    estado = vehicle.StatusString,
                    proveedor = vehicle.Supplier
                };

                var json = JsonConvert.SerializeObject(vehicleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/catalogo/act", content);
                response.EnsureSuccessStatusCode();

                return vehicle;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar vehículo en la API: {ex.Message}", ex);
            }
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

        public async Task<List<SaleRequest>> GetSaleRequestsAsync()
        {
            var response = await _httpClient.GetAsync("/reservas");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SaleRequest>>(content);
        }

        public async Task<String> UpdateSaleRequestAsync(SaleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/reservas/procesar", content);
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

        public async Task<(bool Success, string UserType, string Message)> LoginAsync(string email, string password)
        {
            try
            {
                // Crear el objeto de datos para el login
                var loginData = new
                {
                    usuario = email,
                    contraseña = password,
                    tipoUsuario = "ADMIN",
                    ultimo_acceso = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    registro_cuenta = ""
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
        /*
        // Mock data methods
        private List<Vehicle> GetMockVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 1,
                    Brand = "Mercedes-Benz",
                    Model = "S-Class",
                    Year = 2023,
                    Price = 125000,
                    Category = "Lujo",
                    FuelType = "Híbrido",
                    TransmissionString = "Automática",
                    Kilometers = 5200,
                    Status = VehicleStatus.ForSale,
                    ImageUrl = "https://example.com/mercedes-s-class.jpg",
                    CreatedAt = DateTime.Now.AddMonths(-3)
                },
                new Vehicle
                {
                    Id = 2,
                    Brand = "BMW",
                    Model = "7 Series",
                    Year = 2023,
                    Price = 110000,
                    Category = "Lujo",
                    FuelType = "Híbrido",
                    TransmissionString = "Automática",
                    Kilometers = 3800,
                    Status = VehicleStatus.InStock,
                    ImageUrl = "https://example.com/bmw-7-series.jpg",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new Vehicle
                {
                    Id = 3,
                    Brand = "Audi",
                    Model = "A8",
                    Year = 2023,
                    Price = 105000,
                    Category = "Lujo",
                    FuelType = "Híbrido",
                    TransmissionString = "Automática",
                    Kilometers = 4500,
                    Status = VehicleStatus.InGarage,
                    ImageUrl = "https://example.com/audi-a8.jpg",
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new Vehicle
                {
                    Id = 4,
                    Brand = "Porsche",
                    Model = "911",
                    Year = 2022,
                    Price = 145000,
                    Category = "Deportivo",
                    FuelType = "Gasolina",
                    TransmissionString = "Automática",
                    Kilometers = 8500,
                    Status = VehicleStatus.ForSale,
                    ImageUrl = "https://example.com/porsche-911.jpg",
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new Vehicle
                {
                    Id = 5,
                    Brand = "Maserati",
                    Model = "Ghibli",
                    Year = 2022,
                    Price = 95000,
                    Category = "Lujo",
                    FuelType = "Gasolina",
                    Transmission = "Automática",
                    Kilometers = 12000,
                    Status = VehicleStatus.Sold,
                    ImageUrl = "https://example.com/maserati-ghibli.jpg",
                    CreatedAt = DateTime.Now.AddMonths(-5)
                }
            };
        }

        private List<User> GetMockUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Admin Usuario",
                    Email = "admin@drcars.com",
                    Phone = "600123456",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddYears(-1),
                    LastLogin = DateTime.Now.AddDays(-1)
                },
                new User
                {
                    Id = 2,
                    Name = "Gerente Usuario",
                    Email = "gerente@drcars.com",
                    Phone = "600234567",
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-10),
                    LastLogin = DateTime.Now.AddDays(-2)
                },
                new User
                {
                    Id = 3,
                    Name = "Agente Usuario",
                    Email = "agente@drcars.com",
                    Phone = "600345678",
                    Role = UserRole.SalesAgent,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-8),
                    LastLogin = DateTime.Now.AddDays(-3)
                },
                new User
                {
                    Id = 4,
                    Name = "Visualizador Usuario",
                    Email = "visualizador@drcars.com",
                    Phone = "600456789",
                    Role = UserRole.Viewer,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6),
                    LastLogin = DateTime.Now.AddDays(-5)
                },
                new User
                {
                    Id = 5,
                    Name = "Usuario Inactivo",
                    Email = "inactivo@drcars.com",
                    Phone = "600567890",
                    Role = UserRole.SalesAgent,
                    IsActive = false,
                    CreatedAt = DateTime.Now.AddMonths(-12),
                    LastLogin = DateTime.Now.AddMonths(-3)
                }
            };
        }

        private List<SaleRequest> GetMockSaleRequests()
        {
            return new List<SaleRequest>
            {
                new SaleRequest
                {
                    Id = 1,
                    CustomerName = "Juan Pérez",
                    CustomerEmail = "juan.perez@example.com",
                    CustomerPhone = "600111222",
                    DesiredBrand = "Mercedes-Benz",
                    DesiredModel = "S-Class",
                    Budget = 130000,
                    DeliveryTimeframe = "3 meses",
                    AdditionalDetails = "Preferiblemente color negro con interior beige.",
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new SaleRequest
                {
                    Id = 2,
                    CustomerName = "María García",
                    CustomerEmail = "maria.garcia@example.com",
                    CustomerPhone = "600222333",
                    DesiredBrand = "BMW",
                    DesiredModel = "X5",
                    Budget = 90000,
                    DeliveryTimeframe = "1 mes",
                    AdditionalDetails = "Interesada en financiación.",
                    Status = RequestStatus.Scheduled,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    AppointmentDate = DateTime.Now.AddDays(2)
                },
                new SaleRequest
                {
                    Id = 3,
                    CustomerName = "Carlos Rodríguez",
                    CustomerEmail = "carlos.rodriguez@example.com",
                    CustomerPhone = "600333444",
                    DesiredBrand = "Audi",
                    DesiredModel = "Q7",
                    Budget = 85000,
                    DeliveryTimeframe = "2 meses",
                    AdditionalDetails = "Busca un vehículo familiar con 7 plazas.",
                    Status = RequestStatus.Completed,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    AppointmentDate = DateTime.Now.AddDays(-5)
                },
                new SaleRequest
                {
                    Id = 4,
                    CustomerName = "Ana Martínez",
                    CustomerEmail = "ana.martinez@example.com",
                    CustomerPhone = "600444555",
                    DesiredBrand = "Porsche",
                    DesiredModel = "Cayenne",
                    Budget = 120000,
                    DeliveryTimeframe = "Inmediato",
                    AdditionalDetails = "Prefiere color blanco o gris.",
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new SaleRequest
                {
                    Id = 5,
                    CustomerName = "Roberto Sánchez",
                    CustomerEmail = "roberto.sanchez@example.com",
                    CustomerPhone = "600555666",
                    DesiredBrand = "Maserati",
                    DesiredModel = "Levante",
                    Budget = 110000,
                    DeliveryTimeframe = "3 meses",
                    AdditionalDetails = "Interesado en modelo híbrido si está disponible.",
                    Status = RequestStatus.Cancelled,
                    CreatedAt = DateTime.Now.AddDays(-20)
                }
            };
        }

        private List<Sale> GetMockSales()
        {
            return new List<Sale>
            {
                new Sale
                {
                    Id = 1,
                    VehicleId = 5,
                    CustomerId = 101,
                    SalesAgentId = 3,
                    SalePrice = 92000,
                    SaleDate = DateTime.Now.AddDays(-30),
                    PaymentMethod = "Transferencia Bancaria",
                    Notes = "Cliente satisfecho con la compra."
                },
                new Sale
                {
                    Id = 2,
                    VehicleId = 8,
                    CustomerId = 102,
                    SalesAgentId = 2,
                    SalePrice = 135000,
                    SaleDate = DateTime.Now.AddDays(-25),
                    PaymentMethod = "Financiación",
                    Notes = "Financiación a 5 años con entrada de 30.000€."
                },
                new Sale
                {
                    Id = 3,
                    VehicleId = 12,
                    CustomerId = 103,
                    SalesAgentId = 3,
                    SalePrice = 78000,
                    SaleDate = DateTime.Now.AddDays(-15),
                    PaymentMethod = "Efectivo",
                    Notes = "Cliente internacional."
                },
                new Sale
                {
                    Id = 4,
                    VehicleId = 15,
                    CustomerId = 104,
                    SalesAgentId = 2,
                    SalePrice = 105000,
                    SaleDate = DateTime.Now.AddDays(-10),
                    PaymentMethod = "Transferencia Bancaria",
                    Notes = "Incluye garantía extendida."
                },
                new Sale
                {
                    Id = 5,
                    VehicleId = 18,
                    CustomerId = 105,
                    SalesAgentId = 3,
                    SalePrice = 95000,
                    SaleDate = DateTime.Now.AddDays(-5),
                    PaymentMethod = "Financiación",
                    Notes = "Financiación a 3 años."
                }
            };
        }*/
    }
}
