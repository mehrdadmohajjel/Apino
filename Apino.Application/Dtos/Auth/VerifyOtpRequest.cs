using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Auth
{
    public class VerifyOtpRequest
    {
        public string Mobile { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
