using System.Reflection.Metadata.Ecma335;

namespace ServerStatus {
    public class Memory {
        private int used;
        private int total;
        private int free;

        public int Used => used;

        public int Total => total;

        public int Free => free;

        public Memory() {
            used = getUsed();
            total = getTotal();
            free = total - used;
        }

        private int getTotal() {
            return int.Parse(Server.ExecuteCommand("memoryTotal"));
        }
        
        private int getUsed() {
            return int.Parse(Server.ExecuteCommand("memoryUsed"));
        }
    }
}