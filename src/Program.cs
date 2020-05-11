using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ServerStatus {
    class Program {
        static void Main(string[] args) {
            bool configMode;
            if (args.Contains("-c")) {
                configMode = true;
            } else {
                configMode = false;
            }
            Server server = new Server(configMode);
            Console.WriteLine(JsonSerializer.Serialize(""));
        }
    }
}