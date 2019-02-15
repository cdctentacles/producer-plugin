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
    class ServiceFabricSourceFactory  // It should have been in Service fabric Source factory.
    {
        
        // Need to check the correct format here.
        ServiceFabricSourceFactory()
        {
            this.stateManager = stateManager;
        }
        public ISource CreateSource(IEventCollector collector, IHealthStore healthStore)
        {
            return new ServiceFabricSource(collector, healthStore, stateManager);
        }
    }
}
