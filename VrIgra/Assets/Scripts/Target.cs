using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
	public GameObject DestroyedObject;



    public void DestroyTarget()
	{
		Instantiate(DestroyedObject, transform.position, transform.rotation);

		Destroy(gameObject);
	}
}
