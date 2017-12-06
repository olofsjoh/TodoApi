using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi
{
    public class TodoCsvFormatter : TextOutputFormatter
    {
        public TodoCsvFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof(IEnumerable<Todo>) || type == typeof(Todo);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<Todo>)
            {
                foreach (var todoItem in (IEnumerable<Todo>)context.Object)
                {
                    FormatCsv(buffer, todoItem);
                }
            }
            else
            {
                FormatCsv(buffer, (Todo)context.Object);
            }
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                return writer.WriteAsync(buffer.ToString());
            }
        }

        private static void FormatCsv(StringBuilder buffer, Todo item)
        {
            buffer.AppendLine($"{item.Id},\"{item.Name}\",{item.IsComplete}");
        }
    }
}
