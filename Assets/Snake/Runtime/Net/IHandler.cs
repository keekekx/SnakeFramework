namespace Snake.Net
{
    public interface IHandler<T>
    {
        public void Active(Session<T> session);
        public void ReadMessage(Session<T> session, T message);
        public void Inactive(Session<T> session);
    }
}