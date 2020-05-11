using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ServerStatus {
    class Program {
        static void Main(string[] args) {
            bool configMode;
            if (args.Contains("-c")) {
                configMode = true;
            } else {
                configMode = false;
            }
            Server server = new Server(configMode);
            Console.WriteLine(JsonSerializer.Serialize(""));
        }
    }

    class Server {
        private int cpucount;

        public int CpuCount {
            get => cpucount;
        }

        public Server(bool configMode) {
            if (configMode) {
                CheckConfigFile(true);
            } else {
                CheckConfigFile(false);
            }
            cpucount = int.Parse(ExecuteCommand("nproc", string.Empty));
        }

        public static string ExecuteCommand(string command, string args) {
            var proc = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = command,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            //proc.StartInfo.FileName = command;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output;
        }

        public class ConfigBuilder {
            public int overrideCpuCount { get; set; }
            public bool degreesTemperatureScale { get; set; }
        }

        private static ConfigBuilder CheckConfigFile(bool configMode) {
            if (File.Exists("config.json")) {
                // read from existing config file
                var json = JsonSerializer.Deserialize<ConfigBuilder>(File.ReadAllText("config.json"));
                if (configMode) {
                    Console.WriteLine("Config file already exists. Editing...");
                    var newJson = EditConfig(json);
                    return newJson;
                } else {
                    Console.WriteLine("Config file already exists. Moving on...");
                    return json;
                }
            } else {
                // if config file is gone, create a new one with default settings
                if (configMode) {
                    Console.WriteLine("Config file is missing. Creating custom config file...");
                    var newJson = EditConfig(null);
                    return newJson;
                } else {
                    Console.WriteLine("Config file is missing. Creating new with default values...");
                    return CreateConfig();
                }
            }
        }

        public static ConfigBuilder CreateConfig() {
            ConfigBuilder configBuilder = new ConfigBuilder();
            configBuilder.overrideCpuCount = 0;
            configBuilder.degreesTemperatureScale = true;
            File.WriteAllText("config.json", JsonSerializer.Serialize(configBuilder));
            return configBuilder;
        }
        
        public static ConfigBuilder EditConfig(ConfigBuilder json) {
            ConfigBuilder configBuilder = new ConfigBuilder();
            if (json != null) {
                Console.WriteLine(
                    $"Override CPU count? Current config is set to {json.overrideCpuCount}. Enter 0 to auto-detect the CPU count or enter a number.");
            } else {
                Console.WriteLine("Override CPU count? Enter 0 to auto-detect the CPU count or enter a number.");
            }
            int cpuCount;
            while (true) {
                try {
                    cpuCount = int.Parse(Console.ReadLine());
                    break;
                } catch (FormatException) {
                    Console.WriteLine("Please enter a number.");
                } catch (NullReferenceException) {
                    Console.WriteLine("Please enter a number.");
                }
            }

            if (json != null) {
                Console.WriteLine(
                    $"Celsius or Fahrenheit? Current config is set to {json.degreesTemperatureScale}. Enter true for celsius or false for fahrenheit.");
            } else {
                Console.WriteLine("Celsius or Fahrenheit? Enter true for celsius or false for fahrenheit.");
            }
            bool degreesTemperatureScale;
            while (true) {
                try {
                    degreesTemperatureScale = bool.Parse(Console.ReadLine());
                    break;
                } catch (FormatException) {
                    Console.WriteLine("Please enter either true for celsius or false for fahrenheit.");
                } catch (NullReferenceException) {
                    Console.WriteLine("Please enter either true for celsius or false for fahrenheit.");
                }
            }
            configBuilder.overrideCpuCount = cpuCount;
            configBuilder.degreesTemperatureScale = degreesTemperatureScale;
            File.WriteAllText("config.json", JsonSerializer.Serialize(configBuilder));
            return configBuilder;
        }
    }
}