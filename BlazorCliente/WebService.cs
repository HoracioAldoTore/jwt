using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlazorCliente
{
    public static class WebService
    {
        public static async Task<int> TemperaturaActual(string token)
        {
            int temperatura = 0;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Get, "http://localhost:5266/WeatherForecast/TemperaturaActual");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                temperatura = JsonSerializer.Deserialize<int>(responseBody
                    , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                    );
            }
            return temperatura;
        }

        public static async Task<List<string>> TiposDeClima(string token)
        {
            List<string> pronostico = new List<string>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Get, "http://localhost:5266/WeatherForecast/TiposDeClima");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                pronostico = JsonSerializer.Deserialize<List<string>>(responseBody
                    , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                    );
            }
            return pronostico;
        }

        public static async Task<List<WeatherForecast>> PronosticoDelTiempo(string token)
        {
            List<WeatherForecast> pronostico = new List<WeatherForecast>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Get, "http://localhost:5266/WeatherForecast/PronosticoDelTiempo");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                pronostico = JsonSerializer.Deserialize<List<WeatherForecast>>(responseBody
                    , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                    );
            }
            return pronostico;
        }

        public static async Task<string> Login()
        {
            var credencial = new { usuario = "Sancho", clave = "123" };
            string token = ""; //JSON Web Token (JWT)
                               // Serialize the object to a JSON string
            string parametroJSON = JsonSerializer.Serialize(credencial);

            // Create the StringContent with the JSON payload and specify the media type
            using var contentParam = new StringContent(parametroJSON, Encoding.UTF8, "application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Post, "http://localhost:5266/Seguridad/Login");
                request.Content = contentParam;

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                token = await response.Content.ReadAsStringAsync();

            }

            return token;
        }
    }
}
