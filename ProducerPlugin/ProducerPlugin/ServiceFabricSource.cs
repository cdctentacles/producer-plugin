﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data.Notifications;
using Newtonsoft.Json;
using CDC.EventCollector;
using System.Reflection;

namespace ProducerPlugin
{
    internal class ServiceFabricSource : Source
    {
        internal ReliableStateManager StateManager;
        private ChangeCollector changeCollector;
        private long previousLsn = long.MinValue;
        private Guid partitionId;

        internal ServiceFabricSource(IEventCollector collector, string sourceName,
            ReliableStateManager stateManager, Guid partitionId):base (collector, sourceName)
        {
            this.SourceType = EnumDefinitions.SourceType.ServiceFabric;
            this.changeCollector = new ChangeCollector();
            // Here we registered with the event collector.
            this.partitionId = partitionId;
            this.StateManager = stateManager;
            this.StateManager.TransactionChanged += this.OnTransactionChangedHandler;
            this.StateManager.StateManagerChanged += this.OnStateManagerChangedHandler;
        }

        private void OnTransactionChangedHandler(object sender, NotifyTransactionChangedEventArgs e)
        {
            if (e.Action == NotifyTransactionChangedAction.Commit)
            {
                var allEvents = changeCollector.GetAllChanges();
                var trAppliedEvent = new NotifyTransactionAppliedEvent(e.Transaction, allEvents);
                string eventString = JsonConvert.SerializeObject(trAppliedEvent);
                Byte[] byteStream = Encoding.ASCII.GetBytes(eventString);
                long currentLsn = e.Transaction.CommitSequenceNumber;
                EventCollector.TransactionApplied(this.partitionId, previousLsn, currentLsn, byteStream);
                previousLsn = currentLsn;

                // Flush previous items.
                this.changeCollector.CreateNew();
            }
        }

        public void OnStateManagerChangedHandler(object sender, NotifyStateManagerChangedEventArgs e)
        {
            if (e.Action == NotifyStateManagerChangedAction.Rebuild)
            {
                throw new NotImplementedException();
            }

            ProcessStateManagerSingleEntityNotification(e);
        }

        private void ProcessStateManagerSingleEntityNotification(NotifyStateManagerChangedEventArgs e)
        {
            var operation = e as NotifyStateManagerSingleEntityChangedEventArgs;

            if (operation.Action == NotifyStateManagerChangedAction.Add)
            {
                var reliableStateType = operation.ReliableState.GetType();
                switch (ReliableStateKindUtils.KindOfReliableState(operation.ReliableState))
                {
                    case ReliableStateKind.ReliableDictionary:
                        {
                            var keyType = reliableStateType.GetGenericArguments()[0];
                            var valueType = reliableStateType.GetGenericArguments()[1];
                            this.GetType().GetMethod("ProcessStateManagerDictionaryChangedNotification", BindingFlags.Instance | BindingFlags.NonPublic)
                                .MakeGenericMethod(keyType, valueType)
                                .Invoke(this, new object[] { operation.ReliableState });
                            break;
                        }
                    case ReliableStateKind.ReliableQueue:
                    case ReliableStateKind.ReliableConcurrentQueue:
                    default:
                        break;

                }
            }
        }

        internal void ProcessStateManagerDictionaryChangedNotification<TKey, TValue>(NotifyStateManagerChangedEventArgs e)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            var operation = e as NotifyStateManagerSingleEntityChangedEventArgs;
            var dictionary = (IReliableDictionary<TKey, TValue>)operation.ReliableState;
            dictionary.RebuildNotificationAsyncCallback = this.OnDictionaryRebuildNotificationHandlerAsync;
            dictionary.DictionaryChanged += this.OnDictionaryChangedHandler;
        }

        public async Task OnDictionaryRebuildNotificationHandlerAsync<TKey, TValue>(
        IReliableDictionary<TKey, TValue> origin,
        NotifyDictionaryRebuildEventArgs<TKey, TValue> rebuildNotification)
        where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            this.changeCollector.CreateNew();
            var enumerator = rebuildNotification.State.GetAsyncEnumerator();
            // We will send the event as it is. But the previousLsn and nextLsn would be -1.
            var rebuildEvent = new NotifyRebuildEvent<TKey, TValue>(origin.Name.ToString(), rebuildNotification.State);
            string rebuildEventString = JsonConvert.SerializeObject(rebuildEvent);
            Byte[] byteStream = Encoding.ASCII.GetBytes(rebuildEventString);
            EventCollector.TransactionApplied(this.partitionId, -1, -1, byteStream);
        }

        public void OnDictionaryChangedHandler<TKey, TValue>(object sender, NotifyDictionaryChangedEventArgs<TKey, TValue> e)
        where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            var state = sender as IReliableState;
            this.changeCollector.AddNewEvent(state.Name.ToString(),e);
        }
    }
}
