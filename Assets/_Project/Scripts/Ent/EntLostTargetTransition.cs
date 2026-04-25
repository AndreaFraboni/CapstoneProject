public class EntLostTargetTransition : EntBaseFSMTransition
{
    public override bool IsConditionMet()
    {
        if (!_controller.HasCurrentTarget()) return true;
        return false;
    }
}
