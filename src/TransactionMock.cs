using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;

namespace ProducerPlugin
{
    public class TransactionMock : ITransaction
    {
        // Always increasing and it is only available after transaction is succesful.
        public long CommitSequenceNumber { get; }

        public long TransactionId { get; }

        public TransactionMock(long transactionId, long commitSequenceNumber)
        {
            this.TransactionId = transactionId;
            this.CommitSequenceNumber = commitSequenceNumber;
        }

        public void Abort()
        {
            // Doing something.             
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Do nothing.
        }

        public Task<long> GetVisibilitySequenceNumberAsync()
        {
            throw new NotImplementedException();
        }
    }
}
