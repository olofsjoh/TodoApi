using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;

namespace IntegrationTests.TodoController
{
    public class Get : TodoControllerTestBase
    {
        private readonly HttpClient _client;

        public Get()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsListOfTodos(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Todo>>(stringResponse).ToList();

            Assert.Equal(3, result.Count());
        }
    }
}
