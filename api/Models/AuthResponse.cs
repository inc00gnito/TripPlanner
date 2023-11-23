using api.Models.DTOs;

namespace api.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public AccountDto Account { get; set; }
    }
}
