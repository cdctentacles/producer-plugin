using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public class ReliableCollectionChange
    {
        public string CollectionName;
        public EventArgs EventArgs;


        public ReliableCollectionChange(string name, EventArgs eventArgs)
        {
            this.CollectionName = name;
            this.EventArgs = eventArgs;
         }
    }
}
