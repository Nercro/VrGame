using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float Speed = 3;
	public float Gravity = -3;

	private CharacterController characterController;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Vector3 movementZ = Input.GetAxisRaw("Vertical") * Vector3.forward * Speed * Time.deltaTime;
		Vector3 movementX = Input.GetAxisRaw("Horizontal") * Vector3.right * Speed * Time.deltaTime;

		Vector3 movement = transform.TransformDirection(movementZ + movementX);

		movement.y = Gravity * Time.deltaTime;

		characterController.Move(movement);
	}
}
