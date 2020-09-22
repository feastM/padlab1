using System;
using Common;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Subscriber");
            string topic;
            Console.WriteLine("Enter Topic");
            topic = Console.ReadLine().ToLower();
            var subscriberSocket = new SubscriberSocket(topic);

            subscriberSocket.Connect(Settings.BROKER_IP,Settings.BROKER_PORT);
            Console.WriteLine("press to exit");
            Console.ReadLine();
        }
    }
}
