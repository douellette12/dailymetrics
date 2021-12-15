using System;
using System.Collections.Generic;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class DailySummaryBreech
    {
        public DateTime ReportDate { get; set; }
        public string AssignedOrganization { get; set; }
        public string AssignedGroup { get; set; }
        public int? HighBreech { get; set; }
        public int? MediumBreech { get; set; }
        public int? LowBreech { get; set; }
    }
}
