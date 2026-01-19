using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("TiposDeClima")]    
        public List<string> GetTiposDeClima()
        {
            List<string> climas = new List<string>()
            {
                "Tropical","Seco","Templado", "Frio", "Polar"
            };

            return climas;
        }
        
        [HttpGet("TemperaturaActual")]
        [Authorize]
        public int GetTemperaturaActual()
        {
            int temperatura = Random.Shared.Next(-20, 55);

            return temperatura;
        }

        [HttpGet("PronosticoDelTiempo")]
        [Authorize(Roles = "Admin, SuperAdmin")]
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
    }
}
