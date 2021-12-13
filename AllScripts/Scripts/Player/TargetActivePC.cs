using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class TargetActivePC : MonoBehaviour 
{
	public GameObject hitGameObject;					//public, PlayerAttack Script needs to access it
	public delegate void ClickPlayer();
	public static event ClickPlayer OnPlayerClicked;
    private Camera mainCamera;
	private Transform myTransform;
	private string hitTag;
	private PlayerAttack playerAttack;
	private ThirdPersonUserControl thirdPersonUser;
	private ThirdPersonCharacter thirdPersonChar;
	private PlayerAI playerAI;
	private PlayerMoveAI playermoveAI;
    private DisplayLoot displayLoot;
    private DisplayPortraits displayPortraits;
	private Animator anim;
	private UnityEngine.AI.NavMeshAgent nav;
	private Rigidbody rig;
    private Camera camera;
    private LayerMask layerMask;
    private PlayerAICombat playerAICombat;
    private bool alreadyLoaded = false;
    private Rect portraitWindowRect = new Rect(Screen.width - (Screen.width * 0.10f), 0, (Screen.width * 0.10f), (Screen.height ));


    //VARIABLES TO SEARCH FOR NEARBY ENEMIES
    public List <GameObject> targets;
	public GameObject enemy;
	private GameObject selectedTarget;
	private GameObject gameController;
	private GameController gcon;
    private CombatController combatController;
    //	private MyGUI myGUI;
    private GeneralWindow generalWindow;
    private DisplayToolBar displayToolBar;
    private DisplayOptionsScript displayOptions;

    //VARIABLES TO INTERACT WITH OBJECTS
    public string abilityInUse = "";               // If an ability is in Use, target will not take effect, 
	public bool pickLockOn = true;					//skill active or not
	public bool trap = false;
    public GameObject target;
    private float distMax = 4;
    private float timeInter = 3;                    // Standard minimum time to interact with objects
    private bool countDownDone = true;              // if true character may use traps/lock skills
    private bool combatTime = true;                 // if true character may attack. 
    private float cooldown = 2.5f;
    private float timer;

    //VARIABLES FOR THE COROUTINE
    private GameObject gc;
        private enum State
        {
            Small,
            Big
        }
       private State _state;
    private GameObject circleBlue;
    private Vector3 scaleOriginal;

    void Awake ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController");
        gcon = gc.GetComponent<GameController>();
        //        myGUI = gc.GetComponent<MyGUI>();
        generalWindow = gc.GetComponent<GeneralWindow>();
        combatController = gc.GetComponent<CombatController>();
        displayToolBar = gc.GetComponent<DisplayToolBar>();
        playerAICombat = GetComponent<PlayerAICombat>();
        displayOptions = gc.GetComponent<DisplayOptionsScript>();
        camera = Camera.main;
        target = (Instantiate(Resources.Load("Models/Object/Target"), transform.position, transform.rotation) as GameObject);
        target.name = "Target" + gameObject.name;
        if( gameObject.name == "Rose")
        {
            GetComponent<PlayerAIRose>().target = target;
        }
        target.SetActive(false);
        layerMask = 1 << 8;
    //    layerMask = ~layerMask;

    }

	void Start () 
	{
        gc = GameObject.FindGameObjectWithTag("GameController");
        gcon = gc.GetComponent<GameController>();
        mainCamera = Camera.main;
		myTransform = transform;
		playerAttack = GetComponent<PlayerAttack>();
		thirdPersonUser = GetComponent<ThirdPersonUserControl>();
		thirdPersonChar = GetComponent<ThirdPersonCharacter>();
        displayLoot = gc.GetComponent<DisplayLoot>();
        displayToolBar = gc.GetComponent<DisplayToolBar>();
        generalWindow = gc.GetComponent<GeneralWindow>();
        playerAI = GetComponent<PlayerAI>();
		playermoveAI = GetComponent<PlayerMoveAI>();
        displayPortraits = gc.GetComponent<DisplayPortraits>();
		anim = GetComponent<Animator>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		rig = GetComponent <Rigidbody>();
        timer = Time.time;
    }

	void OnEnable ()
	{        
        gc = GameObject.FindGameObjectWithTag("GameController");
		gcon = gc.GetComponent<GameController>();
        gcon.activePC = gameObject;
     //   Debug.Log(gameObject.name);
        circleBlue = transform.Find("CircleBlue").gameObject;
        scaleOriginal = circleBlue.transform.localScale;
        StartCoroutine("FSM");
        _state = State.Big;
        //    target.SetActive(true);
    }

    void OnDisable ()
    {
        if (target)
        {
            target.SetActive(false);
        }
        circleBlue.transform.localScale = scaleOriginal;
        
        StopCoroutine("FSM");  
    }

	void Update () 
	{
		RayCastTargetting();
	}

	void RayCastTargetting ()
	{


        if (Input.GetMouseButtonUp(0) && gcon.inDialogue == false)
		{
            if (displayLoot.enabled == false && generalWindow.enabled == false && displayOptions.enabled == false)
			{
                if (portraitWindowRect.Contains(Input.mousePosition) == true || displayToolBar.toolBarRect.Contains(Input.mousePosition) == true)
                {

                    return;
                }
                else
                {
                    //           Debug.Log("fine2");
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100, layerMask))
                    {
                        float distance = Vector3.Distance(hit.point, myTransform.position);
                        string hitTag = hit.transform.tag;
                        Vector3 hitPos = hit.point;
                        hitGameObject = hit.transform.gameObject;


                        //Enemy Region
                        #region 
                        if (hitTag == Tags.enemy)
                        {
                            gcon.ChangeCursorNormal();
                            enemy = hitGameObject;
                            GameObject player = gcon.activePC;
                            playerAttack.target = hitGameObject;
                            if (gameObject.name == "Lycaon")
                            {
                                GetComponent<PlayerAIBlackWolf>().enemy = hitGameObject;
                                if (GetComponent<PlayerAIBlackWolf>().enabled)
                                {
                                    GetComponent<PlayerAIBlackWolf>().target.SetActive(false);
                                }
                            }
                            else if (gameObject.name == "Rose")
                            {
                                
                                GetComponent<PlayerAIRose>().enemy = hitGameObject;
                                if (GetComponent<PlayerAIRose>().enabled)
                                {
                                    GetComponent<PlayerAIRose>().target.SetActive(false);
                                }
                            }
                            else if (gameObject.name == "Kira")
                            {
                                PlayerAIKira aiKira = GetComponent<PlayerAIKira>();
                                aiKira.enemy = hitGameObject;
                                if (aiKira.enabled)
                                {
                                    if (aiKira.target == null)
                                    {
                                        aiKira.target = transform.Find("CircleBlue").gameObject;
                                    }
                                    GetComponent<PlayerAIKira>().target.SetActive(false);
                                }
                            }
                            else
                            {
                                
                                playerAICombat.enemy = hitGameObject;
                                if (playerAICombat.enabled)
                                {
                                    playerAICombat.target.SetActive(false);
                                }
                            }                            


                            if (gc.GetComponent<CombatController>().inCombat == false)
                            {
                                gc.GetComponent<CombatController>().ChangeToBattle();
                            }

                            if (camera.GetComponent<MouseOrbitImp>().enabled == false)
                            {
                                nav.stoppingDistance = playerAttack.distance;
                            }
                            else
                            {
                                if (GetComponent<ThirdPersonUserKira>() != null)
                                {
                                    GetComponent<PlayerAIKira>().TargetCommandsAttack(hitGameObject);
                                }
                                
                                else if (GetComponent<ThirdPersonUserControl>().enabled == false)
                                {
                                    playerAICombat.TargetCommandsAttack(hitGameObject);
                                }
                                else
                                {
                                    if ((timer + 2.5f) < Time.time)
                                    {
                                        Debug.Log(timer + "/" + Time.time);
                                        playerAttack.target = hitGameObject;
                                        GetComponent<PlayerAICombat>().enemy = hitGameObject;
                                        playerAttack.Attack();
                                        timer = Time.time;
                                        Debug.Log(timer);
                                    }
                                }
                            }
                        }
                        #endregion


                        //Terrain hit
                        #region
                        else if (hitGameObject.tag == "Terrain")
                        {
                            gcon.ChangeCursorNormal();
                            if (gcon.inDialogue == false)
                            {
                                nav.stoppingDistance = 0.1f;
                                target.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                                if (gameObject.name == "Kira")
                                {
                                    gameObject.GetComponent<PlayerAIKira>().target.SetActive(true);
                                }

                                else if (gameObject.name != "Rose" && gameObject.name != "Lycaon")
                                {
                                    if (playerAICombat.enabled)
                                    {
                                        playerAICombat.target.SetActive(true);
                                        playerAICombat.enemy = null;
                                        enemy = null;
                                        playerAICombat.ChangeMoveTarget();
                                    }
                                    else if (playerAI.enabled)
                                    {
                                        playerAI.target.SetActive(true);
                                    }
                                }
                                else if (gameObject.name == "Rose")
                                {
                                    gameObject.GetComponent<PlayerAIRose>().target.SetActive(true);
                                }
                                else if (gameObject.name == "Lycaon")
                                {
                                    gameObject.GetComponent<PlayerAIBlackWolf>().target.SetActive(true);
                                }
                            }
                        }
                        #endregion


                        //Player region
                        #region
                        else if (hitTag == Tags.player)
                        {
                            gcon.ChangeCursorNormal();
                            if (hitGameObject.GetComponent<TargetActivePC>() == this)
                            {
                                return;
                            }

                            this.enabled = false;
                            gcon.ChangeActivePlayer(hitGameObject);
                        }
                        #endregion


                        /*
                        else if (distance > 6)
                        {
                            DialogueManager.ShowAlert("You are too far away to interact");
                        }*/
                        else
                        {
                            //NPC region
                            #region
                            if (hitTag == "NPC")
                            {
                                float distNPC = Vector3.Distance(hitGameObject.transform.position, transform.position);


                         //       Debug.Log("NPC" + hitGameObject.name);
                                if (hitGameObject.GetComponent<npcAI>() != null && gcon.currentCursor == gcon.cursorAttack)
                                {
                                    Debug.Log("npcAi");

                                    if ( hitGameObject.GetComponent<npcAI>().potentialEnemy == true)
                                    {
                                        if (hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>() != null)
                                        {
                                            hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>().WarnAll(hitGameObject, gameObject);
                                        }
                                        else
                                        {
                                            hitGameObject.GetComponent<EnemyStats>().ChangeToEnemy();
                                            if (hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>() != null)
                                            {
                                                hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>().WarnAll(hitGameObject, gameObject);
                                            }
                                        }

                                        Debug.Log("npcAi2");
                                    }
                                }
                                else if (hitGameObject.GetComponent<npcAIPatrol>() != null  && gcon.currentCursor == gcon.cursorAttack)
                                {

                                    if (hitGameObject.GetComponent<npcAIPatrol>().potentialEnemy == true)
                                    {
                               //         Debug.Log("npcAiPatrol" + hitGameObject.GetComponent<npcAIPatrol>().potentialEnemy);

                                        hitGameObject.GetComponent<EnemyStats>().ChangeToEnemy();
                                        if (hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>() != null)
                                        {
                                            hitGameObject.transform.parent.GetComponent<EnemyAIGeneral>().WarnAll(hitGameObject, gameObject);
                                        }
                                    }
                                    else
                                    {
                                        gcon.ChangeCursorNormal();
                                    }

                                }


                                //    else if (distNPC < 6)

                                else if (hitGameObject.GetComponent<ConversationTrigger>())
                                {
                                    //                       Debug.Log("Hit");
                                    Transform playerConTransform = gcon.playerConversation.transform;
                                    hitGameObject.GetComponent<ConversationTrigger>().actor = playerConTransform;
                                    
                                    //   DialogueManager.ConversationModel.ActorInfo.Name = "Player";
                                    Texture2D img1 = gc.GetComponent<GeneralWindow>().img1;
                                    
                                    Debug.Log(img1);
                           //         Debug.Log(DialogueManager.ConversationModel.ActorInfo.Name);
                                    

                                    float distanceAdjust = 6;
                                    if (hitGameObject.GetComponent<IgnoreDistanceTalk>() != null)
                                    {
                                        distanceAdjust = hitGameObject.GetComponent<IgnoreDistanceTalk>().maxDistance;
                                    }

                                    if (distNPC < 6 || distNPC < distanceAdjust )
                                    {
                                        string conv = hitGameObject.GetComponent<ConversationTrigger>().conversation.ToString();
                                        Transform actor = gcon.player.transform;
                                        //   Debug.Log(gcon.player + "/" + conv);
                                        DialogueManager.StartConversation(conv, actor);

                                        //This is to change npcAI to Idle so npc doesnt go away walking while in conversation
                                        if (hitGameObject.GetComponent<npcAIPatrol>())
                                        {
                                            hitGameObject.GetComponent<npcAIPatrol>().conversation = true;
                                        }
                                    }
                                }

                                else
                                {
                                    DialogueManager.ShowAlert("You are too far away to interact");
                                }

                            }
                            #endregion

                            //Inter region
                            #region
                            //if player click on an interactable object
                            else if (hitTag == "Inter")
                            {

        //                        Debug.Log("inter");
                                if (gcon.inDialogue == false)
                                {
                                    bool trapAttached = hitGameObject.GetComponent<Trap>();
                                    float distToInter = Vector3.Distance(hitPos, transform.position);

                                    if (hitGameObject.GetComponents<ExitArea>() != null)
                                    {
                                        if (hitGameObject.GetComponent<ExitArea>())
                                        {
                                            if (distToInter < 6)
                                            {
                                    //            Debug.Log(hitGameObject);
                                                if (hitGameObject.GetComponent<ExitArea>().enabled == true)
                                                {
                                                    hitGameObject.GetComponent<ExitArea>().TriggerExit();
                                                }
                                            }
                                            else
                                            {
                                                DialogueManager.ShowAlert("You are too far away to interact");
                                            }
                                        }
                                    }
                                    ///THERE IS A TRAP DISCOVERED***************************************
                                    if (trapAttached == true)
                                    {
                                 //       Debug.Log(trap);
                                        if (hitGameObject.GetComponent<Trap>().discovered == true)
                                        {
                          //                  Debug.Log("true");
                                            trap = true;
                                        }
                                        else
                                        {
                          //                  Debug.Log("false");
                                            trap = false;
                                        }
                                    }
                                    else
                                    {
                                        trap = false;
                                    }

                                    if (countDownDone == false)
                                    {

                                        return;
                                    }

                   //                 Debug.Log(trap);
                                    //IF THERE IS A TRAP ALREADY DISCOVERED ***********************************************
                                    //***************************************************************************************
                                    if (trap == true)
                                    {

                                        int trapSkill = DialogueLua.GetActorField(gameObject.name, "trampAbilitySkill").AsInt + DialogueLua.GetActorField(gameObject.name, "buffTrampAbilitySkill").AsInt;
                      //                  Debug.Log(trapSkill + "/" + gameObject.name);
                                        hitGameObject.GetComponent<Trap>().Disarm(trapSkill, gameObject);
                                        countDownDone = false;
                                        Invoke("Timer", timeInter);
                                    }
                                    //NO TRAP DISCOVERED BUT THERE IS A LOCK
                                    else if (hitGameObject.GetComponent<Lock>() != null)
                                    {
                                        int picklockSkill = GetComponent<PlayerStats>().totTramp;
                                        hitGameObject.GetComponent<Lock>().PickLock(picklockSkill, gameObject);
                                    }
                                    // NO TRAP DISCOVERED AND INTERACTABLE OBJECT
                                    else
                                    {

                                        if (hitGameObject.GetComponent<SecretDoor>())
                                        {
                                            if (distToInter > 4.0f)
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                                return;
                                            }
                                            if (hitGameObject.GetComponent<SecretDoor>() == enabled)
                                            {
                                                if (hitGameObject.GetComponent<SecretDoor>().discovered == true)
                                                {
                                                    hitGameObject.GetComponent<SecretDoor>().DisableRed();
                                                }
                                            }
                                        }

                                        if (hitGameObject.GetComponent<OpenDoor>() != null)
                                        {
                                            if (distToInter > 4.0f)
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                                return;
                                            }
                                            if (hitGameObject.GetComponent<OpenDoor>().enabled == false)
                                            {
                                                return;
                                            }

                                            float distToObject = Vector3.Distance(transform.position, hitGameObject.transform.position);
                                            if (distToObject <= distMax)
                                            {
                                                hitGameObject.GetComponent<OpenDoor>().OpenAndClose();
                                                if (hitGameObject.GetComponent<Trap>() != null)
                                                {
                                                    hitGameObject.GetComponent<Trap>().TriggerTrap(gameObject);
                                                }
                                            }
                                            else
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                            }
                                        }
                                        else if (hitGameObject.GetComponent<Chest>() != null)
                                        {
                                            if (distToInter > 4.0f)
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                                return;
                                            }
                                            hitGameObject.GetComponent<Chest>().closeWindow = true;
                                            hitGameObject.GetComponent<Chest>().OpenAndClose(gameObject);
                                            if (hitGameObject.GetComponent<Trap>() != null)
                                            {
                                                hitGameObject.GetComponent<Trap>().TriggerTrap(gameObject);
                                            }
                                        }
                                        else if (hitGameObject.GetComponent<Loot>() != null || hitGameObject.name == "LootBag")
                                        {
                                            if (distToInter > 4.0f)
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                                return;
                                            }
                                            float distToObject = Vector3.Distance(transform.position, hitGameObject.transform.position);
                                            if (distToObject <= distMax)
                                            {
                                                hitGameObject.GetComponent<Loot>().ItemsToGenInventory();
                                            }
                                            else
                                            {
                                                DialogueManager.ShowAlert("You are too far away");
                                            }
                                        }
                                        else
                                        {
                              //              Debug.Log("what else?");
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
		}
	}

	public void AddAllPC()
	{
		if (enemy != null)
		{
			return;
		}		
		
		Debug.Log ("AddAllPC");
		targets.Clear();
		selectedTarget = null;
		GameObject[] go = GameObject.FindGameObjectsWithTag(Tags.enemy);
		
		foreach (GameObject enemies in go)
		{
			if ( Vector3.Distance (enemies.transform.position, transform.position) < 100)
			{
				//Create a vector from the enemy to the player and store the angle between it and forward
				Vector3 direction =enemies.transform.position - transform.position;
				float angle = Vector3.Angle (direction, transform.forward);
				
				RaycastHit hit;
				
				//... and if a raycast towards the player hits something...
				if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, 100))
				{
					
					//... and if the raycast hits the player...
					if (hit.collider.gameObject == enemies)
						
					{
						AddTarget(enemies);							
					}
				}
			}
		}		
		
		Debug.Log (targets.Count);
		if (targets.Count == 0)
		{
			enemy = null;
		}		
	}
	
	public void AddTarget(GameObject enemies)
	{
		targets.Add (enemies);
		SortTargetByDistance();
	}
	
	private void SortTargetByDistance()
	{
		
		targets.Sort(delegate(GameObject t1, GameObject t2)
		             {
			return Vector3.Distance (t1.transform.position, myTransform.position).CompareTo(Vector3.Distance (t2.transform.position, myTransform.position));
		});		
		
		selectedTarget = targets[0];
		Debug.Log (selectedTarget);
		enemy = selectedTarget;
	}

    private void Timer ()
    {
        countDownDone = true;
    }

    private void CombatTimer ()
    {
        combatTime = true;
    }

    private IEnumerator FSM()
    {
        while (true)
        {
            switch (_state)
            {
                case State.Big:
                    BiggerCircle();
                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.Small:
                    SmallerCircle();
                    yield return new WaitForSeconds(0.1f);
                    break;



            }
            yield return null;
        }
    }

    private void SmallerCircle ()
    {
        circleBlue.transform.localScale -= new Vector3(0.02f, 0.02f, 0);
        if (circleBlue.transform.localScale.x < (scaleOriginal.x * 0.9f))
        {
            _state = TargetActivePC.State.Big;
        }        
    }

    private void BiggerCircle ()
    {
    //    Debug.Log(target + "/" + gameObject.name);
        circleBlue.transform.localScale += new Vector3(0.02f, 0.02f, 0);
        
        if (circleBlue.transform.localScale.x > (scaleOriginal.x * 1.4f))
        {
            _state = TargetActivePC.State.Small;
        }

    }
}
