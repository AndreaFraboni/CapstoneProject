using UnityEngine;

public class DetectNearstTargetTransition : BaseFSMTransition
{
    public override bool IsConditionMet()
    {
        if (_controller == null || !_controller.isAlive) return false;
        if (!_controller.IsCurrentTargetMainTarget()) return false;
        //TO DO
        // return _controller ==> TrySetNearstOtherTarget();
        return false;
    }
}
