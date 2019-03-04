using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Newtonsoft.Json;

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
                // deserialize transactionToApply.Data
                var data = Encoding.UTF8.GetString(transactionToApply.Data);
                var deserializedData = JsonConvert.DeserializeObject<NotifyTransactionAppliedEvent>(data);

                using (var tx = this.stateManager.CreateTransaction())
                {
                    // Find which Dictionary is this change for.
                    foreach (var change in deserializedData.Changes)
                    {
                        var dictName = change.CollectionName;
                        // var dictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<X, Y>>(tx, dictName);
                    }

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
