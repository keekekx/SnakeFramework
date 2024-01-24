using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Snake.Logger;

namespace Snake.Net
{
    public class TcpServer<T>
    {
        private readonly Socket _socket = new(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public async void Start(int port, Func<Socket, TcpClient<T>> factory)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(100);
            while (!_cancellationToken.IsCancellationRequested)
            {
                Log.Debug("等待客户端");
                var socket = await _socket.AcceptAsync();
                Log.Debug("客户端进入服务器");
                var client = factory?.Invoke(socket);
                client!.ClientStart();
            }
        }
    }
}