using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
	public float DelayTime = 4f;
	public float Radius = 7;
	public float Force = 500;

	public GameObject ExplosionEffect;

	private float countdown;
	private bool exploded = false;

	private void Start()
	{
		countdown = DelayTime;
	}

	private void Update()
	{
		countdown -= Time.deltaTime;

		if (countdown <= 0 && !exploded)
		{
			exploded = true;

			ExplodeGrenade();
		}
	}

	private void ExplodeGrenade()
	{
		Instantiate(ExplosionEffect, transform.position, transform.rotation);

		Collider[] collidersSolid = Physics.OverlapSphere(transform.position, Radius);

		foreach (Collider solidObject in collidersSolid)
		{
			Target target = solidObject.GetComponent<Target>();

			if (target)
			{
				target.DestroyTarget();
			}
		}

		Collider[] collidersToMove = Physics.OverlapSphere(transform.position, Radius);

		foreach (Collider item in collidersToMove)
		{
			Rigidbody rigidbody = item.GetComponent<Rigidbody>();

			if (rigidbody)
			{
				rigidbody.AddExplosionForce(Force, transform.position, Radius);
			}
		}


		Destroy(gameObject);

	}
}
