using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Events;

namespace AirportSimulator.Models.Simulation.Queue
{
    internal class IQueue
    {
        public Type Name { get; init; }
        public Queue<IEvent> Queue { get; init; }
    }
}
