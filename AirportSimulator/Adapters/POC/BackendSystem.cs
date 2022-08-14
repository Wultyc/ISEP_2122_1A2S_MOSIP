using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Adapters;
using AirportSimulator.Models.Entities;
using AirportSimulator.Models.Events;
using AirportSimulator.Models.Simulation.Queue;

namespace AirportSimulator.Adapters.POC
{
    internal class BackendSystem : IBackendSystem
    {
        public IConfiguration _configuration;
        private readonly Random _random;

        public BackendSystem(IConfiguration configuration)
        {
            _configuration = configuration;
            _random = new Random(_configuration.GetValue<int>("Simulation:RandomSeed"));
        }
        public List<TemporaryEntity> GetPassengers(int numerOfPassenger, int initialPassengerId)
        {
            var passengers = new List<TemporaryEntity>();
            var probabilityCheckIn = _configuration.GetValue<int>("Simulation:Passengers:Probabilities:CheckInOnSite");
            var probabilityPassport = _configuration.GetValue<int>("Simulation:Passengers:Probabilities:ValidatePassport");

            for (int i = 0; i < numerOfPassenger; i++)
            {
                var isDoingOnSiteCheckIn = (_random.Next(0, 100) <= probabilityCheckIn);
                var hasToValidatePassport = (_random.Next(0, 100) <= probabilityPassport);
                passengers.Add(new TemporaryEntity($"Passenger #{initialPassengerId + i}", isDoingOnSiteCheckIn, hasToValidatePassport));
            }

            return passengers;
        }

        public List<PermanentEntity> GetServers()
        {
            var servers = new List<PermanentEntity>();
            var serverMax = 0;

            serverMax = _configuration.GetValue<int>("Simulation:Servers:CheckInAttendant:Count");
            for (int i = 1; i <= serverMax; i++)
            {
                servers.Add(new CheckInAttendant($"Check-in attendent #{i}", $"Attendant at Check-in #{i}"));
            }

            serverMax = _configuration.GetValue<int>("Simulation:Servers:TicketValidator:Count");
            for (int i = 1; i <= serverMax; i++)
            {
                servers.Add(new TicketValidator($"Ticket Validator #{i}", $"Ticket Validator Machine #{i}"));
            }

            serverMax = _configuration.GetValue<int>("Simulation:Servers:MetalDetector:Count");
            for (int i = 1; i <= serverMax; i++)
            {
                servers.Add(new MetalDetector($"Metal Detector #{i}", $"Metal Detector #{i}"));
            }

            serverMax = _configuration.GetValue<int>("Simulation:Servers:PassportValidationAttendant:Count");
            for (int i = 1; i <= serverMax; i++)
            {
                servers.Add(new PassportValidationAttendant($"Passport Validation attendent #{i}", $"Attendant at Passport Validation #{i}"));
            }

            serverMax = _configuration.GetValue<int>("Simulation:Servers:AirplaneEntranceAttendant:Count");
            for (int i = 1; i <= serverMax; i++)
            {
                servers.Add(new AirplaneEntranceAttendent($"Airplane entrance attendent #{i}", $"Attendant at Airplane entrance #{i}"));
            }

            return servers;
        }

        public List<ServerQueue> GetQueues()
        {
            var listOfQueues = new List<ServerQueue>();
            listOfQueues.Add(new ServerQueue { Name = typeof(CheckInAttendant) });
            listOfQueues.Add(new ServerQueue { Name = typeof(TicketValidator) });
            listOfQueues.Add(new ServerQueue { Name = typeof(MetalDetector) });
            listOfQueues.Add(new ServerQueue { Name = typeof(PassportValidationAttendant) });
            listOfQueues.Add(new ServerQueue { Name = typeof(AirplaneEntranceAttendent) });

            return listOfQueues;
        }

        public List<TemporaryEntity> GetTemporaryEntities()
        {
            throw new NotImplementedException();
        }
    }
}
