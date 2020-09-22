using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float DestroyTime = 2;

	private void Start()
	{
		Destroy(gameObject, DestroyTime);
	}
}
