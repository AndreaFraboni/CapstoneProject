using UnityEngine;

public class DistanceTransition : BaseFSMTransition
{
    [SerializeField] private float minDistance = 1.0f;

    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.HasCurrentTarget()) return false;

        float currentDistance = Vector3.Distance(_controller.transform.position, _controller.CurrentTarget.position);

        if (_controller.CurrentTarget.CompareTag(Tags.Player)) minDistance = 0.5f;
        if (!_controller.CurrentTarget.CompareTag(Tags.Player)) minDistance = 1.0f;

        return currentDistance <= minDistance;
    }
}
