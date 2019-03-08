namespace ProducerPlugin
{
    public interface IMessageConverter
    {
        byte[] Serialize<T>(T t);
        T Deserialize<T>(byte[] b);
    }
}