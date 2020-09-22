using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{

	#region SINGLETON
	private static GameManger instance;
    public static GameManger Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType(typeof(GameManger)) as GameManger;

				if (instance == null)
				{
					Debug.Log("No game manager object in scene");
				}
			}

			return instance;
			
		}
	}
	#endregion

	public Text FireModeText;

	private void Start()
	{
		HideFireMode();
	}

	public void ChangeFireMode(FiringMode newFireMode)
	{
		FireModeText.text = newFireMode.ToString();
	}

	public void ShowFireMode()
	{
		FireModeText.enabled = true;
	}

	public void HideFireMode()
	{
		FireModeText.enabled = false;
	}
}
