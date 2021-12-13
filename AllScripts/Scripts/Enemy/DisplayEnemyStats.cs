using UnityEngine;
using System.Collections;

public class DisplayEnemyStats : MonoBehaviour 
{
	void OnEnable ()
	{
		Invoke ("AutoCheck", 0.5f);
	}

	void OnDisable ()
	{

	}

	void OnGUI ()
	{
		GUI.Button (new Rect (Screen.width * 0.5f, Screen.height *0.1f, 300, 50), "Info");
	}

	public void AutoCheck ()
	{
		this.enabled = false;
	}
}
