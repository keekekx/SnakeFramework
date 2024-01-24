using System.Collections.Generic;
using Snake.Singleton;

namespace Snake.AD
{
    public partial class AdManager : ISingleton
    {
        public static AdManager Instance => SingletonObjectPool<AdManager>.Get();

        private readonly Dictionary<string, IAdProxy> _adProxies = new Dictionary<string, IAdProxy>();

        public void Register(string key, IAdProxy adProxy)
        {
            _adProxies.Add(key, adProxy);
        }

        public void Show(string key)
        {
            if (!_adProxies.TryGetValue(key, out var proxy))
            {
                return;
            }

            proxy.Show();
        }
    }
}