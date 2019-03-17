using Microsoft.ServiceFabric.Data.Notifications;

namespace ProducerPlugin
{
    public class NotifyDictionaryItemAddedEventArgsMock<TKey, TValue> : NotifyDictionaryTransactionalEventArgsMock<TKey, TValue>
    {
        public NotifyDictionaryItemAddedEventArgsMock(TransactionMock transaction, TKey key, TValue value) : base(transaction,NotifyDictionaryChangedAction.Add)
        {
            this.Key = key;
            this.Value = value;
        }
        public TKey Key { get; }
        public TValue Value { get; }
    }       
}
