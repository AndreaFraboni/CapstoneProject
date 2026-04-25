using UnityEngine;

public abstract class BaseFSMState : MonoBehaviour
{
    protected EnemyFSMController _controller;
    protected BaseFSMTransition[] _transitions;

    public void Setup(EnemyFSMController controller)
    {
        _controller = controller;
        _transitions = GetComponents<BaseFSMTransition>();
    }

    public BaseFSMTransition[] GetTransitions() => _transitions;

    public virtual void OnStateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void OnStateExit() { }
}
