using System;
using System.Collections.Generic;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class DailySummary
    {
        public DateTime ReportDate { get; set; }
        public string AssignedOrganization { get; set; }
        public string AssignedGroup { get; set; }
        public int IncidentsSettled { get; set; }
        public int IncidentsRounding { get; set; }
        public int IncidentsNew { get; set; }
        public int IncidentsStillOpen { get; set; }
        public int IncidentOlder14Days { get; set; }
        public int IncidentOlder30Days { get; set; }
        public int WorkOrdersSettled { get; set; }
        public int WorkOrdersNew { get; set; }
        public int WorkOrdersStillOpen { get; set; }
        public int WorkOrdersOlder30Days { get; set; }
        public int WorkOrdersOlder60Days { get; set; }
        public int TasksSettled { get; set; }
        public int TasksNew { get; set; }
        public int TasksStillOpen { get; set; }
        public int TasksOlder30Days { get; set; }
        public int TasksOlder60Days { get; set; }
        public int ProjectsSettled { get; set; }
        public int ProjectsNew { get; set; }
        public int ProjectsStillOpen { get; set; }
        public int ProjectsOlder30Days { get; set; }
        public int ProjectsOlder60Days { get; set; }
    }
}
