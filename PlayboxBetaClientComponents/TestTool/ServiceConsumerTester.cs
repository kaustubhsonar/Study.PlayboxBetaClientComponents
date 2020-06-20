using System;
using System.IO;
using System.Threading.Tasks;
using ServiceConsumer;

namespace TestTool
{
    public class ServiceConsumerTester
    {
        private readonly IotHubClient iotHubClient;
        private readonly string largefilePath = @"C:\VinayTestFile.ogg";
        private readonly string filePath = @"C:\VinayTestFile.ogg";

        private static string[] deviceConnectionStrings = {
                 "HostName=playbox.azure-devices.net;DeviceId=9EB219BF-2BF1-4A0A-BAA3-A289327B9138;SharedAccessKey=7FczhQx4mu3b9bjomLZ9qU09fSXPQxHsfJiGEBsK9Nc="
                ,"HostName=playbox.azure-devices.net;DeviceId=221DD402-44F8-4B5C-A98F-1F16CEB36C2A;SharedAccessKey=C5YYELzznJMgPomOcf6YedS4GxeMonSXyhjI3RR5/2A="
                ,"HostName=playbox.azure-devices.net;DeviceId=684DA241-2C7C-4509-AB70-B8AC7A50E91F;SharedAccessKey=WFDxRotdamKwwKXG2gT9zud38ST/nJVZXbsdh6VGqjA="
                ,"HostName=playbox.azure-devices.net;DeviceId=694D76BE-9AB8-4E3F-9CCE-CBFD09690042;SharedAccessKey=6HhmFExPbf/fnFd2nBR6rvwuXmf1mmcdQI4pwwuNphk="
        };

        public ServiceConsumerTester()
        {
            iotHubClient = new IotHubClient();
        }
        public async Task SendMessage()
        {
            do
            {
                int deviceCount = 1;
                int requestCount = 0;
                //Console.WriteLine("no of requests = no of devices X request per device (Note: Enter ctrl+cc to exit)");
                //Console.WriteLine("no of devices: ");
                //int.TryParse(Console.ReadLine(), out deviceCount);
                Console.WriteLine("requests per device:");
                int.TryParse(Console.ReadLine(), out requestCount);

                byte[] recordedBytes = File.ReadAllBytes(filePath);
                for (int device = 0; device < deviceCount; device++)
                {
                    for (int i = 0; i < requestCount; i++)
                    {
                        var deviceTelemetry = new DeviceTelemetry { RecordedTime = DateTime.UtcNow, RecordedStream = recordedBytes };
                        //await iotHubClient.SendToHub(deviceTelemetry);
                        await iotHubClient.SendToHubV1(deviceTelemetry , deviceConnectionStrings[device]);
                        Console.WriteLine($"Message sent to device : {device}");
                    }
                }
                

            } while (Console.ReadLine() != "cc");

            ////////////////////////

        }
    }
}
