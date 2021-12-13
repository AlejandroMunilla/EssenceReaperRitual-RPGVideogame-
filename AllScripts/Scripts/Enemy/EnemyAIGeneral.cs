using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class EnemyAIGeneral : MonoBehaviour {

    public List<GameObject> enemiesAI = new List<GameObject>();
    public bool reocurringEnemies = false;
    private enum State
    {
        Starting,
        Checks
    }
    private State _state;
    private bool _alive = true;

    void Start ()
    {
        _state = EnemyAIGeneral.State.Starting;
        StartCoroutine("FSM");
    }
    
    private IEnumerator FSM()
    {
        while (_alive)
        {
            switch (_state)
            {
                case State.Starting:

                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.Checks:
                    yield return new WaitForSeconds(2);

                    break;
            }
            yield return null;
        }
    }

    void Starting ()
    {
        if (enemiesAI.Count > 0)
        {
            if (reocurringEnemies == false)
            {
                string nameObj = gameObject.name + "Enemy";
                string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                string value = DialogueLua.GetActorField(scene, nameObj).AsString;

                if (value == null || value == "")
                {
                    Debug.Log("null ");
                    _state = EnemyAIGeneral.State.Checks;
                }

                else if (value == "Yes")
                {
                    _state = EnemyAIGeneral.State.Checks;
                }
                else
                {
                    StopCoroutine("FSM");
                    this.enabled = false;
                    gameObject.SetActive(false);
                }
            }            
        }
    }

    void Checks ()
    {
        if (enemiesAI.Count == 0)
        {
            End();
        }
        else
        {

        }
    }

    public void End()
    {
        StopAllCoroutines();
        string nameObj = gameObject.name + "Enemy";
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        DialogueLua.SetActorField(scene, nameObj, "Yes");
        this.enabled = false;
        Debug.Log("End");
   //     Destroy(this);
    }


    public void WarnAll (GameObject watcher, GameObject player)
    {
        foreach (Transform ta in transform)
        {
            Debug.Log(ta.name);
;            if (ta.GetComponent<EnemyAI>())
            {

                EnemyAI ea = ta.GetComponent<EnemyAI>();
                if (ea.player == null)
                {
                    //               Debug.Log(go + "/" + watcher + "/" + player);
                    ea.player = player;
                }

                if (ea.enabled == false)
                {
                    ta.GetComponent<EnemyStats>().ChangeToEnemy();
                }


            }
            else if (ta.GetComponent<EnemyAIAnim>())
            {

                EnemyAIAnim ea = ta.GetComponent<EnemyAIAnim>();


                if (ea.target == null)
                {
                    ea.target = player;
                }

                if (ea.enabled == false)
                {
                    ta.GetComponent<EnemyStats>().ChangeToEnemy();
                }
            }
        }
    }
}
