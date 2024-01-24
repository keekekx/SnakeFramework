using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Snake.Logger;

namespace Snake.Net
{
    public class TcpClient<T>
    {
        private Socket _socket;
        private readonly IPackageProxy<T> _packageProxy;
        private const int BufferMax = 4096;
        private readonly byte[] _buffer;
        private int _readPointer;
        private int _buffedCount;

        private IPEndPoint _remote;

        public bool Connected => _packageProxy.Connected;

        public TcpClient(IPackageProxy<T> proxy, Socket socket = null)
        {
            _packageProxy = proxy;
            _socket = socket;
            _buffer = new byte[BufferMax];
        }

        public async Task Reconnect()
        {
            if (_remote == null)
            {
                Log.Warn("服务器维护的客户端链接，不需要重连");
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(3f));
            await Connect(_remote);
        }

        public async Task Connect(IPEndPoint point)
        {
            _remote = point;
            try
            {
                _socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.NoDelay = true;
                Log.Debug("开始连接服务器");
                await _socket.ConnectAsync(point);
                _packageProxy.Active(this);
            }
            catch (SocketException e)
            {
                Log.Error(e);
                _packageProxy.Inactive(this, -2);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void ClientStart()
        {
            _packageProxy.Active(this);
        }

        public void Receive()
        {
            Log.Debug("Receive");
            _socket.BeginReceive(_buffer, _readPointer, BufferMax - _buffedCount, SocketFlags.None, OnReceive,
                                 null);
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                var len = _socket.EndReceive(ar);
                _readPointer += len;
                _buffedCount += len;
                while (_buffedCount > 0)
                {
                    var used = _packageProxy.Decode(_buffer[.._buffedCount], out var msg);
                    if (used <= 0)
                    {
                        break;
                    }

                    Array.Copy(_buffer, used, _buffer, 0, _buffedCount - used);
                    _readPointer -= used;
                    _buffedCount -= used;
                    _packageProxy.ReadMessage(msg, this);
                }

                Receive();
            }
            catch (SocketException e)
            {
                Log.Error(e.SocketErrorCode);
                _packageProxy.Inactive(this, -1);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void Send(T msg)
        {
            try
            {
                _packageProxy.Encode(msg, out var buffer);
                _socket.Send(buffer);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void Close()
        {
            try
            {
                _packageProxy.Inactive(this, 0);
                if (_socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }

                _socket.Close();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}