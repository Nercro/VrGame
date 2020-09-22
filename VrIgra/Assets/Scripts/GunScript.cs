using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : Interactable
{
	
	public GameObject ProjectilePrefab;
	public Transform MuzzlePosition;
	public ParticleSystem MuzzleFlash;
	public float ProjectilePower;
	public float RateOfFire = 0.1f;

	public float nextFire = 0;

	public FiringMode FiringMode = FiringMode.Semi;

	private bool isFiring = false;
	private bool canShoot = true;
	

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && ActiveHand)
		{
			//prebaciti na drugi fire mode
			SwichFiringMode();
		}

		if (Input.GetMouseButtonDown(0))
		{
			Action(true);
		}

		if (Input.GetMouseButtonUp(0))
		{
			Action(false);
		}


		if (ActiveHand)
		{
			GameManger.Instance.ShowFireMode();
		}
		else
		{
			GameManger.Instance.HideFireMode();
		}
	}

	public override void Action(bool isButtonDown)
	{
		if (isButtonDown == false)
		{
			canShoot = true;
		}

		PrepareToShoot();
		

	}

	private void PrepareToShoot()
	{
		switch (FiringMode)
		{
			case FiringMode.Semi:
				if(canShoot)
				{
					canShoot = false;
					Shoot();
				}
				break;
			case FiringMode.Burst:
				//ispali 3 od jednom ovisno o rate of fire
				if (isFiring == false)
				{
					StartCoroutine(BurstFire());
				}
				break;
			case FiringMode.Auto:
				//ispaljuj stalno ovisno o rate of fire
				if (Time.time > nextFire)
				{
					nextFire = Time.time + RateOfFire;

					Shoot();
				}
				break;
			default:
				break;
		}
	}

	private IEnumerator BurstFire()
	{
		isFiring = true;

		for (int i = 0; i < 3; i++)
		{
			Shoot();

			yield return new WaitForSeconds(RateOfFire);
		}

		yield return new WaitForSeconds(0.3f);
		isFiring = false;
	}


	private void Shoot()
	{
		MuzzleFlash.Play();

		GameObject projectileClone = Instantiate(ProjectilePrefab, MuzzlePosition.position, Quaternion.identity);
		Rigidbody rigidbody = projectileClone.GetComponent<Rigidbody>();

		if (rigidbody)
		{
			rigidbody.AddForce(MuzzlePosition.forward * ProjectilePower, ForceMode.VelocityChange);
		}
		else
		{
			Debug.LogError("Rigidbody not found " + projectileClone.name);
		}

		
	}

	private void SwichFiringMode()
	{
		//dohvatiti length od enuma i spremiti ga u int
		int firingModeLength = System.Enum.GetValues(typeof(FiringMode)).Length;

		FiringMode++;

		//provjeriti da li je broj veci od enuma
		if ((int)FiringMode >= firingModeLength)
		{
			FiringMode = FiringMode.Semi;
		}

		GameManger.Instance.ChangeFireMode(FiringMode);
	}


}
