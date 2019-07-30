using UnityEngine;

public class FoodScript : MonoBehaviour
{
	[HideInInspector] public Color color;
	[HideInInspector] public bool taken = false;

	// Set color of food
	public void SetColor(Color color)
	{
		this.color = color;
		ColorScript.SetColor(gameObject, color);
	}
}
