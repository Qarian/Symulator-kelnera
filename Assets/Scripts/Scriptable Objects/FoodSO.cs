using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Food", fileName = "New Food")]
public class FoodSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
}
