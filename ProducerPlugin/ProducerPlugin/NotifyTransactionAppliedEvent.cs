using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerPlugin
{
    public class NotifyTransactionAppliedEvent: EventArgs
    {
        public ITransaction transaction;
        public IEnumerable<EventArgs> changes; // Should it be Event Args??

        public NotifyTransactionAppliedEvent(ITransaction transaction, IEnumerable<EventArgs> changes)
        {
            this.transaction = transaction;
            this.changes = changes;
        }
    }
}
