using System.Diagnostics;

namespace ServerStatus {
    public class Server {
        private Config config;
        private CPU cpu;

        public Config Config => config;

        public CPU Cpu => cpu;

        public Server(bool configMode) {
            if (configMode) {
                config = new Config(true);
            } else {
                config = new Config(false);
            }
            cpu = new CPU(config.Configuration.overrideCpuCount, config.Configuration.degreesTemperatureScale);
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