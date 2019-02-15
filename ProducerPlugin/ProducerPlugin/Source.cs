using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDC.EventCollector;

namespace ProducerPlugin
{

    // This is the source class which will implement the ISource interface.
    internal class Source   
    {
        internal IEventCollector EventCollector;
        public IHealthStore HealthStore;
        private readonly string SourceName;

        // Define an Enum and use it for sourceType
        internal EnumDefinitions.SourceType SourceType;

        private Guid ServiceId;

        internal Source(IEventCollector eventCollector, IHealthStore healthStore, string sourceName)
        {
            this.EventCollector = eventCollector;
            this.HealthStore = healthStore;
            this.ServiceId = new Guid();
            this.SourceName = sourceName;
            // SourceType will be set in the derived object.
        }

    }
}
