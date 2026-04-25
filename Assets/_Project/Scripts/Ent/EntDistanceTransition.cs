using UnityEngine;

public class EntDistanceTrransition : EntBaseFSMTransition
{
    [SerializeField] private float minDistance = 1.0f;

    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.HasCurrentTarget()) return false;

        float currentDistance = Vector3.Distance(_controller.transform.position, _controller.CurrentTarget.position);

        return currentDistance <= minDistance;
    }
}
