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
    internal class AvgPassportCount : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Avg Passport Count";
        public double Value { get; set; }

        public AvgPassportCount ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);
        }

        public ISnapshotMetric Calculate()
        {
            var passports = _environment.TemporaryEntities.Where( x => x.HasToValidatePassport == true );
            var passportCount = passports.Count();
            Value = Math.Round ( (double) passportCount/_environment.TemporaryEntities.Count * 100 );
            // calculate the total percentage of temporary entities on the instant which have passport to control
            return this;
        }
    }
}
