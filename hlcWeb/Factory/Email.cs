using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace hlcWeb.Factory
{
	public static class Email
	{
	    private static readonly string _apiKey;
	    private static string _status;

        static Email()
	    {
	        _apiKey = "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";
	        _status = "";
	    }

	    public static bool SendForgotPasswordEmail()
	    {
            SendEmail("jeff.holman@yahoo.com", "Jeff Holman", "Test Email - Logon"); //.Wait();
	        return true;
	    }

	    private static async Task SendEmail(string toEmailAddress, string toEmailName, string subject)
	    {
	        //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
	        var client = new SendGridClient(_apiKey);
	        var msg = new SendGridMessage()
	        {
	            From = new EmailAddress("noreply@hlcweb.com", "HLC Site"),
	            Subject = subject,
	            PlainTextContent = "This is a test from the HLC site.",
	            HtmlContent = "<strong>This is a test from the HLC site.</strong>"
	        };
	        msg.AddTo(new EmailAddress(toEmailAddress, toEmailName));
	        var response = await client.SendEmailAsync(msg);
	        _status = response.StatusCode.ToString();
	    }
    }
}