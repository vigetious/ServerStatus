using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ServerStatus {
    public class Core {
        private readonly int _corenumber;
        private readonly float _coretemp;
        private readonly List<double> _coreutil;

        public float Coretemp => _coretemp;

        public int Corenumber => _corenumber;

        public List<double> Coreutil => _coreutil;

        public Core(int corenumber, int hyperthreadcore, JObject cputemp, bool degreesTemperatureScale) {
            _corenumber = corenumber;
            _coretemp = getCpuTemperature(cputemp, degreesTemperatureScale);
            _coreutil = getCpuUtilization(corenumber, hyperthreadcore);
        }

        private float getCpuTemperature(JObject sensorsJson, bool degreesTemperatureScale) {
            float coreTemp = 0;
            foreach (JToken attribute in sensorsJson["coretemp-isa-0000"]) {
                JProperty attProperty = attribute.ToObject<JProperty>();
                if (attProperty.Name == "Core " + _corenumber) {
                    foreach (var temp in attribute.Children()) {
                        if (degreesTemperatureScale) {
                            coreTemp = float.Parse(temp["temp" + (_corenumber + 2) + "_input"].ToString());
                        } else {
                            coreTemp = ConvertToF(float.Parse(temp["temp" + (_corenumber + 2) + "_input"].ToString()));
                        }
                    }
                }
            }

            return coreTemp;
        }
        
        private float ConvertToF(float temp) {
            return (temp * 9 / 5) + 32;
        }

        private List<double> getCpuUtilization(int coreNumber, int hyperThreadCore) {
            List<int> cores = new List<int>();
            cores.Add(coreNumber);
            cores.Add(hyperThreadCore);
            List<double> cpuUtilization = new List<double>(); // list of cpu cores, each holding stat data
            using (StreamReader fs = new StreamReader("/proc/stat")) {
                string line;
                while((line = fs.ReadLine()) != null) {
                    foreach (var x in cores) {
                        int lastIdle = 0;
                        int lastTotal = 0;
                        if (line.StartsWith($"cpu{x} ")) {
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