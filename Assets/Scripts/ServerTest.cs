using System;
using System.Net;
using Net;
using Snake.Net;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    private TcpClient<NetMessage> _tcpClient;
    private TcpServer<NetMessage> _tcpServer;

    async void Start()
    {
        _tcpServer = new TcpServer<NetMessage>();
        _tcpServer.Start(8890, socket => new TcpClient<NetMessage>(new TcpPackageProxy(), socket));

        _tcpClient = new TcpClient<NetMessage>(new TcpPackageProxy());
        // var ip = IPAddress.Loopback;
        // await _tcpClient.Connect(new IPEndPoint(ip, 8890));
    }

    private void Update()
    {
        if (_tcpClient.Connected)
        {
            _tcpClient.Send(new NetMessage
            {
                Message = "Hello World"
            });
        }
    }

    private void OnDestroy()
    {
        _tcpClient.Close();
    }
}