using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.Services
{
    public interface IEmailSender
    {
        
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
        Task<bool> SendResetPasswordEmail(string toEmail, string fullName, string callBackUrl);        
        Task<bool> SendWelcomeEmail(string toEmail, string fullName);
        

    }
}
