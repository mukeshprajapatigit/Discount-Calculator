using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldCalc.Api.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; } = false;
    }
}
