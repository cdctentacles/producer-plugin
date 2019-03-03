using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDC.EventCollector;

namespace ProducerPlugin
{

    // This is the source class which will implement the ISource interface.
    internal class Source : ISource
    {
        internal IEventCollector EventCollector;
        private readonly string SourceName;

        internal EnumDefinitions.SourceType SourceType;

        private Guid ServiceId;

        internal Source(IEventCollector eventCollector, string sourceName)
        {
            this.EventCollector = eventCollector;
            this.ServiceId = new Guid();
            this.SourceName = sourceName;
        }

        public Guid GetSourceId()
        {
            return this.ServiceId;
        }
    }
}
