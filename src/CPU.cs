using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerStatus {
    public class CPU {
        private readonly int _cpucount;
        private readonly int _cputheorycount;
        private readonly List<float> _cputemp;
        private readonly List<double> _cpuutil;
        
        public int CpuCount {
            get => _cpucount;
        }

        public List<double> CpuUtil => _cpuutil;

        public List<float> Cputemp => _cputemp;

        public CPU(int cpuCount, bool degreesTemperatureScale) {
            _cpucount = getCpuCount(cpuCount);
            _cputheorycount = getCpuTheoryCount();
            _cputemp = getCpuTemperatures(degreesTemperatureScale);
            _cpuutil = getCpuUtilization();
            foreach (var VARIABLE in _cpuutil) {
                Console.WriteLine(VARIABLE);
            }
        }

        private static int getCpuCount(int cpuOverride) {
            if (cpuOverride == 0) {
                return int.Parse(Server.ExecuteCommand("cpuCount"));
            } else {
                return cpuOverride;
            }
        }

        private static int getCpuTheoryCount() {
            return int.Parse(Server.ExecuteCommand("cpuTheoryCount")) - 2;
        }
        
        private List<float> getCpuTemperatures(bool degreesTemperatureScale) { // SORT THIS OUT, NEEDS OPTIMIZING
            string sensorsJson = Server.ExecuteCommand("cpuTemp");
            JObject test = JsonConvert.DeserializeObject<JObject>(sensorsJson);
            List<float> cpuTempList = new List<float>();
            foreach (JToken attribute in test["coretemp-isa-0000"]) {
                JProperty attProperty = attribute.ToObject<JProperty>();
                for (var x = 0; x < _cpucount; x++) {
                    if (attProperty.Name == "Core " + x) {
                        foreach (var temp in attribute.Children()) {
                            if (degreesTemperatureScale) {
                                cpuTempList.Add(float.Parse(temp["temp" + (x + 2) + "_input"].ToString()));
                            } else {
                                cpuTempList.Add(ConvertToF(float.Parse(temp["temp" + (x + 2) + "_input"].ToString())));
                            }
                        }
                    }
                }
            }

            return cpuTempList;
        }

        private float ConvertToF(float temp) {
            return (temp * 9 / 5) + 32;
        }

        private List<double> getCpuUtilization() {
            List<double> cpuUtilization = new List<double>(); // list of cpu cores, each holding stat data
            using (StreamReader fs = new StreamReader("/proc/stat")) {
                string line;
                while((line = fs.ReadLine()) != null) {
                    for (var x = 0; x < _cputheorycount; x++) {
                        int lastIdle = 0;
                        int lastTotal = 0;
                        if (line.StartsWith($"cpu{x}")) {
                            List<string> toList = line.Split(" ").ToList();
                            toList.RemoveAt(0);
                            List<int> selectedSplit = toList.Select(int.Parse).ToList();
                            int idle = selectedSplit[3];
                            int total = selectedSplit.Sum();
                            int idleDelta = idle - lastIdle;
                            int totalDelta = total - lastTotal;
                            lastIdle = idle;
                            lastTotal = total;
                            var utilization = 100.0 * (1.0 - (float)idleDelta / (float)totalDelta);
                            cpuUtilization.Add(utilization);
                        }
                    }
                }
            }

            return cpuUtilization;
        }
    }
}