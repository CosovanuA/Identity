using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Domain.DTO
{
    public class LoginUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
