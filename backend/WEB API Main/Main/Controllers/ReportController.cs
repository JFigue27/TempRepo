using BusinessSpecificLogic.Reports;
using Reusable;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WEBAPI.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        private LoggedUser loggedUser;
        public ReportController(LoggedUser loggedUser)
        {
            loggedUser = new LoggedUser((ClaimsIdentity)User.Identity);
        }

        public ReportController()
        {

        }

        protected bool isValidJSValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null" || value == "undefined")
            {
                return false;
            }

            return true;
        }

        protected bool isValidParam(string param)
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

        [HttpGet, Route("api/Report/Headcount/{userName}")]
        public HttpResponseMessage Headcount(string userName)
        {
            Headcount report = new Headcount(userName)
            {
                loggedUser = loggedUser
            };

            HttpResponseMessage result = null;
            try
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(report.generate()));
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = report.fileName;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Helper: " + report.DebugErrorHelper + " " + ex.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        [HttpGet, Route("api/Report/AllTrainings/{userName}")]
        public HttpResponseMessage AllTrainings(string userName)
        {
            AllTrainings report = new AllTrainings(userName)
            {
                loggedUser = loggedUser
            };

            HttpResponseMessage result = null;
            try
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(report.generate()));
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = report.fileName;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Helper: " + report.DebugErrorHelper + " " + ex.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        [HttpGet, Route("api/Report/AboutToExpire/{userName}")]
        public HttpResponseMessage AboutToExpire(string userName)
        {
            AboutToExpire report = new AboutToExpire(userName)
            {
                loggedUser = loggedUser
            };

            HttpResponseMessage result = null;
            try
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(report.generate()));
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = report.fileName;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Helper: " + report.DebugErrorHelper + " " + ex.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}