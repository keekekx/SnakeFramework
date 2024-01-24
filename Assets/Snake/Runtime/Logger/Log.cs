namespace Snake.Logger
{
    public static class Log
    {
        private static ILogger _logger;

        public static void Debug(object message, object obj = null)
        {
            _logger?.Debug(message, obj);
        }

        public static void Info(object message, object obj = null)
        {
            _logger?.Info(message, obj);
        }

        public static void Warn(object message, object obj = null)
        {
            _logger?.Warn(message, obj);
        }

        public static void Error(object message, object obj = null)
        {
            _logger?.Error(message, obj);
        }
    }
}