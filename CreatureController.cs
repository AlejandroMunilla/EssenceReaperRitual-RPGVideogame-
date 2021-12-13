///AMG March 19th, 2017
///This is used to check distance of creatures / NPC / Allies and others
///if within distance, activate objects, otherwise they remain inactive to optimize game

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreatureController : MonoBehaviour {

    public float sightDistance = 70;
    public List<GameObject> creatures = new List<GameObject>();


    public void CheckCreature (GameController gameController, GameObject creature)
    {
        bool withinSight = false;

        foreach (GameObject player in gameController.players)
        {
      //      Debug.Log(player);
            float distancePlayer = Vector3.Distance(player.transform.position, creature.transform.position);
            if (distancePlayer <= 70)
            {
                withinSight = true;
            }
        }

        if (withinSight == true)
        {
            creature.SetActive(true);
        }
        else
        {
            creature.SetActive(false);
        }

    }



}
