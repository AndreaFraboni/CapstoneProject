using UnityEngine;

public class EntTargetTooFarTransition : EntBaseFSMTransition
{
    [SerializeField] private float maxAttackDistance = 1.0f;

    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.HasCurrentTarget()) return false;

        float distance = Vector3.Distance(_controller.transform.position, _controller.CurrentTarget.position);

        return distance > maxAttackDistance;
    }
}