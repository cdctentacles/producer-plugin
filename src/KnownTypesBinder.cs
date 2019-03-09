using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Serialization;

namespace ProducerPlugin
{
    internal class KnownTypesBinder : ISerializationBinder
    {
        private IList<Type> knownTypes { get; set; }

        public KnownTypesBinder(IList<Type> knownTypes)
        {
            this.knownTypes = knownTypes;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return knownTypes.SingleOrDefault(t => t.FullName == typeName && t.Assembly.GetName().Name == assemblyName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.GetName().Name;
            typeName = serializedType.FullName;
        }
    }
}