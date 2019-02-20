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
    internal class SourceFactory
    {

        public EnumDefinitions.SourceType sourceType;
        // Need to check the correct format here.
        public SourceFactory(EnumDefinitions.SourceType sourceType)
        {
            this.sourceType = sourceType;
        }
        public Source CreateSource(IEventCollector collector, IHealthStore healthStore, string sourceName)
        {
            switch(this.sourceType)
            {

                case EnumDefinitions.SourceType.ServiceFabric:
                    {
                        return new ServiceFabricSource(collector, healthStore, sourceName);
                    }
                case EnumDefinitions.SourceType.Postgress:
                case EnumDefinitions.SourceType.ElasticSearch:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
