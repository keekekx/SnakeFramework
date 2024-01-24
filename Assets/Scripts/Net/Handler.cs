using System;
using Snake.Logger;
using Snake.Net;

namespace Net
{
    public class Handler : IHandler<NetMessage>
    {
        public void Active(Session<NetMessage> session)
        {
            Log.Info("客户端接入");
        }

        public void ReadMessage(Session<NetMessage> session, NetMessage message)
        {
            Log.Info(message.Message);
        }

        public void Inactive(Session<NetMessage> session)
        {
            Log.Info("客户端断开");
        }
    }
}