using UnityEngine;

public class GreaterDistanceTransition : BaseFSMTransition
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _maxDistance = 1.5f;

    public override bool IsConditionMet()
    {
        if (_controller == null || _target == null) return false;

        float currentDistance = Vector3.Distance(_controller.transform.position, _target.position);

        return currentDistance > _maxDistance;
    }
}