using System;
using System.Collections.Generic;
using System.Threading;

namespace ServerStatus {
    public class NetworkInterface {
        private readonly string _name;
        private long _recieved;

        public long Recieved {
            get => _recieved;
            set => _recieved = value;
        }

        public string Name => _name;

        public NetworkInterface(string interfaceName) {
            _name = interfaceName;
            _recieved = getRecievedBytes();
        }
        
        private long getRecievedBytes() {
            return long.Parse(Server.ExecuteCommand($"networkSpeed {_name} rx_bytes"));
        }

        public float convertToMb(long bytes) {
            return bytes / 20140;
        }
    }
}