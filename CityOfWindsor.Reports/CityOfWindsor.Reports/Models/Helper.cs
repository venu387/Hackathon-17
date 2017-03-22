using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace CityOfWindsor.Reports.Models
{
    public class Helper
    {
        //[Obsolete]
        ///// <summary>
        ///// Returns the http response in xml or json format depending on the user request
        ///// </summary>
        ///// <param name="content">The data to be formatted</param>
        ///// <param name="type">XML or JSON format</param>
        ///// <returns>HTTP Reponse object which the user will receive</returns>
        //public HttpResponseMessage Get(object content, string type = "xml")
        //{
        //    if (content != null)
        //    {
        //        if (type.ToUpper() == "JSON")
        //        {
        //            return Request.CreateResponse<object>(HttpStatusCode.OK, content, Configuration.Formatters.JsonFormatter);
        //        }
        //        return Request.CreateResponse<object>(HttpStatusCode.OK, content, Configuration.Formatters.XmlFormatter);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse<object>(HttpStatusCode.InternalServerError, new Error() { ErrorMessage = "No data retrieved" }, Configuration.Formatters.XmlFormatter);
        //    }
        //}
    }

    [Serializable]
    public class Error
    {
        public string ErrorMessage = string.Empty;
    }
}