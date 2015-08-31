// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactController.cs" company="">
//   
// </copyright>
// <summary>
//   The contact controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LMS.API.Models;

    /// <summary>
    /// The contact controller.
    /// </summary>
    public class ContactController : ApiBaseController
    {
        /// <summary>
        /// The send email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [HttpPost]
        public async Task<IHttpActionResult> SendEmail([FromBody] Email email)
        {
            using (var client = new SmtpClient())
            {
                var mail = new MailMessage();
                mail.To.Add(string.Join(",", email.Recipients));
                mail.Subject = email.Subject;
                mail.Body = email.Body;

                try
                {
                    client.Send(mail);
                    return this.Ok("message sent");
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