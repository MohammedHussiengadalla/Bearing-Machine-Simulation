using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearingMachineModels
{
    public class PerformanceMeasures
    {
        public PerformanceMeasures()
        {

        }

        public decimal BearingCost { get; set; }
        public decimal DelayCost { get; set; }
        public decimal DowntimeCost { get; set; }
        public decimal RepairPersonCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
