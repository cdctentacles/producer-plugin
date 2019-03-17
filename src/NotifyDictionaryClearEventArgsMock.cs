using Microsoft.ServiceFabric.Data.Notifications;


namespace ProducerPlugin
{
    public class NotifyDictionaryClearEventArgsMock<TKey, TValue> : NotifyDictionaryChangedEventArgs<TKey, TValue>
    {
        public NotifyDictionaryClearEventArgsMock(long csn) : base(NotifyDictionaryChangedAction.Clear)
        {
            this.CommitSequenceNumber = CommitSequenceNumber;
        }
        public long CommitSequenceNumber { get;  }
    }

}
