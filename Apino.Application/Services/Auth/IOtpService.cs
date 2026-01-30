using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Auth
{
    public interface IOtpService
    {
        Task SendAsync(string mobile);
        Task VerifyAsync(string mobile, string code);   // 👈 اینجا نام VerifyAsync هست
    }
}
