using Public.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windsor.ServiceRequests.Filters;

namespace Windsor.ServiceRequests.Entities
{
    /// <summary>
    /// The entity will hold the details of a service request that has been received from the public
    /// </summary>
    [Serializable]
    public class ServiceRequest
    {
        #region Public Fields

        public int ServiceID;
        public string ServiceDescription = string.Empty;
        public Department Department = Department.Default;
        public ReportMethod ReportMethod = ReportMethod.Default;
        public DateTime CreatedOn = new DateTime();
        public DateTime ModifiedOn = new DateTime();
        public string Block = string.Empty;
        public string Street = string.Empty;
        public string Ward = string.Empty;
        public string XLatitude = string.Empty;
        public string YLongitude = string.Empty;
        public string Status = string.Empty;

        #endregion Public Fields
        
        #region Public Methods

        //Write methods for basic 'CRUD' operations

        #endregion Public Methods
    }
}
