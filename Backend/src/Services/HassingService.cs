using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Services.IServices;

namespace src.Services
{
    public class HassingService : IHassingService
    {
        public string CreateHash(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public bool VerifyHash(string plainText, string hassedText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hassedText);
        }
    }
}