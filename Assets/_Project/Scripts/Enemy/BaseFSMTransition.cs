using UnityEngine;

public abstract class BaseFSMTransition : MonoBehaviour
{
    [SerializeField] private BaseFSMState _targetState;

    protected EnemyFSMController _controller;

    public void Setup(EnemyFSMController controller)
    {
        _controller = controller;
    }

    public BaseFSMState GetTargetState() => _targetState;

    public abstract bool IsConditionMet();
}