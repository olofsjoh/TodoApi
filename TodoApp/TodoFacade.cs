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
    class TodoFacade
    {
        private HttpClient _client = new HttpClient();

        public TodoFacade(string baseAddress)
        {
            if (baseAddress == null)
                throw new ArgumentException("baseAddress is null");

            _client.BaseAddress = new Uri(baseAddress);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Test()
        {
            var response = _client.GetAsync("api/todo").Result;

            var jsonString =  response.Content.ReadAsStringAsync();

            var x = JsonConvert.DeserializeObject<List<Todo>>(jsonString.Result);
        }

        public async Task<List<Todo>> GetTodoAsync(CancellationToken cancellationToken)
        {
           // await Task.Delay(3000);

            cancellationToken.ThrowIfCancellationRequested();

            var response = await _client.GetAsync("api/todo", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Todo>>(jsonString);
            }
            return new List<Todo>();
        }
    }
}
