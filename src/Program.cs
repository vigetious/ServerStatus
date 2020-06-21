using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace ServerStatus {
    class Program {
        static void Main(string[] args) {
            Network oldRecieved = new Network();
            while (true) {
                Server server;
                if (args.Contains("-c")) {
                    server = new Server(true);
                } else {
                    server = new Server(false);
                }

                for (int x = 0; x < server.Cpu.Cpucores.Count; x++) {
                    Console.WriteLine(
                        $"Core {server.Cpu.Cpucores[x].Corenumber}: temp: {server.Cpu.Cpucores[x].Coretemp}");
                }

                Console.WriteLine(
                    $"Memory: Total: {server.Memory.Total}M; Used: {server.Memory.Used}M; Free: {server.Memory.Free}M");
                
                Network network = new Network();
                
                for (int i = 0; i < network.InterfaceNames.Count; i++) {
                    long newRecieved = network.Speeds[i].Recieved;
                    if (oldRecieved.Speeds[i].Recieved != 0) {
                        Console.WriteLine($"Speed of {network.Speeds[i].Name}: " + (newRecieved - oldRecieved.Speeds[i].Recieved) / 102400.0);
                    }
                    oldRecieved.Speeds[i].Recieved = newRecieved;
                }
                
                Thread.Sleep(1000);
            }
        }
    }
}