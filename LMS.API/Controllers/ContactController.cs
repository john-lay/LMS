using System.Net;
using System.Web.Http;
using System.Net.Mail;

namespace LMS.API.Controllers
{
    using System;
    using System.Linq;

    using LMS.API.Models;

    public class ContactController : ApiBaseController
    {
        // POST: api/Users
        [HttpPost]
        public IHttpActionResult SendEmail([FromBody]Email email)
        {
            using (var client = new SmtpClient())
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(string.Join(",", email.Recipients));
                mail.Subject = email.Subject;
                mail.Body = email.Body;

                try
                {
                    client.Send(mail);
                    return Ok("message sent");
                }
                catch (Exception ex)
                {
                    // should be logging the exception here...
                    throw new Exception("There was an error sending your message, please try again.", ex);
                }
            }
        }
    }
}
