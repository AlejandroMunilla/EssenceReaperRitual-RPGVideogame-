using UnityEngine;
using System.Collections;

public class PartyManagerStand : MonoBehaviour {

    private PartyManager partyManager;
    private GameController gameController;
	// Use this for initialization
	void Awake ()
    {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        partyManager = gc.GetComponent<PartyManager>();
        gameController = gc.GetComponent<GameController>();
	}

    void OnMouseUp ()
    {
        float distanceMin = 6.0f;
        bool withinRange = false;
        foreach (GameObject go in gameController.players)
        {
            float distanceToGo = Vector3.Distance(gameObject.transform.position, go.transform.position);
            if (distanceToGo < distanceMin)
            {
                withinRange = true;
            }
        }
        
        if (withinRange == true)
        {
            partyManager.enabled = true;
        }
        
        Debug.Log("Up");
    }
	

}
