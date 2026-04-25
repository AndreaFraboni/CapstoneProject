using UnityEngine;

public abstract class EntBaseFSMState : MonoBehaviour
{
    protected EntFSMController _controller;
    protected EntBaseFSMTransition[] _transitions;

    public void Setup(EntFSMController controller)
    {
        _controller = controller;
        _transitions = GetComponents<EntBaseFSMTransition>();
    }

    public EntBaseFSMTransition[] GetTransitions() => _transitions;

    public virtual void OnStateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void OnStateExit() { }
}
