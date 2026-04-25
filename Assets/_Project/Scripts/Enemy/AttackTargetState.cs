using UnityEngine;

public class AttackTargetState : BaseFSMState
{
    [SerializeField] private float _timeBetweenAttacks = 2f;
    [SerializeField] private float _rotationSpeed = 8f;

    private float _nextAttackTime;

    public override void OnStateEnter()
    {
        if (_controller == null) return;

        _controller.ValidateCurrentTarget();

        if (_controller.agent != null)
        {
            _controller.agent.isStopped = true;
            _controller.agent.ResetPath();
            _controller.agent.velocity = Vector3.zero;
        }

        if (_controller.HasCurrentTarget()) _controller.StartPlayAttackAnimation();
        _nextAttackTime = Time.time + _timeBetweenAttacks;
    }

    public override void StateUpdate()
    {
        if (_controller == null || !_controller.isAlive) return;

        _controller.ValidateCurrentTarget();

        Transform target = _controller.CurrentTarget;
        if (target == null) return;
        Vector3 dirTotarget = target.position - _controller.transform.position;
        dirTotarget.y = 0f;

        if (dirTotarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirTotarget.normalized);
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }

        if (Time.time >= _nextAttackTime)
        {
            if (_controller.HasCurrentTarget()) _controller.StartPlayAttackAnimation();
            _nextAttackTime = Time.time + _timeBetweenAttacks;
        }
    }

    public override void OnStateExit()
    {
        _controller.StopAttack();
        if (_controller.agent != null) _controller.agent.isStopped = false;
    }
}
