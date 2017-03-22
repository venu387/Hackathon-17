using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CityOfWindsor.WebApplication.Entities
{
    [Serializable]
    public class ServiceRequest
    {
        public string ServiceDescription = string.Empty;
        public string Department = string.Empty;
        public string ReportingMethod = string.Empty;
        public string ReportedOn = string.Empty;
        public string Status = string.Empty;
        public Address Address = new Address();
    }
    [Serializable]
    public class Address
    {
        public string Block = string.Empty;
        public string Street = string.Empty;
        public string Ward = string.Empty;
        public string XLatitude = string.Empty;
        public string YLongitude = string.Empty;
    }
}