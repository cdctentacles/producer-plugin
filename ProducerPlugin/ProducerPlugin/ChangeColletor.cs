using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    internal class ChangeCollector
    {
        internal Dictionary<string, List<ReliableCollectionChange>> eventDict;
        private object wrlock;

        internal ChangeCollector()
        {
            wrlock = new object();
        }
      
        internal void CreateNew()
        {
            lock(wrlock)
            {
                this.eventDict = new Dictionary<string, List<ReliableCollectionChange>>();
            }
        }

        internal List<EventArgs> GetAllChanges()
        {
            // It will be only called when we receive transaction applied event.

            List<EventArgs> listOfEvents = new List<EventArgs>();
            foreach(var key in this.eventDict.Keys)
            {
                foreach(var change in this.eventDict[key])
                {
                    listOfEvents.Add(change.eventArgs);
                }
            }

            return listOfEvents;
        }

        internal void AddNewEvent(string stateName, EventArgs eventArgs)
        {
            // It will always be called when eventDict is not null.
            var change = new ReliableCollectionChange(stateName, eventArgs);
            lock(wrlock)
            {
                if (!eventDict.ContainsKey(stateName))
                {
                    eventDict[stateName] = new List<ReliableCollectionChange>();
                }
                eventDict[stateName].Add(change);
            }
            return;
        }
    }
}
