using System;
using System.Collections.Generic;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class WorkOrderSummary
    {
        public string CustomerNetworkId { get; set; }
        public string CustomerName { get; set; }
        public string WorkOrderId { get; set; }
        public string WorkOrderType { get; set; }
        public DateTime DateNew { get; set; }
        public int? DaysOld { get; set; }
        public string CurrentStatus { get; set; }
        public string Priority { get; set; }
        public string AssignedOrganization { get; set; }
        public string AssignedGroup { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToRemedyId { get; set; }
        public string AssignedToNetworkId { get; set; }
        public string Company { get; set; }
        public string CompanyDescription { get; set; }
        public string BusinessUnit { get; set; }
        public string BusinessUnitDescription { get; set; }
        public string DeptId { get; set; }
        public string DeptDescription { get; set; }
        public string EmplId { get; set; }
        public string Summary { get; set; }
        public string RequestManager { get; set; }
        public string SupportOrganization { get; set; }
        public string SupportGroup { get; set; }
    }
}
