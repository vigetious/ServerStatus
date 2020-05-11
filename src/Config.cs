using System;
using System.IO;
using System.Text.Json;

namespace ServerStatus {
    public class Config {
                public class ConfigBuilder {
            public int overrideCpuCount { get; set; }
            public bool degreesTemperatureScale { get; set; }
        }

        public static ConfigBuilder CheckConfigFile(bool configMode) {
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

        private static ConfigBuilder CreateConfig() {
            ConfigBuilder configBuilder = new ConfigBuilder();
            configBuilder.overrideCpuCount = 0;
            configBuilder.degreesTemperatureScale = true;
            File.WriteAllText("config.json", JsonSerializer.Serialize(configBuilder));
            return configBuilder;
        }
        
        private static ConfigBuilder EditConfig(ConfigBuilder json) {
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