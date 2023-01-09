using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TPL
{
    public static class Program
    {
        private static string MeasureScannerTime(IPScanner scanner)
        {
            var ipAddrs = new[] {IPAddress.Parse("192.168.0.1"), IPAddress.Parse("127.0.0.1")/*, Place your ip addresses here*/};
            var ports = Enumerable.Range(1, 1000).ToArray();

            var timer = new Stopwatch();
            
            timer.Start();
            scanner.Scan(ipAddrs, ports).Wait();
            timer.Stop();
            return $"{scanner.GetType().ToString()}: {timer.ElapsedMilliseconds}ms";
        }
        
        public static void Main(string[] args)
        {
            // var ipAddrs = new[] {IPAddress.Parse("192.168.0.1"), IPAddress.Parse("127.0.0.1")/*, Place your ip addresses here*/};
            // var ports = new[] {21, 25, 80, 443, 3389, 8000, 5432, 3000 };
            //
            // new AsyncAwaitScanner().Scan(ipAddrs, ports).Wait();
            
            using StreamWriter file = new("measurements.txt");
            file.WriteLine(MeasureScannerTime(new SequentialScanner()));
            file.WriteLine(MeasureScannerTime(new ParallelScanner()));
            file.WriteLine(MeasureScannerTime(new AsyncAwaitScanner()));
        }
    }
}