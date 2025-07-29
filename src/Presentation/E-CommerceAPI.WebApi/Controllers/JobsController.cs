using Hangfire;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobsController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpPost("fire-and-forget")]
        public IActionResult FireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => Console.WriteLine("🔥 Fire-and-forget job executed!"));
            return Ok("Fire-and-forget job created");
        }

        [HttpPost("delayed")]
        public IActionResult DelayedJob()
        {
            _backgroundJobClient.Schedule(() => Console.WriteLine("⏰ Delayed job executed after 30 seconds"), TimeSpan.FromSeconds(30));
            return Ok("Delayed job scheduled");
        }

        [HttpPost("recurring")]
        public IActionResult RecurringJob()
        {
            _recurringJobManager.AddOrUpdate("my-recurring-job", () => Console.WriteLine("🔁 Recurring job executed!"), Cron.Minutely);
            return Ok("Recurring job created to run every minute");
        }
    }
}

