using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.Time;
using AirportSimulator.Models.Simulation.Snapshot.SnapshotsValues;

namespace AirportSimulator.Models.Simulation.Snapshot
{
    internal interface ISnapshotAggregator
    {
        public IEnvironment _environment { get; init; }
        public TimeInstant Time { get; init; }
        public int TimeSinceLastIterarion { get; init; }
        public List<ISnapshotMetric> Values { get; init; }

        public ISnapshotAggregator MakeSnapshots();
    }
}
