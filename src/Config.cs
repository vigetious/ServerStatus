using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerStatus {
    public class Config {
        private ConfigBuilder config;

        public ConfigBuilder Configuration => config;

        public Config(bool configMode, string config = "") {
            if (configMode) {
                ConfigBuilder newConfig = JsonConvert.DeserializeObject<ConfigBuilder>(config);
                this.config = CheckConfigFile(configMode, newConfig);
            } else {
                this.config = CheckConfigFile(configMode, new ConfigBuilder());
            }
        }
        
        public class ConfigBuilder {
            public int overrideCpuCount { get; set; }
            public bool degreesTemperatureScale { get; set; }
        }

        public static ConfigBuilder CheckConfigFile(bool configMode, ConfigBuilder newConfig) {
            if (File.Exists("config/config.json")) {
                // read from existing config file
                var json = JsonSerializer.Deserialize<ConfigBuilder>(File.ReadAllText("config/config.json"));
                if (configMode) {
                    Console.WriteLine("Config file already exists. Editing...");
                    File.WriteAllText("config/config.json", JsonSerializer.Serialize(newConfig));
                    return newConfig;
                    //var newJson = EditConfig(newConfig);
                    //return newJson;
                } else {
                    Console.WriteLine("Config file already exists. Moving on...");
                    return json;
                }
            } else {
                // if config file is gone, create a new one with default settings
                if (configMode) {
                    Console.WriteLine("Config file is missing. Creating custom config file...");
                    File.WriteAllText("config/config.json", JsonSerializer.Serialize(newConfig));
                    return newConfig;
                    //var newJson = EditConfig(null);
                    //return newJson;
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
            File.WriteAllText("config/config.json", JsonSerializer.Serialize(configBuilder));
            return configBuilder;
        }
        
        private static ConfigBuilder EditConfig(ConfigBuilder json) {
            ConfigBuilder configBuilder = new ConfigBuilder();
            if (json != null) {
                Console.WriteLine($"Override physical CPU count? Current config is set to {json.overrideCpuCount}. Enter 0 to auto-detect the CPU count or enter a number.");
            } else {
                Console.WriteLine("Override physical CPU count? Enter 0 to auto-detect the CPU count or enter a number.");
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
            File.WriteAllText("config/config.json", JsonSerializer.Serialize(configBuilder));
            return configBuilder;
        }
    }
}