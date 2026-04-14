using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class LoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
