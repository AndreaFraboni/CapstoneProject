public class EntIdleState : EntBaseFSMState
{
    public override void OnStateEnter()
    {
        if (_controller == null || _controller.agent == null) return;

        _controller.agent.isStopped = true;
        _controller.agent.ResetPath();
        _controller.StopAttack();

        if (_controller.anim != null)
        {
            _controller.anim.SetBool("walking", false);
            _controller.anim.SetBool("isAttacking", false);
        }
    }

    public override void StateUpdate()
    {
        if (_controller == null || _controller.agent == null || !_controller.isAlive) return;
    }

    public override void OnStateExit()
    {
        if (_controller == null || _controller.agent == null) return;

        _controller.agent.isStopped = false;
    }


}
