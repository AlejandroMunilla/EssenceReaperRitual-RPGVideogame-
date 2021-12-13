using UnityEngine;
using System.Collections;

public class EnemyAIAnim : MonoBehaviour {

    public bool specialAbility = false;
    public bool groupAI = false;
    public string idle = "idle";
    public string move = "walk";
    public int attackNo = 0;
    private bool alive = true;
    private bool inCombat = false;
    private float speed;
    private float distAttack;
    private float timeStun = 20;
    private float timeHold = 0;
    private GameObject gc;
    public GameObject target = null;
    public enum State
    {
        Idle,
        Move,
        IdleCombat,
        MoveCombat,
        Attack,
        Patrol,
        Guard,
        Stun,
        Hold
    }
    public State state;   
    private Animation anim;
    private AnimationClip clip;
    private UnityEngine.AI.NavMeshAgent nav;
    private GameController gameController;
    private EnemyStats es;
    private EnemyAttack ea;
    private EnemyAIGeneral enemyAIGeneral;
    private Rigidbody rigidbody;

    void Start ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController");
        gameController = gc.GetComponent<GameController>();
        rigidbody = GetComponent<Rigidbody>();
        es = GetComponent<EnemyStats>();
        ea = GetComponent<EnemyAttack>();
        anim = GetComponent<Animation>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        GameObject parentRoot = transform.root.gameObject;
        if (parentRoot.GetComponent<EnemyAIGeneral>())
        {
            enemyAIGeneral = parentRoot.GetComponent<EnemyAIGeneral>();
            groupAI = true;
        }
        else
        {
            groupAI = false;
        }
        anim.Play(idle);
        StartCoroutine("FSM");
        state = EnemyAIAnim.State.Idle;            
	}

    void OnEnable ()
    {
        Start();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator FSM ()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Idle:
                    Idle();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Move:
                    Move();
                    yield return new WaitForSeconds(0);
                    break;

                case State.MoveCombat:
                    MoveCombat();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Attack:
                    Attack();
                    yield return new WaitForSeconds(4);
                    break;

                case State.Stun:                    
                    yield return new WaitForSeconds(timeStun);
                    Stun();
                    break;

                case State.Hold:
                    yield return new WaitForSeconds(timeHold);
                    Hold ();
                    break;

            }
            yield return null;
        }
    }

    void Idle ()
    {
        if (target == null)
        {            
            float minDistance = 10000;
            GameObject go = null;
            foreach (GameObject player in gameController.players)
            {
                if (player.tag == "Player")
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    //          Debug.Log(distance + "/" + es.sightDistance);
                    if (distance <= es.sightDistance)
                    {
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            go = player;
                        }

                    }
                }

            }

            if (go != null)
            {
                inCombat = true;
                if (gc.GetComponent<CombatController>().inCombat == false)
                {
                    gc.GetComponent<CombatController>().ChangeToBattle();
                }
                
                target = go;
                if (groupAI == true)
                {
                    transform.parent.gameObject.GetComponent<EnemyAIGeneral>().WarnAll(gameObject, go);
                }
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < es.attackRange)
                {
                    if (attackNo == 0)
                    {
                        anim.Play("attack");
                    }
                    else
                    {
                        int animNo = Random.Range(1, attackNo);
                        anim.Play("attack " + animNo.ToString());
                    }
                    
                    transform.LookAt(target.transform);
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    state = EnemyAIAnim.State.Attack;
                }
                else
                {
                    anim.Play(move);
                    anim.Play();
                    nav.destination = target.transform.position;
                    rigidbody.constraints = RigidbodyConstraints.None;
                    state = EnemyAIAnim.State.MoveCombat;
                }
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < es.attackRange)
            {
                transform.LookAt(target.transform);
                if (attackNo == 0)
                {
                    anim.Play("attack");
                }
                else
                {
                    int animNo = Random.Range(1, attackNo);
                    anim.Play("attack " + animNo.ToString());
                }
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                state = EnemyAIAnim.State.Attack;
            }
            else
            {
                anim.Play(move);
                rigidbody.constraints = RigidbodyConstraints.None;
                state = EnemyAIAnim.State.MoveCombat;
            }
        }        
    }

    void Move ()
    {

    }

    void IdleCombat ()
    {

    }

    void MoveCombat ()
    {
        nav.destination = target.transform.position;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < es.attackRange)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            state = EnemyAIAnim.State.Attack;
            if (attackNo == 0)
            {
                anim.Play("attack");
            }
            else
            {
                int animNo = Random.Range(1, attackNo);
                anim.Play("attack " + animNo.ToString());
            }
        }
    }

    void Attack ()
    {

        if (target.GetComponent<PlayerStats>().curHealth <= 0)
        {
            target = null;
            state = EnemyAIAnim.State.Idle;
            return;
        }

        if (anim.name != "attack")
        {
            if (attackNo == 0)
            {
                anim.Play("attack");
            }
            else
            {
                int animNo = Random.Range(1, attackNo);
                anim.Play("attack " + animNo.ToString());
            }
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > es.attackRange)
        {
            anim.Play(move);
            rigidbody.constraints = RigidbodyConstraints.None;
            state = EnemyAIAnim.State.MoveCombat;
        }
        else
        {
            ea.Attack(target);
            transform.LookAt(target.transform);
        }
    }

    void Stun ()
    {
        transform.Find("StunEffect").gameObject.SetActive(false);
        state = State.Idle;
    }

    void Hold ()
    {
    //    transform.Find("StunEffect").gameObject.SetActive(false);
        state = State.Idle;
    }

    public void ChangeToStun (float timeToStun)
    {
        timeStun = timeToStun;
        state = State.Stun;
    }


    public void ChangeToHold(float timeToHold)
    {
        if (state == State.Hold)
        {
            timeHold = timeToHold;
        }
        else if (state == State.Stun)
        {

        }
        else
        {
            timeHold = timeToHold;
            state = State.Hold;
        }

    }
}

