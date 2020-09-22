using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandController : MonoBehaviour
{
	public SteamVR_Action_Boolean TriggerButtonAction;


	private SteamVR_Behaviour_Pose pose;
	private FixedJoint fixedJoint;

	private InteractableObject currentInteractableObject; //informacija koji objekt nam je trenutno u ruci
	[SerializeField]
	private List<InteractableObject> interactableObjects = new List<InteractableObject>(); //spremamo sve objekte sa kojima smo u koliziji

	private string tagToCompare = "Interactable";

	private void Start()
	{
		pose = GetComponent<SteamVR_Behaviour_Pose>();
		fixedJoint = GetComponent<FixedJoint>();
			
	}

	private void Update()
	{
		if (TriggerButtonAction.GetStateUp(pose.inputSource))
		{
			Debug.Log("Trigger up " + pose.inputSource);

			DropObject();
		}

		if (TriggerButtonAction.GetStateDown(pose.inputSource))
		{
			Debug.Log("Trigger down " + pose.inputSource);

			PickupObject();
		}

		
	}

	private void OnTriggerEnter(Collider other)
	{
		//ubacujemo objekt u listu
		if (other.gameObject.CompareTag(tagToCompare))
		{
			InteractableObject interactable = other.gameObject.GetComponent<InteractableObject>();

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
			InteractableObject interactable = other.gameObject.GetComponent<InteractableObject>();

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

		//postavljamo objekt na poziciju nase ruke
		currentInteractableObject.transform.position = transform.position;

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



	private InteractableObject NearestObject()
	{
		InteractableObject nerarestObject = null;

		float minDistance = float.MaxValue;
		float distance;

		foreach (InteractableObject interactable in interactableObjects)
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
