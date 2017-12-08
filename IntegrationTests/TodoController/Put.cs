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

        [Fact]
        public async Task ReturnsNotFoundForId0()
        {
            var todo = CreateMissingTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/todo", jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestGivenNoName()
        {
            var todo = CreateEmptyNameTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/todo/1", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestGivenTooLongName()
        {
            var todo = CreateTooLongNameTodo();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/todo/1", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", stringResponse);
            Assert.Contains("The field Name must be a string or array type with a maximum length of '20'.", stringResponse);
        }

        [Fact]
        public async Task ReturnsOkGivenValidTodo()
        {
            var todo = CreateValidTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/todo/1", jsonContent);

            response.EnsureSuccessStatusCode();
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
