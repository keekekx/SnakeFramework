using System;
using System.Collections.Generic;
using Snake.Singleton;

namespace Snake.Event
{
    public class EventManager : ISingleton
    {
        public static EventManager Instance => SingletonObjectPool<EventManager>.Get();

        #region Event

        private readonly Dictionary<Type, IEventListener> _eventListeners = new Dictionary<Type, IEventListener>();

        public void Register<T>(Action<T> action) where T : IEvent
        {
            var objType = typeof(T);
            if (!_eventListeners.TryGetValue(objType, out var listener))
            {
                listener = new EventListener<T>();
                _eventListeners.Add(objType, listener);
            }

            ((EventListener<T>)listener).Actions += action;
        }

        public void Unregister<T>(Action<T> action) where T : IEvent
        {
            var objType = typeof(T);
            if (!_eventListeners.TryGetValue(objType, out var listener))
            {
                return;
            }

            ((EventListener<T>)listener).Actions -= action;
        }

        public void Broadcast<T>(T obj) where T : IEvent
        {
            var objType = typeof(T);
            if (!_eventListeners.TryGetValue(objType, out var listener))
            {
                return;
            }

            ((EventListener<T>)listener).OnReceive(obj);
        }

        #endregion

        #region Handler

        private readonly Dictionary<Type, Dictionary<Type, IEventHandler>> _eventHandlers =
            new Dictionary<Type, Dictionary<Type, IEventHandler>>();


        public void RegisterHandler<T1, T2>(Func<T1, T2> func) where T1 : IEvent where T2 : IEvent
        {
            var objType = typeof(T1);
            if (!_eventHandlers.TryGetValue(objType, out var handleDict))
            {
                handleDict = new Dictionary<Type, IEventHandler>();
                _eventHandlers.Add(objType, handleDict);
            }

            var retType = typeof(T2);
            if (!handleDict.TryGetValue(retType, out var handler))
            {
                handler = new EventHandler<T1, T2>();
                handleDict.Add(retType, handler);
            }

            ((EventHandler<T1, T2>)handler).Func += func;
        }

        public void UnregisterHandler<T1, T2>(Func<T1, T2> func) where T1 : IEvent where T2 : IEvent
        {
            var objType = typeof(T1);
            if (!_eventHandlers.TryGetValue(objType, out var handleDict))
            {
                return;
            }

            var retType = typeof(T2);
            if (!handleDict.TryGetValue(retType, out var handler))
            {
                return;
            }

            ((EventHandler<T1, T2>)handler).Func -= func;
        }

        public T2 Request<T1, T2>(T1 obj) where T1 : IEvent where T2 : IEvent
        {
            var objType = typeof(T1);
            if (!_eventHandlers.TryGetValue(objType, out var handleDict))
            {
                return default;
            }

            var retType = typeof(T2);
            if (!handleDict.TryGetValue(retType, out var handler))
            {
                return default;
            }

            return ((EventHandler<T1, T2>)handler).OnRequest(obj);
        }

        #endregion
    }
}