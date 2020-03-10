using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Food List", fileName = "New Food List")]
public class FoodListSO : ScriptableObject
{
    public List<FoodSO> list;

    public FoodSO this[int id]
    {
        get => list[id];
        set => list[id] = value;
    }
}
