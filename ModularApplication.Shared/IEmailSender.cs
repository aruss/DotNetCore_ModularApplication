namespace ModularApplication.Shared
{
    using System.Threading.Tasks;

    /// <summary>
    /// Simple example of an email sender. 
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="message">Email content.</param>
        Task SendEmailAsync(string email, string subject, string message); 
    }
}
