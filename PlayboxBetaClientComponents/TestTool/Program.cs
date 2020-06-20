using System;

namespace TestTool
{
    class Program
    {
        private readonly static string deviceConnectionString = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_CONN_STRING");

        static void Main(string[] args)
        {
            ServiceConsumerTester serviceConsumerTester = new ServiceConsumerTester();
            serviceConsumerTester.SendMessage().GetAwaiter();
            Console.ReadLine();//Important to keep the Async methods running
        }
    }
}
