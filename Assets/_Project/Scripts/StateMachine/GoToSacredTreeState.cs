using UnityEngine;

public class GoToSacredTreeState : BaseFSMState
{
    [SerializeField] private Transform _sacredTree;

    public override void OnStateEnter()
    {
        if (_controller.agent == null || _sacredTree == null) return;

        _controller.agent.isStopped = false;
        _controller.agent.SetDestination(_sacredTree.position);
    }

    public override void StateUpdate()
    {
    }

    public override void OnStateExit()
    {

    }

}
