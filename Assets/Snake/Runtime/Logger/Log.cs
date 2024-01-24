using UnityEngine;

namespace Snake.Logger
{
    public static class Log
    {
        public enum ELevel
        {
            All,
            Debug,
            Info,
            Warn,
            Error,
        }

        public static ILogger Logger { get; set; }
        public static ELevel Level { get; set; } = ELevel.All;

        public static void Debug(object message, Object obj = null)
        {
            if (Level > ELevel.Debug)
            {
                return;
            }

            Logger?.Debug(message, obj);
        }

        public static void Info(object message, Object obj = null)
        {
            if (Level > ELevel.Info)
            {
                return;
            }

            Logger?.Info(message, obj);
        }

        public static void Warn(object message, Object obj = null)
        {
            if (Level > ELevel.Warn)
            {
                return;
            }

            Logger?.Warn(message, obj);
        }

        public static void Error(object message, Object obj = null)
        {
            if (Level > ELevel.Error)
            {
                return;
            }

            Logger?.Error(message, obj);
        }
    }
}