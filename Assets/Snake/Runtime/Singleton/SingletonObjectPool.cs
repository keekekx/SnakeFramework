namespace Snake.Singleton
{
    public static class SingletonObjectPool<T> where T : class, ISingleton, new()
    {
        private static T _instance;

        public static T Get()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new T();
            _instance.OnSingletonInit();

            return _instance;
        }

        public static void Return()
        {
            _instance.OnSingletonDispose();
            _instance = null;
        }
    }
}