using UnityEngine;

[CreateAssetMenu(fileName = "Tower Data", menuName = "ScriptableObjects/TowerData", order = 0)]
public class SO_TowerData : ScriptableObject
{
    public string towerName;

    public GameObject towerPrefab;
    public GameObject ghostPrefab;

    public SO_BulletData bulletdata;

    public int goldPrice;

    public float range;
    public float fireRate;
    public float shootForce;
}
