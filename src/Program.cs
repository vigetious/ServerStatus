using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Newtonsoft.Json;

namespace ServerStatus {
    [RestResource]
    class Program {
        static void Main(string[] args) {
            using (var restServer = new RestServer()) {
                restServer.Port = "9090";
                restServer.LogToConsole().Start();
                Console.ReadLine();
                restServer.Stop();
            }
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/server")]
        public IHttpContext GetServer(IHttpContext content) {
            Server server = new Server();
            content.Response.SendResponse(JsonConvert.SerializeObject(server));
            return content;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/config")]
        public IHttpContext GetServerConfig(IHttpContext content) {
            Server server = new Server();
            content.Response.SendResponse(JsonConvert.SerializeObject(server.Config.Configuration));
            return content;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/config")]
        public IHttpContext SendServerConfig(IHttpContext content) {
            Config newConfig = new Config(true, content.Request.Payload);
            content.Response.SendResponse("success");
            return content;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/network")]
        public IHttpContext SendServerNetwork(IHttpContext content) {
            Network network = new Network();
            content.Response.SendResponse(JsonConvert.SerializeObject(network));
            return content;
        }
    }
}