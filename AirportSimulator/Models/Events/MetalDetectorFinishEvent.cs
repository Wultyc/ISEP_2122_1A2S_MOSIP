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
    internal class MetalDetectorFinishEvent : IEvent
    {
        private readonly IEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly Random _random;
        public string EventName { get; init; } = "METAL_DETECTOR_FINISH_EVENT";
        public string EventDescription { get; init; } = "Passanger finishes metal detector";
        public Type? ServerType { get; init; } = typeof(MetalDetector);
        public PermanentEntity? Server { get; set; } = null;
        public TemporaryEntity TemporaryEntity { get; init; }
        public ITimeInstant TimeInstant { get; init; }
        public ITimeInstant HandlingTimeInstant { get; set; }
        public bool IsHandled { get; set; } = false;
        public bool IsQueueble { get; init; } = false;

        public MetalDetectorFinishEvent(IEnvironment environment, IConfiguration configuration, Random random, PermanentEntity? server, TemporaryEntity temporaryEntity, ITimeInstant timeInstant)
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

            IEvent newEvent;

            if (TemporaryEntity.HasToValidatePassport)
            {
                newEvent = new PassportValidationTravelEvent(_environment, _configuration, _random, null, TemporaryEntity, TimeInstant.NextTimeInstant());
            }
            else
            {
                newEvent = new PlaneEntranceTravelEvent(_environment, _configuration, _random, null, TemporaryEntity, TimeInstant.NextTimeInstant());
            }

            _environment.Events.Add(newEvent);

            Server.IsBusy = false;

            IsHandled = true;
            HandlingTimeInstant = handlingTimeInstant;
        }
    }
}
