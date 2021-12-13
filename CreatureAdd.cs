using UnityEngine;

public class CreatureAdd : MonoBehaviour {

	void Awake ()
    {
        AddCreature();
	}

    void AddCreature ()
    {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        CreatureController creatureController = gc.GetComponent<CreatureController>();
        creatureController.creatures.Add(gameObject);
        this.enabled = false;    
    }
}
