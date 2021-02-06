using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WeatherForecast.Helper
{
    public class ApiCaller
    {
        public static async Task<ApiResponse> Get(string url, string authorizationId = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(authorizationId))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", authorizationId);
                }

                var request = await client.GetAsync(url);

                if (request.IsSuccessStatusCode)
                {
                    return new ApiResponse { Response = await request.Content.ReadAsStringAsync() };
                }

                return new ApiResponse { ErrorMessage = request.ReasonPhrase };
            }
        }
    }

    public class ApiResponse
    {
        public bool Successful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }
}
