using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TableDetector : MonoBehaviour
{
    [SerializeField] GameObject sphere = default;
    [SerializeField] GameObject mesh = default;

    [Space]
	[SerializeField] Slider waitingBar = default;
	[SerializeField] float timeForOrder = 4f;
    [SerializeField] float maxTime = 15f;
    bool waiting = false;
    float currentTime;
    [SerializeField] Vector2Int money = default;
    public Color color = default;

    Rigidbody targetFoodRb;
	FoodScript targetScript;
	Interactive tableInteractive;

    [HideInInspector] public int id = 0;

    private void Start()
    {
		waitingBar.gameObject.SetActive(false);
		sphere.SetActive(false);
		SetColor();

        Interactive interactive = sphere.AddComponent(typeof(Interactive)) as Interactive;
        interactive.SetAction(PlaceOrder);
		tableInteractive = mesh.GetComponent<Interactive>();
		tableInteractive.SetAction(SitCustomer);
    }

    private void OnValidate()
    {
        SetColor();
    }

    private void Update()
    {
		if (waiting)
		{
			currentTime += Time.deltaTime;
			waitingBar.value = currentTime / maxTime;
			if (currentTime >= maxTime)
				End(false);
		}
			
        if (targetFoodRb != null)
        {
			if (targetScript.taken == false && targetFoodRb.velocity == Vector3.zero)
                End(true);
        }
    }

	//posadzenie klienta przy stole
	void SitCustomer()
	{
		StartCoroutine(ActivateOrder());
		tableInteractive.active = false;
		Destroy(GameManager.singleton.customer);
		GameManager.singleton.customerSelected = false;
	}

	//Klient zaczyna zamawiać jedzenie
    private IEnumerator ActivateOrder()
    {
        yield return new WaitForSeconds(timeForOrder);
        sphere.SetActive(true);
        waiting = true;
		waitingBar.gameObject.SetActive(true);
		waitingBar.value = 0;
	}

	// Składanie zamówienia, funkcja dodawan do interaktywnego obiektu
    private void PlaceOrder()
    {
        sphere.SetActive(false);
        GameManager.singleton.OrderFood(id);
    }

	// Wychodzenie klienta z restauracji - jak się najadł lub zniecierpliwił
    private void End(bool eaten)
    {
		if (eaten)
		{
			Destroy(targetFoodRb.gameObject);
			GameManager.singleton.ChangeScore((int)Mathf.Lerp(money.y, money.x, currentTime / maxTime));
		}
		//GameManager.singleton.freeTables.Add(this);
		currentTime = 0;
		waiting = false;
		waitingBar.gameObject.SetActive(false);
		tableInteractive.active = true;
	}

    private void OnTriggerEnter(Collider collider)
    {
		targetScript = collider.GetComponent<FoodScript>();
		if (targetScript != null && targetScript.color == color)
		{
			targetFoodRb = collider.GetComponent<Rigidbody>();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(targetFoodRb != null && other.GetComponent<Rigidbody>() == targetFoodRb)
		{
			targetFoodRb = null;
			targetScript = null;
		}
	}

	private void SetColor()
    {
        ColorScript.SetColor(sphere, color);
        ColorScript.SetColor(mesh, color);
    }
}
