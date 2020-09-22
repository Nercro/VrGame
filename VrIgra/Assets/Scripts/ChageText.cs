using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ChageText : MonoBehaviour
{
	public DataSO DataScriptableObject;
	public Text StartText;

	private void Start()
	{
		SetNewText();
	}

	public void SetNewText()
	{
		StartText.text = DataScriptableObject.StartText;
	}
}

[CustomEditor(typeof(ChageText))]
public class ChangeTextInEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ChageText chageText = (ChageText)target;

		if (GUILayout.Button("Change Text"))
		{
			chageText.SetNewText();
		}
	}
}

