using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

    public float sightDistance = 75;
    public List<GameObject> enemies = new List<GameObject>();
    private GameController gameController;

    void Start ()
    {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        gameController = gc.GetComponent<GameController>();
	}

    public void CheckDistance(GameObject player)
    {
        foreach (GameObject creature in enemies)
        {
            if (creature == null)
            {
                return;
            }
            float distance = Vector3.Distance(player.transform.position, creature.transform.position);
            if (distance <= sightDistance)
            {
                creature.SetActive(true);
                if (creature.transform.root.gameObject.GetComponent<EnemyAIGeneral>())
                {
                    GameObject go = creature.transform.root.gameObject;
                    if (go.activeSelf)
                    {

                    }
                    else
                    {
                        go.SetActive(true);
                    }
                }
            }
            else
            {
                if (gameController.inCombat == false)
                {
            //        Debug.Log("Peace");
                    if (creature.transform.root.gameObject.GetComponent<EnemyAIGeneral>())
                    {
                        GameObject go = creature.transform.root.gameObject;
                        EnemyAIGeneral enemyAIGeneral = go.GetComponent<EnemyAIGeneral>();
                        bool allOutOfSight = true;
                        foreach (GameObject go2 in enemyAIGeneral.enemiesAI)
                        {
                            float distance2 = Vector3.Distance(player.transform.position, go2.transform.position);
                            if (distance2 <= sightDistance)
                            {
                                allOutOfSight = false;
                            }

                        }

                        if (allOutOfSight == true)
                        {
                            creature.SetActive(false);
                        }
                    }
                    else
                    {
                        creature.SetActive(false);
                    }                    
                }
            }
        }
    }
}
