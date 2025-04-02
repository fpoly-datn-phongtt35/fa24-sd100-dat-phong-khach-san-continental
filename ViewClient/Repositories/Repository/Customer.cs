using Domain.DTO.Client;
using Domain.DTO.Customer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using ViewClient.Models.DTO.Login;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class Customer : ICustomer
    {
        private readonly HttpClient _httpClient;

        public Customer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ClientInsertCustomerViewModel> ClientInsertCustomer(ClientCreateCustomerRequest request)
        {
            string url = $"https://localhost:7130/api/Customer/ClientCreateCustomer";
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ClientInsertCustomerViewModel>(resultString);
                return result;
            }

            // Xử lý lỗi nếu cần
            return null;
        }

        public async Task<string> ClientUpdatePassword(ClientUpdatePassword request)
        {
            string url = "https://localhost:7130/api/Customer/UpdatePassword";

            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                    if (result != null && result.ContainsKey("message"))
                    {
                        return result["message"];
                    }

                    return "Không nhận được phản hồi từ API.";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Lỗi API: {response.StatusCode} - {errorMessage}";
                }
            }
            catch (Exception ex)
            {
                return $"Đã xảy ra lỗi khi gọi API: {ex.Message}";
            }
        }

        public async Task<CustomerGetByIdRequest> GetCustomerById(Guid id)
        {
            string url = $"https://localhost:7130/api/Customer/GetCustomerById?Id={id}";
            var response = await _httpClient.PostAsJsonAsync(url, id);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CustomerGetByIdRequest>(resultString);
                return result;
            }

            // Xử lý lỗi nếu cần
            return null;
        }

        public async Task<int> UpdateCustomer(CustomerUpdateRequest request)
        {
            string url = $"https://localhost:7130/api/Customer/UpdateCustomer";
            var response = await _httpClient.PutAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);
                return result;
            }

            // Xử lý lỗi nếu cần
            return 0;
        }
    }
}
