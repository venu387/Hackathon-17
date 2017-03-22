using Public.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windsor.ServiceRequests.Entities;

namespace Windsor.ServiceRequests.Filters
{
    public class Filter_ServiceRequest
    {
        public string ServiceDescription = string.Empty;
        public Department Department = Department.Default;
        public ReportMethod ReportMethod = ReportMethod.Default;
        public DateTime CreatedOnFrom = new DateTime();
        public DateTime CreatedOnTo = new DateTime();
        public string Block = string.Empty;
        public string Street = string.Empty;
        public string Ward = string.Empty;
        public string Status = string.Empty;

        /// <summary>
        /// Retrieve the data from db based on the filter parameters
        /// </summary>
        /// <returns>The list of service requests after filtering</returns>
        public List<ServiceRequest> FilterData()
        {
            List<ServiceRequest> _listServiceRequests = new List<ServiceRequest>();
            try
            {
                DataTable dt = new DataTable();
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                //Filter by description
                if (!string.IsNullOrEmpty(ServiceDescription))
                {
                    sqlParameterList.Add(new SqlParameter("@ServiceDescription", ServiceDescription));
                }
                //By Dept
                if (Department != ServiceRequests.Department.Default)
                {
                    sqlParameterList.Add(new SqlParameter("@Department", Department));
                }
                //By Report method
                if (ReportMethod != ServiceRequests.ReportMethod.Default)
                {
                    sqlParameterList.Add(new SqlParameter("@ReportMethod", ReportMethod));
                }
                //By Date range
                if (CreatedOnFrom != null && CreatedOnFrom > DateTime.MinValue)
                {
                    sqlParameterList.Add(new SqlParameter("@CreatedOnFrom", CreatedOnFrom));
                }
                if (CreatedOnTo != null && CreatedOnTo > DateTime.MinValue)
                {
                    sqlParameterList.Add(new SqlParameter("@CreatedOnTo", CreatedOnTo));
                }
                if (!string.IsNullOrEmpty(Block))
                {
                    sqlParameterList.Add(new SqlParameter("@Block", Block));
                }
                if (!string.IsNullOrEmpty(Street))
                {
                    sqlParameterList.Add(new SqlParameter("@Street", Street));
                }
                if (!string.IsNullOrEmpty(Ward))
                {
                    sqlParameterList.Add(new SqlParameter("@Ward", Ward));
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    sqlParameterList.Add(new SqlParameter("@Status", Status));
                }
                dt = DAL.FillDataTable("usp_RetrieveServiceRequestReport", sqlParameterList.ToArray(), CommandType.StoredProcedure, DAL.ConnectionStringCityOfWindsor);

                //Read the datatable
                if (dt.Rows.Count > 0)
                {
                    for (int indexRow = 0; indexRow < dt.Rows.Count; indexRow++)
                    {
                        ServiceRequest serviceRequest = new ServiceRequest();
                        if (dt.Rows[indexRow]["Block"] != null)
                        {
                            serviceRequest.Block = dt.Rows[indexRow]["Block"].ToString();
                        }
                        serviceRequest.CreatedOn = Convert.ToDateTime(dt.Rows[indexRow]["CreatedOn"]);
                        serviceRequest.Department = (Department)Convert.ToInt16(dt.Rows[indexRow]["Department"]);
                        serviceRequest.ModifiedOn = Convert.ToDateTime(dt.Rows[indexRow]["ModifiedOn"]);
                        serviceRequest.ReportMethod = (ReportMethod)Convert.ToInt16(dt.Rows[indexRow]["ReportMethod"]);
                        serviceRequest.ServiceDescription = dt.Rows[indexRow]["ServiceDescription"].ToString();
                        serviceRequest.ServiceID = Convert.ToInt32(dt.Rows[indexRow]["ServiceID"]);
                        if (dt.Rows[indexRow]["Street"] != null)
                        {
                            serviceRequest.Street = dt.Rows[indexRow]["Street"].ToString();
                        }
                        if (dt.Rows[indexRow]["Ward"] != null)
                        {
                            serviceRequest.Ward = dt.Rows[indexRow]["Ward"].ToString();
                        }
                        if (dt.Rows[indexRow]["X"] != null)
                        {
                            serviceRequest.XLatitude = dt.Rows[indexRow]["X"].ToString();
                        }
                        if (dt.Rows[indexRow]["Y"] != null)
                        {
                            serviceRequest.YLongitude = dt.Rows[indexRow]["Y"].ToString();
                        }
                        if (dt.Rows[indexRow]["Status"] !=null)
                        {
                            serviceRequest.Status = dt.Rows[indexRow]["Status"].ToString();
                        }
                        _listServiceRequests.Add(serviceRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging or send an email about the exception
                string s = "";
                string s2 = s;
            }
            return _listServiceRequests;
        }
    }
}