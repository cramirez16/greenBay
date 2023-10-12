using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Services.IServices
{
    public interface IHassingService
    {
        public string CreateHash(string text);
        public bool VerifyHash(string plainText, string hassedText);
    }
}