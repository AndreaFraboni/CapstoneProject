using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;

    private void Awake()
    {
        if (_pc == null) _pc = GetComponentInParent<PlayerController>();
    }

    public void AE_FootStepSfx()
    {
        AudioManager.Instance.PlaySFX("FootStepSound");
    }

    public void AE_DestroyGameObject()
    {
        if (_pc == null) return;
        _pc.DestroyGOPlayer();
    }


}
