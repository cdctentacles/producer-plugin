using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    internal class ChangeCollector
    {
        internal Dictionary<string, ReliableCollectionChange> eventDict;

        internal ChangeCollector()
        {
            this.eventDict = new Dictionary<string, ReliableCollectionChange>();
        }

        internal void CreateNew()
        {
            this.eventDict = new Dictionary<string, ReliableCollectionChange>();
        }

        internal List<ReliableCollectionChange> GetAllChanges()
        {
            return new List<ReliableCollectionChange>(this.eventDict.Values);
        }

        internal void AddNewEvent(string stateName, EventArgs eventArgs)
        {
            if (!eventDict.ContainsKey(stateName))
            {
                eventDict[stateName] = new ReliableCollectionChange(stateName, new List<EventArgs>() { eventArgs } );
            }
            else
            {
                eventDict[stateName].AddNewEvent(eventArgs);
            }
        }
    }
}
