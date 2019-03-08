using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public class NotifyTransactionAppliedEvent : EventArgs
    {
        public ITransaction Transaction;
        public IEnumerable<ReliableCollectionChange> Changes;

        public NotifyTransactionAppliedEvent(ITransaction transaction, IEnumerable<ReliableCollectionChange> changes)
        {
            this.Transaction = transaction;
            this.Changes = changes;
        }
    }
}
