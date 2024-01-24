namespace Snake.Net
{
    public interface IPackageProxy<T>
    {
        public bool Connected { get; set; }
        public int Decode(byte[] buffer, out T output);
        public void Encode(T message, out byte[] data);
        public void Active(TcpClient<T> client);
        public void ReadMessage(T message, TcpClient<T> client);
        public void Inactive(TcpClient<T> client, int state);
    }
}