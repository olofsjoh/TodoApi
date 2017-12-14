using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;

namespace IntegrationTests.TodoController
{
    public class Delete : TodoControllerTestBase
    {
        private readonly HttpClient _client;

        public Delete()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            var response = await _client.DeleteAsync($"/api/{controllerName}/0");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsOkGivenValidTodo(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(stringResponse).ToList();
            var idToDelete = todos.FirstOrDefault().Id;
            var response2 = await _client.DeleteAsync($"/api/{controllerName}/{idToDelete}");
            response2.EnsureSuccessStatusCode();

            var deleteMessage = await response2.Content.ReadAsStringAsync();
        }
    }
}
