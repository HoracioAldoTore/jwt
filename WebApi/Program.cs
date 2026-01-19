
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    #region Expiration
                    ValidateLifetime = true,       //Expiration - Ensure lifetime validation is active
                    RequireExpirationTime = true,   //Expiration - Ensures the 'exp' claim is present
                    ClockSkew = TimeSpan.Zero,      //Expiration - Eliminates the 5-minute tolerance
                    #endregion Expiration
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your_issuer",
                    ValidAudience = "your_audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Tu_Clave_Secreta_Super_Segura_De_Al_Menos_32_Caracteres_aaaa"))
                };
            });

            // In Program.cs (or Startup.ConfigureServices)
            // Add CORS services.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOriginsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.Run();
        }
    }
}
