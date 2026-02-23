using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using My2Home.Core.AppSettings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace My2Home.Web.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<AppSettings> _appSettings;
        private IHostingEnvironment _env;
        private MailjetClient client;

        public EmailSender(IOptions<AppSettings> appSettings, IHostingEnvironment env)
        {
            _appSettings = appSettings;
            _env = env;

            client = new MailjetClient(_appSettings.Value.MailJet.MailJetApiKeyPublic, _appSettings.Value.MailJet.MailJetApiKeyPrivate);

            client.Version = ApiVersion.V3_1;

        }
        public Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult<bool>(true);
        }



        public async Task<bool> SendResetPasswordEmail(string toEmail, string fullName, string callBackUrl)
        {
            var pathToFile = $"{Directory.GetCurrentDirectory()}/wwwroot/Templates/EmailTemplate/ResetPassword.html";
            var builder = new StringBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.Append(SourceReader.ReadToEnd());
            }
            builder.Replace("{{0}}", fullName);
            builder.Replace("{{1}}", callBackUrl);
            var plainTextContent = Regex.Replace(builder.ToString(), "<[^>]*>", "");
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, builder.ToString());           
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray
            {
                new JObject {
                 {"From", new JObject {
                  {"Email",  _appSettings.Value.MailJet.MailJetFromEmail},
                  {"Name", _appSettings.Value.MailJet.MailJetFromName}
                  }},

                 { "To", new JArray {
                  new JObject {
                   {"Email", toEmail},
                   {"Name", fullName }
                   }
                  }},
                 {"Subject", "Forgot Password!"},
                 {"TextPart", plainTextContent},
                 {"HTMLPart", builder.ToString()}
                 }

            });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public  async Task<bool> SendWelcomeEmail(string toEmail, string fullName)
        {
            //var pathToFile = $"{_env.ContentRootPath}/Views/Email/Index.cshtml";
            var pathToFile = $"{Directory.GetCurrentDirectory()}/wwwroot/Templates/EmailTemplate/welcome.html";
            var builder = new StringBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.Append(SourceReader.ReadToEnd());
            }
            builder.Replace("{{0}}", fullName);

            var plainTextContent = Regex.Replace(builder.ToString(), "<[^>]*>", "");

            var pathToFileAttachment = $"{Directory.GetCurrentDirectory()}/wwwroot/Templates/doc/welcome_i_Hostel.pdf";

            var bytes = File.ReadAllBytes(pathToFileAttachment);
            var file = Convert.ToBase64String(bytes);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray
            {
                new JObject {
                 {"From", new JObject {
                  {"Email",  _appSettings.Value.MailJet.MailJetFromEmail},
                  {"Name", _appSettings.Value.MailJet.MailJetFromName}
                  }},

                 { "To", new JArray {
                  new JObject {
                   {"Email", toEmail},
                   {"Name", fullName}
                   }
                  }},
                 {"Subject", "Welcome!"},
                 {"TextPart", plainTextContent},
                 {"HTMLPart", builder.ToString()},
                 {"Attachments", new JArray {
                     new JObject {
                         {"ContentType", "application/pdf"},
                         {"Filename", "welcome_i_Hostel.pdf"},
                         {"Base64Content", file}

                     }
                 } }
                 }

            });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

            //return Task.FromResult<bool>(true);
        }
    }
}
