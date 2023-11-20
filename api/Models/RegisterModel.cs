﻿using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Account name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Account name should be between 3 and 16 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Account name can only have numbers and letters")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password name is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
            ErrorMessage =
                "Password requires minimum eight characters, at least one letter, one number and one special character")]
        public string Password { get; set; }

        public RegisterModel(string accountName, string email, string password)
        {
            AccountName = accountName;
            Email = email;
            Password = password;
        }
    }
}
