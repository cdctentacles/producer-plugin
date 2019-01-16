using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public interface ISource
    {
        // Need to figure out these members will be private/public/internal/protected.
        IEventCollector EventCollector { get; }
        IHealthStore HealthStore { get; }
        string SourceName { get; }

        // Define an Enum and use it for sourceType
        EnumDefinitions.SourceType SourceType { get; }

        bool IsListeningFromStartOfService { get; }

        Guid ServiceId { get; }
    }
}
