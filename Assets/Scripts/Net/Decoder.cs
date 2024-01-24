using System;
using System.Text;
using Snake.Net;

namespace Net
{
    public class Decoder : IDecoder<NetMessage>
    {
        public int Decode(Memory<byte> buffer, out NetMessage output)
        {
            output = null!;
            if (buffer.Length < 4)
            {
                return 0;
            }

            var len = BitConverter.ToInt32(buffer[..4].Span);
            if (buffer.Length < len + 4)
            {
                return 0;
            }

            output = new NetMessage
            {
                Message = Encoding.UTF8.GetString(buffer.Slice(4, len).Span)
            };
            return len + 4;
        }
    }
}