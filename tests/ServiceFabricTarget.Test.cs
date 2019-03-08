using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
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
        public ServiceFabricTargetTest()
        {
            var knownTypeBinder = new KnownTypesBinder();
            // Since we don't know all ReliableDictionaries and
            // on backup cluster, we are the one creating the data
            // we need to ask customer to register with us all the IReliableDictionary.
            knownTypeBinder.KnownTypes = new List<Type>() {
                typeof(NotifyDictionaryItemAddedEventArgs<Guid, long>),
                typeof(TransactionMock),
            };

            jsonSettings.SerializationBinder = knownTypeBinder;
            jsonSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
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
            var encodedEvent = JsonConvert.SerializeObject(notifyEvent, this.jsonSettings);

            var decodedEvent = JsonConvert.DeserializeObject<NotifyTransactionAppliedEvent>(encodedEvent, this.jsonSettings);
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

        JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
    }

    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.FullName == typeName && t.Assembly.GetName().Name == assemblyName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.GetName().Name;
            typeName = serializedType.FullName;
        }
    }
}
