using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace CoupnsKE.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(subject, htmlMessage, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.eu.mailgun.net/v3/mail.couponske.codes");
            client.Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("MailgunAPI"));
            var request = new RestRequest();
            request.AddParameter("domain", "mail.couponske.codes", ParameterType.UrlSegment);
            request.Resource = "https://api.eu.mailgun.net/v3/mail.couponske.codes/messages";
            request.AddParameter("from", "Steve from CouponsKE <no-reply@couponske.codes>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;

            return client.ExecuteAsync(request);
        }
    }
}
