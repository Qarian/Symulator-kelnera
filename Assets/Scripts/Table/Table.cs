using UnityEngine;

public class Table : MonoBehaviour
{
	public Transform positions = default;
	public TableDetector tableDetector = default;
	[SerializeField] Interactive interactiveComponent = default;
	[SerializeField] GameObject sphere = default;
	[SerializeField] GameObject mesh = default;
	[HideInInspector] CustomersCluster customersAtTable;
	Interactive tableInteractive;
	
	[HideInInspector] public int id = 0;
	public Color color = default;

	private void Start()
	{
		SetColor();

		Interactive interactive = sphere.AddComponent(typeof(Interactive)) as Interactive;
		interactive.SetAction(PlaceOrder);
		tableInteractive = mesh.GetComponent<Interactive>();
		tableInteractive.SetAction(SitCustomer);
		sphere.SetActive(false);
	}

	private void OnValidate()
	{
		SetColor();
	}

	#region customers
	// Posadzenie klienta przy stole
	void SitCustomer()
	{
		customersAtTable = GameManager.singleton.ChooseTable(this);
	}

	// Klienci wybrali jedzenie
	public void ActivateOrder()
	{
		sphere.SetActive(true);
	}

	// Składanie zamówienia, funkcja dodawan do interaktywnego obiektu
	private void PlaceOrder()
	{
		sphere.SetActive(false);
		GameManager.singleton.OrderFood(id, customersAtTable.numberOfCustomers);
	}
	
	public void EatFood(Rigidbody food)
	{
		Destroy(food.gameObject);
		if(customersAtTable.EatFood())
		{
			GameManager.singleton.FreeTable(this);
		}
	}
	#endregion

	public void Enable()
	{
		interactiveComponent.active = true;
	}

	public void Disable()
	{
		interactiveComponent.active = false;
	}

	// Ustawianie koloru stołu
	private void SetColor()
	{
		ColorScript.SetColor(sphere, color);
		ColorScript.SetColor(mesh, color);
	}
}
