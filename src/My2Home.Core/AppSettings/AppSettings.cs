using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Core.AppSettings
{
    public interface IAppSettings
    {

    }
    public class AppSettings : IAppSettings
    {
        public BaseUrls BaseUrls { get; set; }
        public DataBaseSettings DataBaseSettings { get; set; }
        public AppClient AppClient { get; set; }
        public JWTSettings JWTSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public IdentityAuthConfig IdentityAuthConfig {get;set;}
        public SendGridSettings SendGridSettings { get; set; }
        public MailJet MailJet { get; set; }
    }


    public class BaseUrls
    {
        public string Api { get; set; }
        public string Auth { get; set; }
        public string Web { get; set; }
    }

    public class DataBaseSettings
    {
        public string ConnectionString { get; set; }
        //public string TestConnectionString { get; set; }
        //public string ProductionConnectionString { get; set; }
    }

    public class AppClient
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class JWTSettings
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(1);
        public string TokenType { get; } = "bearer";
    }

    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public string SecondayDomain { get; set; }

        public int SecondaryPort { get; set; }

        public string UsernameEmail { get; set; }

        public string UsernamePassword { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public string CcEmail { get; set; }
    }

    public class SendGridSettings
    {
        public string SendGridApiKey { get; set; }
        public string SendGridUserName { get; set; }
        public string SendGridPassword { get; set; }
        public string SendGridHost { get; set; }
        public int SendGridPort { get; set; }

    }

    public class MailJet
    {
        public string MailJetApiKeyPublic { get; set; }
        public string MailJetApiKeyPrivate { get; set; }
        public string MailJetFromEmail { get; set; }
        public string MailJetFromName { get; set; }
    }

}
