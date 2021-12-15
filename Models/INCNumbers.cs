using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnectHuddle1.Models
{
    public class INCNumbers : DailySummary
    {
        public int TotalBreached { get; set; }
        public double IncidentBreachPercentage { get; set; }
    }
}
