namespace ModularApplication.Web
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ModularApplication.Shared;

    public class SmtpEmailSender : IEmailSender
    {
        private readonly ILogger<SmtpEmailSender> _logger; 

        public SmtpEmailSender(ILogger<SmtpEmailSender> logger)
        {
            this._logger = logger; 
        }

        public async Task SendEmailAsync(
            string email, 
            string subject,
            string message)
        {
            // Do SMPT call here ...
            this._logger.LogInformation("SmtpEmailSender.SendEmailAsync...");
        }
    }
}
