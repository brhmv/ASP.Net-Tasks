
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using TodoWebService.Data;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Todo
{
    public class DeadlineService(TodoDbContext context) : IHostedService
    {
        Timer? _timer;

        readonly TodoDbContext _context = context;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async List<TodoItem> SelectTodosViaDeadeline()
        {
            var todoList = await _context.TodoItems.ToListAsync();
            var ad = await _context.users


            var b = DateTime.Now.Day;

            foreach (var todo in todoList)
            {
                if (todo.Deadline.Day == b + 1)
                {

                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Background service has stopped!");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _timer = null;
        }
    }
}