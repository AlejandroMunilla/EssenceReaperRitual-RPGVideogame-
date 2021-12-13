using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class EnemyAdd : MonoBehaviour {

    private bool _alive = true;
    private enum State
    {
        Starting,
        Checks
    }
    private State _state;
    private GameController gameController;
    private GameObject gc;
    
    
    public bool recurringEnemy = false;

    private void Awake ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController");
        gameController = gc.GetComponent<GameController>();
    }

    private void OnEnable ()
    {
        _state = EnemyAdd.State.Starting;
        StartCoroutine("FSM");
    }
 
    private IEnumerator FSM()
    {
        while (_alive)
        {
            switch (_state)
            {
                case State.Starting:
                    Starting();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Checks:
                    Checks();
                    yield return new WaitForSeconds(0);
                    break;
            }
            yield return null;
        }
    }


    private void Starting ()
    {
        if (gameController.players.Count > 0)
        {
   //         Debug.Log("Add :" + gameObject.name);
            string nameObj = gameObject.name + "Enemy";
            string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string value = DialogueLua.GetActorField(scene, nameObj).AsString;

            if (recurringEnemy == false)
            {
                if (value == null || value == "")
                {
    //                Debug.Log("null");
                    //          AddEnemy();
                    _state = EnemyAdd.State.Checks;
                    DialogueLua.SetActorField(scene, nameObj, "Yes");
                }

                else if (value == "No")
                {
                    _state = EnemyAdd.State.Checks;
                    DialogueLua.SetActorField(scene, nameObj, "Yes");
                }
                else
                {
                    StopCoroutine("FSM");
                    this.enabled = false;
                    Destroy(this);
                }
            }
        }
    }

    void Checks ()
    {
        AddEnemy();
        StopCoroutine("FSM");
        this.enabled = false;
        Destroy(this);
    }

    private void AddEnemy()
    {
        gc.GetComponent<EnemyController>().enemies.Add(gameObject);
        this.enabled = false;
    }

    public void End ()
    {
        string nameObj = gameObject.name + "Enemy";
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        DialogueLua.SetActorField(scene, nameObj, "Yes");
        this.enabled = false;
        Destroy(this);
    }
}
