using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        [HttpPost("GenerateQR")]
        public IActionResult GenerateQR([FromBody] ApiRequest apiRequest)
        {
            var jsonRequest = JsonConvert.SerializeObject(apiRequest);

            var client = new RestClient("https://api.vietqr.io/v2/generate");
            var request = new RestRequest();

            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", jsonRequest, ParameterType.RequestBody);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                return BadRequest();
            }

            var content = response.Content;
            var dataResult = JsonConvert.DeserializeObject<ApiResponse>(content);

            //chuyển về byte array
            var qrImageBytes = Base64ToImage(dataResult.data.qrDataURL.Replace("data:image/png;base64,", ""));

            return File(qrImageBytes, "image/png");
 
        }

        private byte[] Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            return imageBytes;
        }
    }
}
