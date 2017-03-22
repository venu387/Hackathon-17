using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Windsor.ServiceRequests.Filters;
using Windsor.ServiceRequests;
using Windsor.ServiceRequests.Entities;
using CityOfWindsor.Reports.Models;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace CityOfWindsor.Reports.Controllers
{
    public class ServiceRequestController : ApiController
    {
        [ActionName("subset")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public JsonResult<object> FilterServiceRequests(string dept = "", string repmethod = "", string from = "", string to = "", string block = "", string street = "", string ward = "", string status = "", string descr = "")
        {
            //Read the parameters
            try
            {
                Filter_ServiceRequest filterRequest = new Filter_ServiceRequest();
                filterRequest.Block = block;
                filterRequest.CreatedOnFrom = Convert.ToDateTime(from);
                filterRequest.CreatedOnTo = Convert.ToDateTime(to);
                filterRequest.Department = (Department)Convert.ToInt16(dept);
                filterRequest.ReportMethod = (ReportMethod)Convert.ToInt16(repmethod);
                filterRequest.ServiceDescription = descr;
                filterRequest.Street = street;
                filterRequest.Ward = ward;
                filterRequest.Status = status;

                //Get the data from the database
                List<Windsor.ServiceRequests.Entities.ServiceRequest> tempListServices = filterRequest.FilterData();
                ServiceRequestModel ServiceRequestModel = new ServiceRequestModel();
                ServiceRequestModel.ListOfServiceRequest = new List<Models.ServiceRequest>();
                
                for (int indexServices = 0; indexServices < tempListServices.Count; indexServices++)
                {
                    ServiceRequestModel.ListOfServiceRequest.Add(new Models.ServiceRequest()
                    {
                        Address = new Address()
                        {
                            Block = tempListServices[indexServices].Block,
                            Street = tempListServices[indexServices].Street,
                            Ward = tempListServices[indexServices].Ward,
                            XLatitude = tempListServices[indexServices].XLatitude,
                            YLongitude = tempListServices[indexServices].YLongitude
                        },
                        Department = Enum.GetName(typeof(Department), tempListServices[indexServices].Department),
                        ReportedOn = tempListServices[indexServices].CreatedOn.ToString("MM'-'dd'-'yyyyThh':'mm':'ss"),
                        ReportingMethod = Enum.GetName(typeof(ReportMethod), tempListServices[indexServices].ReportMethod),
                        ServiceDescription = tempListServices[indexServices].ServiceDescription,
                        Status = tempListServices[indexServices].Status
                    });
                }
                //return ObjectHelper.GetSerializedString(ServiceRequestModel.ListOfServiceRequest, type);
                return Json((object)ServiceRequestModel.ListOfServiceRequest, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });

                //if (Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")))
                //{
                //    string contentType = Request.Headers.FirstOrDefault(h => h.Key == "Content-Type").Value.FirstOrDefault();
                //    if (!string.IsNullOrEmpty(contentType) && contentType.ToUpper().Contains("JSON"))
                //    {
                //        return GetHttpResponse(ServiceRequestModel.ListOfServiceRequest, HttpStatusCode.OK, "JSON");
                //    }
                //    else
                //    {
                //        return GetHttpResponse(ServiceRequestModel.ListOfServiceRequest, HttpStatusCode.OK);
                //    }
                //}
                //else if(type.ToUpper() == "JSON")
                //{
                //    return GetHttpResponse(ServiceRequestModel.ListOfServiceRequest, HttpStatusCode.OK, "JSON");
                //}
                //else
                //{
                //    return GetHttpResponse(ServiceRequestModel.ListOfServiceRequest, HttpStatusCode.OK);
                //}
            }
            catch (Exception ex)
            {
                Error err = new Error() { ErrorMessage = ex.Message };
                //return ObjectHelper.GetSerializedString(err);
                return Json((object)err, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
                //return GetHttpResponse(new Error() { ErrorMessage = ex.Message }, HttpStatusCode.InternalServerError);
            }
            //return Json("Dept:" + dept + "RepMethod:" + repmethod + "From:" + from + "To:" + to + "Block:" + block + "Street:" + street + "Ward:" + ward);
        }

        /// <summary>
        /// Returns the http response in xml or json format depending on the user request
        /// </summary>
        /// <param name="content">The data to be formatted</param>
        /// <param name="type">XML or JSON format</param>
        /// <returns>HTTP Reponse object which the user will receive</returns>
        public HttpResponseMessage GetHttpResponse(object content, HttpStatusCode code, string type = "xml")
        {
            HttpResponseMessage res = null;
            if (content != null)
            {
                if (type.ToUpper() == "JSON")
                {
                    res = Request.CreateResponse<object>(code, content);
                    res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                res = Request.CreateResponse<object>(code, content);
                res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            }
            else
            {
                res = Request.CreateResponse<object>(HttpStatusCode.InternalServerError, "No data retrieved");
                res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                //return Request.CreateResponse<object>(HttpStatusCode.InternalServerError, new Error() { ErrorMessage = "No data retrieved" }, Configuration.Formatters.XmlFormatter);
            }
            return res;
        }
    }
}
