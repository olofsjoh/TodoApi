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

        [Fact]
        public async Task ReturnsNotFoundForId0()
        {
            var response = await _client.GetAsync($"/api/todo/0");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsCorrectOfTodos()
        {
            var response = await _client.GetAsync($"/api/todo/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Todo>(stringResponse);
        }
    }
}
