using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    internal class NotifyTransactionAppliedEvent : EventArgs
    {
        public ITransaction Transaction;
        public List<ReliableCollectionChange> Changes;

        public NotifyTransactionAppliedEvent(ITransaction transaction, List<ReliableCollectionChange> changes)
        {
            this.Transaction = transaction;
            this.Changes = changes;
        }
    }
}
