using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{
	public Transform positions = default;
	public TableDetector tableDetector = default;
	[SerializeField] Interactive interactiveComponent = default;
	[SerializeField] GameObject sphere = default;
	[SerializeField] GameObject mesh = default;
	Interactive tableInteractive;

	float currentTime;
	
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

	//posadzenie klienta przy stole
	void SitCustomer()
	{
		StartCoroutine(ActivateOrder());
		tableInteractive.active = false;
		GameManager.singleton.customerSelected = false;
	}

	public void Enable()
	{
		interactiveComponent.active = true;
	}

	public void Disable()
	{
		interactiveComponent.active = false;
	}

	IEnumerator ActivateOrder()
	{
		yield return new WaitForSeconds(5);
	}

	// Składanie zamówienia, funkcja dodawan do interaktywnego obiektu
	private void PlaceOrder()
	{
		sphere.SetActive(false);
		GameManager.singleton.OrderFood(id);
	}

	// Ustawianie koloru stołu
	private void SetColor()
	{
		ColorScript.SetColor(sphere, color);
		ColorScript.SetColor(mesh, color);
	}
}
