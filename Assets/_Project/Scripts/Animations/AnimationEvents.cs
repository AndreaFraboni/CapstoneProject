using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;
    [SerializeField] private EnemyFSMController _ec;
    [SerializeField] private EntFSMController _entc;

    private void Awake()
    {
        if (_pc == null) _pc = GetComponentInParent<PlayerController>();
        if (_ec == null) _ec = GetComponentInParent<EnemyFSMController>();
        if (_entc == null) _entc = GetComponentInParent<EntFSMController>();
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

    public void AE_DestroyEntGameObject()
    {
        if (_entc == null) return;
        _entc.DestroyGOEnt();
    }

    public void AE_StartAttackAnimation()
    {
        //Debug.Log("START ENEMY ATTACK EVENT chiamato da: " + gameObject.name);
        if (_ec != null) _ec.enemyHandHitbox.EnableHitbox();
    }

    public void AE_EndAttackAnimation()
    {
        //Debug.Log("END ENEMY ATTACK chiamato da: " + gameObject.name);
        if (_ec != null) _ec.StopAttack();
        if (_ec != null) _ec.enemyHandHitbox.DisableHitbox();
    }

    public void AE_EndRoll()
    {
        if (_pc == null) return;
        _pc.EndRoll();
    }

    public void AE_StartEntAttackAnimation()
    {
        //Debug.Log("START ENT ATTACK EVENT chiamato da: " + gameObject.name);
        if (_entc == null) return;

        if (!_entc.IsAttacking)
        {
            Debug.LogWarning("Bloccato EnableHitbox: Ent non è in attacco reale");
            return;
        }

        _entc.HandHitbox.EnableHitbox();
    }

    public void AE_EndEntAttackAnimation()
    {
        //Debug.Log("END ENT ATTACK chiamato da: " + gameObject.name);
        if (_entc != null) _entc.StopAttack();
        if (_entc != null) _entc.HandHitbox.DisableHitbox();
    }

}
