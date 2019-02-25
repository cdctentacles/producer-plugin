using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Preview;
using Microsoft.ServiceFabric.Data;
using CDC.EventCollector;

namespace ProducerPlugin
{
    internal class ServiceFabricSourceFactory
    {
        internal ReliableStateManager stateManager;
        // Need to check the correct format here.
        public ServiceFabricSourceFactory(ReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public Source CreateSource(IEventCollector collector, string sourceName, Guid  partitionId)
        {
            return new ServiceFabricSource(collector, sourceName, stateManager, partitionId);
        }
    }
}
