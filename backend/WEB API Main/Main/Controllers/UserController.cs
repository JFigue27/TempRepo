using BusinessSpecificLogic.Logic;
using Newtonsoft.Json;
using Reusable;
using Reusable.Email;
using System;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseController<User>
    {
        public UserController(IUserLogic logic) : base(logic) { }

        [HttpGet, Route("getByRole/{role}")]
        public CommonResponse getByRole(string role)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                response.Success(logic.GetListWhere(u => u.Value, u => u.Role == role));
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }
            return response;
        }

        [HttpPost, Route("SendTestEmail")]
        public CommonResponse SendTestEmail([FromBody] string user)
        {
            CommonResponse response = new CommonResponse();
            User entity;

            try
            {
                entity = JsonConvert.DeserializeObject<User>(user);

                EmailService email = new EmailService("secure.emailsrvr.com", 587);
                email.EmailAddress = entity.Email;
                email.Password = entity.EmailPassword;

                email.From = entity.Email;
                email.To.Add(entity.Email);
                email.Subject = "Test Email from Training System.";
                email.Body = "This is a Test Email sent from Training System.";

                email.SendMail();

                return response.Success();
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

    }
}