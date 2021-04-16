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

        //Internal models start
        private class Weather
        {
            public string location { get; set; }
            public string weather { get; set; }
            public string weatherDescription { get; set; }
            public string temperature { get; set; }
        }


        public class ChatbotResponse
        {
            public Respons[] responses { get; set; }
        }

        public class Respons
        {
            public string type { get; set; }
            public int delay { get; set; }
            public string message { get; set; }
        }

        //Internal models end

        public ChatbotController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public ActionResult Index([FromQuery] string challenge)
        {
            //Chatbot.com verification for enabling webhook | Returns challenge string
            return Ok(challenge is null ? "OK" : challenge);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult WeatherInfo([FromBody] dynamic content)
        {
            string locationFromRequest = GetUserLocation(content);
            var result = WeatherData(locationFromRequest);
            //Get only the ecessary weather data and map it into the model
            var structuredResult = JObject.Parse(result);
            var weatherObject = new Weather
            {
                location = locationFromRequest,
                weather = (string)structuredResult["weather"].Select(p => p["main"]).FirstOrDefault(),
                weatherDescription = (string)structuredResult["weather"].Select(p => p["description"]).FirstOrDefault(),
                temperature = (string)structuredResult["main"]["temp"]
            };

            var message = GenerateResponse(weatherObject); //get Chatbot.com compatible response

            result = JsonConvert.SerializeObject(message, Formatting.Indented);
            //return as json
            return Ok(result);
        }

        private string GetUserLocation(dynamic content)
        {
            var parsedJson = JObject.Parse(Convert.ToString(content));
            var location = (string)parsedJson["attributes"]["default_city"];

            return location;
        }

        private ChatbotResponse GenerateResponse(Weather weather)
        {
            //Convert Weather object to a model that can generate JSON in the format expected by Chatbot.com
            var response = new Respons[1];
            response[0] = new Respons
            {
                delay = 1000,
                type = "text",
                message = $"The weather at {weather.location} is: '{weather.weather} | {weather.weatherDescription}' with a temperature of {weather.temperature} degrees celsius."
            };

            return new ChatbotResponse { responses = response };
        }

        private string WeatherData(string location)
        {
            //Call OpenWeather API to get weather data
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.openweathermap.org/data/2.5/weather");
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
