using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Auth.Login
{
    public class LoginQueryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
