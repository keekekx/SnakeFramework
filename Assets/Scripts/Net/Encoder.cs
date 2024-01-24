using System;
using System.Text;
using Snake.Net;

namespace Net
{
    public class Encoder : IEncoder<NetMessage>
    {
        public void Encode(NetMessage message, out byte[] data)
        {
            var str = Encoding.UTF8.GetBytes(message.Message);
            data = new byte[str.Length + 4];
            var b = BitConverter.GetBytes(str.Length);
            Array.Copy(b, data, 4);
            Array.Copy(str, 0, data, 4, str.Length);
        }
    }
}