using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IAuthorization
    {
        public RegisterModel Register([FromBody] RegisterModel model);
        public string Login([FromBody] LoginModel model);
        public string CreateToken(Account account);
    }
}
