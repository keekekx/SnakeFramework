using System;
using System.Text;
using Snake.Logger;
using Snake.Net;

namespace Net
{
    public class TcpPackageProxy : IPackageProxy<NetMessage>
    {
        public bool Connected { get; set; }

        public int Decode(byte[] buffer, out NetMessage output)
        {
            output = null!;
            if (buffer.Length < 4)
            {
                return 0;
            }

            var len = BitConverter.ToInt32(buffer[..4]);
            if (len == 0)
            {
                return 0;
            }

            var end = len + 4;
            if (buffer.Length < end)
            {
                return 0;
            }

            output = new NetMessage
            {
                Message = Encoding.UTF8.GetString(buffer[4..end])
            };
            return end;
        }

        public void Encode(NetMessage message, out byte[] data)
        {
            var str = Encoding.UTF8.GetBytes(message.Message);
            data = new byte[str.Length + 4];
            var b = BitConverter.GetBytes(str.Length);
            Array.Copy(b, data, 4);
            Array.Copy(str, 0, data, 4, str.Length);
        }

        public void Active(TcpClient<NetMessage> client)
        {
            Log.Debug("连接成功");
            client.Receive();
            Connected = true;
        }

        public void ReadMessage(NetMessage message)
        {
            Log.Debug(message.Message);
        }

        public async void Inactive(TcpClient<NetMessage> client, int state)
        {
            Connected = false;
            Log.Debug("连接断开");
            if (state == 0)
            {
                return;
            }

            await client.Reconnect();
        }
    }
}