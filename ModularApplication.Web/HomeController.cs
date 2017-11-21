namespace ModularApplication.Web
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ModularApplication.Shared;

    public class HomeController : Controller 
    {
        private readonly IEmailSender _emailSender; 

        public HomeController(IEmailSender emailSender)
        {
            this._emailSender = emailSender; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await this._emailSender
                .SendEmailAsync("jim@panse.com", "Hey!", "..."); 

            return this.Content("Hallo Welt (MVC)"); 
        }
    }
}
