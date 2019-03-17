using Microsoft.ServiceFabric.Data.Notifications;

namespace ProducerPlugin
{
    public class NotifyDictionaryItemRemovedEventArgsMock<TKey, TValue> : NotifyDictionaryTransactionalEventArgsMock<TKey, TValue>
    {
        public NotifyDictionaryItemRemovedEventArgsMock(TransactionMock transaction, TKey key) : base(transaction, NotifyDictionaryChangedAction.Add)
        {
            this.Key = key;
        }
        public TKey Key { get; }
    }
}
