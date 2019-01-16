using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Preview;

namespace ProducerPlugin
{
    class ServiceFabricSourceFactory
    {
        ServiceFabricSourceFactory(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public ISource CreateSource(IEventCollector collector, IHealthStore healthStore)
        {
            return new ServiceFabricSource(collector, healthStore, stateManager);
        }
    }
}
