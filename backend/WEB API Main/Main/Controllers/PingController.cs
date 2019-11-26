using System.Collections.Generic;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [AllowAnonymous]
    public class PingController : ApiController
    {
        // GET: api/Ping
        public IEnumerable<string> Get()
        {
            return new string[] { "Training", "is", "working!" };
        }

        // GET: api/Ping/5
        public string Get(int id)
        {
            return "Training is working!";
        }

        // POST: api/Ping
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Ping/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Ping/5
        public void Delete(int id)
        {
        }
    }
}
