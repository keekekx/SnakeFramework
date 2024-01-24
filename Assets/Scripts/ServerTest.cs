using System.Net;
using Net;
using Snake.Net;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    private TcpClient<NetMessage> _tcpClient;

    async void Start()
    {
        _tcpClient = new TcpClient<NetMessage>();
        var ip = IPAddress.Loopback;
        await _tcpClient.Connect(new IPEndPoint(ip, 8890), new Handler(), new Encoder(), new Decoder());
        _tcpClient.Send(new NetMessage
        {
            Message = "hello world"
        });
    }
}