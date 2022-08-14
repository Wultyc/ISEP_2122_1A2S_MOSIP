using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using AirportSimulator.Models.Simulation.Environment;

namespace AirportSimulator.Models.Simulation.SnapshotValues.SnapshotUtilities;
    internal class SnapshotUtilities : ISnapshotUtilities
    {
        public IEnvironment _environment { get; init; }

        public SnapshotUtilities ( IEnvironment environment)
        {
            _environment = environment;
        }

        public double CalculateAvgTime(string eventStartName, string eventEndName)
        {   
            double result = 0;
            var timeServer = 0;
            var timeCount = 0;
            foreach (var endEvent in _environment.Events.Where( x => x.IsHandled == true && x.EventName == eventEndName)) {
                var startEvent = _environment.Events.Where( x => x.IsHandled == true && x.EventName == eventStartName && x.TemporaryEntity == endEvent.TemporaryEntity);
                timeServer += endEvent.HandlingTimeInstant.Time - startEvent.First().HandlingTimeInstant.Time;
                timeCount ++;
            }
            if (timeCount != 0) {
               result = timeServer/timeCount;
            }
            return result;
        }
        public double CalculateServerOcupation()
        { 
            var occupiedServers = _environment.Servers.Where(x => x.IsBusy == true).Count();
            double result = (double) occupiedServers/_environment.Servers.Count * 100;
            return Math.Round(result,2);
        }
        public double CalulateServerOcupationPerType (Type serverType) {
            var servers =_environment.Servers.Where( 
                x => x.GetType() == serverType 
            ).ToList();
            double result = (double) servers.Where( x => x.IsBusy == true).Count()/servers.Count * 100;
             
            return Math.Round(result, 2);
        }
    }
