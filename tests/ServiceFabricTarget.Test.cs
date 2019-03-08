using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProducerPlugin;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 8)]

namespace sf.cdc.plugin.tests
{
    public class ServiceFabricTargetTest
    {
        IList<Type> knownTypes;

        public ServiceFabricTargetTest()
        {
            this.knownTypes = new List<Type>() {
                typeof(IReliableDictionary<Guid, long>),
                typeof(TransactionMock),
            };
        }

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

            var jsonMessageConverter = new JsonMessageConverter(this.knownTypes);
            var encodedEvent = jsonMessageConverter.Serialize(notifyEvent);
            var decodedEvent = jsonMessageConverter.Deserialize<NotifyTransactionAppliedEvent>(encodedEvent);

            Assert.Equal(100, decodedEvent.Transaction.CommitSequenceNumber);
            Assert.Equal(200, decodedEvent.Transaction.TransactionId);
            Assert.Equal(1, decodedEvent.Changes.Count());

            var firstChange = decodedEvent.Changes.First();
            Assert.Equal("mydict", firstChange.CollectionName);
            Assert.True(firstChange.EventArgs is NotifyDictionaryItemAddedEventArgs<Guid, long>);
            var decodedItemAddedArg = firstChange.EventArgs as NotifyDictionaryItemAddedEventArgs<Guid, long>;
            Assert.Equal(dictKey, decodedItemAddedArg.Key);
            Assert.Equal(dictValue, decodedItemAddedArg.Value);
        }
    }
}
