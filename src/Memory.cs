using System.Reflection.Metadata.Ecma335;

namespace ServerStatus {
    public class Memory {
        private int used;
        private int total;

        public Memory() {
            used = getUsed();
            total = getTotal();
        }

        private int getTotal() {
            return int.Parse(Server.ExecuteCommand("memoryTotal"));
        }
        
        private int getUsed() {
            return int.Parse(Server.ExecuteCommand("memoryUsed"));
        }
    }
}