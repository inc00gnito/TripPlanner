using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IAuthorization
    {
        public Register Register([FromBody] Register model);
        public string Login([FromBody] LoginModel model);
        public string CreateToken(Account account);
    }
}
