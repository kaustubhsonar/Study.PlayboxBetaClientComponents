using System;
using System.Diagnostics;

namespace TestTool
{
    public class DatPersisterTester
    {
        public void Persist()
        {
            Stopwatch stopwatch = new Stopwatch();
            string testTrack = "test string inserted from tool";//ToDO-Replace it with JSON metadata
            DataPersister dataPersister = new DataPersister();
            stopwatch.Start();
            for (int i = 0; i < 40000; i++)
            {
                dataPersister.PersistTrackInformation(testTrack, DateTime.UtcNow);
            }
            stopwatch.Stop();
            Console.WriteLine("Insertion took {0} seconds", stopwatch.Elapsed.Seconds);
        }

    }
}
