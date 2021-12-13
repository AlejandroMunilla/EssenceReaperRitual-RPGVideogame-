using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class EnemyAI : MonoBehaviour 
{
	public bool playerInSight = false;
	public bool playerInRange = false;
    public bool magicActive = false;
    public bool specialAbilityActive = false;
    public string specialAbility = "";
    public string animatorName = "None";
    public string[] priorityTargets = null;
	public float smallRadius = 2f;
    public float specialAbilityCooldown = 60;
    public float specialAbilityRange = 15;
    public float timeHold = 0;
    public bool inBattle = false;
    public bool changeAnimator = false;
    public bool ignoreSightDistance = false;
	public GameObject player;
    public GameObject magicWeapon;


    private int layerMask;
    private float fieldOfViewAngle = 150f;
    private float specialAbilityTimer = -25;
    private float timeStun = 20;
    
    private bool _alive;            					//is Enemy Alive? 
    private GameObject tempPlayer;
	private GameObject gc;
    
	private EnemyMove enemyMove;
	private EnemyAttack enemyAttack;
	private EnemyStats enemyStats;
    public EnemyAIGeneral enemyAIGeneral;
    public GameController gameController;
	private CombatController combatController;
	private UnityEngine.AI.NavMeshAgent nav;
    private UnityEngine.AI.NavMeshObstacle navObs;
    private RuntimeAnimatorController animator;
	private Animator anim;
    public enum State
    {
        Idle,
        Search,
        Move,
        Attack,
        Flee,
        Stun,
        Hold
    }
    public State _state;
    //	public SphereCollider myCollider;

    //VARIABLES FOR Target Next Enemy functions, which is in charge of choosing next PC alive
    public List <GameObject> targets;
	private GameObject selectedTarget;
	private Transform myTransform;
    private float counterCheck;
    private float counterWatch;
 //   int layer_mask = LayerMask.GetMask("Default");

    // Use this for initialization
    void Start () 
	{
		enemyMove = GetComponent<EnemyMove>();
	//	myCollider = GetComponent<SphereCollider>();
		enemyAttack = GetComponent <EnemyAttack>();
		enemyStats = GetComponent <EnemyStats>();
        if (transform.root != transform)
        {
            GameObject rootGO = transform.root.gameObject;
       //     Debug.Log(rootGO);
            enemyAIGeneral = rootGO.GetComponent<EnemyAIGeneral>();
        }        
		nav = GetComponent <UnityEngine.AI.NavMeshAgent>();
        navObs = GetComponent<UnityEngine.AI.NavMeshObstacle>();
		anim = GetComponent <Animator>();
		gc = GameObject.FindGameObjectWithTag ("GameController");
        gameController = gc.GetComponent<GameController>();        
		combatController = gc.GetComponent <CombatController>();
    	playerInSight = false;
		_alive = true;
		StartCoroutine ("FSM");
		_state = EnemyAI.State.Idle;
		nav.stoppingDistance = 1.8f;
        counterCheck = 0;
        counterWatch = 0;
        specialAbilityTimer = -50;
        layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (changeAnimator == true)
        {
  //          Debug.Log(animatorName);
            anim.runtimeAnimatorController = Instantiate(Resources.Load("Animator/" +  animatorName)) as RuntimeAnimatorController;
        }
    }

    void OnEnable ()
    {
        Start();
    }

	private void OnDisable ()
	{
		StopAllCoroutines();
	}
	
	private IEnumerator FSM ()
	{
		while (_alive)
		{
			switch (_state)
			{
			case State.Idle:
				Idle();
				yield return new WaitForSeconds (0.5f);
				break;

			case State.Search:
				Search();
				yield return new WaitForSeconds (0.5f);
				break;

			case State.Move:
                    yield return new WaitForSeconds(0);
                    Move ();
				break;
				
			case State.Attack:
                    
                    Attack();
                    yield return new WaitForSeconds(2);
                    break;
				
			case State.Flee:
                    yield return new WaitForSeconds(0.5f);
                    Flee ();
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
	
	private void Idle()
	{
     //   Debug.Log("Idle");
	//	myCollider.radius = radiusBig;
		anim.SetFloat ("Forward", 0);
		anim.SetFloat ("Turn", 0);

		if (player != null)
		{
            if (changeAnimator == true)
            {
                if (anim.runtimeAnimatorController != animator)
                {
                    anim.runtimeAnimatorController = animator;
                }
            }
            if (inBattle == false)
            {
                inBattle = true;
            }
            


            float distPlayer = Vector3.Distance (player.transform.position, transform.position);				
			
			if (distPlayer < enemyStats.attackRange)
			{
                combatController.ChangeToBattle();
                anim.SetBool("Attacking", true);
             //   nav.enabled = false;
                _state = EnemyAI.State.Attack;
			//	enemyMove.NavAnimSetup();
			}
			else
			{
                anim.SetBool("Attacking", false);
                enemyMove.player = player;
				nav.enabled = true;
                nav.stoppingDistance = enemyStats.attackRange;
                nav.destination = player.transform.position;
				nav.Resume();
				nav.speed = 1;
				_state = EnemyAI.State.Move;
                enemyMove.enabled = true;
				enemyMove.NavAnimSetup();
			}
		}
		else
		{
			_state = EnemyAI.State.Search;
		}
	}

	private void Search ()
	{
        
        if (player != null)
        {
            Debug.Log(player.name + "/" + player.tag + "/" + DialogueLua.GetActorField(player.name, "curHealth").AsInt);
            if (player.tag == "PlayerDead" || player.tag == "npcDead" || DialogueLua.GetActorField(player.name, "curHealth").AsInt <= 0)
            {
                player = null;
            }

            else if (player.GetComponent<npcAI>() != null)
            {
                if (player.GetComponent<npcStats>().curHealth <= 0)
                {
                    player = null;
                }
            }
        }

   		   
        if (player == null  )
		{
            player = null;
			AddAllPC();
			return;
		}
		else
		{
     //       Debug.Log(player.name + "/" + player.tag + "/" + DialogueLua.GetActorField(player.name, "curHealth").AsInt);

            inBattle = true;	
            
            gc.GetComponent<CombatController>().ChangeToBattle();
	
			float distPlayer = Vector3.Distance (player.transform.position, transform.position);				
		


			if (distPlayer < enemyStats.attackRange)
			{
				_state = EnemyAI.State.Attack;
				enemyMove.NavAnimSetup();
			}            
			else
			{	
				nav.enabled = true;
                nav.stoppingDistance = enemyStats.attackRange;
                nav.destination = player.transform.position;
				nav.Resume();
				nav.speed = 1;
	//			myCollider.radius = smallRadius;                
                _state = EnemyAI.State.Move;
                enemyMove.enabled = true;
                enemyMove.player = player;
				enemyMove.NavAnimSetup();
                enemyAttack.target = player;
			}
		}
	}

	private void Move()
	{

        if (player != null)
        {
            if (player.tag == "PlayerDead" || player.tag == "npcDead")
            {
                player = null;
            }
        }

        if (player != null)
        {
            if (player.GetComponent<PlayerStats>() != null)
            {
                if (DialogueLua.GetActorField(player.name, "curHealth").AsInt <= 0)
                {
                    player = null;
                }
            }
            else if (player.GetComponent<npcAI>() != null)
            {
                if (player.GetComponent<npcStats>().curHealth <= 0)
                {
                    player = null;
                }
            }
        }
        //     Debug.Log("Move" + "/" + gameObject);
        counterWatch += Time.deltaTime;
		if (player != null)
		{
			float distPlayer = Vector3.Distance (player.transform.position, transform.position);
            
            if (counterWatch > counterCheck + 2 && priorityTargets.Length == 0)
            {
        //        Debug.Log("check");
                if (enemyStats.smartTarget == "")
                {
                    CheckCloserEnemies(distPlayer);
                    counterCheck = counterWatch;
                }
                else
                {

                }
            }



            bool lineOfSight = true;


            if (enemyStats.attackRange > 4)
            {
                lineOfSight = CheckLineOfSight();
                Debug.Log(gameObject.name + "line of sight" + lineOfSight);
                
            }
            else
            {
                nav.stoppingDistance = 1;
            }

     //       Debug.Log(specialAbility + "/" + Time.time + "/" + specialAbilityActive + "/" + specialAbilityCooldown);
            if (specialAbility != "None" && (Time.time - specialAbilityTimer) >= specialAbilityCooldown)
            {
                if ((Time.time - specialAbilityTimer) >= specialAbilityCooldown || specialAbilityTimer < 0)

                {
                    if (gameObject.name == "Nissa")
                    {
                        Debug.Log("else");
                    }

           //         Debug.Log(distPlayer);
                    if (distPlayer <= specialAbilityRange)
                    {
                        specialAbilityTimer = Time.time;
                        transform.Find(specialAbility).gameObject.SetActive(true);
                        _state = State.Idle;

                    }
                }
            }

            //       Debug.Log(distPlayer + "/" + lineOfSight);
      //      Debug.Log(distPlayer + "/" + gameObject.name + "/" + lineOfSight);
            //Does target is within attack range and is within lineOfSight?
            if (distPlayer < (enemyStats.attackRange +0.1f) && lineOfSight == true)
			{
                
            //    nav.enabled = false;
                enemyAttack.target = player;
                anim.SetBool("Attacking", true);

                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf != true)
                    {
                        //            Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(true);
                    }
                }
                _state = EnemyAI.State.Attack;
                enemyMove.enabled = false;
		//		enemyMove.NavAnimSetup();
			}
			else
			{
                //   Debug.Log(distPlayer + "/Mayor");
                anim.SetFloat("Forward", 1);
                anim.SetBool("Attacking", false);
                nav.destination = player.transform.position;
				enemyMove.NavAnimSetup();
			}
		}
		else if (inBattle == true)
		{
            enemyMove.enabled = false;
            _state = EnemyAI.State.Search;
		}
		
	}
	
	private void Attack()
	{
		inBattle = true;
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);

        if (player != null)
        {
            if (player.tag == "PlayerDead" || player.tag == "npcDead")
            {
                player = null;
            }
        }

        if (player != null)
        {
            if (player.GetComponent<PlayerStats>() != null)
            {
                if (DialogueLua.GetActorField(player.name, "curHealth").AsInt <= 0)
                {
                    player = null;
                }
            }
            else if (player.GetComponent<npcAI>() != null)
            {
                if (player.GetComponent<npcStats>().curHealth <= 0)
                {
                    player = null;
                }
            }
        }

        if (player == null)
        {
            enemyAttack.target = null;
            anim.SetBool("Attacking", false);
            _state = EnemyAI.State.Search;
            return;
        }


        else if (player.tag == "PlayerDead" || player.tag == "npcDead" )

        {
            player = null;
            enemyAttack.target = null;
            anim.SetBool("Attacking", false);
            _state = EnemyAI.State.Search;
            return;
        }
        else if (player != null && specialAbilityActive == false)
		{

            transform.LookAt(player.transform);

       //     Debug.Log(Time.time + "/" + specialAbilityCooldown);
            //If there is an special ability and cooldown has passed out
            if (specialAbility != "None" && (Time.time - specialAbilityTimer) >= specialAbilityCooldown)
            {
                if ((Time.time - specialAbilityTimer) >= specialAbilityCooldown || specialAbilityTimer < 0)

                {
                    if (gameObject.name == "Nissa")
                    {
                        Debug.Log("else");
                    }
                    float distPlayer = Vector3.Distance(player.transform.position, transform.position);
                    if (distPlayer <= specialAbilityRange)
                    {
                        specialAbilityTimer = Time.time;
                        transform.Find(specialAbility).gameObject.SetActive(true);
                        _state = State.Idle;
                    }
                }
            }
            else if (player.tag == "Ally")
            {
                #region
                if (player.GetComponent<npcStats>().curHealth <= 0)
                {
                    player = null;
                    enemyAttack.target = null;
                    anim.SetBool("Attacking", false);
                    _state = EnemyAI.State.Search;
                    return;
                }
                float distPlayer = Vector3.Distance(player.transform.position, transform.position);
                //          npcStats ps = (npcStats)player.GetComponent("npcStats");

                if (distPlayer > enemyStats.attackRange)
                {
                    anim.SetBool("Attacking", false);
                    nav.enabled = true;
                    nav.destination = player.transform.position;
                    nav.Resume();
                    nav.speed = 1;
                    //		myCollider.radius = smallRadius;
                    _state = EnemyAI.State.Move;
                    enemyMove.enabled = true;
                    enemyMove.NavAnimSetup();
                }
                else
                {
                    anim.SetBool("Attacking", true);
                //    nav.enabled = false;
                    enemyMove.NavAnimSetup();
                    transform.LookAt(player.transform);
                    enemyAttack.Attack(player);
                }
                #endregion
            }
            else
            #region
            {
                if (magicWeapon != null)
                {
                    if (magicWeapon.activeSelf != true)
                    {
        //                Debug.Log("Magic no active, we activate it");
                        magicWeapon.SetActive(true);
                    }
                }

                if (player.GetComponent<PlayerStats>() != null)
                {
                    if (player.GetComponent<PlayerStats>().curHealth <= 0)
                    {
                        player = null;
                        enemyAttack.target = null;
                        anim.SetBool("Attacking", false);
                        _state = EnemyAI.State.Search;
                        return;
                    }
                }
                else if (player.GetComponent<npcStats>() != null)
                {
                    if (player.GetComponent<npcStats>().curHealth <= 0)
                    {
                        player = null;
                        enemyAttack.target = null;
                        anim.SetBool("Attacking", false);
                        _state = EnemyAI.State.Search;
                        return;
                    }
                }


                float distPlayer = Vector3.Distance(player.transform.position, transform.position);
  //              PlayerStats ps = (PlayerStats)player.GetComponent("PlayerStats");

                if (distPlayer > enemyStats.attackRange)
                {
                    anim.SetBool("Attacking", false);
                    nav.enabled = true;
                    nav.destination = player.transform.position;
                    nav.Resume();
                    nav.speed = 1;
                    //		myCollider.radius = smallRadius;
                    if (magicWeapon != null)
                    {
                        if (magicWeapon.activeSelf == true)
                        {
                            Debug.Log("Magic no active, we activate it");
                            magicWeapon.SetActive(false);
                        }
                    }

                    _state = EnemyAI.State.Move;
                    enemyMove.enabled = true;
                    enemyMove.NavAnimSetup();
                }
                else
                {
                   
                    if (magicActive != true)
                    {
                        anim.SetBool("Attacking", true);
                //        nav.enabled = false;
                        enemyMove.NavAnimSetup();
                        transform.LookAt(player.transform);
                        enemyAttack.Attack(player);
                    }
                    else
                    {
                        if (magicWeapon != null)
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
            #endregion

        }
		else if (inBattle == true)
		{
      //      nav.enabled = false;
            enemyMove.enabled = false;
            _state = EnemyAI.State.Search;
		}
	}
	
	private void Flee()
	{
		
		_state = EnemyAI.State.Move;
	}

   private void Stun ()
    {
        transform.Find("StunEffect").gameObject.SetActive(false);
        this.enabled = false;
        Invoke("ActivateThis", 0);
    }

    void Hold()
    {
        //    transform.Find("StunEffect").gameObject.SetActive(false);
        _state = State.Idle;
    }

    public void AddAllPC()
	{
        Debug.Log(priorityTargets.Length);

        GameObject PC = null;
        player = null;

       PC = AddPCByDistance();



        if (PC == null)
		{
   //         myCollider.enabled = false;
		}
		else
		{

			player = PC;
			enemyAttack.target = PC;
			enemyMove.player = PC;
			Debug.Log ("The winner is:" + player);
            if (combatController.inCombat == false)
            {
                combatController.ChangeToBattle();
            }
            if (enemyAIGeneral != null)
            {
                enemyAIGeneral.WarnAll(gameObject, player);
            }
		}
	}

    private void CheckCloserEnemies (float distPlayer)
    {
        //This foreach iterate among PC distances, so it attacks the closer PC if very close
        foreach (GameObject go in gameController.players)
        {
            if (go == player)
            {

            }
            else
            {
                float distanceOtherPC = Vector3.Distance(go.transform.position, transform.position);
                if (distanceOtherPC < 3)
                {
                    if (distPlayer > distanceOtherPC)
                    {
                        if (go.GetComponent<PlayerStats>().curHealth > 0)
                        {
                            SetNewEnemy(go);
                            distPlayer = Vector3.Distance(player.transform.position, transform.position);
                        }
                    }
                }
            }
        }
    }

    private void CheckWeakestMember()
    {
        foreach (GameObject go in gameController.players)
        {
            float distancePlayer = Vector3.Distance(go.transform.position, transform.position);
            if (distancePlayer <= enemyStats.sightDistance)
            {
                string profession = go.GetComponent<PlayerStats>().profession;
                if (profession == "Mentalist" || profession == "Mage" || profession == "Thief" )
                {

                }

            }
        }
    }


    public void SetNewEnemy (GameObject newPlayer)
    {
        enemyMove.player = newPlayer;
        enemyAttack.target = newPlayer;
        player = newPlayer;
        counterCheck = counterWatch - 6;
    }

    public void ChangeToSearch()
    {
        _state = State.Search;
    }

    public void ChangeToStun (float timeToStun)
    {
        timeStun = timeToStun;
        _state = State.Stun;
    }

    public void ChangeToHold (float timeToHold)
    {
        if (_state == State.Hold )
        {
            timeHold = timeToHold;
        }
        else if (_state == State.Stun)
        {

        }      
        else
        {
            timeHold = timeToHold;
            _state = State.Hold;
        }

    }

    private bool CheckLineOfSight ()
    {
        bool lineOfSight = false;
        RaycastHit hit;
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        Debug.Log("checklineofSight");

        if (Physics.Raycast(transform.position, targetDirection, out hit, 100, layerMask )  )
        {
            Debug.DrawRay(transform.position, targetDirection * hit.distance, Color.red);
            lineOfSight = true;

        }
        else
        {
            Debug.DrawRay(transform.position, targetDirection * hit.distance, Color.yellow);
        }

        return lineOfSight;
    }

    private GameObject AddPriorityTargets ()
    {
        float minDistance = Mathf.Infinity;             //Initiate distance at max possible value
        GameObject PC = null;                           //in case all PC die... sad

        for (int cnt = 0; cnt < priorityTargets.Length; cnt++)
        {
            if (PC == null)
            {
                foreach (GameObject go in gameController.players)
                {
                    if (priorityTargets[cnt] == go.name)
                    {
                        float playerDistance = Vector3.Distance(go.transform.position, transform.position);

                        if (ignoreSightDistance == true)
                        {




                        }

                        /*
                        else if (enemyStats.sightDistance >= playerDistance)
                        {
                            //Create a vector from the enemy to the player and store the angle between it and forward
                            Vector3 direction = go.transform.position - transform.position;
                            float angle = Vector3.Angle(direction, transform.forward);

                            //if the angle between forward and where the player is, is less than half the angle of view...
                            if (angle < fieldOfViewAngle * 0.5f)
                            {
                                RaycastHit hit;

                                if (combatController.inCombat == true)
                                {
                                    //... the player is in sight.
                                    if (playerDistance < minDistance)
                                    {
                                        //...  this player is closer than any previous player
                                        minDistance = playerDistance;
                                        PC = go;
                                        //                 Debug.Log(PC);
                                    }
                                }

                                //... and if a raycast towards the player hits something...
                                else if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
                                {
                                    //... and if the raycast hits the player...
                                    if (hit.collider.gameObject.tag == Tags.player)
                                    {
                                        //... the player is in sight.
                                        if (playerDistance < minDistance)
                                        {
                                            //...  this player is closer than any previous player
                                            minDistance = playerDistance;
                                            PC = go;
                                            //                                      Debug.Log (PC);
                                        }
                                    }
                                }
                            }
                            //           Debug.Log(enemyStats.sightDistance +"/" + playerDistance);
                        }

                        if (DialogueLua.GetActorField(go.name, "curHealth").AsInt > 0)
                        {
                            PC = go;
                        }*/
                    }
                }
            }
        }


        return PC;

    }


    private GameObject AddPCByDistance ()
    {
        float minDistance = Mathf.Infinity;             //Initiate distance at max possible value
        GameObject PC = null;                           //in case all PC die... sad
        
        foreach (GameObject enemyPC in gc.GetComponent<GameController>().players)
        {

            Debug.Log(enemyPC.name);
            if (enemyPC.tag == "Player" && DialogueLua.GetActorField (enemyPC.name, "curHealth").AsInt > 0)
            {
                //make two functions; 1 for when in battle and the other when not in battle. call either from here
                float playerDistance = Vector3.Distance(enemyPC.transform.position, transform.position);
                string profession = DialogueLua.GetActorField(enemyPC.name, "profession").AsString;




                if (enemyStats.hearingDistance >= playerDistance || combatController.inCombat == true)
                {
                    //Create a vector from the enemy to the player and store the angle between it and forward
                    Vector3 direction = enemyPC.transform.position - transform.position;

                    RaycastHit hit;

                    //... and if a raycast towards the player hits something...

                    if (combatController.inCombat == true)
                    {
                        if (enemyStats.smartTarget != null && enemyStats.smartTarget == profession)
                        {

                            PC = enemyPC;
                            minDistance = 0;
                        }
                        //... the player is in sight.
                        else if (playerDistance < minDistance)
                        {
                            //...  this player is closer than any previous player
                            minDistance = playerDistance;
                            PC = enemyPC;
                            //                 Debug.Log(PC);
                        }
                    }
                    else if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
                    {
                        //... and if the raycast hits the player...
                        if (hit.collider.gameObject.tag == Tags.player)
                        {
                            if (enemyStats.smartTarget != null && enemyStats.smartTarget == profession)
                            {

                                PC = enemyPC;
                                minDistance = 0;
                            }

                            //...  this player is closer than any previous player and it is not a priority target
                            else if (playerDistance < minDistance)
                            {

                                minDistance = playerDistance;
                                PC = enemyPC;
                                //                 Debug.Log(PC);
                            }
                        }

                    }
                }
                else if (enemyStats.sightDistance >= playerDistance)
                {
                    //Create a vector from the enemy to the player and store the angle between it and forward
                    Vector3 direction = enemyPC.transform.position - transform.position;
                    float angle = Vector3.Angle(direction, transform.forward);

                    //if the angle between forward and where the player is, is less than half the angle of view...
                    if (angle < fieldOfViewAngle * 0.5f)
                    {
                        RaycastHit hit;

                        if (combatController.inCombat == true)
                        {
                            //... the player is in sight.
                            if (playerDistance < minDistance)
                            {
                                //...  this player is closer than any previous player
                                minDistance = playerDistance;
                                PC = enemyPC;
                                //                 Debug.Log(PC);
                            }
                        }

                        //... and if a raycast towards the player hits something...
                        else if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
                        {
                            //... and if the raycast hits the player...
                            if (hit.collider.gameObject.tag == Tags.player)
                            {
                                //... the player is in sight.
                                if (playerDistance < minDistance)
                                {
                                    //...  this player is closer than any previous player
                                    minDistance = playerDistance;
                                    PC = enemyPC;
                                    //                                      Debug.Log (PC);
                                }
                            }
                        }
                    }
                    //           Debug.Log(enemyStats.sightDistance +"/" + playerDistance);
                }
            }
        }


        foreach (GameObject enemyPC in gc.GetComponent<GameController>().npc)
        {
            //      Debug.Log(enemyPC);
            if (enemyPC.tag == "Ally")
            {
                float playerDistance = Vector3.Distance(enemyPC.transform.position, transform.position);
                if (enemyStats.hearingDistance >= playerDistance)
                {
                    //Create a vector from the enemy to the player and store the angle between it and forward
                    Vector3 direction = enemyPC.transform.position - transform.position;

                    RaycastHit hit;

                    //... and if a raycast towards the player hits something...
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
                    {
                        //... and if the raycast hits the player...
                        if (hit.collider.gameObject.tag == Tags.player)
                        {
                            //... the player is in sight.
                            if (playerDistance < minDistance)
                            {
                                //...  this player is closer than any previous player
                                minDistance = playerDistance;
                                PC = enemyPC;
                                Debug.Log(PC);
                            }
                        }

                    }

                    /*
                    if (playerDistance < minDistance)
                    {
                        minDistance = playerDistance;
                        PC = enemyPC;
                                             Debug.Log (PC);
                    }*/
                }
                else if (enemyStats.sightDistance >= playerDistance)
                {
                    //Create a vector from the enemy to the player and store the angle between it and forward
                    Vector3 direction = enemyPC.transform.position - transform.position;
                    float angle = Vector3.Angle(direction, transform.forward);

                    //if the angle between forward and where the player is, is less than half the angle of view...
                    if (angle < fieldOfViewAngle * 0.5f)
                    {
                        RaycastHit hit;

                        //... and if a raycast towards the player hits something...
                        if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
                        {
                            //... and if the raycast hits the player...
                            if (hit.collider.gameObject.tag == Tags.player)
                            {
                                //... the player is in sight.
                                if (playerDistance < minDistance)
                                {
                                    //...  this player is closer than any previous player
                                    minDistance = playerDistance;
                                    PC = enemyPC;
                                    Debug.Log(PC);
                                }
                            }
                        }
                    }
                    //           Debug.Log(enemyStats.sightDistance +"/" + playerDistance);
                }
            }
        }

        return PC;
    }

    private void ActivateThis ()
    {
        this.enabled = true;
    }


}