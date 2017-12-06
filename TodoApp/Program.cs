using System;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var facade = new TodoFacade("http://localhost:58071");

            var task = facade.GetTodoAsync(CancellationToken.None);

            task.ContinueWith(x =>
                {
                    var todos = x.Result;
                    foreach(var todo in todos)
                    {
                        Console.WriteLine(todo.Name);
                    }

                    Environment.Exit(0);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.ReadLine();
        }
    }
}
