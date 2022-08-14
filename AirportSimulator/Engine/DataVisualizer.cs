using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using AirportSimulator.Adapters;
using AirportSimulator.Models.Simulation.Environment;
using AirportSimulator.Models.Simulation.Snapshot;

namespace AirportSimulator.Engine
{
    internal class DataVisualizer
    {
        private readonly IConfiguration _configuration;
        private Simulator _simulator { get; init; }
        private IEnvironment _environment { get; init; }
        private IBackendSystem _backendSystem { get; init; }
        private string _csvHeader = "";
        private string _csvData = "";

        public DataVisualizer(IConfiguration configuration, Simulator simulator, IEnvironment environment, IBackendSystem backendSystem)
        {
            _configuration = configuration;
            _simulator = simulator;
            _environment = environment;
            _backendSystem = backendSystem;
        }
        public void Run()
        {
            var onConsoleReportIsEnabled = _configuration.GetValue<bool>("DataVisualizer:OnScreenReport:Enable");
            var onFileReport = _configuration.GetValue<bool>("DataVisualizer:OnFileReport:Enable");
            _environment.Snapshots.ForEach(x => {
                if (onConsoleReportIsEnabled) PrintToConsole(x);
                if (onFileReport) PrepareToCSV(x);
            });

            if (onFileReport) PrintToCSV();
        }

        private void PrintToConsole(ISnapshotAggregator snapshot)
        {
            var separationLineChar = _configuration.GetValue<char>("DataVisualizer:OnScreenReport:SeparationLineChar");
            var separationLineLen = _configuration.GetValue<int>("DataVisualizer:OnScreenReport:SeparationLineLength");
            var innerLineIndentation = _configuration.GetValue<string>("DataVisualizer:OnScreenReport:InnerLineIndentation");

            Console.WriteLine(new String(separationLineChar, separationLineLen));

            Console.WriteLine($"Snapshot for time {snapshot.Time.Time}");
            Console.WriteLine($"Time since last snapshot {snapshot.TimeSinceLastIterarion}");
            snapshot.Values.ForEach(x => {
                Console.WriteLine($"{innerLineIndentation}{x.Lable}: {x.Value}");
            });
        }

        private void PrepareToCSV(ISnapshotAggregator snapshot)
        {
            var separator = _configuration.GetValue<string>("DataVisualizer:OnFileReport:CSVSeparator");
            var linebreak = _configuration.GetValue<string>("DataVisualizer:OnFileReport:CSVLineBreak");
            var writeHeader = _csvHeader == "";
            var header = $"Time{separator}Time Since Last Snapshot{separator}";
            var data = snapshot.Time.Time + separator + snapshot.TimeSinceLastIterarion + separator;

            snapshot.Values.ForEach(x => {
                if (writeHeader)
                    header += x.Lable + separator;

                data += x.Value + separator;
            });

            if (writeHeader)
                _csvHeader += header.Remove(header.Length - 1, 1) + linebreak;

            _csvData += data.Remove(data.Length - 1, 1) + linebreak;
        }

        private void PrintToCSV()
        {
            var filePath = _configuration.GetValue<string>("DataVisualizer:OnFileReport:FilePath");
            if (filePath == null || filePath == "")
                filePath = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                  "AirportSimulatorData.csv");

            File.WriteAllText(filePath, _csvHeader + _csvData);
        }
    }
}
