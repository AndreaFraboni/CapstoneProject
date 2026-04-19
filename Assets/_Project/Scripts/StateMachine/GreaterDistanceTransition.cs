using UnityEngine;

public class GreaterDistanceTransition : BaseFSMTransition
{
    [SerializeField] private float _maxDistance = 1.5f;

    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.HasCurrentTarget()) return false;
        float currentDistance = Vector3.Distance(_controller.transform.position, _controller.CurrentTarget.position);
        return currentDistance > _maxDistance;
    }
}