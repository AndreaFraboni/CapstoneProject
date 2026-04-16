using UnityEngine;

public class AttackSacredTreeState : BaseFSMState
{
    [SerializeField] private Transform _sacredTree;

    public override void OnStateEnter()
    {
        if (_controller.agent != null)
        {
            _controller.agent.isStopped = true;
            _controller.agent.ResetPath();
        }
    }

    public override void StateUpdate()
    {
        if (_sacredTree == null) return;

        Vector3 dirToSacredTree = _sacredTree.position - _controller.transform.position;
        dirToSacredTree.y = 0f;


        if (dirToSacredTree != Vector3.zero)
        {
            _controller.transform.rotation = Quaternion.LookRotation(dirToSacredTree);
        }

    }

    public override void OnStateExit()
    {
        if (_controller.agent != null)
        {
            _controller.agent.isStopped = false;
        }
    }



}
