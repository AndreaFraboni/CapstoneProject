using UnityEngine;

public class DistanceTransition : BaseFSMTransition
{
    [SerializeField] private float _minDistance = 1f;

    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.HasCurrentTarget()) return false;
        float currentDistance = Vector3.Distance(_controller.transform.position, _controller.CurrentTarget.position);
        return currentDistance <= _minDistance;
    }
}
