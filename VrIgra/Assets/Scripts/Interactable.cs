using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
	public Vector3 ObjectOffset;

	[HideInInspector]
	public HandInteractionController ActiveHand;

	private void Start()
	{
		gameObject.tag = "Interactable";
	}

	public virtual void Action(bool isButtonDown) //napraviti virtual
	{
		Debug.Log("Object Action Triggered");
	}

	public void ApplyOffset(Transform handTransform)
	{
		//zakacimo objekt za ruku
		transform.SetParent(handTransform);
		transform.localRotation = Quaternion.identity;
		transform.localPosition = ObjectOffset; //dodajemo offset
		transform.SetParent(null);

	}
}
