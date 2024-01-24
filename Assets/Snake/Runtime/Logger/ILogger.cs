using UnityEngine;

namespace Snake.Logger
{
    public interface ILogger
    {
        public void Debug(object message, Object obj)
        {
            
        }

        public void Info(object message, Object obj)
        {
        }

        public void Warn(object message, Object obj)
        {
        }

        public void Error(object message, Object obj)
        {
        }
    }
}