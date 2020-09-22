using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; //za prikupljanje inputa

public class VRMovementController : MonoBehaviour
{
	public float CharacterSpeed = 1; //definira koliko brzo ce se player kretati kada pritisnemo touchpad
	public float MaxSpeed = 2; //maksimalna brzina kretnje

	public SteamVR_Action_Boolean ButtonPressed; //detektira da li je stisnuta tipka na touchpadu
	public SteamVR_Action_Vector2 MoveValue; //vrijednost vektora 2 u trenutku pritiska touchpada


	private CharacterController characterController;
	private float speed;

	private Transform cameraPosition;
	private Transform headPostion; // pozicija VR-a

	private void Start()
	{
		characterController = GetComponent<CharacterController>();

		//dohvacanje pozicija
		cameraPosition = SteamVR_Render.Top().origin;
		headPostion = SteamVR_Render.Top().head;
	}


	private void Update()
	{
		ControlHead();
		CharacterHeight();
		MoveCharacter();		
	}


	/// <summary>
	/// sluzi za odvajanje glave od character contorllera
	/// </summary>
	private void ControlHead()
	{

		//spremamo poziciju i rotaciju kamere
		Vector3 position = cameraPosition.position;
		Quaternion rotation = cameraPosition.rotation;

		//rotiramo charcter controller ovisno o kretnji glave
		transform.eulerAngles = new Vector3(0, headPostion.rotation.eulerAngles.y, 0);

		//vracamo rotaciju 
		cameraPosition.position = position;
		cameraPosition.rotation = rotation;
	}

	private void MoveCharacter()
	{
		//spremamo orijentaciju character controllera
		Vector3 rotation = new Vector3(0, transform.eulerAngles.y, 0);
		Quaternion quaternion = Quaternion.Euler(rotation); //pretvaramo rotaciju u quaternione

		Vector3 movement = Vector3.zero;

		//provjeravamo ako je tipka otpustena, ako je stavljamo brzinu na 0
		if (ButtonPressed.GetStateUp(SteamVR_Input_Sources.Any))
		{
			speed = 0;
		}

		if (ButtonPressed.state)
		{
			speed += MoveValue.axis.y * CharacterSpeed;
			speed = Mathf.Clamp(speed, -MaxSpeed, MaxSpeed);//clapamo vrijednosti da ne idu preko max speeda

			movement += quaternion * (speed * Vector3.forward) * Time.deltaTime;
		}


		//prijenjujemo move na charactera
		characterController.Move(movement);
	}


	private void CharacterHeight()
	{
		//dohvacamo glavu
		float newHeight = Mathf.Clamp(headPostion.localPosition.y, 1, 2);// ogranicavamo vrijednost izmedu 1 i 2 "metra"
		characterController.height = newHeight;

		//postavljamo novi centar character controllera
		Vector3 newCenter = Vector3.zero;
		newCenter.y = characterController.height / 2; //visina na pola
		newCenter.y += characterController.skinWidth;

		//pomicemo capsule collider u local space-u
		newCenter.x = headPostion.localPosition.x;
		newCenter.z = headPostion.localPosition.z;

		//rotiramo capsule collider da bue ujednacen sa glavom
		newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;


		characterController.center = newCenter;

	}
}
