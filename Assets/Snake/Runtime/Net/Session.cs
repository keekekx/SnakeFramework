using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Snake.Logger;

namespace Snake.Net
{
    public class Session<T>
    {
        private readonly Socket _socket;
        private readonly IHandler<T> _handler;
        private readonly IEncoder<T> _encoder;
        private readonly IDecoder<T> _decoder;

        public Session(Socket socket, IHandler<T> handler, IEncoder<T> encoder, IDecoder<T> decoder)
        {
            _socket = socket;
            _handler = handler;
            _encoder = encoder;
            _decoder = decoder;
        }

        public async void Start(int bufferSize, CancellationToken cancellationToken)
        {
            _handler.Active(this);

            var buffer = new Memory<byte>(new byte[bufferSize]);
            var pointer = 0;
            var totalLen = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var len = await _socket.ReceiveAsync(buffer.Slice(pointer, bufferSize - pointer), SocketFlags.None,
                                                         cancellationToken);
                    pointer += len;
                    totalLen += len;
                    while (totalLen >= 4)
                    {
                        var used = _decoder.Decode(buffer[..totalLen], out var msg);
                        if (used <= 0)
                        {
                            break;
                        }

                        buffer[used..].CopyTo(buffer);
                        pointer -= used;
                        totalLen -= used;
                        _handler.ReadMessage(this, msg);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    break;
                }
            }

            _handler.Inactive(this);
        }

        public async Task Send(T msg)
        {
            _encoder.Encode(msg, out var data);
            await _socket.SendAsync(data, SocketFlags.None);
        }
    }
}