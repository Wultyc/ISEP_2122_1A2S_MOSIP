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
    internal class PeopleInQueue : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Total people in queue";
        public double Value { get; set; }

        public PeopleInQueue ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);

        }

        public ISnapshotMetric Calculate()
        {
            _environment.Queues.ForEach(x => {
                Value += x.Queue.Count;
            });

            return this;
        }
    }
}
