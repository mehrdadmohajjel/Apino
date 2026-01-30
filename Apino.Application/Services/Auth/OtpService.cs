using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Auth
{
    public class OtpService : IOtpService
    {
        private readonly IReadDbContext _db;
        private readonly ISmsSender _smsSender;

        public OtpService(
            IReadDbContext db,
            ISmsSender smsSender)
        {
            _db = db;
            _smsSender = smsSender;
        }

        public async Task SendAsync(string mobile)
        {
            try
            {
                var code = Random.Shared.Next(10000, 99999).ToString();

                var otp = new OtpCode
                {
                    Mobile = mobile,
                    Code = code,
                    ExpireAt = DateTime.UtcNow.AddMinutes(2),
                    IsUsed = false,
                    CreationDateTime = DateTime.Now
                };

                _db.OtpCodes.Add(otp);
                var x = await _db.SaveChangesAsync();

                //await _smsSender.SendAsync(mobile, $"کد ورود: {code}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // یا Log
                throw;
            }

        }

        public async Task VerifyAsync(string mobile, string code)
        {
            var otp = await _db.OtpCodes
                .Where(x =>
                    x.Mobile == mobile &&
                    x.Code == code &&
                    !x.IsUsed &&
                    x.ExpireAt > DateTime.UtcNow)
                .OrderByDescending(x => x.CreationDateTime)
                .FirstOrDefaultAsync();

            if (otp == null)
                throw new Exception("OTP نامعتبر است");

            otp.IsUsed = true;
            await _db.SaveChangesAsync();
        }
    }
}