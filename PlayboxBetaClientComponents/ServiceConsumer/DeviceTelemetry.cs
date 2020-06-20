using System;
using System.Net.WebSockets;
using System.Threading;

namespace ServiceConsumer
{
    public class DeviceTelemetry
    {
        public DateTime RecordedTime { get; set; }
        public byte[] RecordedStream { get; set; }
        
    }
}
