using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ServerStatus {
    class Program {
        static void Main(string[] args) {
            Server server;
            if (args.Contains("-c")) {
                server = new Server(true);
            } else {
                server = new Server(false);
            }
            Console.WriteLine(server.Cpu.Cputemp);
        }
    }
}