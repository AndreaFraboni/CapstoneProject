using UnityEngine;

public class CrowdedTransition : BaseFSMTransition
{
    [SerializeField] private float _checkRadius = 0.8f;
    [SerializeField] private LayerMask _enemyLayer;

    public override bool IsConditionMet()
    {
        if (_controller == null) return false;

        Collider[] hits = Physics.OverlapSphere(_controller.transform.position, _checkRadius, _enemyLayer);

        foreach (Collider hit in hits)
        {
            if (hit.transform == _controller.transform) continue;

            EnemyFSMController otherEnemy = hit.GetComponentInParent<EnemyFSMController>();
            if (otherEnemy != null && otherEnemy != _controller) return true;
        }

        return false;
    }
}
