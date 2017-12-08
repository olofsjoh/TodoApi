using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;

namespace IntegrationTests.TodoController
{
    public class Post : TodoControllerTestBase
    {
        private readonly HttpClient _client;

        public Post()
        {
            _client = base.GetClient();
        }

        [Fact]
        public async Task ReturnsOkGivenValidTodo()
        {
            var todo = CreateValidTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/todo", jsonContent);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var createdTodo = JsonConvert.DeserializeObject<Todo>(stringResponse);

            Assert.Equal("Buy beer", createdTodo.Name);
            Assert.False(createdTodo.IsComplete);
            Assert.True(createdTodo.Id > 0);
        }

        [Fact]
        public async Task ReturnsNotOkGivenNullTodo()
        {
            var todo = CreateNullTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/todo", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task ReturnsNotOkGivenEmptyNameTodo()
        {
            var todo = CreateInvalidTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/todo", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", stringResponse);
            Assert.Contains("The Name field is required.", stringResponse);
        }

        [Fact]
        public async Task ReturnsNotOkGivenTooLongNameTodo()
        {
            var todo = CreateNameTooLongTodo();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/todo", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", stringResponse);
            Assert.Contains("The field Name must be a string or array type with a maximum length of '20'.", stringResponse);
        }

        private Todo CreateValidTodo()
        {
            return new Todo() { Id = 4, Name = "Buy beer", IsComplete = false };
        }

        private Todo CreateNullTodo()
        {
            return null;
        }

        private Todo CreateInvalidTodo()
        {
            return new Todo() { Id = 4, Name = "", IsComplete = false };
        }

        private Todo CreateNameTooLongTodo()
        {
            return new Todo() { Id = 4, Name = "Buy beerToLoooooooooooooooooooooooooooooooooooooooooooooog", IsComplete = false };
        }
    }
}
