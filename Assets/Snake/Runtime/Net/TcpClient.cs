using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Snake.Net
{
    public class TcpClient<T>
    {
        private readonly Socket _socket = new(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationToken _cancellationToken = CancellationToken.None;
        private Session<T> _session;

        public async Task Connect(IPEndPoint point, IHandler<T> handler, IEncoder<T> encoder, IDecoder<T> decoder)
        {
            Console.WriteLine("开始连接服务器");
            _socket.NoDelay = true;
            await _socket.ConnectAsync(point);
            _session = new Session<T>(_socket, handler, encoder, decoder);
            Console.WriteLine("连接成功");
            _session.Start(4096, _cancellationToken);
        }

        public async void Send(T msg)
        {
            await _session.Send(msg);
        }
    }
}