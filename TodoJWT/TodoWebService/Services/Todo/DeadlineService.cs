using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TodoWebService.Data;

namespace TodoWebService.Services.Todo
{
    public class DeadlineService(TodoDbContext context) : IHostedService
    {
        Timer? _timer;

        private readonly TodoDbContext _context = context;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SendEmails();
            return Task.CompletedTask;
        }

        private void SendEmails()
        {
            List<string>? emails = SelectTodosViaDeadeline();

            if (emails != null && emails.Count != 0)
            {
                using var client = new SmtpClient("smtp.some.com");
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("someamazingemail@email.com", "password");
                client.EnableSsl = true;
                client.Port = 587;

                foreach (var email in emails)
                {
                    var message = new MailMessage(
                        "someamazingemail@email.com",
                        email,
                        "Task Deadline Reminder", 
                        "Your task is due tomorrow. Please complete it on time."
                    );

                    client.Send(message);
                }
            }
        }

        public List<string>? SelectTodosViaDeadeline()
        {
            var todoList = _context.TodoItems.ToList();
            var ad = _context.Users.ToListAsync();

            var emails = new List<string>();

            foreach (var todo in todoList)
            {
                if (todo.Deadline.Day == DateTime.Now.Day + 1)
                {
                    emails.Add(todo.User.Email!);
                }
            }

            return emails.Count == 0 ? null : emails!;
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