using UnityEngine;

public class EntAttackState : EntBaseFSMState
{
    [SerializeField] private float _timeBetweenAttacks = 2f;
    [SerializeField] private float _rotationSpeed = 8f;

    private float _nextAttackTime;

    public override void OnStateEnter()
    {
        if (_controller == null || _controller.agent == null) return;

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
        if (_controller == null || _controller.agent == null || !_controller.isAlive) return;

        _controller.ValidateCurrentTarget();

        Transform target = _controller.CurrentTarget;
        if (target == null) return;
        Vector3 direction = target.position - _controller.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
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
