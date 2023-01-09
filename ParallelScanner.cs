using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TPL
{
    public class ParallelScanner : IPScanner
    {
        public Task Scan(IPAddress[] ipAddrs, int[] ports)  
        {  
            var tasks = ipAddrs  
                .AsParallel()  
                .SelectMany(ip => PingAddress(ip, ports).Result)  
                .ToList();  
  
            return Task.WhenAll(tasks);  
        }  
  
        private static Task<IEnumerable<Task>> PingAddress(IPAddress ipAddr, int[] ports, int timeout = 3000)  
        {  
            using var ping = new Ping();  
            Console.WriteLine($"Pinging {ipAddr}");  
             
            return ping 
                .SendPingAsync(ipAddr, timeout)  
                .ContinueWith(p =>  
                {  
                    Console.WriteLine($"Pinged {ipAddr}: {p.Result.Status}");
                    if (p.Result.Status == IPStatus.Success)
                    {
                        return ports.Select(p => CheckPort(ipAddr, p));
                    }
                    return Enumerable.Empty<Task>();
                });  
        }  
  
        private static async Task CheckPort(IPAddress ipAddr, int port, int timeout = 3000)  
        {  
            using var tcpClient = new TcpClient();
            
            Console.WriteLine($"Checking {ipAddr}:{port}");  
            var portStatus = await tcpClient.ConnectAsync(ipAddr, port, timeout);  
            Console.WriteLine($"Checked {ipAddr}:{port} - {portStatus}");  
        }  
    }
}