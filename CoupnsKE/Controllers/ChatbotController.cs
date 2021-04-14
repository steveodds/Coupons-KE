using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace CoupnsKE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private class Weather
        {
            public string weather { get; set; }
            public string weatherDescription { get; set; }
            public string temperature { get; set; }
        }

        public ChatbotController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var request = _contextAccessor.HttpContext.Request;

            var obj = GetRequestBodyAsync(request);

            throw new Exception($"req ==> [{obj}]");
            //return Ok(obj);
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {

            string objRequestBody;

            // IMPORTANT: Ensure the requestBody can be read multiple times.
            HttpRequestRewindExtensions.EnableBuffering(request);

            // IMPORTANT: Leave the body open so the next middleware can read it.
            using (StreamReader reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                string strRequestBody = await reader.ReadToEndAsync();
                objRequestBody = strRequestBody;

                // IMPORTANT: Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;
            }
            return objRequestBody;
        }

        [HttpGet]
        [Route("/WeatherInfo")]
        public ActionResult WeatherInfo([FromQuery] string location)
        {
            var result = WeatherData(location);
            //result = result.Remove(0, 1);
            //result = result.Remove(result.Length - 1, 1);
            var structuredResult = JObject.Parse(result);
            var weatherObject = new Weather
            {
                weather = (string)structuredResult["weather"].Select(p => p["main"]).FirstOrDefault(),
                weatherDescription = (string)structuredResult["weather"].Select(p => p["description"]).FirstOrDefault(),
                temperature = (string)structuredResult["main"]["temp"]
            };

            result = JsonConvert.SerializeObject(weatherObject, Formatting.Indented);

            return Ok(result);
        }

        private string WeatherData(string location)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.openweathermap.org/data/2.5/weather");
            //client.Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("MailgunAPI"));
            var request = new RestRequest();
            request.AddParameter("q", location);
            request.AddParameter("units", "metric");
            request.AddParameter("appid", Environment.GetEnvironmentVariable("WeatherAPI"));
            request.Method = Method.GET;

            var result = client.Execute(request);

            return result.Content;
        }
    }
}
