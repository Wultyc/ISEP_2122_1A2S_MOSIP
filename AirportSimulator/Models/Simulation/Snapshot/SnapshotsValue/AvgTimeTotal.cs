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
    internal class AvgTimeTotal : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Avg Time Total";
        public double Value { get; set; }

        public AvgTimeTotal ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);

        }

        public ISnapshotMetric Calculate()
        {
            Value = _utilities.CalculateAvgTime("ARRIVE_EVENT", "PLANE_ENTRANCE_FINISH_EVENT");
            return this;
        }
    }
}
