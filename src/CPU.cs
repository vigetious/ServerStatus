using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerStatus {
    public class CPU {
        
        private readonly List<float> _cputemp;
        private readonly List<double> _cpuutil;
        private readonly List<Core> _cpucores;

        private readonly JObject _newcputemp;
        


        public List<double> CpuUtil => _cpuutil;

        public List<float> Cputemp => _cputemp;

        public List<Core> Cpucores => _cpucores;

        public CPU(int cpuCount, bool degreesTemperatureScale) {
            _cpucores = new List<Core>();
            _newcputemp = getCpuTemperatures();
            for (var x = 0; x < cpuCount; x++) {
                int hyperthreadcore = x + cpuCount;
                _cpucores.Add(new Core(x, hyperthreadcore, _newcputemp, degreesTemperatureScale));
            }
        }

        private JObject getCpuTemperatures() {
            string sensorsJson = Server.ExecuteCommand("cpuTemp");
            JObject temps = JsonConvert.DeserializeObject<JObject>(sensorsJson);
            return temps;
        }
    }
}