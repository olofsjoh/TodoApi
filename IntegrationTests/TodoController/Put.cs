using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;

namespace IntegrationTests.TodoController
{
    public class Put : TodoControllerTestBase
    {
        private readonly HttpClient _client;

        public Put()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            var todo = CreateMissingTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/{controllerName}", jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsBadRequestGivenNoName(string controllerName)
        {
            var todo = CreateEmptyNameTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/{controllerName}/1", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsBadRequestGivenTooLongName(string controllerName)
        {
            var todo = CreateTooLongNameTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/{controllerName}/1", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", stringResponse);
            Assert.Contains("The field Name must be a string or array type with a maximum length of '20'.", stringResponse);
        }

        [Theory]
        [InlineData("todo")]
        [InlineData("todofilter")]
        public async Task ReturnsOkGivenValidTodo(string controllerName)
        {
            var todo = CreateValidTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/{controllerName}/1", jsonContent);

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();


        }

        private Todo CreateValidTodo()
        {
            return new Todo() { Id = 1, Name = "Buy Coce", IsComplete = true };
        }

        private Todo CreateMissingTodo()
        {
            return new Todo() { Id = 0, Name = "Buy Soda", IsComplete = false };
        }

        private Todo CreateEmptyNameTodo()
        {
            return new Todo() { Id = 1, Name = "", IsComplete = false };
        }

        private Todo CreateTooLongNameTodo()
        {
            return new Todo() { Id = 1, Name = "1111111111111111111111111111111111111111111111", IsComplete = false };
        }
    }
}
