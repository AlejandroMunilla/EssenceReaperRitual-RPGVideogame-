using UnityEngine;
using System.Collections;

public class LightSmall : MonoBehaviour 
{
	void Start()
	{
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc.GetComponent<GameController>().day == true)
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive (false);
			}
		}
	}
}
