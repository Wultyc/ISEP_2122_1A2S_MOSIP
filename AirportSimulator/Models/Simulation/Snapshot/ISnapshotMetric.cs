using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.SnapshotValues.SnapshotUtilities;

namespace AirportSimulator.Models.Simulation.Snapshot
{
    internal interface ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; }
        public double Value { get; set; }

        public ISnapshotMetric Calculate();
    }
}
