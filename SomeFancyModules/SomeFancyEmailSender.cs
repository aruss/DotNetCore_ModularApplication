namespace SomeFancyModules
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ModularApplication.Shared;

    public class SomeFancyEmailSender : IEmailSender
    {
        private readonly ILogger<SomeFancyEmailSender> _logger;

        public SomeFancyEmailSender(ILogger<SomeFancyEmailSender> logger)
        {
            this._logger = logger;
        }

        public async Task SendEmailAsync(
            string email, 
            string subject,
            string message)
        {
            // Call some fancy API here 
            this._logger
                .LogInformation("SomeFancyEmailSender.SendEmailAsync...");
        }
    }
}
