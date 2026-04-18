using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data", menuName = "ScriptableObjects/BulletData", order = 0)]
public class SO_BulletData : ScriptableObject
{
    public string BulletName;

    public Bullet bulletPrefab;

    public int damage;
    public Material expMaterial;
    public DamageTarget damageTarget;

}
