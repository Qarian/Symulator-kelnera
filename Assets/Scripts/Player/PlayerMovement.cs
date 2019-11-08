using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float speed = 1f;
	[SerializeField] float maxSpeed = 5f;
	[SerializeField] float jumpForce = 10f;
	[SerializeField] float maxHeight = 8f;

	Vector3 moveAxis = Vector3.zero;
	bool canJump = false;
	
	[HideInInspector]
	public Rigidbody rb;
	
	public static PlayerMovement singleton;
	private void Awake()
	{
		singleton = this;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		// Get movement input
		moveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		// Jumping
		if (canJump && Input.GetKeyDown(KeyCode.Space) && transform.position.y < maxHeight){
			rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
		}
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

	// Check if player can jump
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Enviroment" || other.tag == "Table")
		canJump = true;
	}

	// Disable player ability to jump
	private void OnTriggerExit(Collider other)
	{
		canJump = false;
	}
}
