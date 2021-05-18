namespace Bus.Abstractions
{
    public interface IBusPersistentConnection<out TModel>
    {
        TModel CreateModel();
    }
}