using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    internal class ReliableCollectionChange
    {
        public string CollectionName;
        public List<EventArgs> EventArgs;


        public ReliableCollectionChange(string name, List<EventArgs> eventArgs)
        {
            this.CollectionName = name;
            this.EventArgs = eventArgs;
        }

        public void AddNewEvent(EventArgs eventArgs)
        {
            this.EventArgs.Add(eventArgs);
        }
    }
}
