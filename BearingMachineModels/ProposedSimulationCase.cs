using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearingMachineModels
{
    public class ProposedSimulationCase
    {
        public ProposedSimulationCase()
        {
            Bearings = new List<Bearing>();
        }

        public List<Bearing> Bearings { get; set; }
        public int FirstFailure { get; set; }
        public int AccumulatedHours { get; set; }
        public int RandomDelay { get; set; }
        public int Delay { get; set; }
    }
}
