using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return User.Identity?.Name;
        }
    }
}
