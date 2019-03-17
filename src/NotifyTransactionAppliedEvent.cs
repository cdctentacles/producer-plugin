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
        public TransactionMock Transaction;
        public List<ReliableCollectionChange> Changes;

        public NotifyTransactionAppliedEvent(TransactionMock transaction, List<ReliableCollectionChange> changes)
        {
            this.Transaction = transaction;
            this.Changes = changes;
        }
    }
}
