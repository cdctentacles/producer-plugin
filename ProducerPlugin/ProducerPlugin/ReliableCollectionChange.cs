using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public class ReliableCollectionChange
    {
        public string collectionName;
        public EventArgs eventArgs;


        public ReliableCollectionChange(string name, EventArgs eventArgs)
        {
            this.collectionName = name;
            this.eventArgs = eventArgs;
         }
    }
}
