using UnityEngine;

public class DistanceTransition : BaseFSMTransition
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minDistance = 1f;

    public override bool IsConditionMet()
    {
        if (_controller == null || _target == null) return false;

        float currentDistance = Vector3.Distance(_controller.transform.position, _target.position);

        if (currentDistance <= _minDistance) return true;

        return false;
    }
}
