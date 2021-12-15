using System;
using System.Collections.Generic;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class CareConnectReportTeam
    {
        public CareConnectReportTeam()
        {
            CareConnectReportPeople = new HashSet<CareConnectReportPerson>();
        }

        public int TeamId { get; set; }
        public string SupportGroup { get; set; }
        public string LocationIncident { get; set; }
        public string LocationWorkOrder { get; set; }
        public string LocationHuddleBoard { get; set; }

        public virtual ICollection<CareConnectReportPerson> CareConnectReportPeople { get; set; }
    }
}
