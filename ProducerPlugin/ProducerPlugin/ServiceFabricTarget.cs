using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.ServiceFabric.Data;

namespace ProducerPlugin
{
    public class ServiceFabricPersistentCollector : IPersistentCollector
    {
        public ServiceFabricPersistentCollector(Guid partitionId, IReliableStateManager stateManager, IHealthStore healthStore)
        {
            this.Id = partitionId;
            this.stateManager = stateManager;
            this.healthStore = healthStore;
        }

        virtual async public Task PersistTransactions(PartitionChange partitionChange)
        {
            if (partitionChange.PartitionId != this.Id)
            {
                var message = $"Got wrong PartitionChange {partitionChange.PartitionId} data in ServiceFabricPersistentCollector {this.Id}";
                this.healthStore.WriteError(message);
                throw new InvalidOperationException(message);
            }

            foreach (var transactionToApply in partitionChange.Transactions)
            {
                using (var tx = this.stateManager.CreateTransaction())
                {
                    // deserialize transactionToApply.Data
                    // Find which Dictionary is this change for.
                    // Find which key/value is this change for.
                    await tx.CommitAsync();
                }
            }
        }

        public Guid GetId()
        {
            return this.Id;
        }

        Guid Id;
        IReliableStateManager stateManager;
        IHealthStore healthStore;
    }
}
