using System;

namespace Snake.Event
{
    public interface IEventHandler
    {
    }

    public class EventHandler<T1, T2> : IEventHandler where T1 : IEvent where T2 : IEvent
    {
        public event Func<T1, T2> Func;

        public T2 OnRequest(T1 obj)
        {
            return Func!.Invoke(obj);
        }
    }
}