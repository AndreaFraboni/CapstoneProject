public class EntIdleToMoveTransition : EntBaseFSMTransition
{
    public override bool IsConditionMet()
    {
        if (_controller == null) return false;
        if (!_controller.isAlive) return false;

        if (_controller.HasCurrentTarget()) return true;

        return false;
    }
}
