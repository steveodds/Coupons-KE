﻿using Microsoft.AspNetCore.Identity.UI.Services;
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
            client.BaseUrl = new Uri("https://api.mailgun.net/v3/sandboxe2699fd52df14274999de029fc67d28e.mailgun.org");
            client.Authenticator = new HttpBasicAuthenticator("api", "***REMOVED***");
            var request = new RestRequest();
            request.AddParameter("domain", "www.couponske.codes", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Steve from CouponsKE <no-reply@couponske.codes>");
            request.AddParameter("to", email);
            request.AddParameter("to", "admin@couponske.codes");
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;

            return client.ExecuteAsync(request);
        }
    }
}
