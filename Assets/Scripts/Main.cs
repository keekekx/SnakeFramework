using System.Collections;
using Snake.State;

public class Main : IState
{
    public IEnumerator OnEnter()
    {
        TestStateMachine.Machine.ChangeTo<Home>();
        yield return null;
    }

    public IEnumerator OnExit()
    {
        yield return null;
    }
}
