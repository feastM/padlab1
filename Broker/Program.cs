using System;
using System.Threading.Tasks;
using Common;

namespace Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Broker");
            BrokerSocket socket = new BrokerSocket();
            socket.Start(Settings.BROKER_IP,Settings.BROKER_PORT);
            var x = new Worker();
            Task.Factory.StartNew(x.DoSendMessageWork, TaskCreationOptions.LongRunning);
            Console.ReadLine();
        }
    }
}
