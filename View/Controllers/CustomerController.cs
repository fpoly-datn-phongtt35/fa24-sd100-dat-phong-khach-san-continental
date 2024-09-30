using Domain.DTO.Customer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using View.Models.Customer;

namespace View.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomerController()
        {
            _httpClient = new HttpClient();
        }

        public ActionResult Index()
        {
            string requestURL = "https://localhost:7130/api/Customer/GetListCustomer";

            var customerRequest = new CustomerGetByUserNameRequest
            {
                UserName = "", 
                PageIndex = 1,
                PageSize = 10
            };

            var response = _httpClient.PostAsJsonAsync(requestURL, customerRequest).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<List<Customer>>(response.Content.ReadAsStringAsync().Result);
                return View(result);
            }
            else
            {
                // Handle error (optional)
                ViewBag.ErrorMessage = "Error retrieving customers: " + response.ReasonPhrase;
                return View(new List<Customer>()); // Return an empty list or handle as needed
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateCustomer(Domain.DTO.Customer.CustomerCreateRequest customer)
        {
            var apiUrl = "https://localhost:7130/api/Customer/CreateCustomer"; // Replace with your API URL
            var json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Handle success (e.g., redirect or display a message)
                return RedirectToAction("Success");
            }
            else
            {
                // Handle error (e.g., log it and display error information)
                var errorResponse = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = errorResponse;
                return View("Error");
            }
        }
    }
}
