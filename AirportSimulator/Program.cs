using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Engine;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Adapters;
using AirportSimulator.Adapters.POC;

// Define application dependencies

var configuration = new ConfigurationBuilder()
		.AddEnvironmentVariables()
		.AddCommandLine(args)
		.AddJsonFile("appsettings.json")
		.Build();

var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(builder =>
				{
					builder.Sources.Clear();
					builder.AddConfiguration(configuration);
				})
				.ConfigureServices(services =>
				{
					services.AddSingleton<IBackendSystem, BackendSystem>();
					services.AddSingleton<IEnvironment, SimulationEnvironment>();
					services.AddSingleton<DataVisualizer>();
					services.AddSingleton<Simulator>();
					services.AddSingleton<Engine>();
				})
				.Build();

// Run engine
host.Services.GetService<Engine>()?.StartEngine();

// Close the application