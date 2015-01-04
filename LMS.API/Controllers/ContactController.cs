using System.Net;
using System.Web.Http;
using System.Net.Mail;

namespace LMS.API.Controllers
{
    using System.Linq;

    using LMS.API.Models;

    public class ContactController : ApiBaseController
    {
        // POST: api/Users
        [HttpPost]
        public IHttpActionResult SendEmail([FromBody]Email email)
        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Credentials = new NetworkCredential("userName", "password");
            smtpServer.Port = 587; // Gmail works on this port

            mail.From = new MailAddress("myemail@gmail.com");
            mail.To.Add(string.Join(",", email.Recipients));
            mail.Subject = email.Subject;
            mail.Body = email.Body;

            smtpServer.Send(mail);

            return Ok("message sent");
        }
    }
}
