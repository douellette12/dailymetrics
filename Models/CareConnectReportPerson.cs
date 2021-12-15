using System;
using System.Collections.Generic;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class CareConnectReportPerson
    {
        public string EmployeeId { get; set; }
        public int TeamId { get; set; }

        public virtual CareConnectReportTeam Team { get; set; }
    }
}
