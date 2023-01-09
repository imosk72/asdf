using System.Net;
using System.Threading.Tasks;

namespace TPL
{
    public interface IPScanner
    {
        Task Scan(IPAddress[] ipAddrs, int[] ports);
    }
}