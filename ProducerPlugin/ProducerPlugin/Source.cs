using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private bool IsListeningFromStartOfService;

        private Guid ServiceId;

        internal Source(IEventCollector eventCollector, IHealthStore healthStore, bool IsListeningFromStartOfService, string sourceName)
        {
            this.EventCollector = eventCollector;
            this.HealthStore = healthStore;
            this.ServiceId = new Guid();
            this.SourceName = sourceName;
            // SourceType will be set in the derived object.
        }

    }
}
