using Microsoft.AspNetCore.Mvc;

namespace SampleApi2.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return Ok(User.Identity?.Name);
        }
    }
}
