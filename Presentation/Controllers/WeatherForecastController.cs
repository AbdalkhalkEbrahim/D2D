using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly D2DContext _context;

       

       
        public WeatherForecastController(ILogger<WeatherForecastController> logger, D2DContext context)
        {
            _logger = logger;

            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpDelete]

        public async Task< IActionResult> Delete(string email)
        {
            if (email!= "abdomedhat762002@gmail.com"&&email!= "abdomedhat200267@gmail.com"&&email!= "abdelrahman.medhat.hassona@gmail.com")
                return BadRequest("you are not medhat");

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user!=null)
            {
                _context.Users.Remove(user);
              await  _context.SaveChangesAsync();
                return Ok($"User with email {email} has been deleted.");
            }
            return NotFound($"User with email {email} not found.");
        }
       
    }
}
