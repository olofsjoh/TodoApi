using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Filters
{
    public class ValidateTodoExistsAttribute : TypeFilterAttribute
    {
        public ValidateTodoExistsAttribute():base(typeof(ValidateTodoExistsFilterImpl))
        {
        }

        private class ValidateTodoExistsFilterImpl : IAsyncActionFilter
        {
            private readonly ITodoRepository _todoRepository;

            public ValidateTodoExistsFilterImpl(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if ((await _todoRepository.ListAsync()).All(a => a.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(new ApiResponse(404, $"Todo not found with id {id.Value}"));
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
