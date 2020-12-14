using IdentityModel.Client;
using mvcapp;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi1;
using WebApi1.Contracts.Dto;
using Xunit;


namespace Tests
{
    public class IntegrationTestExample : IClassFixture<AppTestFixture<Startup>>
    {
        private readonly TokenClient _tokenClient;
        private readonly HttpClient _client;
       // private readonly string _accessToken;

        public IntegrationTestExample(AppTestFixture<Startup> factory)
        {

            _client = factory.CreateClient();
            var disco = DiscoveryClient.GetAsync("http://localhost:5500").Result;
            _tokenClient = new TokenClient(disco.TokenEndpoint, "mvc", "secret");
           

        }

        /// <summary>
        /// Тест для webapi
        /// Предварительно берем access_token с IS4 (IS4 должен быть запущен на порту 5500)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CanGetWeather()
        {
            var tokenResponse = await _tokenClient.RequestResourceOwnerPasswordAsync("test2@ya.ru", "123123");
            _client.SetBearerToken(tokenResponse.AccessToken);
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("WeatherForecast");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            //// Deserialize and examine results.
            var data = await httpResponse.Content.ReadAsStringAsync();
           var weatherForecasts = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(data);
                       

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);            
           
            Assert.NotEmpty(weatherForecasts);
            Assert.Equal(5,weatherForecasts.Count());            

        }

        [Fact]
        public async Task CanGetProducts()
        {
            var tokenResponse = await _tokenClient.RequestResourceOwnerPasswordAsync("test2@ya.ru", "123123");
            _client.SetBearerToken(tokenResponse.AccessToken);
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/api/Products/GetProducts");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            //// Deserialize and examine results.
            var data = await httpResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(data);


            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            Assert.NotEmpty(products);
            Assert.Equal(5, products.Count());

        }
    }
}
