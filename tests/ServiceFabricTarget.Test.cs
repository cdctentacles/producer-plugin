using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.ServiceFabric.Data.Notifications;
using Newtonsoft.Json;
using ProducerPlugin;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 8)]

namespace sf.cdc.plugin.tests
{
    public class ServiceFabricTargetTest
    {
        [Fact]
        public void DecodeMessages()
        {
            var transaction = new TransactionMock(100, 200);
            var changes = new List<ReliableCollectionChange>();
            var dictKey = new Guid();
            var dictValue = 400;
            var itemAddedEventArg = new NotifyDictionaryItemAddedEventArgs<Guid, long>(transaction, dictKey, dictValue);

            changes.Add(new ReliableCollectionChange("mydict", itemAddedEventArg));

            var notifyEvent = new NotifyTransactionAppliedEvent(transaction, changes);


            var encodedEvent = JsonConvert.SerializeObject(notifyEvent, this.jsonSettings);
            var decodedEvent = JsonConvert.DeserializeObject<NotifyTransactionAppliedEvent>(encodedEvent, this.jsonSettings);
        }

        JsonSerializerSettings jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    }
}