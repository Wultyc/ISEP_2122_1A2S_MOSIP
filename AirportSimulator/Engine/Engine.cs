using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Adapters;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Entities;

namespace AirportSimulator.Engine
{
    internal class Engine
    {
        private readonly IConfiguration _configuration;
        private Simulator _simulator { get; init; }
        private DataVisualizer _dataVisualizer { get; init; }
        private IEnvironment _environment { get; init; }
        private IBackendSystem _backendSystem { get; init; }

        public Engine(IConfiguration configuration, Simulator simulator, DataVisualizer dataVisualizer, IEnvironment environment, IBackendSystem backendSystem)
        {
            _configuration = configuration;
            _simulator = simulator;
            _dataVisualizer = dataVisualizer;
            _environment = environment;
            _backendSystem = backendSystem;
        }
        public void StartEngine()
        {
            Console.WriteLine("Starting Engine");
            InitPassengers();
            InitServers();
            InitQueues();
            LoadInitialEvents();
            RunSimulation();
            StopEngine();
        }
        public void StopEngine()
        {
            Console.WriteLine("Stoping Engine");
            _dataVisualizer.Run();
        }
        private void InitPassengers() => _environment.TemporaryEntities.AddRange(_backendSystem.GetPassengers( _configuration.GetValue<int>("Simulation:ScheduleEvents:InitialEvents:Quantity"),1 ));
        private void InitServers() => _environment.Servers.AddRange(_backendSystem.GetServers());
        private void InitQueues() => _environment.Queues.AddRange(_backendSystem.GetQueues());
        private void LoadInitialEvents() => _environment.Events.AddRange(_simulator.GetListOfInitialEvents());
        private void RunSimulation() => _simulator.Run();
    }
}
