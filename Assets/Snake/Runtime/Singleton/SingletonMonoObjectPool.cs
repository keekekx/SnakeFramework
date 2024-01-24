using UnityEngine;

namespace Snake.Singleton
{
    public static class SingletonMonoObjectPool<T> where T : Component, ISingleton, new()
    {
        private static T _instance;

        public static T Get()
        {
            if (_instance != null)
            {
                return _instance;
            }

            var go = new GameObject(typeof(T).Name);
            _instance = go.AddComponent<T>();
            _instance.OnSingletonInit();

            return _instance;
        }

        public static void Return()
        {
            _instance.OnSingletonDispose();
            Object.Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}