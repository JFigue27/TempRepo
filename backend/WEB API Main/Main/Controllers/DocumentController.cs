using Newtonsoft.Json;
using Reusable;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReusableWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class DocumentController<Document> : BaseController<Document> where Document : BaseDocument
    {
        protected new IDocumentLogic<Document> logic;
        public DocumentController(IDocumentLogic<Document> logic) : base(logic)
        {
            this.logic = logic;
        }

        [HttpPost, Route("Finalize")]
        virtual public CommonResponse Finalize([FromBody]string value)
        {
            CommonResponse response = new CommonResponse();
            Document entity;

            try
            {
                entity = JsonConvert.DeserializeObject<Document>(value);
                return logic.Finalize(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        [HttpPost, Route("Unfinalize")]
        virtual public CommonResponse Unfinalize([FromBody]string value)
        {
            CommonResponse response = new CommonResponse();
            Document entity;

            try
            {
                entity = JsonConvert.DeserializeObject<Document>(value);
                return logic.Unfinalize(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }
    }
}