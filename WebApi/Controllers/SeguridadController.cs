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
    public class SeguridadController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login(Credencial credencial)
        {
            string token = "";

            if (credencial.Usuario == "Sancho" && credencial.Clave == "123")
            {
                token = GenerarToken();                
            }

            return Ok(token);
        }

        private string GenerarToken()
        {
            // 1. Definir la clave secreta (debe guardarse en variables de entorno o Secret Manager)
            var secretKey = "Tu_Clave_Secreta_Super_Segura_De_Al_Menos_32_Caracteres_aaaa";
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);

            // 2. Configurar los Claims (datos del usuario que viajan en el token)
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, "sancho"));
            claims.AddClaim(new Claim(ClaimTypes.Email, "sancho@ejemplo.com"));
            claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            claims.AddClaim(new Claim("tipo_de_barriga", "grande")); //Afirmarcion personalizada

            // 3. Crear los parámetros de firma (Algoritmo HMAC SHA256)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddSeconds(10), // Duración del token               
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // 4. Crear el token y convertirlo a string
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false; //Quita los Claims: exp, nbf, iat
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }
    }
}
