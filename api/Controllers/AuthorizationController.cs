using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using AutoMapper;
using api.Models.DTOs;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorization _authorization;
        private readonly IAccount _account;
        private readonly IMapper _mapper;

        public AuthorizationController(IAuthorization authorization, IAccount account, IMapper mapper)
        {
            _authorization = authorization;
            _account = account;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<AuthResponse> Register([FromBody] Register model)
        {
            var registered = _authorization.Register(model);
            var account = _account.CreateAccount(registered);
            var token = _authorization.CreateToken(account);

            var authResponse = new AuthResponse()
            {
                Username = _mapper.Map<AccountDto>(account).Username,
                Token = token
            };

            return Ok(authResponse);
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult<AuthResponse> Login([FromBody] LoginModel login)
        {
            var loggedToken = _authorization.Login(login);

            AccountDto account = new AccountDto()
            {
                Username = login.Login
            };

            var authResponse = new AuthResponse()
            {
                Username = account.Username,
                Token = loggedToken
            };
            return Ok(authResponse);

        }
    }
}
