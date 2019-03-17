using Microsoft.ServiceFabric.Data.Notifications;

namespace LearningDeserialization
{
    public class NotifyDictionaryItemUpdatedEventArgsMock<TKey, TValue> : NotifyDictionaryTransactionalEventArgsMock<TKey, TValue>
    {
        public NotifyDictionaryItemUpdatedEventArgsMock(TransactionMock transaction, TKey key, TValue value) : base(transaction, NotifyDictionaryChangedAction.Add)
        {
            this.Key = key;
            this.Value = value;
        }
        public TKey Key { get; }
        public TValue Value { get; }
    }
}
