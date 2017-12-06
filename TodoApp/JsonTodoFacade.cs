using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace TodoApp
{
    class JsonTodoFacade : TodoFacade
    {
        public JsonTodoFacade(string baseAddress) :base(baseAddress)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected override List<Todo> Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<List<Todo>>(content);
        }
    }
}
