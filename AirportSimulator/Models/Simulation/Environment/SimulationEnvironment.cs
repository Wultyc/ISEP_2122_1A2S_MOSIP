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
    internal class SimulationEnvironment :IEnvironment
    {
        public List<IEvent> Events { get; init; } = new List<IEvent>();
        public List<TemporaryEntity> TemporaryEntities { get; init; } = new List<TemporaryEntity>();
        public List<PermanentEntity> Servers { get; init; } = new List<PermanentEntity>();
        public List<ServerQueue> Queues { get; init; } = new List<ServerQueue>();
        public List<ISnapshotAggregator> Snapshots { get; init; } = new List<ISnapshotAggregator>();
    }
}
