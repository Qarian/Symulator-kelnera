using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField]
    Transform player = default;
	
	[SerializeField]
	float cameraSpeed = default;
	[SerializeField]
	Vector2 cameraYbounds = default;
	
	[SerializeField]
	KeyCode mouseModeSwitch = default;
	bool mouseVisible = true;
	
	float rotationY;
	
	void Start() 
	{
		mouseVisible = Cursor.visible;
		if (mouseVisible)
			ChangeMouseMode();
	}
	
	void Update()
	{
		if (!mouseVisible) {
			float yAngle = cameraSpeed * Input.GetAxis("Mouse Y");
			float xAngle = cameraSpeed * Input.GetAxis("Mouse X");
		
			player.Rotate(0, xAngle, 0);
			
			rotationY = Mathf.Clamp(rotationY + yAngle, cameraYbounds.x, cameraYbounds.y);
			transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		}
		
		if (Input.GetKeyDown(mouseModeSwitch))
			ChangeMouseMode();
	}
	
	void ChangeMouseMode()
	{
		if (mouseVisible)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else 
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		mouseVisible = !mouseVisible;
	}
}
