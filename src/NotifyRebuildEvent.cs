using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    internal class NotifyRebuildEvent<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public string stateName;
        public IAsyncEnumerable<KeyValuePair<TKey, TValue>> State { get; }

        public NotifyRebuildEvent(string stateName, IAsyncEnumerable<KeyValuePair<TKey, TValue>> state)
        {
            this.stateName = stateName;
            this.State = state;
        }
    }
}
