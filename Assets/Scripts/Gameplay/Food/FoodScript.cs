using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FoodScript : Interactive
{
	[SerializeField] private Renderer meshRenderer = default;
	public Transform meshTransform;
	public Rigidbody Rigidbody { get; private set; }
	public int OrderId { get; private set; }
	public int CustomerId { get; private set; }

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		// No interaction with food - only for modifying cursor
		SetAction(() => { });
	}
	
	public void Init(Color newColor, int orderId, int customerId)
	{
		OrderId = orderId;
		CustomerId = customerId;
		ColorScript.SetColor(meshRenderer, newColor);
	}
}
