using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace ServerStatus {
    class Program {
        static void Main(string[] args) {
            //while (true) {
                Server server;
                if (args.Contains("-c")) {
                    server = new Server(true);
                } else {
                    server = new Server(false);
                }
                for (int x = 0; x < server.Cpu.Cpucores.Count; x++) {
                        Console.WriteLine($"Core {server.Cpu.Cpucores[x].Corenumber}: temp: {server.Cpu.Cpucores[x].Coretemp}");
                }
                //Thread.Sleep(1000);
            //}
        }
    }
}