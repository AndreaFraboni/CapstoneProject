using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;
    [SerializeField] private EnemyFSMController _ec;

    private void Awake()
    {
        if (_pc == null) _pc = GetComponentInParent<PlayerController>();
    }

    public void AE_FootStepSfx()
    {
        AudioManager.Instance.PlayFootsteps("FootStepSound");
    }

    public void AE_DestroyGameObject()
    {
        if (_pc == null) return;
        _pc.DestroyGOPlayer();
    }

    public void AE_DestroyEnemyGameObject()
    {
        if (_ec == null) return;
        _ec.DestroyGOEnemy();
    }


}
