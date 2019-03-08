using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public static class EnumDefinitions
    {
        public enum SourceType 
            {
            ServiceFabric = 0,
            Postgress,
            ElasticSearch
        }
    }
}
