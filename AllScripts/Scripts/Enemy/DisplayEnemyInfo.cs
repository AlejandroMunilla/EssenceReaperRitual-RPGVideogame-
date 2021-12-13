using UnityEngine;
using System.Collections;

public class DisplayEnemyInfo : MonoBehaviour {

	private float heightAddjust;

	// Use this for initialization
	void Start () 
	{
		heightAddjust = Screen.height;
	}

	void OnGUI ()
	{
		Vector2 targetPos = Camera.main.WorldToScreenPoint (transform.position);
		GUI.Label (new Rect (targetPos.x, (Screen.height - targetPos.y), 50, 25), GetComponent<EnemyStats>().curHealth + "/" + GetComponent <EnemyStats>().health);
	}
}
