using UnityEngine;

public abstract class EntBaseFSMTransition : MonoBehaviour
{
    [SerializeField] private EntBaseFSMState _targetState;

    protected EntFSMController _controller;

    public void Setup(EntFSMController controller)
    {
        _controller = controller;
    }

    public EntBaseFSMState GetTargetState() => _targetState;

    public abstract bool IsConditionMet();
}