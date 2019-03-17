using Microsoft.ServiceFabric.Data.Notifications;

namespace ProducerPlugin
{
    public class NotifyDictionaryTransactionalEventArgsMock<TKey, TValue> : NotifyDictionaryChangedEventArgs<TKey, TValue>
    {
        public NotifyDictionaryTransactionalEventArgsMock(TransactionMock transaction, NotifyDictionaryChangedAction action): base(action)
        {
            this.transaction = transaction;
        }
        public TransactionMock transaction;
    }
}
