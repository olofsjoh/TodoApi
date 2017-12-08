﻿using Newtonsoft.Json;
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

        [Fact]
        public async Task ReturnsNotFoundForId0()
        {
            var response = await _client.DeleteAsync($"/api/todo/0");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsOkGivenValidTodo()
        {
            var response = await _client.GetAsync($"/api/todo");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(stringResponse).ToList();
            var idToDelete = todos.FirstOrDefault().Id;
            var response2 = await _client.DeleteAsync($"/api/todo/{idToDelete}");
            response2.EnsureSuccessStatusCode();
        }
    }
}
