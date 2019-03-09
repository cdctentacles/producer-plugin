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
    class User
    {
        public string name;
        public User(string n)
        {
            name = n;
        }
    }

    public class ServiceFabricTargetTest
    {
        IList<Type> knownTypes;

        public ServiceFabricTargetTest()
        {
            this.knownTypes = new List<Type>() {
                typeof(IReliableDictionary<Guid, User>),
                typeof(TransactionMock),
            };
        }

        [Fact]
        public void JsonMessageConverterTest()
        {
            var transaction = new TransactionMock(100, 200);
            var changes = new List<ReliableCollectionChange>();
            var dictKey = new Guid();
            var dictValue = new User("ashish");
            var itemAddedEventArg = new NotifyDictionaryItemAddedEventArgs<Guid, User>(transaction, dictKey, dictValue);

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
            Assert.True(firstChange.EventArgs is NotifyDictionaryItemAddedEventArgs<Guid, User>);
            var decodedItemAddedArg = firstChange.EventArgs as NotifyDictionaryItemAddedEventArgs<Guid, User>;
            Assert.Equal(dictKey, decodedItemAddedArg.Key);
            Assert.Equal(dictValue.name, decodedItemAddedArg.Value.name);
        }
    }
}
