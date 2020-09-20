using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IConfiguration _config { get; }

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //retrieve access token
            var serverClient = _httpClientFactory.CreateClient();

            var identiyServerUrl = _config.GetSection("Servers").GetSection("IdentityServer").Value;

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(identiyServerUrl);

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "ApiOne",
                });

            //retrieve secret data
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var apiOneUrl = _config.GetSection("Servers").GetSection("ApiOne").Value;

            var response = await apiClient.GetAsync($"{apiOneUrl}/secret");

            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content,
            });
        }
    }
}
