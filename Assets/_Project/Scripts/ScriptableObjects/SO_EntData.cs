using UnityEngine;

[CreateAssetMenu(fileName = "Ent Data", menuName = "ScriptableObjects/EntData", order = 0)]
public class SO_EntData : ScriptableObject
{
    public string EntName;

    public GameObject EntPrefab;
    public GameObject ghostPrefab;

    public int goldPrice;
    public int blueGemsPrice;
}
