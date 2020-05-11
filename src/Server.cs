using System.Diagnostics;

namespace ServerStatus {
    public class Server {
        private int cpucount;

        public int CpuCount {
            get => cpucount;
        }

        public Server(bool configMode) {
            if (configMode) {
                Config.CheckConfigFile(true);
            } else {
                Config.CheckConfigFile(false);
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
    }
}