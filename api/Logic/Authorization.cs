using api.Data;
using api.Exceptions;
using api.Interfaces;
using api.Models;
using api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Logic
{
    public class Authorization : IAuthorization
    {
        private readonly IConfiguration _conf;
        private readonly DataContext _db;

        public Authorization(IConfiguration conf, DataContext db)
        {
            _conf = conf;
            _db = db;
        }
        public string CreateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_conf["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claimsForToken = new List<Claim>
        {
            new("id", account.Id.ToString()),
            new("accName", account.Username),
            new("accEmail", account.Email)
        };

            var jwtSecurityToken = new JwtSecurityToken(
                _conf["Authentication:Issuer"],
                _conf["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public string Login(LoginModel model)
        {
            var acc = _db.Accounts.FirstOrDefault(e =>
                e.Username.ToLower() == model.Login.ToLower()
                || e.Email.ToLower() == model.Login.ToLower());
            if (acc == null)
                throw new NotFoundException("Account doesn't exists");
            if (!SecurePasswordHasher.Verify(model.Password, acc.Password))
                throw new BadRequestException("Invalid password");

            var token = CreateToken(acc);
            return token;
        }

        public Register Register([FromBody] Register model)
        {
            if (_db.Accounts.Any(e => e.Username == model.Username))
                throw new ConflictException("Account name already exists");

            if (_db.Accounts.Any(e => e.Email == model.Email))
                throw new ConflictException("This email was already used");
            model.Password = SecurePasswordHasher.Hash(model.Password);
            return model;
        }
    }
}
