using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Dtos
{
    public class EndpointResponseDto
    {
        public bool bidAmountInvalid { get; set; }
        public bool userNotFound { get; set; }
        public bool notSallabel { get; set; }
        public bool itemNotFound { get; set; }
        public bool notEnoughtMoneyToBid { get; set; }
        public bool bidSuccess { get; set; }
        public bool bidLow { get; set; }
        public bool itemSold { get; set; }
        public bool whatHappened { get; set; }
    }
}