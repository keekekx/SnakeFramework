using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Logger;
using UnityEngine;

namespace Snake.State
{
    public class StateMachine
    {
        public event Action<IState, IState> OnStateSwitchStart;
        public event Action<IState, IState> OnStateSwitchEnd;

        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

        private IState _current;
        private readonly Queue<IState> _stateQueue = new Queue<IState>();
        private Coroutine _switchPipeline;

        public void Add<T>(T state) where T : IState
        {
            var t = typeof(T);
            try
            {
                _states.Add(t, state);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void ChangeTo<T>()
        {
            var t = typeof(T);
            if (!_states.TryGetValue(t, out var state))
            {
                throw new ArgumentException("状态不存在");
            }

            _stateQueue.Enqueue(state);
            if (_switchPipeline != null)
            {
                return;
            }

            _switchPipeline = App.Instance.StartCoroutine(StateSwitchPipeline());
        }

        private IEnumerator StateSwitchPipeline()
        {
            yield return null;

            while (_stateQueue.Count > 0)
            {
                var cur = _current;
                var next = _stateQueue.Dequeue();
                OnStateSwitchStart?.Invoke(_current, next);
                if (cur != null)
                {
                    yield return cur.OnExit();
                }

                yield return next.OnEnter();
                _current = next;
                OnStateSwitchEnd?.Invoke(cur, next);
            }

            _switchPipeline = null;
        }
    }
}