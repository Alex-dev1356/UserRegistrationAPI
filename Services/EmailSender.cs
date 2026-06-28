using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthECAPI.Services
{
    public class EmailSender : IEmailSender<AppUser>
    {
        public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
        {
            // TODO: Integrate your SMTP client or API (SendGrid/Mailkit) here
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
        {
            // TODO: Integrate your email client here
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
        {
            // TODO: Integrate your email client here
            return Task.CompletedTask;
        }
    }
}
