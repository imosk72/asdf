using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TPL
{
    public class AsyncAwaitScanner : IPScanner
    {
        public Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            var tasks = new List<Task>();
            foreach (var ip in ipAddrs)
            {
                tasks.Add(PingAddress(ip, ports));
            }
            return Task.WhenAll(tasks);
        }  
  
        private static async Task PingAddress(IPAddress ipAddr, int[] ports, int timeout = 3000)  
        {  
            using var ping = new Ping();  
            Console.WriteLine($"Pinging {ipAddr}");

            var pingReply = await ping.SendPingAsync(ipAddr, timeout);
            Console.WriteLine($"Pinged {ipAddr}: {pingReply.Status}");
            var tasks = new List<Task>();
            if (pingReply.Status == IPStatus.Success)
            {
                foreach (var port in ports)
                {
                    tasks.Add(CheckPort(ipAddr, port, timeout));
                }
            }
            await Task.WhenAll(tasks);
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