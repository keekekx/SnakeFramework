using Snake;
using Snake.Logger;
using Snake.State;
using UnityEngine;

public class TestStateMachine : MonoBehaviour
{
    public static StateMachine Machine;

    public string Version = "1.0.0";

    private void Start()
    {
        App.Instance.SetBlackboard("version", Version);
        Log.Logger = new UnityLogger("Test");
        Log.Level = Log.ELevel.Debug;
        Machine = new StateMachine();
        Machine.Add(new Main());
        Machine.Add(new Home());

        Machine.OnStateSwitchStart += (c, n) => { Log.Debug($"{c}->{n} Start"); };
        Machine.OnStateSwitchEnd += (c, n) => { Log.Debug($"{c}->{n} End"); };

        Machine.ChangeTo<Main>();
        
        Log.Debug(App.Instance.GetBlackboard<string>("version"));
    }
}