using System.Collections.Generic;
using System.Diagnostics;

namespace ServerStatus {
    public class Server {
        private Config config;
        private readonly CPU _cpu;
        private readonly Memory _memory;
        
        private readonly int _cpucount;
        private readonly int _cpuphysicalcount;

        public Config Config => config;

        public CPU Cpu => _cpu;

        public Memory Memory => _memory;

        public int CpuPhysicalCount {
            get => _cpuphysicalcount;
        }

        public Server(bool configMode) {
            if (configMode) {
                config = new Config(true);
            } else {
                config = new Config(false);
            }
            
            _cpucount = getCpuTheoryCount();
            _cpuphysicalcount = getCpuCount(config.Configuration.overrideCpuCount);

            _cpu = new CPU(_cpuphysicalcount, config.Configuration.degreesTemperatureScale);
            _memory = new Memory();
        }
        
        private static int getCpuCount(int cpuOverride) {
            if (cpuOverride == 0) {
                return int.Parse(Server.ExecuteCommand("cpuPhysicalCount"));
            } else {
                return cpuOverride;
            }
        }
        
        private static int getCpuTheoryCount() {
            return int.Parse(Server.ExecuteCommand("cpuCount")) - 2;
        }

        public static string ExecuteCommand(string args) {
            var proc = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = "/bin/bash",
                    Arguments = $"../../../config/commands.sh {args}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output;
        }
    }
}