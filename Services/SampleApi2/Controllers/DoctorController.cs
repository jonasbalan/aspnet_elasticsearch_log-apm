using Microsoft.AspNetCore.Mvc;
using Common.MS.Auth;
using Microsoft.AspNetCore.Authorization;

namespace SampleApi2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {

        private const string api3UrlBase = "https://internal.api.sample-ms.server.com/api3";

        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger)
        {
            _logger = logger;
        }       

       
        [HttpGet]
        public async Task<IActionResult> GetFromApi3([FromServices] IHttpClientFactory httpClientFactory)
        {
            await Task.Delay(100);// simulate doing something
            using var client = httpClientFactory.CreateClient("internalApis");
            client.BaseAddress = new Uri(api3UrlBase);

            var enumerable = client.GetJsonWithAuthorizationAsync<dynamic>(HttpContext, "doctor");

            return Ok(enumerable);
        }
    }
}