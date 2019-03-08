using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data.Notifications;
using Newtonsoft.Json;

namespace ProducerPlugin
{
    public class JsonMessageConverter : IMessageConverter
    {
        private JsonSerializerSettings jsonSerializerSettings;
        public JsonMessageConverter(IEnumerable<Type> types)
        {
            var knownTypes = new List<Type>(4 * types.Count());
            foreach (var type in types)
            {
                knownTypes.AddRange(this.GetSerializationTypes(type));
            }

            this.jsonSerializerSettings = new JsonSerializerSettings() {
                SerializationBinder = new KnownTypesBinder(knownTypes),
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public byte[] Serialize<T>(T t)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t, this.jsonSerializerSettings));
        }

        public T Deserialize<T>(byte[] b)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(b), this.jsonSerializerSettings);
        }

        private IList<Type> GetSerializationTypes(Type t)
        {
            if (GenericUtils.IsSubClassOfGeneric(t, typeof(IReliableDictionary<,>)))
            {
                var ts = new List<Type>() {
                    typeof(NotifyDictionaryItemAddedEventArgs<,>).MakeGenericType(t.GetGenericArguments()),
                    typeof(NotifyDictionaryClearEventArgs<,>).MakeGenericType(t.GetGenericArguments()),
                    typeof(NotifyDictionaryItemRemovedEventArgs<,>).MakeGenericType(t.GetGenericArguments()),
                    typeof(NotifyDictionaryItemUpdatedEventArgs<,>).MakeGenericType(t.GetGenericArguments()),
                    typeof(NotifyDictionaryRebuildEventArgs<,>).MakeGenericType(t.GetGenericArguments()),
                    t
                };
                ts.AddRange(t.GetGenericArguments());
                return ts;
            }
            else
            {
                return new List<Type>() { t };
            }
        }
    }
}