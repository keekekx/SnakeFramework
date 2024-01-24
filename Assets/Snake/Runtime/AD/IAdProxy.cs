namespace Snake.AD
{
    public interface IAdProxy
    {
        public bool IsReady { get; set; }
        public void Show();

        public void OnShow()
        {
        }

        public void OnFailure()
        {
        }

        public void OnSuccess()
        {
        }

        public void OnClose()
        {
        }
    }
}