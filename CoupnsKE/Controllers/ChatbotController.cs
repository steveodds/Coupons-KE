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

namespace CoupnsKE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {

        private class Weather
        {
            public string weather { get; set; }
            public string weatherDescription { get; set; }
            public string temperature { get; set; }
        }


        [HttpGet]
        public ActionResult Index([FromQuery] string location)
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
