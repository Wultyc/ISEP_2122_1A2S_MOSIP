using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Events;
using AirportSimulator.Models.Simulation.Queue;

namespace AirportSimulator.Adapters
{
    internal interface IBackendSystem
    {
        public List<TemporaryEntity> GetPassengers(int numerOfPassenger, int initialPassengerId);
        public List<PermanentEntity> GetServers();
        public List<ServerQueue> GetQueues();
        public List<TemporaryEntity> GetTemporaryEntities();
    }
}
