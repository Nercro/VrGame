using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInteractionController : MonoBehaviour
{
	public SteamVR_Action_Boolean TriggerButtonAction; //tipka za pokupiti objekt i interakciju sa objektom
	public SteamVR_Action_Boolean GrabAction; //tipka za odbaciti objekt


	private SteamVR_Behaviour_Pose pose;
	private FixedJoint fixedJoint;

	private Interactable currentInteractableObject; //informacija koji objekt nam je trenutno u ruci
	[SerializeField]
	private List<Interactable> interactableObjects = new List<Interactable>(); //spremamo sve objekte sa kojima smo u koliziji

	private string tagToCompare = "Interactable";

	private void Start()
	{
		pose = GetComponent<SteamVR_Behaviour_Pose>();
		fixedJoint = GetComponent<FixedJoint>();

	}

	private void Update()
	{
		
		if (TriggerButtonAction.GetState(pose.inputSource))
		{
			Debug.Log("Trigger down " + pose.inputSource);

			//provjeravamo da li imamo objekt u rukama
			if (currentInteractableObject != null)
			{
				currentInteractableObject.Action(true); //pozivamo akciju na objektu

				return; //izlazimo iz updatea
			}

			PickupObject();
		}

		if (TriggerButtonAction.GetStateUp(pose.inputSource))
		{
			//pozvati action i prosljediti mu state up
			if (currentInteractableObject != null)
			{
				currentInteractableObject.Action(false);
			}
		}


		if (GrabAction.GetStateDown(pose.inputSource))
		{
			Debug.Log(pose.inputSource + " Drop");

			DropObject();
		}


	}

	private void OnTriggerEnter(Collider other)
	{
		//ubacujemo objekt u listu
		if (other.gameObject.CompareTag(tagToCompare))
		{
			Interactable interactable = other.gameObject.GetComponent<Interactable>();

			if (interactable)
			{
				interactableObjects.Add(interactable);
			}
		}


	}

	private void OnTriggerExit(Collider other)
	{
		//izbacujemo objekt iz liste
		if (other.gameObject.tag == tagToCompare)
		{
			Interactable interactable = other.gameObject.GetComponent<Interactable>();

			if (interactable)
			{
				interactableObjects.Remove(interactable);
			}
		}

	}

	public void PickupObject()
	{
		currentInteractableObject = NearestObject();

		if (currentInteractableObject == null)
		{
			return;
		}

		//provjeravamo da li je objekt u ruci
		if (currentInteractableObject.ActiveHand)
		{
			//bacamo objekt prije nego ga prebacimo u drugu ruku
			currentInteractableObject.ActiveHand.DropObject();
		}

		
		currentInteractableObject.ApplyOffset(transform);

		//dohvacamo rigidbody i zakacimo ga za fixed joint
		Rigidbody target = currentInteractableObject.GetComponent<Rigidbody>();
		fixedJoint.connectedBody = target;

		currentInteractableObject.ActiveHand = this;

	}

	public void DropObject()
	{
		if (currentInteractableObject == null)
		{
			return;
		}

		//dohvacamo rigidbodi i dajemo mu velocity od ruke koja ga drzi
		Rigidbody target = currentInteractableObject.GetComponent<Rigidbody>();
		target.velocity = pose.GetVelocity();
		target.angularVelocity = pose.GetAngularVelocity();

		fixedJoint.connectedBody = null;

		currentInteractableObject.ActiveHand = null;
		currentInteractableObject = null;
	}



	private Interactable NearestObject()
	{
		Interactable nerarestObject = null;

		float minDistance = float.MaxValue;
		float distance;

		foreach (Interactable interactable in interactableObjects)
		{
			//spremamo magnitudu
			distance = (interactable.transform.position - transform.position).sqrMagnitude;

			if (distance < minDistance)
			{
				minDistance = distance; //spremamo trenutnu namjamjnu distancu

				nerarestObject = interactable; //spremamo trenutno najblizi objekt
			}
		}

		return nerarestObject;
	}
}
