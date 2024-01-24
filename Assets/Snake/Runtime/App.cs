using System;
using System.Collections.Generic;
using Snake.Logger;
using Snake.Singleton;
using UnityEngine;

namespace Snake
{
    public partial class App : MonoBehaviour, ISingleton
    {
        public static App Instance => SingletonMonoObjectPool<App>.Get();

        #region Blackboard

        private readonly Dictionary<string, object> _sharedBlackboard = new Dictionary<string, object>();

        public T GetBlackboard<T>(string key)
        {
            return _sharedBlackboard.TryGetValue(key, out var val) ? (T)val : default;
        }

        public void SetBlackboard<T>(string key, T value)
        {
            _sharedBlackboard[key] = value;
        }

        #endregion

        /// <summary>
        /// 程序暂停通知
        /// </summary>
        public event Action<bool> OnApplicationPauseEvent;

        public void OnSingletonInit()
        {
            DontDestroyOnLoad(gameObject);
            RegisterTimeUpdate("root", DelayCallUpdate);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnApplicationPauseEvent?.Invoke(pauseStatus);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            TimeScalingUpdate(dt);
        }

        #region Module

        private readonly Dictionary<int, Module> _modules = new Dictionary<int, Module>();

        public void Import<T>() where T : Module, new()
        {
            var code = typeof(T).GetHashCode();
            if (_modules.ContainsKey(code))
            {
                throw new FieldAccessException("模块已存在");
            }

            var module = new T();
            module.OnEnable();
            _modules.Add(code, module);
        }

        #endregion

        #region DelayCall

        private class DelayActionContext
        {
            public float RemainingTime;
            public Action Action;
        }

        private readonly List<DelayActionContext> _delayActionContexts = new List<DelayActionContext>();

        /// <summary>
        /// 主线程调用，用于各类SDK的回调
        /// </summary>
        /// <param name="action"></param>
        public void MainThreadCall(Action action)
        {
            DelayCall(0f, action);
        }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="action"></param>
        public void DelayCall(float delay, Action action)
        {
            _delayActionContexts.Add(new DelayActionContext
            {
                RemainingTime = delay,
                Action = action
            });
        }

        private void DelayCallUpdate(float dt)
        {
            for (var i = 0; i < _delayActionContexts.Count;)
            {
                var ctx = _delayActionContexts[i];
                ctx.RemainingTime -= dt;
                if (ctx.RemainingTime > 0f)
                {
                    i++;
                    continue;
                }

                try
                {
                    ctx.Action.Invoke();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                _delayActionContexts.RemoveAt(i);
            }
        }

        #endregion

        #region Time Scaling

        private class TimeScalingContext
        {
            public float Scaling = 1f;

            private readonly Dictionary<string, TimeScalingContext> _nodes =
                new Dictionary<string, TimeScalingContext>();

            private readonly List<Action<float>> _add = new List<Action<float>>();
            private readonly List<Action<float>> _list = new List<Action<float>>();
            private readonly List<Action<float>> _remove = new List<Action<float>>();

            public TimeScalingContext this[string key]
            {
                get
                {
                    if (_nodes.TryGetValue(key, out var node))
                    {
                        return node;
                    }

                    node = new TimeScalingContext();
                    _nodes.Add(key, node);
                    return node;
                }
            }

            public void Register(Action<float> action)
            {
                _add.Add(action);
            }

            public void Unregister(Action<float> action)
            {
                _remove.Add(action);
            }

            public void OnUpdate(float dt)
            {
                dt *= Scaling;

                foreach (var action in _add)
                {
                    _list.Add(action);
                }

                _add.Clear();

                foreach (var action in _list)
                {
                    action?.Invoke(dt);
                }

                foreach (var action in _remove)
                {
                    _list.Remove(action);
                }

                _remove.Clear();

                foreach (var ctx in _nodes.Values)
                {
                    ctx.OnUpdate(dt);
                }
            }
        }

        private readonly TimeScalingContext _root = new TimeScalingContext();

        public void RegisterTimeUpdate(string path, Action<float> action)
        {
            var c = _root;
            var nodes = path.Split('/');
            if (nodes.Length == 1)
            {
                c.Register(action);
                return;
            }

            nodes = nodes[1..];
            foreach (var s in nodes)
            {
                c = c[s];
            }

            c.Register(action);
        }

        public void UnregisterTimeUpdate(string path, Action<float> action)
        {
            var c = _root;
            var nodes = path.Split('/');
            if (nodes.Length == 1)
            {
                c.Unregister(action);
                return;
            }

            nodes = nodes[1..];
            foreach (var s in nodes)
            {
                c = c[s];
            }

            c.Unregister(action);
        }

        private void TimeScalingUpdate(float dt)
        {
            _root.OnUpdate(dt);
        }

        #endregion
    }
}