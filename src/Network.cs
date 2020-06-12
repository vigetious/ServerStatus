using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerStatus {
    public class Network {
        private readonly List<string> _networkInterfaces;

        public Network() {
            _networkInterfaces = getNetworkInterfaces();
            //Console.WriteLine(_networkInterfaces.Count);
        }
        
        private List<string> getNetworkInterfaces() { // to delete
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