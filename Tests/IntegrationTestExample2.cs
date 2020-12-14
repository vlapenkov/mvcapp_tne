using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using mvcapp;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi1;
using WebApi1.Contracts.Dto;
using Xunit;

namespace Tests
{
    // Предложенный подход на сайте microsoft
    //https://docs.microsoft.com/ru-ru/aspnet/core/test/integration-tests?view=aspnetcore-3.1
    // Для таких тестов не надо запускать IS4 сервер (делется для TestAuthHandler)
    // База данных in-memory

    public class IntegrationTestExample2 : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly TokenClient _tokenClient;
        private readonly HttpClient _client;
        CustomWebApplicationFactory<Startup> _factory;

        public IntegrationTestExample2(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory            
        .CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
            _client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Test");
           
        }

        /// <summary>
        /// Тест для webapi
        /// Предварительно берем access_token с IS4 (IS4 должен быть запущен на порту 5500)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CanGetWeather()
        {                     
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("WeatherForecast");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            //// Deserialize and examine results.
            var data = await httpResponse.Content.ReadAsStringAsync();
           var weatherForecasts = JsonConvert.DeserializeObject<IEnumerable<WebApi1.Contracts.Dto.WeatherForecast>>(data);
                       

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);            
           
            Assert.NotEmpty(weatherForecasts);
            Assert.Equal(5,weatherForecasts.Count());            

        }

        /// <summary>
        /// Данный метод берет данные из endpoint, который сконфигурирован на in-memory database (см. CustomWebApplicationFactory)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CanGetProducts()
        {
            
            var httpResponse = await _client.GetAsync("/api/Products/GetProducts");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            //// Deserialize and examine results.
            var data = await httpResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(data);


            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            Assert.NotEmpty(products);
            Assert.Equal(4, products.Count());

        }

        [Fact]
        public async Task CanGetUser()
        {
           
            var httpResponse = await _client.GetAsync("/api/Products/GetUser");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            //// Deserialize and examine results.
            var data = await httpResponse.Content.ReadAsStringAsync();
            var userName = data;


            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            Assert.NotEmpty(userName);
            Assert.Equal("test2", userName);

        }
    }
}
