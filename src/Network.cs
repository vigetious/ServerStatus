using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerStatus {
    public class Network {
        private readonly List<string> _interfaceNames;
        private readonly List<NetworkInterface> _speeds;

        public List<string> InterfaceNames => _interfaceNames;

        public List<NetworkInterface> Speeds => _speeds;

        public Network() {
            _interfaceNames = getInterfaceNames();
            _speeds = new List<NetworkInterface>();
            for (int i = 0; i < _interfaceNames.Count; i++) {
                _speeds.Add(new NetworkInterface(_interfaceNames[i]));
            }
        }
        
        private List<string> getInterfaceNames() { // to delete
            string interfacesJson = Server.ExecuteCommand("networkInterfaces");
            JArray json = JsonConvert.DeserializeObject<JArray>(interfacesJson);
            List<string> interfaces = new List<string>();
            foreach (var attribute in json) {
                interfaces.Add(attribute["ifname"].ToString());
            }
            return interfaces;
        }
    }
}