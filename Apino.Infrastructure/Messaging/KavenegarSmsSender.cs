using Apino.Application.Services.Auth;
using Kavenegar;
using Microsoft.Extensions.Configuration;

namespace Apino.Infrastructure.Messaging
{
    public class KavenegarSmsSender : ISmsSender
    {
        private readonly SmsSettings _settings;

        public KavenegarSmsSender(IConfiguration config)
        {
            _settings = config.GetSection("SmsSettings").Get<SmsSettings>()!;
        }

        public Task SendAsync(string mobile, string message)
        {
            try
            {
                var api = new KavenegarApi(_settings.ApiKey);
                var result = api.Send(_settings.SenderLine, mobile, message);

                // فقط پیام وضعیت ارسال را چاپ کن
                Console.WriteLine($"Status: {result.Status}, MessageId: {result.Messageid}");

                return Task.CompletedTask;
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                Console.WriteLine("API Error: " + ex.Message);
                throw;
            }
            catch (Kavenegar.Exceptions.HttpException ex)
            {
                Console.WriteLine("HTTP Error: " + ex.Message);
                throw;
            }
        }

  

 
    }
}
