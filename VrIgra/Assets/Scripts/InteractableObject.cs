using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableObject : MonoBehaviour
{
	//informacija sa kojom ruko drzimo objekt
	public HandController ActiveHand;

	private void Start()
	{
		gameObject.tag = "Interactable";
	}
}
