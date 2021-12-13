using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAICombat : MonoBehaviour 
{
	
	public bool _alive = true;
	public bool InCombat;
	public bool defensiveAI = false;
    public bool animOverride = false;
    public bool magic = false;
	public float distToStop = 2.0f;
    public float distToAttack = 2.0f;
	public float sightDistance = 30;
    public float attackTime = 2.5f;
    public float deadtime = 0.1f;
    public float stunTime = 25;
    public float stunTimer = 0;
    public GameObject enemy = null;
	public GameObject player;
    public GameObject target;
    public GameObject magicWeapon;
    
	private Transform myTransform;
	private GameObject selectedTarget;
	public List <GameObject> targets;
	public string behaviour;
	private GameObject gc;
	private GameController gameController;
    private Camera mainCamera;
	private PlayerAttack playerAttack;
	private UnityEngine.AI.NavMeshAgent nav;
	private PlayerMoveAI playerMoveAI;
	private PlayerStats playerStats;
	private CombatController combatController;
	private Animator anim;	
	public enum State
	{
        FirstCheck,
        IdleInCombat,
		MoveInCombat,
		Guard,
		Attack,
		Dead,
        Search,
        IdleActive,
        MoveActive,
        AttackActive,
        MoveTarget,
        Stun,
        CastingSpell,
        GoTo
	}	
	public State _state;


	void Awake ()
	{
		distToStop = 2.0f;
		gc = GameObject.FindGameObjectWithTag("GameController");
        //		GameController gameController = (GameController)gc.GetComponent <GameController>();
        gameController = gc.GetComponent<GameController>();
        combatController = gc.GetComponent <CombatController>();
		player = gameController.player;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = GetComponent <Animator>();
		playerMoveAI = GetComponent<PlayerMoveAI>();
		playerAttack = GetComponent<PlayerAttack>();
		playerStats = GetComponent <PlayerStats>();
		behaviour = playerStats.behaviour;
		myTransform = transform;
		_alive = true;
        mainCamera = Camera.main;
		behaviour = "Aggressive";
	}
    
	void Start () 
	{
        distToStop = 2.0f;
        gc = GameObject.FindGameObjectWithTag("GameController");
        //		GameController gameController = (GameController)gc.GetComponent <GameController>();
        gameController = gc.GetComponent<GameController>();
        combatController = gc.GetComponent<CombatController>();
        player = gameController.player;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerMoveAI = GetComponent<PlayerMoveAI>();
        playerAttack = GetComponent<PlayerAttack>();
        playerStats = GetComponent<PlayerStats>();
        behaviour = playerStats.behaviour;
        myTransform = transform;
        _alive = true;
        mainCamera = Camera.main;
        behaviour = "Aggressive";
        if (GetComponent<TargetActivePC>())
        {
            target = GetComponent<TargetActivePC>().target;
        }
        StartCoroutine("FSM");
        _state = PlayerAICombat.State.FirstCheck;
        if (gameObject.name != "Weirum")
        {
            string animString = playerAttack.animController;
            RuntimeAnimatorController controller = (RuntimeAnimatorController)(Resources.Load("Animator/" + animString, typeof(RuntimeAnimatorController)));
            anim.runtimeAnimatorController = (controller);
        }
        

	}

    void OnEnable()
    {
        if (GetComponent<TargetActivePC>())
        {
            target = GetComponent<TargetActivePC>().target;
        }        
        StartCoroutine("FSM");
        if (_state != State.Stun)
        {
            _state = PlayerAICombat.State.FirstCheck;
            if (gameObject.name != "Weirum")
            {
                string animString = playerAttack.animController;
                RuntimeAnimatorController controller = (RuntimeAnimatorController)(Resources.Load("Animator/" + animString, typeof(RuntimeAnimatorController)));
                anim.runtimeAnimatorController = (controller);
            }

            GameController.OnChangePlayer += ChangePlayer;
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            nav.stoppingDistance = distToStop - 0.2f;
        }

    }
	
	void OnDisable ()
	{
        //	GameController.OnChangePlayer -= ChangePlayer;
        if (_state != State.Stun)
        {
            StopCoroutine("FSM");
            CancelInvoke("AddAllPC");
            StopAllCoroutines();
        }

	}
	
	private IEnumerator FSM ()
	{
		while (_alive)
		{
			switch (_state)
			{
                case State.FirstCheck:
                    yield return new WaitForSeconds(0);
                    FirstCheck();
                    yield return new WaitForSeconds(1);
                    break;

                case State.IdleInCombat:
                    IdleInCombat();
                    yield return new WaitForSeconds(0.5f);
                    break;

                case State.MoveInCombat:
                    MoveInCombat();
                    break;

                case State.MoveTarget:
                    MoveTarget();
                    break;

                case State.GoTo:
                    MoveTarget();
                    break;

                case State.Guard:
                    Guard();
                    break;

                case State.Attack:
                    Attack();
                    yield return new WaitForSeconds(attackTime);
                    break;

                case State.Dead:
                    Dead();
                    yield return new WaitForSeconds(deadtime);
                    break;

                case State.Search:
                    Search();
                    yield return new WaitForSeconds(0.2f);
                    break;

                case State.IdleActive:
                    IdleActive();
                    yield return new WaitForSeconds(0.5f);
                    break;

                case State.MoveActive:
                    MoveActive();
                    break;

                case State.AttackActive:
                    AttackActive();
                    yield return new WaitForSeconds(attackTime);
                    break;

                case State.Stun:
            //        Debug.Log("Start Stun");          
                    yield return new WaitForSeconds(1);
                    Stun();
          //          Debug.Log("Check Stun");
                    break;

                case State.CastingSpell:
                    yield return new WaitForSeconds(1);
                    CastingSpell ();
                    break;
            }
			yield return null;
		}
	}

    void FirstCheck ()
    {
        if (GetComponent<PlayerStats>().loadEnded == true)
        {
            if (playerStats.curHealth <= 0)
            {
               
                _state = PlayerAICombat.State.Dead;
            }
            else
            {
                distToAttack = playerAttack.distance;
                nav.stoppingDistance = distToAttack;
                nav.enabled = false;
                
                if (GetComponent<TargetActivePC>().enabled == false)
                {
                    _state = PlayerAICombat.State.IdleInCombat;
                }
                else
                {
        //            Debug.Log(gameObject.name + "x");
                        
                    //If RTS tactical camera is enable PC is controlled by clicking (this AI script) and not by the button controls.
                    if (mainCamera.GetComponent<RtsCameraImp>().enabled == true)
                    {
                        StartCoroutine("FSM");
                        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                        //     GetComponent<Rigidbody>().isKinematic = true;
                        _state = PlayerAICombat.State.IdleActive;
                    }
                    // ... if first person camera is enabled then this AI script is not needed 
                    else
                    {
                        StopAllCoroutines();
                        this.enabled = false;
                    }
                }
            }
        }
    }

	void IdleInCombat ()
	{

        anim.SetFloat ("Forward", 0);
		anim.SetFloat ("Turn", 0);
        if (nav.enabled == false)
        {
            nav.enabled = true;
        }        
		nav.Resume();



        if (enemy != null)
		{

            if (behaviour == "Aggressive" || behaviour == "Ranged")
			{
			//	Debug.Log (distToStop);

                
				float distEnemy = Vector3.Distance (enemy.transform.position, transform.position);

                if (distEnemy < distToAttack)
				{
                    if (gameObject.name == "Aurelius")
                    {
               //         Debug.Log(magic);
                    }
                    if (magicWeapon != null)
                    {
                        if (magicWeapon.activeSelf != true)
                        {
                            //            Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(true);

                        }
                    }
                    nav.enabled = false;
                    anim.SetBool("Attacking", true);
                    playerAttack.target = enemy;
                    playerMoveAI.distToStop = playerAttack.distance;
                    _state = PlayerAICombat.State.Attack;

                }
				else
				{
					playerMoveAI.target = enemy;
                    playerAttack.target = enemy;
                    playerMoveAI.distToStop = playerAttack.distance;
                    nav.enabled = true;
					playerMoveAI.NavAnimSetup();
                    if (magicWeapon != null)
                    {
                        if (magicWeapon.activeSelf)
                        {
                            magicWeapon.SetActive(false);
                        }
                    }

                    _state = PlayerAICombat.State.MoveInCombat;	
				}
			}
		}
		else
		{
            if (gameController.globalAI == true)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                _state = PlayerAICombat.State.Search;
            }
        
		}
	}

	void MoveInCombat()
	{
        nav.enabled = true;
		nav.Resume();
		if (enemy != null)
		{
            if (enemy.GetComponent<EnemyStats>().curHealth <= 0)
            {
                enemy = null;
                _state = PlayerAICombat.State.Search;
            }
            else
            {
                playerMoveAI.target = enemy;
                playerMoveAI.distToStop = playerAttack.distance;

                float distEnemy = Vector3.Distance(enemy.transform.position, transform.position);

                if (distEnemy < (distToAttack))
                {
                    if (magic != true)
                    {
                        nav.enabled = false;
                        anim.SetBool("Attacking", true);
                        playerAttack.target = enemy;
                        playerMoveAI.distToStop = playerAttack.distance;
                        _state = PlayerAICombat.State.Attack;
                    }
                    else
                    {
                        if (magicWeapon.activeSelf != true)
                        {
                  //          Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(true);
                            _state = PlayerAICombat.State.Attack;
                        }
                    }
                }
                else
                {
                    //	GetComponent<Rigidbody>().isKinematic = true;                
                    playerMoveAI.NavAnimSetup();
                }
            }

		}
		
		else
		{
            nav.enabled = false;
			_state = PlayerAICombat.State.IdleInCombat;
		}
	}
    
	void Guard ()
	{
		
	}
	
	void Attack ()
	{


        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);

        if (target.activeSelf)
        {
            enemy = null;
            if (magicWeapon != null)
            {
                if (magicWeapon.activeSelf)
                {
                    magicWeapon.SetActive(false);
                }
            }

            _state = PlayerAICombat.State.MoveInCombat;
        }
        else if (enemy != null)
        {
            /*
            if (gameObject.name == "Preyton")
            {
                Debug.Log(enemy);
            }*/
            //  transform.LookAt(enemy.transform.position);    //placed on PlayerAttackScript
            if (playerStats.curHealth <= 0)
            {
                nav.enabled = false;
                _state = PlayerAICombat.State.Dead;
                anim.SetBool("Attacking", false);
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf)
                    {
                        magicWeapon.SetActive(false);
                    }
                }

                return;
            }
            EnemyStats es = enemy.GetComponent<EnemyStats>();
            if (es.curHealth == 0)
            {
                enemy = null;
                _state = PlayerAICombat.State.Search;
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf)
                    {
                        magicWeapon.SetActive(false);
                    }
                }

                return;
            }

            if (anim.GetBool("Attacking") == false)
            {
                anim.SetBool("Attacking", true);
            }


            float distEnemy = Vector3.Distance(enemy.transform.position, transform.position);
 //           Debug.Log(distEnemy + "/" + gameObject.name + "/" + distToAttack);
            //	EnemyStats es = (EnemyStats)enemy.GetComponent("EnemyStats");
            if (distEnemy >= distToAttack)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                playerMoveAI.target = enemy;
                anim.SetBool("Attacking", false);
                nav.enabled = true;
                if (magic == true)
                {
                    if (magicWeapon.activeSelf)
                    {
                        magicWeapon.SetActive(false);
                    }
                }
                _state = PlayerAICombat.State.MoveInCombat;
                //			Debug.Log ("dist > dist Stop/" + distEnemy);
            }
            else
            {
                /*
                if (gameObject.name == "Preyton")
                {
                    Debug.Log("within range");
                }*/
                if (enemy != null)
                {
                    transform.LookAt(enemy.transform);
                    if (playerStats.invisible == "Yes")
                    {
                        playerStats.invisible = "No";
                    }

                    if (magicWeapon == null)
                    {
                        playerAttack.Attack();
                    }  
                    else
                    {
                        if (magicWeapon.activeSelf != true)
                        {
                            Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(true);
                        }
                    }                 
                }
            }
        }
        else if (enemy == null)
        {
            enemy = null;
            _state = PlayerAICombat.State.Search;
            if (magicWeapon != null)
            {
                if (magicWeapon.activeSelf)
                {
                    magicWeapon.SetActive(false);
                }
            }
        }
    }
	
	void Dead ()
	{
		anim.SetFloat ("Forward", 0);
		anim.SetFloat ("Turn", 0);

	//	Debug.Log (anim.GetBool ("Dead")+ "/" + gameObject );

		if (anim.GetBool ("Dead") == false)
		{
			anim.SetBool ("Dead", true);
            deadtime = 1;
		}

		if (anim.GetBool ("Attacking") == true)
		{
			anim.SetBool ("Attacking", false);
		}
	}

    void Search()
    {
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);

        if (GetComponent<TargetActivePC>().enabled)
        {
            enemy = null;
            _state = PlayerAICombat.State.IdleActive;
        }
        else if (enemy == null)
        {

            if (gameController.globalAI == true)
            {
                SortEnemies();

                if (enemy != null)
                {
                    playerAttack.target = enemy;
                    _state = PlayerAICombat.State.IdleInCombat;
                }
            }
            else
            {
                _state = PlayerAICombat.State.IdleInCombat;
            }

        }
        else
        {
            playerAttack.target = enemy;
            _state = PlayerAICombat.State.IdleInCombat;
        }
    }

    void IdleActive ()
    {
 //       Debug.Log("IdleActive" + gameObject.name + "/enemy " + enemy + "/Target " + target);
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);
        if (nav.enabled == false)
        {
            nav.enabled = true;
        }
        nav.Resume();

        if (target.activeSelf)
        {
            float distTarget = Vector3.Distance(target.transform.position, transform.position);
            enemy = null;
            if (distTarget < 0.09f)
            {
                Debug.Log("Waiting for instructions");
                target.SetActive(false);
            }
            else
            {
                
                playerMoveAI.target = target;
                playerAttack.target = null;
                playerMoveAI.distToStop = 0.09f;
                nav.enabled = true;
                playerMoveAI.NavAnimSetup();
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf)
                    {
                        magicWeapon.SetActive(false);
                    }
                }

                _state = PlayerAICombat.State.MoveTarget;
            }
        }
        else if (enemy != null)
        {
            if (behaviour == "Aggressive" || behaviour == "Ranged")
            {
                //	Debug.Log (distToStop);
                float distEnemy = Vector3.Distance(enemy.transform.position, transform.position);

                if (distEnemy < distToAttack)
                {
                    nav.enabled = false;
                    anim.SetBool("Attacking", true);
                    playerAttack.target = enemy;
                    playerMoveAI.distToStop = playerAttack.distance;
                    if (magicWeapon != null)
                    {
                        if (magicWeapon.activeSelf != true)
                        {
                 //           Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(true);
                        }
                    }

                    _state = PlayerAICombat.State.AttackActive;
                }
                else
                {
                    playerMoveAI.target = enemy;
                    playerAttack.target = enemy;                   
                    playerMoveAI.distToStop = playerAttack.distance;
                    nav.enabled = true;
                    playerMoveAI.NavAnimSetup();
                    if (magicWeapon != null)
                    {
                        if (magicWeapon.activeSelf)
                        {
                            magicWeapon.SetActive(false);
                        }
                    }
                    _state = PlayerAICombat.State.MoveActive;
                }
            }
        }

    }

    void MoveActive ()
    {
        
        nav.enabled = true;
        nav.Resume();

        if (target.activeSelf)
        {
            playerMoveAI.target = target;
            playerMoveAI.distToStop = 1;
            float distTarget = Vector3.Distance(target.transform.position, transform.position);
            if (distTarget < (0.3f))
            {
                anim.SetBool("Attacking", true);
                nav.enabled = false;
                target.SetActive(false);
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf != true)
                    {
                        Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(true);
                    }
                }
                _state = PlayerAICombat.State.IdleActive;
                playerMoveAI.NavAnimSetup();
            }
            else
            {
                playerMoveAI.NavAnimSetup();
            }
        }
        else if (enemy != null)
        {
            playerMoveAI.target = enemy;
            playerMoveAI.distToStop = playerStats.attackRange;
            float distEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distEnemy < (distToAttack))
            {
                anim.SetBool("Attacking", true);
                nav.enabled = false;
                target.SetActive(false);
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf != true)
                    {
                        Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(true);
                    }
                }
                _state = PlayerAICombat.State.AttackActive;
                playerMoveAI.NavAnimSetup();
            }
            else
            {
                playerMoveAI.NavAnimSetup();
            }
        }
        else
        {
            nav.enabled = false;
            if (magicWeapon != null)
            {
                if (magicWeapon.activeSelf)
                {
                    Debug.Log("Magic no active, we activate it");
                    magicWeapon.SetActive(false);
                }
            }

            _state = PlayerAICombat.State.IdleActive;
        }
    }

    void MoveTarget ()
    {/*
        if (gameObject.name == "Player")
        {
  //          Debug.Log("MoveTargetActive");
        }*/
        nav.enabled = true;
        nav.Resume();
        enemy = null;

        if (target.activeSelf)
        {
            playerMoveAI.target = target;
            playerMoveAI.distToStop = 0.3f;
            float distTarget = Vector3.Distance(target.transform.position, transform.position);

            if (distTarget < (0.2f))
            {
                _state = PlayerAICombat.State.IdleActive;
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf)
                    {
             //           Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(false);
                    }
                }

                target.SetActive(false);
                playerMoveAI.NavAnimSetup();
            }
            else
            {
                playerMoveAI.target = target;
                playerMoveAI.distToStop = 0.2f;
                playerMoveAI.NavAnimSetup();
            }
        }
    }


    private void GoTo()
    {
        //     Debug.Log("goto" + gameObject.name);
        nav.enabled = true;
        nav.Resume();

        if (target != null)
        {
            playerMoveAI.target = target;
            playerMoveAI.distToStop = 0.3f;
            float distTarget = Vector3.Distance(target.transform.position, transform.position);

            if (distTarget < (0.3f))
            {
                _state = State.IdleInCombat ;
                target.SetActive(false);
                playerMoveAI.NavAnimSetup();
            }
            else
            {
                playerMoveAI.target = target;
                playerMoveAI.distToStop = 0.3f;
                playerMoveAI.NavAnimSetup();
            }
        }
        else
        {
            target.SetActive(false);
            _state = State.IdleInCombat;
            playerMoveAI.NavAnimSetup();
        }
    }


    void AttackActive()
    {
   //     Debug.Log("Attack active " + gameObject.name);
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);

        if (target.activeSelf)
        {
            enemy = null;
            _state = PlayerAICombat.State.IdleActive;
        }
        else if (enemy != null)
        {
            //  transform.LookAt(enemy.transform.position);    //placed on PlayerAttackScript
            if (playerStats.curHealth <= 0)
            {
                nav.enabled = false;
                _state = PlayerAICombat.State.Dead;
                anim.SetBool("Attacking", false);
                return;
            }
            EnemyStats es = enemy.GetComponent<EnemyStats>();

            if (es.curHealth == 0)
            {
                enemy = null;
                return;
            }

            if (anim.GetBool("Attacking") == false)
            {
                anim.SetBool("Attacking", true);
            }


            float distEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            //	EnemyStats es = (EnemyStats)enemy.GetComponent("EnemyStats");
            if (distEnemy >= distToAttack)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                playerMoveAI.target = enemy;
                anim.SetBool("Attacking", false);
                nav.enabled = true;
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf)
                    {
                        Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(false);
                    }
                }

                _state = PlayerAICombat.State.MoveActive;
                //			Debug.Log ("dist > dist Stop/" + distEnemy);
            }
            else
            {
                if (enemy != null)
                {
                    transform.LookAt(enemy.transform);
                    /*
                    anim.SetFloat("Forward", 0);
                    int ranNumber = Random.Range(1, 5);
                    anim.SetTrigger("Attack" + ranNumber);
                    */
                    if (playerStats.invisible == "Yes")
                    {
                        playerStats.invisible = "No";
                    }
                    if (magic != true)
                    {
                        playerAttack.Attack();
                    }
                    else
                    {
                        if (magicWeapon.activeSelf != true)
                        {
                            Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(true);

                        }
                    }
                }
            }
        }
    }
    
    public void SortEnemies()
    {
         float minDistance = Mathf.Infinity;             //Initiate distance at max possible value
        GameObject enemySelected = null;                           //in case all PC die... sad

        if (gameController.GetComponent<EnemyController>() == null)
        {
            combatController.ChangeToPeace();
        }
        else
        {
            if (gameController.GetComponent<EnemyController>().enemies.Count > 0)
            {
                foreach (GameObject enemyPC in gameController.GetComponent<EnemyController>().enemies)
                {

                    if (enemyPC.tag == "Enemy")
                    {
                        Transform ta = enemyPC.transform;
                        float playerDistance = Vector3.Distance(enemyPC.transform.position, transform.position);

                        if (playerDistance < minDistance)
                        {
                            minDistance = playerDistance;
                            enemySelected = enemyPC;
                        }
                    }
                }

                enemy = enemySelected;
            }
            else
            {
                combatController.ChangeToPeace();
            }
        }



    }

	//Delegate to change target of all inactive PC at once. 
	void ChangePlayer(GameObject playera)
	{		
		player = playera;		
	}

	public void DefenseBehaviour (GameObject go)
	{
		if (go != null)
		{
			enemy = go;
			_state = PlayerAICombat.State.MoveInCombat;
		}
	}



	public void Die ()
	{
	//	Debug.Log ("Die function");
		_state = PlayerAICombat.State.Dead;
	}

    public void Life ()
    {
        StartCoroutine("FSM");
        nav.enabled = false;
        _state = PlayerAICombat.State.IdleInCombat;
    }

    public void ChangeToNoActive()
    {
        StopAllCoroutines();
        StartCoroutine("FSM");
        _state = PlayerAICombat.State.IdleInCombat;
    }

    public void ChangeToActive()
    {/*
        StopAllCoroutines();
        StartCoroutine("FSM");*/
        _state = PlayerAICombat.State.IdleActive;
    }

    public void ChangeMoveTarget ()
    {
        enemy = null;
        GetComponent<Rigidbody>().isKinematic = true;
        playerMoveAI.target = target;
        anim.SetBool("Attacking", false);
        nav.enabled = true;
        _state = PlayerAICombat.State.MoveActive;
    }

    public void TargetCommandsAttack (GameObject enemyTarget)
    {
        enemy = enemyTarget;
        float distanceEnemy = Vector3.Distance(transform.position, enemyTarget.transform.position);
        if (distanceEnemy <= distToAttack)
        {
            _state = PlayerAICombat.State.Attack;
        }
        else
        {
            _state = PlayerAICombat.State.MoveActive;
        }
        
    }
    /*
    void UpDate ()
    {
        if (gameObject.name == "Fred")
        {
            Debug.Log(_state);
        }
    }*/

    void Stun ()
    {
        stunTimer = stunTimer +1; 
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);
        if (nav.enabled == false)
        {
            nav.enabled = true;
        }
        nav.Resume();
        if (stunTimer == 15)
        {
            int chancesToPass = 0;
            int intelligence = PixelCrushers.DialogueSystem.DialogueLua.GetActorField(gameObject.name, "intelligence").AsInt;
            int willpower = PixelCrushers.DialogueSystem.DialogueLua.GetActorField(gameObject.name, "willpower").AsInt;

            int intelChances = (intelligence - 10) * 8;
            int willChances = (willpower - 10) * 8;
            if (intelChances > willChances)
            {
                chancesToPass = intelChances;
            }
            else
            {
                chancesToPass = willChances;
            }

            int diceRoll = Random.Range(0, 100);
            if (diceRoll < chancesToPass)
            {
                StopStunState();
            }
            
        }
        else if (stunTimer == 25)
        {
            int chancesToPass = 0;
            int intelligence = PixelCrushers.DialogueSystem.DialogueLua.GetActorField(gameObject.name, "intelligence").AsInt;
            int willpower = PixelCrushers.DialogueSystem.DialogueLua.GetActorField(gameObject.name, "willpower").AsInt;

            int intelChances = (intelligence - 10) * 8;
            int willChances = (willpower - 10) * 8;
            if (intelChances > willChances)
            {
                chancesToPass = intelChances;
            }
            else
            {
                chancesToPass = willChances;
            }

            int diceRoll = Random.Range(0, 100);
            if (diceRoll < chancesToPass)
            {
                StopStunState();
            }
        }
        else if (stunTimer >= 35)
        {
            StopStunState();
        }
    }

    void CastingSpell ()
    {
    //    Debug.Log("Casting");
    }

    void StopStunState ()
    {
        int tempCNT = 10000;
        gameObject.tag = "Player";
        for (int cnt = 0; cnt < playerStats.states.Count; cnt++)
        {
            if (playerStats.states[cnt] == "Stun")
            {
                tempCNT = cnt;
            }
        }
        if (tempCNT != 10000)
        {
            playerStats.states.RemoveAt(tempCNT);
        }
        if (transform.Find("States/Stun") != null)
        transform.Find("States/Stun").gameObject.SetActive(false);
        stunTimer = 0;
        playerStats.stunned = false;
        if (this.enabled == true)
        {
            _state = PlayerAICombat.State.IdleInCombat;
        }
        else
        {
            StopAllCoroutines();
        }
        
    }
}