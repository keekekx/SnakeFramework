using System.Collections;

namespace Snake.State
{
    public interface IState
    {
        public IEnumerator OnEnter()
        {
            yield return null;
        }

        public IEnumerator OnExit()
        {
            yield return null;
        }
    }
}