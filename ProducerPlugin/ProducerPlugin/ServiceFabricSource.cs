using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data.Notifications;
using Newtonsoft.Json;

namespace ProducerPlugin
{
    internal class ServiceFabricSource : Source
    {
        internal ReliableStateManager StateManager;
        private ChangeCollector changeCollector;

        internal ServiceFabricSource(IEventCollector collector, IHealthStore healthStore, string sourceName):base (collector, healthStore, IsListeningFromStart, sourceName)
        {
            this.SourceType = EnumDefinitions.SourceType.ServiceFabric;
            // Need to find some way to get stateManagerObject here.
            this.changeCollector = new ChangeCollector();
        }


        public void RegisterEvenCollector()
        {

            // Here we registered with the event collector.
            this.StateManager.TransactionChanged += this.OnTransactionChangedHandler;
            this.StateManager.StateManagerChanged += this.OnStateManagerChangedHandler;
        }

        // Hooking code will be here.
        /**
         1. Transaction Changed event. 
          2. Dictionary Changed Event
        3. StateManagerChanged Event.
            
         */

        private void OnTransactionChangedHandler(object sender, NotifyTransactionChangedEventArgs e)
        {
            if (e.Action == NotifyTransactionChangedAction.Commit)
            {
                // Need to get all the changes and give it to the event collector.
                var allEvents = changeCollector.GetAllChanges();
                string events = JsonConvert.SerializeObject(allEvents);

                var trAppliedEvent = new NotifyTransactionAppliedEvent(e.Transaction, allEvents);
                Byte[] byteStream = Encoding.ASCII.GetBytes(events);
                EventCollector.PushEventsToEventCollector(trAppliedEvent);
            }
        }



        public void OnStateManagerChangedHandler(object sender, NotifyStateManagerChangedEventArgs e)
        {
            if (e.Action == NotifyStateManagerChangedAction.Rebuild)
            {
                this.PushEventsToEventCollector(e);
                return;
            }

            this.ProcessStateManagerSingleEntityNotification(e);
        }

        private void ProcessStateManagerSingleEntityNotification(NotifyStateManagerChangedEventArgs e)
        {
            var operation = e as NotifyStateManagerSingleEntityChangedEventArgs;

            if (operation.Action == NotifyStateManagerChangedAction.Add)
            {

                // Need to have changes here. --> There 
                if (operation.ReliableState is IReliableDictionary<TKey, TValue>)
                {
                    var dictionary = (IReliableDictionary<TKey, TValue>)operation.ReliableState;
                    dictionary.RebuildNotificationAsyncCallback = this.OnDictionaryRebuildNotificationHandlerAsync;
                    dictionary.DictionaryChanged += this.OnDictionaryChangedHandler;
                }

                // It could also be Reliable Queue also, but we don't get its events.
            }
        }


        public async Task OnDictionaryRebuildNotificationHandlerAsync(
        IReliableDictionary<TKey, TValue> origin,
        NotifyDictionaryRebuildEventArgs<TKey, TValue> rebuildNotification)
        {
            this.changeCollector.CreateNew();


            var enumerator = rebuildNotification.State.GetAsyncEnumerator();

            //Create a new event with dictionary rebuild and add key value pairs to it.
            // and then, push it to event collector
            // NotifyDictionaryRebuildEvent.
            // Add new class RebuildChange.
            while (await enumerator.MoveNextAsync(CancellationToken.None))
            {
                this.EventCollector.Add(enumerator.Current.Key, enumerator.Current.Value);
            }
        }

        public void OnDictionaryChangedHandler(object sender, NotifyDictionaryChangedEventArgs<TKey, TValue> e)
        {
            var state = sender as IReliableState;
            this.changeCollector.AddNewEvent(state.Name.ToString(),e);
            
        }
    }
}
