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
    internal class SnapshotAggregator : ISnapshotAggregator
    {
        public IEnvironment _environment { get; init; }
        public TimeInstant Time { get; init; }
        public int TimeSinceLastIterarion { get; init; }
        public List<ISnapshotMetric> Values { get; init; } = new List<ISnapshotMetric>();

        public SnapshotAggregator (IEnvironment environment, TimeInstant time, int timeSinceLastIterarion)
        {
            _environment = environment;
            Time = time;
            TimeSinceLastIterarion = timeSinceLastIterarion;
        }

        public ISnapshotAggregator MakeSnapshots()
        {

            Values.Add(new AvgCheckinAttendantOcupation(_environment).Calculate());
            Values.Add(new AvgMetalDetectorOcupation(_environment).Calculate());
            Values.Add(new AvgPassportAttendantOcupation(_environment).Calculate());
            Values.Add(new AvgPassportCount(_environment).Calculate());
            Values.Add(new AvgPassportDifference(_environment).Calculate());
            Values.Add(new AvgPlaneAttendantOcupation(_environment).Calculate());
            Values.Add(new AvgServerOcupation(_environment).Calculate());
            Values.Add(new AvgTicketValidorOcupation(_environment).Calculate());
            Values.Add(new AvgTimeInCheckIn(_environment).Calculate());
            Values.Add(new AvgTimeInMetalDetector(_environment).Calculate());
            Values.Add(new AvgTimeInQueue(_environment).Calculate());
            Values.Add(new AvgTimePassportValidation(_environment).Calculate());
            Values.Add(new AvgTimeTicketValidation(_environment).Calculate());
            Values.Add(new AvgTimeTotal(_environment).Calculate());
            Values.Add(new PeopleInQueue(_environment).Calculate());
            return this;
        }
    }
}
