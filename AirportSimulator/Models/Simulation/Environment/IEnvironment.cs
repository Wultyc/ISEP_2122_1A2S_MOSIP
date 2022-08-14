using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Events;
using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Simulation.Queue;
using AirportSimulator.Models.Simulation.Snapshot;

namespace AirportSimulator.Models.Simulation.Environment
{
    internal interface IEnvironment
    {
        public List<IEvent> Events { get; init; }
        public List<TemporaryEntity> TemporaryEntities { get; init; }
        public List<PermanentEntity> Servers { get; init; }
        public List<ServerQueue> Queues { get; init; }
        public List<ISnapshotAggregator> Snapshots { get; init; }
    }
}
