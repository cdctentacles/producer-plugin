using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public interface IEventCollector
    {

        // It will contain all the functions that are required in ServiceFabricSource to HookIn.
        void PushEventsToEventCollector(Byte[] byteArray);
    }
}
