using System;

namespace Snake.Event
{
    
    public interface IEventListener
    {
    }

    public class EventListener<T> : IEventListener
    {
        public event Action<T> Actions;

        public void OnReceive(T obj)
        {
            Actions?.Invoke(obj);
        }
    }
}