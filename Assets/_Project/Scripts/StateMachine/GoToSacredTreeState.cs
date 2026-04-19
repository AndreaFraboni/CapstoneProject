using UnityEngine;

public class GoToSacredTreeState : BaseFSMState
{
    [SerializeField] private Transform _sacredTree;

    public override void OnStateEnter()
    {
        if (_controller.agent == null || _sacredTree == null) return;

        _controller.StopAttack();
        _controller.agent.isStopped = false;
        _controller.agent.ResetPath();
        //_controller.agent.SetDestination(_sacredTree.position);
        Vector3 randomOffset = Random.insideUnitSphere * 1.5f;
        randomOffset.y = 0f;
        _controller.agent.SetDestination(_sacredTree.position + randomOffset);
    }

    public override void StateUpdate()
    {
    }

    public override void OnStateExit()
    {

    }

}
