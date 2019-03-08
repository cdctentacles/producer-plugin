using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 8)]

namespace sf.cdc.plugin.tests
{
    public class ServiceFabricTargetTest
    {
        [Fact]
        public void DecodeMessages()
        {
        }
    }
}