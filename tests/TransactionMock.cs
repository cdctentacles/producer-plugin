using System;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Data;

namespace sf.cdc.plugin.tests
{
    class TransactionMock : ITransaction
    {
        public TransactionMock(long commitSequenceNumber, long transactionId)
        {
            this.commitSequenceNumber = commitSequenceNumber;
            this.transactionId = transactionId;
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public Task<long> GetVisibilitySequenceNumberAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public long CommitSequenceNumber
        {
            get { return this.commitSequenceNumber; }
        }

        public long TransactionId
        {
            get { return this.transactionId; }
        }

        long commitSequenceNumber;
        long transactionId;
    }
}