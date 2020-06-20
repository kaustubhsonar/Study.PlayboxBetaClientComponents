using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ServiceConsumer
{
    public class IotHubClient
    {
        private readonly string deviceConnectionString1 = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_CONN_STRING");
        private readonly string deviceConnectionString = "HostName=playbox.azure-devices.net;DeviceId=playbox;SharedAccessKey=dFpuIGiuCunTtUyi+mKn6wcfGl6gLLZqNIrsaPp6+Iw=";




        private readonly TransportType transportType = TransportType.Amqp;

        public IotHubClient()
        {
            if (string.IsNullOrEmpty(deviceConnectionString))
            {
                Debug.WriteLine("Please provide a device connection string");
            }
        }
        public async Task SendToHub(DeviceTelemetry deviceTelemetry)
        {
            try
            {
                using (DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, transportType))
                {
                    //await deviceClient.OpenAsync();//ToDo.Do we need this?
                    //await UpdateTwin(deviceClient);//ToDo-Fix this
                    await SendTelemetry(deviceClient, deviceTelemetry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occured while creating device client " + ex.ToString());
            }
        }

        public async Task SendToHubV1(DeviceTelemetry deviceTelemetry, string clinetDeviceConnectionString)
        {
            try
            {
                using (DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(clinetDeviceConnectionString, transportType))
                {
                    //await deviceClient.OpenAsync();//ToDo.Do we need this?
                    //await UpdateTwin(deviceClient);//ToDo-Fix this
                    await SendTelemetry(deviceClient, deviceTelemetry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occured while creating device client " + ex.ToString());
            }
        }

        //TODo-Should we really use Message? It has restiction of 250kb and JSON serialization much increases the byte array size. Explore all other options of sending
        //ToDo-Handle MessageTooLarge exception
        private static async Task SendTelemetry(DeviceClient deviceClient, DeviceTelemetry deviceTelemetry)
        {
            var payload = JsonConvert.SerializeObject(deviceTelemetry);
            var message = new Message(Encoding.ASCII.GetBytes(payload));
            await deviceClient.SendEventAsync(message);
        }

        private static async Task UpdateTwin(DeviceClient deviceClient)
        {
            //ToDo-Read from the device and fill
            var twinProperties = new TwinCollection();
            twinProperties["connection.type"] = "wi-fi";
            twinProperties["connectionStrength"] = "full";
            await deviceClient.UpdateReportedPropertiesAsync(twinProperties).ConfigureAwait(false);
        }
    }
}
