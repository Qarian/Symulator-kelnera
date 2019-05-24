using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	float speed = default;
	
	Vector3 moveAxis = Vector3.zero;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		moveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	}
	
    private void FixedUpdate()
	{
		Vector3 move = Vector3.zero;
		move += moveAxis.z * transform.forward;
		move += moveAxis.x * transform.right;
		rb.MovePosition(transform.position + move.normalized * speed);
	}
}
