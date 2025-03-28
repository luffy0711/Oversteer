using UnityEngine;

[CreateAssetMenu(fileName = "NewCar", menuName = "Gacha/Car")]
public class CarData : ScriptableObject
{
    public string carName;
    public GameObject carPrefab;
    public int rarity; // 3, 4, or 5 stars
}
