using UnityEngine;

public class AttackSacredTreeState : BaseFSMState
{
    [SerializeField] private Transform _sacredTree;
    [SerializeField] private float _timeBetweenAttacks = 2f;
    [SerializeField] private float _rotationSpeed = 8f;

    private float _nextAttackTime;

    public override void OnStateEnter()
    {
        if (_controller.agent != null)
        {
            _controller.agent.isStopped = true;
            _controller.agent.ResetPath();
            _controller.agent.velocity = Vector3.zero;
        }

        _controller.StartPlayAttackAnimation();

        _nextAttackTime = Time.time + _timeBetweenAttacks;
    }

    public override void StateUpdate()
    {
        if (!_controller.isAlive) return;
        if (_sacredTree == null) return;

        Vector3 dirToSacredTree = _sacredTree.position - _controller.transform.position;
        dirToSacredTree.y = 0f;

        if (dirToSacredTree.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirToSacredTree.normalized);
            _controller.transform.rotation = Quaternion.Slerp(
                _controller.transform.rotation,
                targetRot,
                _rotationSpeed * Time.deltaTime
            );
        }
        
        if (Time.time >= _nextAttackTime)
        {
            _controller.StartPlayAttackAnimation();
            _nextAttackTime = Time.time + _timeBetweenAttacks;
        }
    }

    public override void OnStateExit()
    {
        _controller.StopAttack();

        if (_controller.agent != null) _controller.agent.isStopped = false;
    }
}
