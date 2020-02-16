using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FoodScript : Interactive
{
	[SerializeField] private Renderer meshRenderer = default;
	public Transform meshTransform;
	public new Rigidbody rigidbody;

	[HideInInspector] public Color color;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		SetAction(() => { });
	}

	// Set color of food
	public void SetColor(Color newColor)
	{
		color = newColor;
		ColorScript.SetColor(meshRenderer, newColor);
	}
}
