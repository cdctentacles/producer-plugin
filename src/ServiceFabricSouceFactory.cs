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
    public class ServiceFabricSourceFactory : ISourceFactory
    {
        internal IReliableStateManager stateManager;
        Guid partitionId;
        String sourceName;
        IMessageConverter messageConverter;

        public ServiceFabricSourceFactory(IReliableStateManager stateManager, Guid partitionId,
            String sourceName, IMessageConverter messageConverter)
        {
            this.stateManager = stateManager;
            this.partitionId = partitionId;
            this.sourceName = sourceName;
            this.messageConverter = messageConverter;
        }

        public ISource CreateSource(IEventCollector collector, IHealthStore healthStore)
        {
            return new ServiceFabricSource(collector, this.sourceName, this.stateManager,
                this.partitionId, this.messageConverter, healthStore);
        }
    }
}