using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.Time;

namespace AirportSimulator.Models.Events
{
    internal interface IEvent
    {
        public string EventName { get; init; }
        public string EventDescription { get; init; }
        public Type ServerType { get; init; }
        public PermanentEntity? Server { get; set; }
        public TemporaryEntity TemporaryEntity { get; init; }
        public ITimeInstant TimeInstant { get; init; }
        public ITimeInstant? HandlingTimeInstant { get; set; } 
        public bool IsHandled { get; set; }
        public bool IsQueueble { get; init; }

        public void EventHandler(ITimeInstant handlingTimeInstant);
    }
}
