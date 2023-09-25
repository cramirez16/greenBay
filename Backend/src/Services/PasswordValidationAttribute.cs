using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Services
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string? password = value as string;

            if (string.IsNullOrEmpty(password))
                return false;

            // Check password rules
            bool hasLetter = System.Text.RegularExpressions.Regex.IsMatch(password, "[a-zA-Z]+");
            bool hasNumber = System.Text.RegularExpressions.Regex.IsMatch(password, "\\d+");
            bool hasLetterCase = System.Text.RegularExpressions.Regex.IsMatch(password, "(?=.*[a-z])(?=.*[A-Z])");
            bool hasLength = password.Length >= 6 && password.Length <= 255;

            return hasLetter && hasNumber && hasLetterCase && hasLength;
        }
    }
}