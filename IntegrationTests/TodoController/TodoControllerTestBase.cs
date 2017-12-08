using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TodoApi;

namespace IntegrationTests.TodoController
{
    public class TodoControllerTestBase
    {
        protected HttpClient GetClient()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseEnvironment("Testing");
            var server = new TestServer(builder);
            var client = server.CreateClient();

            // client always expects json results
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
