using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.SnapshotValues.SnapshotUtilities;
using AirportSimulator.Models.Entities;

namespace AirportSimulator.Models.Simulation.Snapshot.SnapshotsValues
{
    internal class AvgPlaneAttendantOcupation : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Airplane Entrance Attendant Ocupation Rate";
        public double Value { get; set; }

        public AvgPlaneAttendantOcupation ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);
        }

        public ISnapshotMetric Calculate()
        {
            Value = _utilities.CalulateServerOcupationPerType(typeof(AirplaneEntranceAttendent));
            return this;
        }
    }
}
