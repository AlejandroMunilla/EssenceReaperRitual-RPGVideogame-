using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour 
{
	private GameObject gameController;

	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}

	void OnConversationStart ()
	{
		gameController.GetComponent<GameController>().Pause();

	}

	void OnConversationEnd ()
	{
		gameController.GetComponent<GameController>().Continue();

	}

}
