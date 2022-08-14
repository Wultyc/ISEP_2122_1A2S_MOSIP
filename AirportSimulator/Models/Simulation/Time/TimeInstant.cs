using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models.Simulation.Time
{
    internal class TimeInstant : ITimeInstant
    {
        public int TimeId { get; init; }
        static int CurrentId { get; set; }
        public int Time { get; init; }

        public TimeInstant(int time)
        {
            CurrentId++;
            TimeId = CurrentId;
            Time = time;
        }

        public int CompareTo(ITimeInstant? obj)
        {
            if (obj == null) return 1;

            var timeComparason = Time.CompareTo(obj.Time);

            if (timeComparason != 0) return timeComparason;

            return TimeId - obj.TimeId;
        }

        public ITimeInstant NextTimeInstant()
        {
            return new TimeInstant(Time + 1);
        }
    }
}
