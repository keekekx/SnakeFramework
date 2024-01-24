namespace Snake.Net
{
    public interface IEncoder<in T>
    {
        public void Encode(T message, out byte[] data);
    }
}