using UnityEngine;
using System.Collections;

public class CheckController : MonoBehaviour {

    private bool _alive = true;
    private GameController gameController;
    private TrapController trapController;
    private CreatureController creatureController;
    private EnemyController enemyController;
    private DisplayToolBar displayToolBar;
    private enum State
    {
        Starting,
        Checks
    }
    private State _state;

    void Start ()
    {
        gameController = GetComponent<GameController>();
        trapController = GetComponent<TrapController>();
        creatureController = GetComponent<CreatureController>();
        enemyController = GetComponent<EnemyController>();
        displayToolBar = GetComponent<DisplayToolBar>();
        StartCoroutine("FSM");
        _state = CheckController.State.Starting;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator FSM()
    {
        while (_alive)
        {
            switch (_state)
            {
                case State.Starting:
                    Starting();
                    yield return new WaitForSeconds(0.9f);
                    break;

                case State.Checks:
                    yield return new WaitForSeconds(1);
           //         Debug.Log("Checks");
                    CheckTraps();
                    CheckCreatures();
                    CheckEnemies();
                    displayToolBar.CoolDownControl();
                    break;
            }
            yield return null;
        }
    }

    void Starting()
    {
        if (gameController.players.Count > 0)
        {
            _state = CheckController.State.Checks;
            AdjustDifficulty();
        }
    }

    void CheckTraps ()
    {
        if (trapController.traps.Count == 0 && trapController.secrets.Count == 0)
        {
            return;
        }
        else
        {
            foreach (GameObject player in gameController.players)
            {
                trapController.CheckPerPlayer(player);
            }
        }
    }

    void CheckCreatures ()
    {
    //    Debug.Log(creatureController.creatures.Count);
        if (creatureController.creatures.Count == 0)
        {
            return;
        }
        else
        {
            foreach (GameObject go in creatureController.creatures )
            {
                creatureController.CheckCreature(gameController, go);
            }
            

        }
    }

    void CheckEnemies ()
    {
 //       Debug.Log(enemyController.enemies.Count);
        if (enemyController)
        {

            if (enemyController.enemies.Count == 0)
            {
                return;
            }
            else
            {
                foreach (GameObject player in gameController.players)
                {
                    enemyController.CheckDistance(player);
                }
            }
        }
    }

    void AdjustDifficulty ()
    {
        string difficulty = "";
        string realism = "";

        if (difficulty == "" || difficulty == "nil")
        {
            difficulty = "Normal";
        }

        GetComponent<DisplayOptionsScript>().difficulty = difficulty;

        if (realism == "" || realism == "nil")
        {
            realism = "Epic Fantasy";
        }

        GetComponent<DisplayOptionsScript>().realism = realism;

    }

    void AddjustEnemies ()
    {        
        if (GetComponent<DisplayOptionsScript>().difficulty == "Easy")
        {
            AdjustEasy();
        }
        else if (GetComponent<DisplayOptionsScript>().difficulty == "Hard")
        {
            AdjustHard();
        }
    }

    void AdjustEasy ()
    {
        foreach (GameObject go in GetComponent<EnemyController>().enemies)
        {
            EnemyStats es = go.GetComponent<EnemyStats>();
            es.health = (int)(es.health * 0.7f);
            es.curHealth = (int)(es.curHealth * 0.7f);
            if (es.defence > 10)
            {
                es.defence = es.defence - 10;
            }
            go.GetComponent<EnemyAttack>().attack = (int)(go.GetComponent<EnemyAttack>().attack * 0.7f);
        }
    }

    void AdjustHard ()
    {
        foreach (GameObject go in GetComponent<EnemyController>().enemies)
        {
            EnemyStats es = go.GetComponent<EnemyStats>();
            es.health = (int)(es.health * 1.3f);
            es.curHealth = (int)(es.curHealth * 1.3f);
            if (es.defence > 10)
            {
                es.defence = es.defence + 10;
            }
            es.armour = (int) (es.armour * 1.1f);
            go.GetComponent<EnemyAttack>().attack = (int)(go.GetComponent<EnemyAttack>().attack * 1.30f);
        }
    }    
}
