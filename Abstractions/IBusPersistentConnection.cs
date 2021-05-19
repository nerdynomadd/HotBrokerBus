namespace HotBrokerBus.Abstractions
{
    public interface IBusPersistentConnection<out TModel>
    {
        TModel CreateModel();
    }
}