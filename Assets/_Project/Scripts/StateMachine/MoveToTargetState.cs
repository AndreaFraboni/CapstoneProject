using UnityEngine;

public class MoveToTargetState : BaseFSMState
{
    public override void OnStateEnter()
    {
        if (_controller == null || _controller.agent == null) return;

        _controller.ValidateCurrentTarget();

        _controller.StopAttack();
        _controller.agent.isStopped = false;
        _controller.agent.ResetPath();

        if (_controller.CurrentTarget != null) _controller.agent.SetDestination(_controller.CurrentTarget.position);

        //Vector3 randomOffset = Random.insideUnitSphere * 1.5f;
        //randomOffset.y = 0f;
        //_controller.agent.SetDestination(_sacredTree.position + randomOffset);

    }

    public override void StateUpdate()
    {
        if (_controller == null || _controller.agent == null || !_controller.isAlive) return;

        _controller.ValidateCurrentTarget();

        if (_controller.CurrentTarget == null) return;

        if (!_controller.agent.pathPending)
        {
            _controller.agent.SetDestination(_controller.CurrentTarget.position);
        }
    }

    public override void OnStateExit()
    {

    }

}
