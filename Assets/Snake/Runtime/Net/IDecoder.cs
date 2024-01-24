using System;

namespace Snake.Net
{
    public interface IDecoder<T>
    {
        public int Decode(Memory<byte> buffer, out T output);
    }
}