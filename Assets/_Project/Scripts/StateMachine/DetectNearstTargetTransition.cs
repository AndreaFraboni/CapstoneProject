using UnityEngine;

public class DetectNearstTargetTransition : BaseFSMTransition
{
    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.isAlive) return false;
        if (!_controller.IsCurrentTargetMainTarget()) return false;

        GameObject newTarget = _controller.CheckNewTarget();

        if (newTarget == null) return false;

        _controller.SetCurrentTarget(newTarget.transform);

        return true;
    }
}
