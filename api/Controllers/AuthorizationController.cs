using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Models;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        public AuthorizationController()
        {
            
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<AuthResponse> Register([FromBody] RegisterModel model)
        {



        }
    }
}
