using UnityEngine;
using System.Collections.Generic;


public class TrapPreFab : MonoBehaviour {

    public GameObject player;
    public int trapLevel;

    private int minPiercingDamage = 1;
    private int maxPiercingDamage = 6;
    private int addPiercingDamage = 1;

    private int minCrushingDamage = 0;
    private int maxCrushingDamage = 0;
    private int addCrushingDamage = 0;

    private int minFireDamage = 0;
    private int maxFireDamage = 0;
    private int addFireDamage = 0;

    private int minAcidDamage = 0;
    private int maxAcidDamage = 0;
    private int addAcidDamage = 0;

    private float distance = 6.0f;
    private GameObject sphere;
    private GameObject gc;
    private EnemyController enemyController;
//    private List<GameObject> enemies= new List<GameObject>();


    // Use this for initialization
    void OnEnable ()
    {
        sphere = transform.Find("Sphere").gameObject;
        gc = GameObject.FindGameObjectWithTag("GameController");
        enemyController = gc.GetComponent<EnemyController>();
        Debug.Log(trapLevel);

        Debug.Log(trapLevel);
        if (trapLevel < 5)
        {
            Level1();
        }
        else if (trapLevel < 10)
        {
            Level5();
        }
        else if (trapLevel < 20)
        {
            Level10();
        }
        else if (trapLevel >= 20)
        {
            Level20();
        }
	}
	
    void OnTriggerEnter (Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "NPC")
        {
            Debug.Log(other.gameObject);
            TriggerTrap();            
        }
    }

    void OnMouseEnter ()
    {
        sphere.SetActive(true);
    }

    void OnMouseExit ()
    {
        sphere.SetActive(false);
    }

    public void TriggerTrap()
    {
        GameObject explosion = transform.Find("Explosion1").gameObject;
        explosion.SetActive(true);
        GetComponent<AudioSource>().Play();

        /*
        foreach (GameObject go in enemyController.enemies)
        {
            enemies.Add(go);
        }*/

        for (int cnt = 0; cnt < enemyController.enemies.Count; cnt++)
        {

            float distanceToEnemy = Vector3.Distance(enemyController.enemies[cnt].transform.position, transform.position);
            if (distanceToEnemy <= 3.2f)
            {
                CalculateDamage(enemyController.enemies[cnt]);  

            }
        }

        Invoke("ToInactive", 0.5f);
    }

    private void ToInactive ()
    {
        Debug.Log("inactive");

        gameObject.SetActive(false);
    }


    private void Level1()
    {
        //At level 1. Spikes trap which cause 1D4 piercing damage to anyone in the area. Area of effect; 6 meters. 
        //Already setip on initial values
        Debug.Log("Level1");
        minPiercingDamage = 1;
        maxPiercingDamage = 6;
        addPiercingDamage = 1;
    }

    private void Level5()
    {
        //At level 5; Spikes and fire effect. 1D5+1 piercing damage + 1D3D fire damage. Area of effect ; 8 meters. 
        minPiercingDamage = 1;
        maxPiercingDamage = 6;
        addPiercingDamage = 3;

        minFireDamage = 1;
        maxFireDamage = 3;
        addFireDamage = 0;

        minCrushingDamage = 0;
        maxCrushingDamage = 0;
        addCrushingDamage = 0;


        minAcidDamage = 0;
        maxAcidDamage = 0;
        addAcidDamage = 0;

        distance = 8.0f;

    }


    private void Level10()
    {
        //At level 10 ; Spike, Fire and Explosive effect. 1D5+3 piercing damage + 1D5+1 fire damage + 1D5 crushing damage. Area of effect 10 meters. 
        minPiercingDamage = 1;
        maxPiercingDamage = 6;
        addPiercingDamage = 3;

        minFireDamage = 1;
        maxFireDamage = 5;
        addFireDamage = 1;

        minCrushingDamage = 1;
        maxCrushingDamage = 5;
        addCrushingDamage = 0;


        minAcidDamage = 0;
        maxAcidDamage = 0;
        addAcidDamage = 0;

        distance = 10.0f;



    }


    private void Level20()
    {
        //At level 20; Spike, Fire, Acid and Explosive effect. 1D5+3 piercing damage +  1D5+3 fire damage + 1D5 +1 crushing damage + 1D5 acid damage. Area of effect 15 meters. 
        minPiercingDamage = 1;
        maxPiercingDamage = 6;
        addPiercingDamage = 3;

        minFireDamage = 1;
        maxFireDamage = 5;
        addFireDamage = 3;

        minCrushingDamage = 1;
        maxCrushingDamage = 5;
        addCrushingDamage = 1;


        minAcidDamage = 1;
        maxAcidDamage = 5;
        addAcidDamage = 3;

        distance = 15.0f;


    }

    private void CalculateDamage(GameObject enemy)
    {
        int dicePiercing = Random.Range(minPiercingDamage, maxPiercingDamage) + addPiercingDamage;
        int diceFire = Random.Range(minFireDamage, maxFireDamage) + addFireDamage;
        int diceCrushing =  Random.Range(minCrushingDamage, maxCrushingDamage) + addCrushingDamage;
        int diceAcid = Random.Range(minAcidDamage, maxAcidDamage) + addAcidDamage;

        EnemyStats es = enemy.GetComponent<EnemyStats>();

        float pierceRes = es.resPiercing;
        float crushingRes = es.resCrushing;
        float fireRes = es.resFire;
        float acidRes = es.resAcid;

        int addjustedPiercingDamage = Mathf.RoundToInt(dicePiercing * pierceRes);
        int addjustedCrushingDamage = Mathf.RoundToInt(diceCrushing * crushingRes);
        int addjustedFireDamage = Mathf.RoundToInt(diceFire * fireRes);
        int addjustedAcidDamage = Mathf.RoundToInt(diceAcid * acidRes);
        int addjustedDamage = addjustedAcidDamage + addjustedCrushingDamage + addjustedFireDamage + addjustedPiercingDamage;

        Debug.Log(enemy.name + "/" + dicePiercing + "/" + addPiercingDamage + "/" + addjustedPiercingDamage);
        es.AddjustCurrentHealth(-addjustedDamage, player);
        gc.GetComponent<DisplayInfo>().AddText(player.name + "´s trap caused " + addjustedDamage + " damage to " + enemy.name);


    }


}
