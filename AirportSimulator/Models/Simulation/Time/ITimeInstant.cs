using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models.Simulation.Time
{
    internal interface ITimeInstant : IComparable<ITimeInstant>
    {
        int TimeId { get; init; }
        static int CurrentId { get; set; }
        int Time { get; init; }
        public ITimeInstant NextTimeInstant();
    }
}
