using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data.Notifications;
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
                        // assume all collections are dictionary.
                        var dictName = change.CollectionName;
                        var eventArgsType = change.EventArgs.GetType();
                        if (eventArgsType.GetGenericArguments().Length != 2)
                        {
                            var msg = $"Dictionary : {dictName} : Got an eventArgs {data} with less than 2 GenericArguments.";
                            this.healthStore.WriteError(msg);
                            throw new InvalidOperationException(msg);
                        }

                        var keyType = eventArgsType.GetGenericArguments()[0];
                        var valueType = eventArgsType.GetGenericArguments()[1];

                        await this.ApplyEventToDictionary(tx, keyType, valueType, dictName, change.EventArgs);
                    }

                    await tx.CommitAsync();
                }
            }
        }

        async Task ApplyEventToDictionary(ITransaction tx, Type keyType, Type valueType, string dictName, EventArgs eventArgs)
        {
            await (Task) this.GetType().GetMethod("ApplyEventToDictionaryGeneric", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(keyType, valueType)
                .Invoke(this, new object[] { tx, dictName, eventArgs });
        }

        async Task ApplyEventToDictionaryGeneric<K, V>(ITransaction tx, string dictName, EventArgs eventArgs)
        where K : IComparable<K>, IEquatable<K>
        {
            var dict = await this.stateManager.GetOrAddAsync<IReliableDictionary<K, V>>(dictName);
            switch (eventArgs)
            {
                case NotifyDictionaryItemAddedEventArgs<K,V> addedItem:
                    await dict.AddAsync(tx, addedItem.Key, addedItem.Value);
                    break;
                case NotifyDictionaryItemRemovedEventArgs<K,V> removedItem:
                    await dict.TryRemoveAsync(tx, removedItem.Key);
                    break;
                case NotifyDictionaryItemUpdatedEventArgs<K, V> updatedItem:
                    await dict.AddOrUpdateAsync(tx, updatedItem.Key, (k) => updatedItem.Value, (k, v) => updatedItem.Value);
                    break;
                case NotifyDictionaryClearEventArgs<K, V> clearedDict:
                    await dict.ClearAsync();
                    break;
                case NotifyDictionaryRebuildEventArgs<K, V> rebuildDict:
                    // not handled.
                    this.healthStore.WriteWarning($"Got rebuild event on dictionary {dictName}. Rebuild event not handled.");
                    break;
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
