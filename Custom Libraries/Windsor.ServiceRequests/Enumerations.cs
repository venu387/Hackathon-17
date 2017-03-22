using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windsor.ServiceRequests
{
    /// <summary>
    /// Enum for department
    /// </summary>
    public enum Department
    {
        Default = 0,
        Inspections = 1,
        BylawEnforcement = 2
    }

    /// <summary>
    /// Enum for Report method
    /// </summary>
    public enum ReportMethod
    {
        Default = 0,
        Phone = 1,
        Web_Intake = 2,
        Text = 3,
        E_Mail = 4
    }
}
