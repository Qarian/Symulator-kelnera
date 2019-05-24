using UnityEngine;
using UnityEngine.UI;

public class CameraSelector : MonoBehaviour
{
	[SerializeField]
	RectTransform crosshair = default;
	[SerializeField]
	Transform holdingPoint = default;

	[SerializeField, Space]
	float maxDistance = 20f;
	[SerializeField]
	float grabbingSpeed = 1f;
	[SerializeField]
	float throwingForce = 100f;
	[SerializeField]
	float grabbingDistance = 0.5f;
	bool grabbed = false;

	[SerializeField, Space]
	Vector2 crosshairSize = new Vector2(10f, 50f);
	[SerializeField]
	float crosshairChangeSpeed = 5f;
	float size;

    new Camera camera;
	Rigidbody holdingObject;

	private void Start()
	{
		camera = GetComponent<Camera>();
		size = crosshair.sizeDelta.x;
	}

	private void Update()
	{
		//Czy trzyma się jakiegoś przedmiotu
		if (holdingObject == null)
		{
			crosshair.gameObject.SetActive(true);
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			// czy cos sie trafilo
			if (Physics.Raycast(ray, out hit, maxDistance))
			{
				Rigidbody rb = hit.rigidbody;
				Interactive interactive = hit.transform.GetComponent<Interactive>();
				
				// czy trafiono w coś interaktywnego
				if (interactive != null)
				{
					size += crosshairChangeSpeed * Time.deltaTime;
					if (Input.GetKeyDown(KeyCode.E))
						interactive.Interaction();
				}
				// czy trafiono w coś do podnoszenia
				else if (rb != null && !rb.isKinematic)
				{
					// Czy naciśnięto przycisk
					if (Input.GetMouseButtonDown(0))
					{
						crosshair.gameObject.SetActive(false);
						holdingObject = rb;
						holdingObject.useGravity = false;
					}
					size += crosshairChangeSpeed * Time.deltaTime;
				}
				
				else
				{
					size -= crosshairChangeSpeed * Time.deltaTime;
				}
				size = Mathf.Clamp(size, crosshairSize.x, crosshairSize.y);
				crosshair.sizeDelta = new Vector2(size, size);
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				holdingObject.useGravity = true;
				holdingObject = null;
				return;
			}

			if (Input.GetMouseButtonDown(1))
			{
				holdingObject.useGravity = true;
				holdingObject.AddForce(transform.forward * throwingForce);
				holdingObject = null;
				return;
			}
		}
	}

	private void FixedUpdate()
	{
		if (holdingObject != null)
		{
			if (!grabbed)
			{
				holdingObject.MovePosition(Vector3.Lerp(holdingObject.position, holdingPoint.position, Time.deltaTime * grabbingSpeed));
				if (Vector3.Distance(holdingObject.position, holdingPoint.position) <= grabbingDistance)
					grabbed = true;
			}
			else
			{
				holdingObject.MovePosition(holdingPoint.position);
			}
		}
	}
}
