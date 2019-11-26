using Newtonsoft.Json;
using Reusable;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReusableWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class BaseController<Entity> : ReadOnlyBaseController<Entity> where Entity : BaseEntity
    {
        protected new ILogic<Entity> logic;
        public BaseController(ILogic<Entity> logic) : base(logic)
        {
            this.logic = logic;
        }

        // POST: api/Base
        [HttpPost, Route("")]
        virtual public CommonResponse Post([FromBody]string value)
        {
            CommonResponse response = new CommonResponse();
            Entity entity;

            try
            {
                entity = JsonConvert.DeserializeObject<Entity>(value);

                return logic.AddTransaction(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        // POST: api/Base
        [HttpPost, Route("AddToParent/{type}/{parentId}")]
        virtual public CommonResponse AddToParent(string type, int parentId, [FromBody]string value)
        {
            CommonResponse response = new CommonResponse();

            Entity entity;
            try
            {
                Type parentType = Type.GetType("BusinessSpecificLogic.EF." + type + ", BusinessSpecificLogic", true);
                entity = JsonConvert.DeserializeObject<Entity>(value);

                MethodInfo method = logic.GetType().GetMethod("AddToParent");
                MethodInfo generic = method.MakeGenericMethod(parentType);
                response = (CommonResponse)generic.Invoke(logic, new object[] { parentId, entity });
                return response;
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        // POST: api/Base
        [HttpPost, Route("SetProperty/{sProperty}/{newValue}")]
        virtual public CommonResponse SetProperty(string sProperty, string newValue, [FromBody]string value)
        {
            CommonResponse response = new CommonResponse();

            Entity entity;
            try
            {
                entity = JsonConvert.DeserializeObject<Entity>(value);
                return logic.SetPropertyValue(entity, sProperty, newValue);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        // PUT: api/Base/5
        [HttpPut, Route("")]
        virtual public CommonResponse Put(int id, [FromBody]string value)
        {
            CommonResponse response = new CommonResponse();
            Entity entity;

            try
            {
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //entity = jsonSerializer.Deserialize<Entity>(value);
                entity = JsonConvert.DeserializeObject<Entity>(value);

                return logic.UpdateTransaction(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        // DELETE: api/Base/
        [HttpDelete, Route("")]
        virtual public CommonResponse Delete(int id)
        {
            return logic.Remove(id);
        }

        [HttpPost, Route("RemoveEntity")]
        virtual public CommonResponse Delete([FromBody]string value)
        {
            CommonResponse response = new CommonResponse();
            Entity entity;

            try
            {
                entity = JsonConvert.DeserializeObject<Entity>(value);
                return logic.Remove(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        [HttpGet, Route("GetAvailableForEntity/{sEntityType}/{id}")]
        virtual public CommonResponse GetAvailableForEntity(string sEntityType, int id)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                Type forEntityType = Type.GetType("BusinessSpecificLogic.EF." + sEntityType + ", BusinessSpecificLogic", true);

                MethodInfo method = logic.GetType().GetMethod("GetAvailableFor");
                MethodInfo generic = method.MakeGenericMethod(forEntityType);
                response = (CommonResponse)generic.Invoke(logic, new object[] { id });
                return response;
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        [HttpPost, Route("RemoveFromParent/{type}/{parentId}")]
        virtual public CommonResponse RemoveFromParent(string type, int parentId, [FromBody]string value)
        {
            CommonResponse response = new CommonResponse();

            Entity entity;
            try
            {
                Type parentType = Type.GetType("BusinessSpecificLogic.EF." + type + ", BusinessSpecificLogic", true);
                entity = JsonConvert.DeserializeObject<Entity>(value);

                MethodInfo method = logic.GetType().GetMethod("RemoveFromParent");
                MethodInfo generic = method.MakeGenericMethod(parentType);
                response = (CommonResponse)generic.Invoke(logic, new object[] { parentId, entity });
                return response;
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.Message, e);
            }
        }
    }
}