using Newtonsoft.Json;
using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReusableWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class ReadOnlyBaseController<Entity> : ApiController where Entity : BaseEntity
    {
        protected IReadOnlyLogic<Entity> logic;

        public ReadOnlyBaseController(IReadOnlyLogic<Entity> logic)
        {
            this.logic = logic;
            this.logic.LoggedUser = new LoggedUser((ClaimsIdentity)User.Identity);
        }

        protected bool isValidJSValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null" || value == "undefined")
            {
                return false;
            }

            return true;
        }

        protected bool isValidQParamNumber(string value)
        {
            int number;
            if (string.IsNullOrWhiteSpace(value) || value == "null" || value == "undefined" || !int.TryParse(value, out number))
            {
                return false;
            }

            return true;
        }

        protected virtual bool isValidParam(string param)
        {
            //reserved and invalid params:
            if (new string[] {
                "perPage",
                "page",
                "filterGeneral",
                "itemsCount",
                "noCache",
                "totalItems",
                "parentKey",
                "parentField",
                "filterUser"
            }.Contains(param))
                return false;

            return true;
        }

        // GET: api/Base
        [HttpGet, Route("")]
        virtual public CommonResponse Get()
        {
            return logic.GetAll();
        }

        // GET: api/Base/5
        [HttpGet, Route("")]
        virtual public CommonResponse Get(int id)
        {
            return logic.GetByID(id);
        }

        // GET: api/Base/5
        [HttpGet, Route("getSingleWhere/{prop}/{q_value}")]
        virtual public CommonResponse GetSingleWhere(string prop, string q_value)
        {
            CommonResponse response = new CommonResponse();
            List<Expression<Func<Entity, bool>>> wheres = new List<Expression<Func<Entity, bool>>>();

            try
            {
                string sPropertyName = prop;//.Substring(6);

                PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                Type tProp = oProp.PropertyType;
                //Nullable properties have to be treated differently, since we 
                //  use their underlying property to set the value in the object
                if (tProp.IsGenericType
                    && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //Get the underlying type property instead of the nullable generic
                    tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                }

                ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                var value = Expression.Constant(Convert.ChangeType(q_value, tProp));

                // let's perform the conversion only if we really need it
                var converted = value.Type != childProperty.Type
                    ? Expression.Convert(value, childProperty.Type)
                    : (Expression)value;

                Expression comparison = Expression.Equal(childProperty, converted);

                Expression<Func<Entity, bool>> lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);

                wheres.Add(lambda);
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }
            return logic.GetSingleWhere(wheres.ToArray());
        }

        [HttpPost, Route("Create")]
        virtual public CommonResponse Create([FromBody]string value)
        {
            Entity entity;

            try
            {
                entity = JsonConvert.DeserializeObject<Entity>(value);
                return logic.CreateInstance(entity);
            }
            catch (Exception e)
            {
                return logic.CreateInstance();
            }
        }

        // POST: api/Base
        [HttpGet, Route("GetSingleByParent/{parentType}/{parentId}")]
        virtual public CommonResponse GetSingleByParent(string parentType, int parentId)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                Type parentClassName = Type.GetType("BusinessSpecificLogic.EF." + parentType + ", BusinessSpecificLogic", true);
                MethodInfo method = logic.GetType().GetMethod("GetSingleByParent");
                MethodInfo generic = method.MakeGenericMethod(parentClassName);
                response = (CommonResponse)generic.Invoke(logic, new object[] { parentId });
                return response;
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        // POST: api/Base
        [HttpGet, Route("GetAllByParent/{parentType}/{parentId}")]
        virtual public CommonResponse GetAllByParent(string parentType, int parentId)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                Type parentClassName = Type.GetType("CMDLogic.EF." + parentType + ", CMDLogic", true);
                MethodInfo method = logic.GetType().GetMethod("GetAllByParent");
                MethodInfo generic = method.MakeGenericMethod(parentClassName);
                response = (CommonResponse)generic.Invoke(logic, new object[] { parentId });
                return response;
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        [HttpGet, Route("GetPage/{perPage}/{page}")]
        virtual public CommonResponse GetPage(int perPage, int page)
        {
            CommonResponse response = new CommonResponse();
            List<Expression<Func<Entity, bool>>> db_wheres = new List<Expression<Func<Entity, bool>>>();
            List<Expression<Func<Entity, bool>>> wheres = new List<Expression<Func<Entity, bool>>>();

            string filterGeneral = HttpContext.Current.Request["filterGeneral"];
            if (!isValidJSValue(filterGeneral))
            {
                filterGeneral = "";
            }

            string filterParentField = HttpContext.Current.Request["parentField"];
            if (!isValidJSValue(filterParentField))
            {
                filterParentField = "";
            }

            string filterParentKey = HttpContext.Current.Request["parentKey"];
            if (!isValidJSValue(filterParentKey))
            {
                filterParentKey = "";
            }

            string filterUser = HttpContext.Current.Request["filterUser"];
            if (!isValidJSValue(filterUser))
            {
                filterUser = "";
            }
            else
            {
                if (typeof(Entity).IsSubclassOf(typeof(BaseDocument)))
                {
                    wheres.Add(e => (e as BaseDocument).InfoTrack.User_AssignedToKey == int.Parse(filterUser));
                }
            }

            try
            {
                if (filterParentKey.Length > 0 && filterParentField.Length > 0)
                {

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, filterParentField);

                    Expression comparison = Expression.Equal(childProperty, Expression.Constant(int.Parse(filterParentKey)));

                    Expression<Func<Entity, bool>> lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);

                    //ConstantExpression idExpression = Expression.Constant(int.Parse(filterParentKey), typeof(int));
                    //BinaryExpression parentFieldEqualsIdExpression = Expression.Equal(parentField, idExpression);
                    //Expression<Func<int, bool>> lambda1 =
                    //    Expression.Lambda<Func<int, bool>>(
                    //        parentFieldEqualsIdExpression,
                    //        new ParameterExpression[] { parentField });

                    //Expression<Func<Entity, bool>> where = entityFiltered =>
                    //        typeof(Entity).GetProperty(filterParentField).GetValue(entityFiltered, null).ToString() == filterParentKey;

                    db_wheres.Add(lambda);
                }

                foreach (var queryParam in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    string queryParamValue = HttpContext.Current.Request.QueryString[queryParam];
                    if (isValidParam(queryParam) && isValidJSValue(queryParamValue))
                    {
                        string sPropertyName = queryParam;//.Substring(6);

                        PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                        if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                        Type tProp = oProp.PropertyType;
                        //Nullable properties have to be treated differently, since we 
                        //  use their underlying property to set the value in the object
                        if (tProp.IsGenericType
                            && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            //Get the underlying type property instead of the nullable generic
                            tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                        }

                        ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                        Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                        var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                        // let's perform the conversion only if we really need it
                        var converted = value.Type != childProperty.Type
                            ? Expression.Convert(value, childProperty.Type)
                            : (Expression)value;

                        Expression<Func<Entity, bool>> lambda;
                        if (tProp == typeof(String))
                        {
                            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                        }
                        else
                        {
                            Expression comparison = Expression.Equal(childProperty, converted);
                            lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                        }

                        db_wheres.Add(lambda);
                    }
                }
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }

            return logic.GetPage(perPage, page, filterGeneral, wheres.ToArray(), orderBy, db_wheres.ToArray());
        }

        protected Expression<Func<Entity, object>> orderBy = null;
    }
}