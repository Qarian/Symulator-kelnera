using UnityEngine;

public class CameraSelector : MonoBehaviour
{
	[SerializeField] RectTransform crosshair = default;
	[SerializeField] Transform holdingPoint = default;

	[SerializeField, Space] float maxDistance = 20f;
	[SerializeField] float grabbingSpeed = 1f;
	[SerializeField] float throwingForce = 1000f;
	[SerializeField] float grabbingDistance = 0.5f;
	[SerializeField] float maxWeight = 0.5f;
	bool grabbed = false;

	[SerializeField, Space]
	Vector2 crosshairSize = new Vector2(10f, 50f);
	[SerializeField]
	float crosshairChangeSpeed = 5f;
	float size;

    new Camera camera;
	Rigidbody holdingObject;

    Interactive currentInteractive;
	FoodScript food;

    private void Start()
	{
		camera = GetComponent<Camera>();
		size = crosshair.sizeDelta.x;
	}

	private void Update()
	{
		//If player is not holding any object
		if (holdingObject == null)
		{
            crosshair.gameObject.SetActive(true);

			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			// czy cos sie trafilo
			if (Physics.Raycast(ray, out hit, maxDistance))
			{
				Rigidbody rb = hit.rigidbody;
                currentInteractive = hit.transform.GetComponent<Interactive>();
				// czy trafiono w coś interaktywnego
				if (currentInteractive && currentInteractive.active)
				{
					size += crosshairChangeSpeed * Time.deltaTime;
					if (Input.GetKeyDown(KeyCode.E))
						currentInteractive.Interaction();
					size += 2 * crosshairChangeSpeed * Time.deltaTime;
				}

				// czy trafiono w coś do podnoszenia
				if (rb && !rb.isKinematic)
				{
                    // Czy naciśnięto przycisk
                    if (Input.GetMouseButtonDown(0))
							GrabItem(rb);
                    size += 2 * crosshairChangeSpeed * Time.deltaTime;
                }
			}
		}
		else
		{
			holdingObject.transform.position = holdingPoint.position;
			if (currentInteractive && currentInteractive.active)
            {
                //size += crosshairChangeSpeed * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.E))
                    currentInteractive.Interaction();
            }

			// Drop object
            if (Input.GetMouseButtonDown(0))
				LeaveItem();
			// Throw object
			if (Input.GetMouseButtonDown(1))
			{
				holdingObject.AddForce(transform.forward * throwingForce);
				LeaveItem();
			}
		}
        size -= crosshairChangeSpeed * Time.deltaTime;

		//Change size of crosshair
        size = Mathf.Clamp(size, crosshairSize.x, crosshairSize.y);
        crosshair.sizeDelta = new Vector2(size, size);
    }

	private void FixedUpdate()
	{
		if (holdingObject)
		{
			if (!grabbed)
			{
				holdingObject.MovePosition(Vector3.Lerp(holdingObject.position, holdingPoint.position, Time.deltaTime * grabbingSpeed));
				if (Vector3.Distance(holdingObject.position, holdingPoint.position) <= grabbingDistance)
				{
					holdingObject.transform.SetParent(holdingPoint);
					grabbed = true;
				}
			}
		}
	}

	// Grab object
	private void GrabItem(Rigidbody rb)
	{
        if (rb.mass > maxWeight)
        {
            Debug.Log("Za ciezkie");
            return;
        }

        holdingObject = rb;
		holdingObject.useGravity = false;
		

		food = rb.GetComponent<FoodScript>();
		if (food)
			food.taken = true;
	}

	// Drop object
	private void LeaveItem()
	{
		holdingObject.useGravity = true;
		holdingObject.transform.SetParent(null);
		holdingObject = null;

		if (food)
		{
			food.taken = false;
			food = null;
		}
	}
}
