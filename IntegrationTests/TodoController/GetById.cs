using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;

namespace IntegrationTests.TodoController
{
    public class GetById : TodoControllerTestBase
    {
        private readonly HttpClient _client;

        public GetById()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}/0");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsCorrectOfTodos(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Todo>(stringResponse);
        }
    }
}
