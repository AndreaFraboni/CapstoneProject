using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    public void AE_FootStepSfx()
    {
        AudioManager.Instance.PlaySFX("FootStepSound");
    }

}
