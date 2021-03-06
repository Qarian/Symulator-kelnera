﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] Transform groundChecker = default;

	[Space]
	[SerializeField] float speed = 1f;
	[SerializeField] float maxSpeed = 5f;
	[SerializeField] float jumpForce = 10f;

	Vector3 moveAxis = Vector3.zero;
	bool onGround = false;
	bool canJump = false;
	
	[HideInInspector]
	public Rigidbody rb;

	private bool registerInput = true;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		GameManager.singleton.onInputToggle.Add(ChangeInput);
	}
	
	void Update()
	{
		if (registerInput)
		{
			// Get movement input
			moveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			// Jumping
			if (canJump && Input.GetKeyDown(KeyCode.Space)){
				if (!onGround)
					canJump = false;
				rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
			}
		}
		CheckGround();
	}
	
	// Calculate valocity of player
	private void FixedUpdate()
	{
		Vector3 accelaration = Vector3.zero;
		accelaration += moveAxis.z * transform.forward;
		accelaration += moveAxis.x * transform.right;
		accelaration = accelaration.normalized * speed * Time.fixedDeltaTime;
		rb.velocity = Vector3.ClampMagnitude(rb.velocity + accelaration, maxSpeed);
	}

	private void CheckGround()
	{
		RaycastHit[] hits = new RaycastHit[2];
		int hitCounts = Physics.RaycastNonAlloc(groundChecker.position, Vector3.down, hits, 0.2f);
		if (hitCounts > 0)
		{
			onGround = true;
			canJump = true;
		}
		else
			onGround = false;
	}

	private void ChangeInput()
	{
		registerInput = !registerInput;
	}
}
