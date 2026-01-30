using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Auth
{
    public interface ISmsSender
    {
        Task SendAsync(string mobile, string message);

    }
}
