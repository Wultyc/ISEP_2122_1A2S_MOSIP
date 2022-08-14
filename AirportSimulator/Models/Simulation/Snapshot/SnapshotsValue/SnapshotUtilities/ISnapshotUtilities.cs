using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Simulation.Environment;

namespace AirportSimulator.Models.Simulation.SnapshotValues.SnapshotUtilities
{
    internal interface ISnapshotUtilities
    {
        public IEnvironment _environment { get; init; }
        public double CalculateAvgTime(string eventStartName, string eventEndName);
        public double CalculateServerOcupation();
        public double CalulateServerOcupationPerType (Type serverType);

    }
}
