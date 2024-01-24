using System;
using Snake.Singleton;

namespace Snake.IAP
{
    public partial class IAPManager : ISingleton
    {
        public static IAPManager Instance => SingletonObjectPool<IAPManager>.Get();
        
        public event Action<string> OnRequestPurchase;
        public event Action<string> OnConfirmPurchase;
        public event Func<string, string> OnGetProductPriceString;

        /// <summary>
        /// 获取商品价格
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public string GetProductPriceString(string productId)
        {
            return OnGetProductPriceString?.Invoke(productId);
        }

        /// <summary>
        /// 购买请求
        /// </summary>
        /// <param name="productId"></param>
        public void RequestPurchase(string productId)
        {
            OnRequestPurchase?.Invoke(productId);
        }

        /// <summary>
        /// 确认购买
        /// </summary>
        /// <param name="productId"></param>
        public void ConfirmPurchase(string productId)
        {
            OnConfirmPurchase?.Invoke(productId);
        }
    }
}