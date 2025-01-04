using Domain.DTO.Post;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViewClient.ViewModels;

namespace ViewClient.Controllers
{
    public class PostController : Controller
    {
        HttpClient _client;

        public PostController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> GetAllPostTerms()
        {
            string requestUrl = "api/Post/GetAllPostTerms";
            try
            {
                var response = await _client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                var terms = JsonConvert.DeserializeObject<List<TermsViewModel>>(responseString);

                if (terms == null || !terms.Any())
                {
                    return View("Error");
                }

                return View(terms);
            }
            catch (Exception)
            {
                return View("Error");
            };
        }
    }
}
