using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Adapters;
using AirportSimulator.Models.Events;
using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.Time;
using AirportSimulator.Models.Simulation.Snapshot;

namespace AirportSimulator.Engine
{
    internal class Simulator
    {
        private IEnvironment _environment { get; init; }
        private IBackendSystem _backendSystem { get; init; }
        private readonly IConfiguration _configuration;
        private readonly Random _random;
    public Simulator(IEnvironment environment, IBackendSystem backendSystem, IConfiguration configuration)
        {
            _environment = environment;
            _backendSystem = backendSystem;
            _configuration = configuration;
            _random = new Random(_configuration.GetValue<int>("Simulation:RandomSeed"));
        }
        public List<IEvent> GetListOfInitialEvents()
        {
            var maxIntreval = _configuration.GetValue<int>("Simulation:ScheduleEvents:InitialEvents:MaxInitialEventTime");
            return GetListOfNewEvents(_environment.TemporaryEntities, 0, maxIntreval);
        }

        public void Run()
        {
            Console.WriteLine("Starting Simulation Loop");

            var loopTimeLimit = _configuration.GetValue<int>("Simulation:Loop:TimeLimit");
            var hasEventsToProcess = true;
            var loopCount = 0;
            var lastIterationTime = -1;
            var currentIterationTime = -1;

            do
            {
                loopCount++;
                Console.WriteLine("Loop iteration #" + loopCount);
                lastIterationTime = currentIterationTime;

                ScheduleNewEvents(currentIterationTime);

                //Retrieve events that are excutable in this iteration
                var applicableEvents = _environment.Events
                                        .Where(x => x.IsHandled != true && x.TimeInstant.Time > currentIterationTime && x.TimeInstant.Time <= loopTimeLimit)
                                        .OrderBy(x => x.TimeInstant.Time)
                                        .GroupBy(x => x.TimeInstant.Time)
                                        .ToList();
                
                //Retrieve queues with pending events
                var queuesWithEvents = _environment.Queues
                                        .Where(x => x.Queue.Count > 0)
                                        .ToList();

                //If no record available to process, end the simulation
                if (applicableEvents.Count < 1 && queuesWithEvents.Count < 1)
                {
                    Console.WriteLine("No more events to handle");
                    hasEventsToProcess = false;
                    continue;
                }

                //Trigger and enqueue events
                if( applicableEvents.Count > 0 )
                {
                    applicableEvents[0]?.ToList().ForEach(evnt =>
                    {
                        currentIterationTime = evnt.TimeInstant.Time;

                        if (evnt.IsQueueble)
                        {
                            _environment.Queues.Find(queue => queue.Name == evnt.ServerType)?.Queue.Enqueue(evnt);
                        }
                        else
                        {
                            evnt.EventHandler(evnt.TimeInstant);
                            evnt.IsHandled = true;
                        }
                    });
                }

                //Assign events to the servers and trigger them
                _environment.Servers.ForEach(server =>
                {
                    if(server.IsBusy != true)
                    {
                        var queue = _environment.Queues.Find(queue => queue.Name == server.GetType())?.Queue;
                        
                        var newEventToHandle = (queue != null && queue.Count > 0) ? queue.Dequeue() : null;

                        if (newEventToHandle != null)
                        {
                            server.IsBusy = true;
                            newEventToHandle.Server = server;
                            newEventToHandle.EventHandler(new TimeInstant(currentIterationTime));
                            newEventToHandle.IsHandled = true;
                        }
                    }
                });

                //Take a snapshot of the environment
                _environment.Snapshots.Add( new SnapshotAggregator( _environment, new TimeInstant(currentIterationTime), currentIterationTime - lastIterationTime ).MakeSnapshots() );

            } while ( ContinueLoop(currentIterationTime, loopCount, hasEventsToProcess) );

            Console.WriteLine("Stoping Simulation Loop");
        }
        private bool ContinueLoop(int currentTime, int loopCount, bool hasEventsToProcess)
        {
            var loopCountLimit = _configuration.GetValue<int>("Simulation:Loop:CountLimit");
            var loopTimeLimit = _configuration.GetValue<int>("Simulation:Loop:TimeLimit");
            
            return (loopCount <= loopCountLimit) &&  (currentTime <= loopTimeLimit) && hasEventsToProcess;
        }

        private void ScheduleNewEvents(int periodLowerLimit)
        {
            var maxArriveEvents = _configuration.GetValue<int>("Simulation:ScheduleEvents:MaxArrivals");
            var listOfArrivalEvents = _environment.Events
                                        .Where(x => x.EventName == "ARRIVE_EVENT")
                                        .Count();

            if (listOfArrivalEvents >= maxArriveEvents)
                return;

            var periodHightLimit = periodLowerLimit + _configuration.GetValue<int>("Simulation:ScheduleEvents:DuringSimulation:ArriveRate:TimeUnits");
            var NrOfArrivalEventsInPeriod = _environment.Events
                                            .Where(x => x.EventName == "ARRIVE_EVENT"
                                                        && x.TimeInstant.Time >= periodLowerLimit
                                                        && x.TimeInstant.Time <= periodHightLimit)
                                            .Count();
            var arriveRate = _configuration.GetValue<int>("Simulation:ScheduleEvents:DuringSimulation:ArriveRate:Rate");
            if (NrOfArrivalEventsInPeriod >= arriveRate)
                return;

            var missingArrivals = arriveRate - NrOfArrivalEventsInPeriod;
            
            missingArrivals = (listOfArrivalEvents + missingArrivals <= maxArriveEvents)
                                ? missingArrivals
                                : maxArriveEvents - listOfArrivalEvents;

            var passengerNr = _environment.TemporaryEntities.Count + 1;
            var newPassengers = _backendSystem.GetPassengers(missingArrivals, passengerNr);

            _environment.TemporaryEntities.AddRange(newPassengers);
            _environment.Events.AddRange(GetListOfNewEvents(newPassengers, periodLowerLimit, periodHightLimit));
        }
        private List<IEvent> GetListOfNewEvents(List<TemporaryEntity> passengersList, int passengerInitialArriveTime, int passengerLastlArriveTime)
        {
            var listOfEvents = new List<IEvent>();
            var numerOfPassenger = passengersList.Count;
            var minIntreval = _configuration.GetValue<int>("Simulation:Passengers:MinimumInterval");
            var maxIntreval = _configuration.GetValue<int>("Simulation:Passengers:MaximumInterval");

            passengersList.ForEach(te =>
            {
                var newPassengerMinTime = minIntreval + passengerInitialArriveTime;
                var newPassengerMaxTime = maxIntreval + passengerInitialArriveTime;
                newPassengerMaxTime = (newPassengerMaxTime <= passengerLastlArriveTime) ? maxIntreval : passengerLastlArriveTime;

                passengerInitialArriveTime = _random.Next(newPassengerMinTime, newPassengerMaxTime);

                listOfEvents.Add(new ArriveEvent(_environment, _configuration, _random, null, te, new TimeInstant(passengerInitialArriveTime)));
            });

            return listOfEvents;
        }
    }
}