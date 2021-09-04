using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearingMachineModels
{
    public class CurrentSimulationCase
    {
        public CurrentSimulationCase()
        {
            this.Bearing = new Bearing();
        }
        public Bearing Bearing { get; set; }
        public int AccumulatedHours { get; set; }
        public int RandomDelay { get; set; }
        public int Delay { get; set; }        
    }
}
