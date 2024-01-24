using UnityEngine;

namespace Snake.Logger
{
    public class UnityLogger : ILogger
    {
        private readonly string _tag;

        public UnityLogger(string tag)
        {
            _tag = tag;
        }

        public void Debug(object message, Object obj)
        {
            UnityEngine.Debug.Log($"[{_tag}][Debug]{message}", obj);
        }

        public void Info(object message, Object obj)
        {
            UnityEngine.Debug.Log($"[{_tag}][Info]{message}", obj);
        }

        public void Warn(object message, Object obj)
        {
            UnityEngine.Debug.LogWarning($"[{_tag}][Warn]{message}", obj);
        }

        public void Error(object message, Object obj)
        {
            UnityEngine.Debug.LogError($"[{_tag}][Error]{message}", obj);
        }
    }
}