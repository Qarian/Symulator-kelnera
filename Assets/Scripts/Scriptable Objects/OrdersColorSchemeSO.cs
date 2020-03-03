using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Order Color Scheme", fileName = "Order Color Scheme")]
public class OrdersColorSchemeSO : ScriptableObject
{
    public List<Color> colors;
}
