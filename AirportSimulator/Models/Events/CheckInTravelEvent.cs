using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.Time;

namespace AirportSimulator.Models.Events
{
    internal class CheckInTravelEvent : IEvent
    {
        private readonly IEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly Random _random;
        public string EventName { get; init; } = "CHECK_IN_TRAVEL_EVENT";
        public string EventDescription { get; init; } = "Passeger travels to check-in";
        public Type? ServerType { get; init; } = null;
        public PermanentEntity? Server { get; set; } = null;
        public TemporaryEntity TemporaryEntity { get; init; }
        public ITimeInstant TimeInstant { get; init; }
        public ITimeInstant? HandlingTimeInstant { get; set; }
        public bool IsHandled { get; set; } = false;
        public bool IsQueueble { get; init; } = false;

        public CheckInTravelEvent(IEnvironment environment, IConfiguration configuration, Random random, PermanentEntity? server, TemporaryEntity temporaryEntity, ITimeInstant timeInstant)
        {
            _environment = environment;
            _configuration = configuration;
            _random = random;
            Server = server;
            TemporaryEntity = temporaryEntity;
            TimeInstant = timeInstant;
        }

        public void EventHandler(ITimeInstant handlingTimeInstant)
        {
            HandlingTimeInstant = handlingTimeInstant;

            Console.WriteLine($"  Handling Event {EventName} for Temporary Entity {TemporaryEntity.Lable} scheduled for execution at {TimeInstant.Time} at Time Instant {HandlingTimeInstant.Time}");

            var minTime = _configuration.GetValue<int>("Simulation:Events:CheckIn:Travel:NextEventMinimumInterval");
            var maxTime = _configuration.GetValue<int>("Simulation:Events:CheckIn:Travel:NextEventMaximumInterval");
            var timeToStartNextEvent = HandlingTimeInstant.Time + _random.Next(minTime, maxTime);

            _environment.Events.Add(new CheckInStartEvent(_environment, _configuration, _random, null, TemporaryEntity, new TimeInstant(timeToStartNextEvent)));

            IsHandled = true;
            HandlingTimeInstant = handlingTimeInstant;
        }
    }
}
