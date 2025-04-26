using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DRCars.Models;

namespace DRCars.Utils
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient()
        {
            _baseUrl = AppConfig.ApiBaseUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            var response = await _httpClient.GetAsync("/api/vehicles");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Vehicle>>(content);
        }

        public async Task<Vehicle> GetVehicleAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/vehicles/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(content);
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/vehicles", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/vehicles/{vehicle.Id}", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(responseContent);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(content);
        }

        public async Task<List<SaleRequest>> GetSaleRequestsAsync()
        {
            var response = await _httpClient.GetAsync("/api/salerequests");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SaleRequest>>(content);
        }

        public async Task<SaleRequest> UpdateSaleRequestAsync(SaleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/salerequests/{request.Id}", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SaleRequest>(responseContent);
        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            var response = await _httpClient.GetAsync("/api/sales");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Sale>>(content);
        }

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            var json = JsonConvert.SerializeObject(sale);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/sales", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Sale>(responseContent);
        }
    }
}