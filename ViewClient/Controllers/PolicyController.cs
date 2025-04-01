using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViewClient.ViewModels;

namespace ViewClient.Controllers
{
    public class PolicyController : Controller
    {
        HttpClient _client;

        public PolicyController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> GetAllPolicyTerms()
        {
            string requestUrl = "api/Policy/GetAllPolicyTerms";
            try
            {
                var response = await _client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                var terms = JsonConvert.DeserializeObject<List<TermsViewModel>>(responseString);

                return View(terms);
            }
            catch (Exception)
            {
                return View("Error");
            };
        }
    }
}
