﻿using Domain.DTO.Client;
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

        public async Task<DataTable> ClientUpdatePassword(ClientUpdatePassword request)
        {
            string url = $"https://localhost:7130/api/Customer/UpdatePassword";
            var response = await _httpClient.PutAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DataTable>(resultString);
                return result;
            }

            // Xử lý lỗi nếu cần
            return null;
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
