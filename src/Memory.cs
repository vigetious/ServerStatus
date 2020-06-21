using System.Reflection.Metadata.Ecma335;

namespace ServerStatus {
    public class Memory {
        private readonly int _used;
        private readonly int _total;
        private readonly int _free;

        public int Used => _used;

        public int Total => _total;

        public int Free => _free;

        public Memory() {
            _used = getUsed();
            _total = getTotal();
            _free = _total - _used;
        }

        private int getTotal() {
            return int.Parse(Server.ExecuteCommand("memoryTotal"));
        }
        
        private int getUsed() {
            return int.Parse(Server.ExecuteCommand("memoryUsed"));
        }
    }
}