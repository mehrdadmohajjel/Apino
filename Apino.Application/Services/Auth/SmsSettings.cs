using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Auth
{
    public class SmsSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string SenderLine { get; set; } = string.Empty;
    }
}
