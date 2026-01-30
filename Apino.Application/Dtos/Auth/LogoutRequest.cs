using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Auth
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
    }
}
