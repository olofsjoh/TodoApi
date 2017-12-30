using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.Infrastructure.Data;
using TodoApi.Interfaces;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Todos"));



            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("text/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
                options.OutputFormatters.Add(new TodoCsvFormatter());
            })
            .AddXmlSerializerFormatters();

            services.AddScoped<ITodoRepository, TodoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            this.Configure(app, env);

            PopulateTestData(app);
        }

        private void PopulateTestData(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<AppDbContext>();
            var todos = dbContext.Todos;
            foreach (var todo in todos)
            {
                dbContext.Remove(todo);
            }
            dbContext.SaveChanges();

            dbContext.Todos.Add(new Todo { Id = 1, Name = "Start the dishwasher", IsComplete = true });
            dbContext.Todos.Add(new Todo { Id = 2, Name = "Buy milk", IsComplete = false });
            dbContext.Todos.Add(new Todo { Id = 3, Name = "Pay bills", IsComplete = true });

            dbContext.SaveChanges();
        }
    }
}
