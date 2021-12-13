using UnityEngine;
using System.Collections;

public class Taunt : MonoBehaviour {
    private int distance = 12;
    private int manaConsume = 6;
    private GameObject player;
    private GameObject sphere;
    private EnemyController enemyController;
    private bool alive = true;
    private AudioClip clip;
    private AudioSource audioSource;

	// Use this for initialization
    void Awake ()
    {
        sphere = transform.Find("Sphere").gameObject;
        player = transform.root.gameObject;
        GameObject go = GameObject.FindGameObjectWithTag("GameController");
        audioSource = player.GetComponent<AudioSource>();
        enemyController = go.GetComponent<EnemyController>();
        AdjustLevel();
        string gender = PixelCrushers.DialogueSystem.DialogueLua.GetActorField(player.name, "gender").AsString;
 //       Debug.Log(gender);
        if (gender == "Female")
        {
            clip = (AudioClip)(Resources.Load("Audio/Player/Female/BeatsMe"));
        }
        else
        {
            clip = (AudioClip)(Resources.Load("Audio/Player/Male/BeatsMe"));
        }
        audioSource.clip = clip;
    }


	void OnEnable ()
    {
        AdjustLevel();
        if (player.GetComponent<PlayerStats>().curMana >= manaConsume)
        {
            Activate();
        }
        else
        {
            sphere.SetActive(false);
            PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("Insufficient mana");
        }
              
   //     Invoke("Activate", 4f);
	}

    public void Activate ()
    {
        player.GetComponent<PlayerStats>().curMana = player.GetComponent<PlayerStats>().curMana - manaConsume;
        sphere.SetActive(true);
        CheckEnemies();
        audioSource.Play();
        StartCoroutine("SphereEffect");
    }

    private void AdjustLevel ()
    {
        int level = PixelCrushers.DialogueSystem.DialogueLua.GetActorField("Player", "level").AsInt;
        int levelAdd = (int) (Mathf.Ceil(level / 2));
        distance = 4 + levelAdd;
    }

    IEnumerator SphereEffect ()
    {
        while (alive)
        {
            sphere.transform.localScale += new Vector3(0.25f, 0.1f, 0.25f);

            if ((int)(sphere.transform.localScale.x) > distance)
            {
       //         Debug.Log(sphere.transform.localScale.x + "/" + distance);
                sphere.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.SetActive(false);
                StopCoroutine("SphereEffect");
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void CheckEnemies ()
    {
        if (enemyController.enemies != null)
        {
            if (enemyController.enemies.Count != 0)
            {
                foreach (GameObject enemy in enemyController.enemies)
                {
                    float distanceEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    //        Debug.Log(distanceEnemy + "/" + enemy);
                    if (distanceEnemy <= distance)
                    {
                        if (enemy.GetComponent<EnemyAI>())
                        {
                            enemy.GetComponent<EnemyAI>().SetNewEnemy(player);
                        }
                        else if (enemy.GetComponent<EnemyAIAnim>())
                        {
                            enemy.GetComponent<EnemyAIAnim>().target = player;
                            Debug.Log(enemy);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("No enemies");
        }
       
    }
}
