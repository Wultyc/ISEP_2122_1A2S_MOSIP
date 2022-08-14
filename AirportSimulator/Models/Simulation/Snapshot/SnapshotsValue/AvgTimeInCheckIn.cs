using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.SnapshotValues.SnapshotUtilities;

namespace AirportSimulator.Models.Simulation.Snapshot.SnapshotsValues
{
    internal class AvgTimeInCheckIn : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Avg Time In Check In";
        public double Value { get; set; }

        public AvgTimeInCheckIn ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);

        }

        public ISnapshotMetric Calculate()
        {
            Value = _utilities.CalculateAvgTime("CHECK_IN_START_EVENT", "CHECK_IN_FINISH_EVENT");
            return this;
        }
    }
}
