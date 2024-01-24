using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Snake.Net
{
    public class TcpServer
    {
        private readonly Socket _socket = new(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public async Task Start<T>(int port, IHandler<T> handler, IEncoder<T> encoder, IDecoder<T> decoder)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(100);
            while (!_cancellationToken.IsCancellationRequested)
            {
                var socket = await _socket.AcceptAsync();
                var session = new Session<T>(socket, handler, encoder, decoder);
                session.Start(4096, _cancellationToken);
            }
        }
    }
}