using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TodoApp
{
    abstract class Facade
    {
        protected HttpClient _client = new HttpClient();

        public Facade(string baseAddress)
        {
            if (baseAddress == null)
                throw new ArgumentException("baseAddress is null");

            _client.BaseAddress = new Uri(baseAddress);
           
        }

        public async Task<List<Todo>> GetTodoAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var response = await _client.GetAsync("api/todo", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return Deserialize(content); 
            }
            return new List<Todo>();
        }

        protected abstract List<Todo> Deserialize(string content);
    }
}
