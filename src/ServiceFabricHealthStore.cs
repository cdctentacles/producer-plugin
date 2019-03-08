using System;
using CDC.EventCollector;

namespace ProducerPlugin
{
    public class ServiceFabricHealthStore : IHealthStore
    {
        public void WriteError(string msg)
        {
        }

        public void WriteWarning(string msg)
        {
        }

        public void WriteInfo(string msg)
        {
        }

        public void WriteNoise(string msg)
        {
        }
    }
}