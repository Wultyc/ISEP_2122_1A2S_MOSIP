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
    internal class AvgPassportDifference : ISnapshotMetric
    {
        public IEnvironment _environment { get; init; }
        public ISnapshotUtilities _utilities {get; init; }
        public string Lable { get; init; } = "Avg Passport Difference";
        public double Value { get; set; }

        public AvgPassportDifference ( IEnvironment environment)
        {
            _environment = environment;
            _utilities = new SnapshotUtilities(_environment);
        }

        public ISnapshotMetric Calculate()
        {
            
            var timeMean = (double) _utilities.CalculateAvgTime("ARRIVE_EVENT", "PLANE_ENTRANCE_FINISH_EVENT");

            var timeServer = 0;
            var timeCount = 0;
            double result;
            foreach (var endEvent in _environment.Events.Where( x => x.IsHandled == true && x.EventName == "PLANE_ENTRANCE_FINISH_EVENT" && x.TemporaryEntity.HasToValidatePassport == true)) {
                var startEvent = _environment.Events.Where( x => x.IsHandled == true && x.EventName == "ARRIVE_EVENT" && x.TemporaryEntity == endEvent.TemporaryEntity);
                timeServer += endEvent.HandlingTimeInstant.Time - startEvent.First().HandlingTimeInstant.Time;
                timeCount ++;
            }
            if (timeCount == 0) {
                result = 0;
                Value = 0;
            }
            else {
                result = (double) timeServer/timeCount;

                Value = (double)result - timeMean;
            }    
            
            return this;
        }
    }
}
