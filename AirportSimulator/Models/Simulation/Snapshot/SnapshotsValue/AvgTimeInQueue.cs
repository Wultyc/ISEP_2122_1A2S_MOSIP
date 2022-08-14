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
    internal class AvgTimeInQueue : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Avg Time In Queue";
        public double Value { get; set; }

        public AvgTimeInQueue ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);
        }

        public ISnapshotMetric Calculate()
        {
            var queuebleEvents = _environment.Events.Where( x => x.IsHandled == true && x.IsQueueble == true && x.TimeInstant.Time != x.HandlingTimeInstant.Time );

            var timeSum = 0;
            queuebleEvents.ToList().ForEach( x => timeSum += x.HandlingTimeInstant.Time - x.TimeInstant.Time );
            
            Value = (queuebleEvents.Count() > 0) ? timeSum / queuebleEvents.Count() : 0;
            return this;
        }
    }
}
