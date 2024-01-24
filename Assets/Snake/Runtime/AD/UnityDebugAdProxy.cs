using Snake.Logger;

namespace Snake.AD
{
    public class UnityDebugAdProxy : IAdProxy
    {
        public bool IsReady
        {
            get => true;
            set => throw new System.NotImplementedException();
        }

        public void Show()
        {
            Log.Debug("开始展示广告");
        }
    }
}