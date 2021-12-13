using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(FadeScreen))]
[RequireComponent(typeof(Pause))]
[RequireComponent(typeof(SaveGame))]
[RequireComponent(typeof(DalilaController))]
[RequireComponent(typeof(CombatController))]
[RequireComponent(typeof(DisplayAI))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioController))]
[RequireComponent(typeof(LoadGame))]
[RequireComponent(typeof(TrapController))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(ExpController))]
[RequireComponent(typeof(DisplayBooks))]
[RequireComponent(typeof(GeneralWindow))]
[RequireComponent(typeof(DisplayItemScript))]
[RequireComponent(typeof(DisplayCharacter))]
[RequireComponent(typeof(DisplayPortraits))]
[RequireComponent(typeof(DisplayToolBar))]
[RequireComponent(typeof(CameraMap))]
[RequireComponent(typeof(WorldMap))]
[RequireComponent(typeof(DisplayLoot))]
[RequireComponent(typeof(DisplayOptionsScript))]
[RequireComponent(typeof(DisplayAI))]
[RequireComponent(typeof(DisplayAssets))]

public class GameController : MonoBehaviour 
{
    #region
    public bool day = true;
	public bool inCombat = false;
    public bool abilityInUse = false;
	public bool inDialogue = false;
    public bool dontKillPlayer = false;
    public bool dialogue;
	public bool inPause; 
	public bool playerActive = true;
    public bool shopping = false;
	public bool PC2Active = false;
	public bool PC3Active = false;
	public bool PC4Active = false;
	public bool PC5Active = false;
    public bool globalAI = true;
	public GameObject player;
	public GameObject PC2Obj;
	public GameObject PC3Obj;
	public GameObject PC4Obj;
	public GameObject PC5Obj;
	public GameObject activePC;
    public GameObject dummyPlayer;
	public List <GameObject> players = new List<GameObject> ();
    public List<GameObject> npc = new List<GameObject>();
    public List<GameObject> cameraDialogue = new List<GameObject>();
    public GameObject playerConversation;
	public bool playerDead;
	public bool PC2Dead;
	public bool PC3Dead;
	public bool PC4Dead;
	public bool PC5Dead;
	public bool sequenceOn;
	public string mainPlayerName;
	public string PC2;
	public string PC3;
	public string PC4;
	public string PC5;
	public string profession;
	public string race;
	public string gender;
	public string saveDirectory;
	public string scene;
    public List<GameObject> healers = new List<GameObject>();
    public List<GameObject> removePC = new List<GameObject>();
	private CombatController combatController;
    private RtsCameraImp rtsCameraImpr;
    private bool rtsCam;
    public bool dmCameraControl = false;
   
    private string healerVar;
    private int pointsVar;
    private int membersVar;


	// this delegate allows to change all "player" target on "PlayerAI" scripts, so PC follows activePC. Used when ActivePC switch to another character
	public delegate void ChangePlayer(GameObject player);
	public static event ChangePlayer OnChangePlayer;
    private GameObject tempActivePC;
    private string activePlayerName;
	private GameObject[] spawnPoints;
	private GameObject DA;
	private Camera mainCamera;

	//Traps
	public GameObject [] allTramps;
	public List<GameObject> enemies = new List<GameObject> ();

    //Universal info
    private CursorMode cursorMode = CursorMode.Auto;
    public Texture2D cursorAttack;
    public Texture2D cursorNormal;
    public Texture2D cursorPoint;
    public Texture2D cursorTarget;
    public Texture2D currentCursor;
    private Vector2 hotSpot = Vector2.zero;

    //Audio
    private AudioSource barks;
    #endregion


    void Awake ()
	{
		inCombat = false;
        if (transform.Find("DummyPlayer").gameObject != null)
        {
            dummyPlayer = transform.Find("DummyPlayer").gameObject;
        }
        else
        {
            Debug.Log("no dummy player");
        }

        if (transform.Find ("Barks"))
        {
            barks = transform.Find("Barks").gameObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("no barks");
        }
 //       Debug.Log(dummyPlayer);
	//	GetAllTrapsInScene();
	}

	void Start () 
	{
        //	Screen.SetResolution (800, 600, true);
        Time.timeScale = 1;
		mainCamera = Camera.main;
        if (GetComponent<CombatController>())
        {
            combatController = GetComponent<CombatController>();
        }
		if (mainCamera.GetComponent<RtsCameraImp>())
        {
            rtsCameraImpr = mainCamera.GetComponent<RtsCameraImp>();
        }
        GetComponent<FadeScreen>().fadeDir = -1;
        GetComponent<FadeScreen>().fadeSpeed = 0.15f;

        cursorNormal = (Texture2D)(Resources.Load("Icons/Cursor/Normal"));
        cursorPoint = (Texture2D)(Resources.Load("Icons/Cursor/Point"));
        cursorAttack = (Texture2D)(Resources.Load("Icons/Cursor/Attack"));
        cursorTarget = (Texture2D)(Resources.Load("Icons/Cursor/Target"));
        currentCursor = cursorNormal;
        Cursor.SetCursor(cursorNormal, Vector2.zero, cursorMode);
        if (GetComponent<CheckController>() == null)
        {
            Debug.LogWarning ("no check controller");
        }

    }

    void OnEnable ()
    {
        mainCamera = Camera.main;
        if (GetComponent<CombatController>())
        {
            combatController = GetComponent<CombatController>();
        }
        if (mainCamera.GetComponent<RtsCameraImp>())
        {
            rtsCameraImpr = mainCamera.GetComponent<RtsCameraImp>();
        }
    }

	public void ChangeActivePlayer( GameObject Destiny)
	{
        tempActivePC = activePC;  //change all activePC in the scripts below for tempactive
        activePC = Destiny;
        //disable Player1 controls
        if (tempActivePC.name == "Lycaon")
        {
            tempActivePC.GetComponent<ThirdPersonUserBlackWolf>().enabled = false;
            tempActivePC.GetComponent<PlayerAIBlackWolf>().enabled = false;
            tempActivePC.GetComponent<PlayerAIBlackWolf>().enabled = true;
            tempActivePC.GetComponent<PlayerAIBlackWolf>().player = Destiny;
            tempActivePC.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
            tempActivePC.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
   //         Debug.Log(tempActivePC);
            if (tempActivePC.name == "Weirum")
            {
                tempActivePC.GetComponent<ThirdPersonCharacterWolf>().enabled = false;
                tempActivePC.GetComponent<ThirdPersonUserControlWolf>().enabled = false;
            }
            else if (tempActivePC.name == "Kira")
            {
                tempActivePC.GetComponent<ThirdPersonCharacterKira>().enabled = false;
                tempActivePC.GetComponent<ThirdPersonUserKira>().enabled = false;
            }
            else if (tempActivePC.name == "Rose")
            {
                tempActivePC.GetComponent<ThirdPersonCharacterRose>().enabled = false;
                tempActivePC.GetComponent<ThirdPersonUserRose>().enabled = false;
            }
            else
            {
                tempActivePC.GetComponent<ThirdPersonCharacter>().enabled = false;
                tempActivePC.GetComponent<ThirdPersonUserControl>().enabled = false;
            }


            if (combatController.inCombat == false)
            {
                if (tempActivePC.name == "Kira")
                {
                    tempActivePC.GetComponent<PlayerAIKira>().player = Destiny;
                    tempActivePC.GetComponent<PlayerAIKira>().enabled = true;
                    tempActivePC.GetComponent<PlayerAIKira>().ChangeToNoActive();
                }

                else if (tempActivePC.name != "Rose")
                {
                    tempActivePC.GetComponent<PlayerAI>().player = Destiny;
                    tempActivePC.GetComponent<PlayerAI>().enabled = true;
                    tempActivePC.GetComponent<PlayerAI>().ChangeToNoActive();
                }
                else
                {
                    PlayerAIRose pRose = tempActivePC.GetComponent<PlayerAIRose>();
                    pRose.player = Destiny;
                    if (pRose.enabled == false)
                    {
                        pRose.enabled = true;
                    }
                   
                    pRose.ChangeToNoActive();
                }
  
            }
            else
            {

                if (tempActivePC.name == "Kira")
                {
                    tempActivePC.GetComponent<PlayerAIKira>().player = Destiny;
                    tempActivePC.GetComponent<PlayerAIKira>().enabled = true;
                    tempActivePC.GetComponent<PlayerAIKira>().ChangeToNoActive();
                }
                else if (tempActivePC.name != "Rose")
                {
                    tempActivePC.GetComponent<PlayerAICombat>().player = Destiny;
                    tempActivePC.GetComponent<PlayerAICombat>().enabled = true;
                    tempActivePC.GetComponent<PlayerAICombat>().ChangeToNoActive();
                }
                else
                {
                    tempActivePC.GetComponent<PlayerAIRose>().player = Destiny;
                    tempActivePC.GetComponent<PlayerAIRose>().enabled = true;
                    tempActivePC.GetComponent<PlayerAIRose>().ChangeToNoActive();
                }

                
            }
            if (tempActivePC.name == "Kira")
            {

            }
            else if (tempActivePC.name != "Rose")
            {
                tempActivePC.GetComponent<PlayerMoveAI>().enabled = true;
            }
            else
            {
                tempActivePC.GetComponent<PlayerMoveAIRose>().enabled = true;
            }
            
            tempActivePC.GetComponent<Rigidbody>().isKinematic = true;              //if this is not done, movement issues

       }
        tempActivePC.GetComponent<TargetActivePC>().enabled = false;
        
        MouseOrbitImp cameraControl = (MouseOrbitImp)mainCamera.GetComponent("MouseOrbitImp");
        if (cameraControl.enabled == true)
        {
            cameraControl.ChangeActivePlayer(Destiny);
        }
        else
        {
            rtsCameraImpr.Follow(Destiny.transform, true);
        }

        activePC = Destiny;     //Destiny is the PC that is about to be become active.    
                                //enable/disable controllers of player2(Destiny)

        if (rtsCameraImpr.enabled == false)
        {
            if (Destiny.GetComponent<PlayerAI>() != null)
            {
                Destiny.GetComponent<PlayerAI>().StopCoroutine("FSM");
                Destiny.GetComponent<PlayerAICombat>().StopCoroutine("FSM");
                Destiny.GetComponent<PlayerAICombat>().CancelInvoke("AddAllPC");
                Destiny.GetComponent<PlayerAI>().enabled = false;
                Destiny.GetComponent<PlayerAICombat>().enabled = false;
                Destiny.GetComponent<PlayerMoveAI>().enabled = true; //????????????
            }
            else
            {


                Debug.Log("SetupROse and others no AI: CancelInvoke");
                if (Destiny.name == "Kira")
                {
                    Destiny.GetComponent<PlayerAIKira>().StopCoroutine("FSM");
                    Destiny.GetComponent<PlayerAIKira>().CancelInvoke("AddAllPC");
                    Destiny.GetComponent<PlayerAIKira>().enabled = false;
                }
            }


            //	Destiny.GetComponent<PlayerMoveAI>().enabled = false;
            if (Destiny.name == "Kira")
            {
                Destiny.GetComponent<ThirdPersonUserKira>().enabled = true;
                Destiny.GetComponent<ThirdPersonCharacterKira>().enabled = true;
            }

            else if (Destiny.name == "Weirum")
            {
                Destiny.GetComponent<ThirdPersonUserControlWolf>().enabled = true;
                Destiny.GetComponent<ThirdPersonCharacterWolf>().enabled = true;
            }
            else if (Destiny.name == "Rose")
            {
                Destiny.GetComponent<ThirdPersonCharacterRose>().enabled = true;
                Destiny.GetComponent<ThirdPersonUserRose>().enabled = true;
                Destiny.GetComponent<PlayerAIRose>().StopCoroutine("FSM");
                Destiny.GetComponent<PlayerAIRose>().enabled = false;
                Destiny.GetComponent<PlayerMoveAIRose>().enabled = true;
            }
            else if (Destiny.name == "Lycaon")
            {
                Destiny.GetComponent<ThirdPersonCharacterBlackWolf>().enabled = true;
                Destiny.GetComponent<ThirdPersonUserBlackWolf>().enabled = true;
                Destiny.GetComponent<PlayerAIBlackWolf>().StopCoroutine("FSM");
                Destiny.GetComponent<PlayerAIBlackWolf>().enabled = false;
                Destiny.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
            }
            else
            {
                Destiny.GetComponent<ThirdPersonCharacter>().enabled = true;
                Destiny.GetComponent<ThirdPersonUserControl>().enabled = true;
            }
        }
        // if RTS camera is active
        else
        {
            rtsCameraImpr.Follow(Destiny, false);
            if (combatController.inCombat == false)
            {
                if (Destiny.name == "Kira")
                {
                    if (Destiny.GetComponent<PlayerAIKira>().target == null)
                    {

                        Destiny.GetComponent<PlayerAIKira>().target = Destiny.GetComponent<TargetActivePC>().target;
                    }
                    Destiny.GetComponent<PlayerAIKira>().enabled = true;
                    Destiny.GetComponent<PlayerAIKira>().ChangeToActive();
                }

                else if (Destiny.name != "Rose" && Destiny.name != "Lycaon")
                {
                    if (Destiny.GetComponent<PlayerAI>().target == null)
                    {

                        Destiny.GetComponent<PlayerAI>().target = Destiny.GetComponent<TargetActivePC>().target;
                    }
                    Destiny.GetComponent<PlayerAI>().enabled = true;
                    Destiny.GetComponent<PlayerAI>().ChangeToActive();
                }
                else
                {
                    if (Destiny.name == "Rose")
                    {
                        if (Destiny.GetComponent<PlayerAIRose>().target == null)
                        {

                            Destiny.GetComponent<PlayerAIRose>().target = Destiny.GetComponent<TargetActivePC>().target;
                        }
                        Destiny.GetComponent<PlayerAIRose>().enabled = false;
                        Destiny.GetComponent<PlayerAIRose>().enabled = true;
                        Destiny.GetComponent<PlayerAIRose>().ChangeToActive();
                    }
                    else if (Destiny.name == "Lycaon")
                    {
                        if (Destiny.GetComponent<PlayerAIBlackWolf>().target == null)
                        {

                            Destiny.GetComponent<PlayerAIBlackWolf>().target = Destiny.GetComponent<TargetActivePC>().target;
                        }
                        Destiny.GetComponent<PlayerAIBlackWolf>().enabled = false;
                        Destiny.GetComponent<PlayerAIBlackWolf>().enabled = true;
                        Destiny.GetComponent<PlayerAIBlackWolf>().ChangeToActive();
                    }
                }
            }
            else
            {

            }
            if (Destiny.name == "Kira")
            {

            }


            else if (Destiny.name != "Rose" && Destiny.name != "Lycaon")
            {
                Destiny.GetComponent<PlayerMoveAI>().enabled = true; //????????????
            }
            else if (Destiny.name == "Rose")
            {
                Destiny.GetComponent<PlayerMoveAIRose>().enabled = true;
            }
            else if (Destiny.name == "Lycaon")
            {
                Destiny.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
            }

        }

        Destiny.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Destiny.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        if (inDialogue == false)
        {
            Destiny.GetComponent<TargetActivePC>().enabled = true;
        }
        Destiny.GetComponent<Rigidbody>().isKinematic = false;      //is this is not done, player Animator move in the same spot
        Destiny.GetComponent<Rigidbody>().isKinematic = true; 		//???

        ActionChangePlayer(Destiny);
        GetComponent<DisplayToolBar>().UpdateActivePC(Destiny);
        PlayBark(Destiny);
	}

	// delegate to switch all PlayerAI player variable to the current Active PC 
	// right after ActivePC changes
	public void ActionChangePlayer(GameObject go)
	{
//		OnChangePlayer(go);		
        foreach (GameObject play in players)
        {
        //    Debug.Log(play);
            if (play.GetComponent<PlayerAI>() != null)
            {
                if (play.GetComponent<PlayerAI>().player != go)
                {
                    play.GetComponent<PlayerAI>().player = go;
                }
            }
            else if (play.GetComponent<PlayerAIRose>() != null)
            {
                play.GetComponent<PlayerAIRose>().player = go;
            }
            else if (play.GetComponent<PlayerAIBlackWolf>() != null)
            {
                play.GetComponent<PlayerAIBlackWolf>().player = go;
            }

        }

	}

	public void MainControllerDead ()
	{
		GameObject chosen = null;
        

		foreach (GameObject go in players)
		{
			if (go.GetComponent <PlayerStats>().deadstate == false)
			{
                bool stateNo = false;
                foreach (string st in go.GetComponent<PlayerStats>().states)
                {
                    if (st == "Stun")
                    {
                        stateNo = true;
                    }
                }
				if (chosen == null && stateNo == false)
				{
					chosen = go;
				}
			}
		}

        if (chosen != null)
        {
            ChangeActivePlayer(chosen);
        }
        else
        {
            Debug.Log("GameOver!!");
        }

	}

	public void OnlyOneActive (string active)
	{
		if (active == mainPlayerName)
		{
			playerActive = true;
			PC2Active = false;
			PC3Active = false;
			PC4Active = false;
			PC5Active = false;
			activePC = player;
		}
		else if (active == PC2 || PC2 != "null")
		{
			playerActive = false;
			PC2Active = true;
			PC3Active = false;
			PC4Active = false;
			PC5Active = false;
			activePC = PC2Obj;
		}
		else if (active == PC3 || PC3 != "null")
		{
			playerActive = false;
			PC2Active = false;
			PC3Active = true;
			PC4Active = false;
			PC5Active = false;
			activePC = PC3Obj;
			Debug.Log (PC3);
		}
		else if (active == PC4 || PC4 != "null" )
		{
			playerActive = false;
			PC2Active = false;
			PC3Active = false;
			PC4Active = true;
			PC5Active = false;
			activePC = PC4Obj;
		}
		else if (active == PC5 || PC5 != "null")
		{
			playerActive = false;
			PC2Active = false;
			PC3Active = false;
			PC4Active = false;
			PC5Active = true;
			activePC = PC5Obj;
		}
	}

	public void targetInactivePC (GameObject enemy)
	{
		if (playerActive == true)
		{
			player.GetComponent<PlayerAI>().enemy = enemy;
		}

		if (PC2Active == true)
		{
			PC2Obj.GetComponent<PlayerAI>().enemy = enemy;
		}

		if (PC3Active == true)
		{
			PC3Obj.GetComponent<PlayerAI>().enemy = enemy;
		}

		if (PC4Active == true)
		{
			PC4Obj.GetComponent<PlayerAI>().enemy = enemy;
		}

		if (PC5Active == true)
		{
			PC4Obj.GetComponent<PlayerAI>().enemy = enemy;
		}


	}

	public void Pause()
	{
		inDialogue = true;

		PausePC (player);

		if (PC2 != "null")
		{
			PausePC (PC2Obj);
		}

		if (PC3 != "null")
		{
			PausePC (PC3Obj);
		}

		if (PC4 != "null")
		{
			PausePC (PC4Obj);
		}

		if (PC5 != "null")
		{
			PausePC (PC5Obj);
		}
	}

	void PausePC(GameObject PC)
	{

		if (PC == player)
		{

		}
		else 
		{

		}
	}

	public void Continue()
	{
		ContinuePC (player);

		if (PC2 != "null")
		{
			ContinuePC (PC2Obj);
		}
		
		if (PC3 != "null")
		{
			ContinuePC (PC3Obj);
		}
		
		if (PC4 != "null")
		{
			ContinuePC (PC4Obj);
		}
		
		if (PC5 != "null")
		{
			ContinuePC (PC5Obj);
		}

		inDialogue = false;
	}

	void ContinuePC (GameObject PC)
	{

		if (PC == player)
		{
			
		}
		else 
		{
			
		}
	}

	void GetAllTrapsInScene()
	{
		allTramps = GameObject.FindGameObjectsWithTag("Trap");
	}

	public void DistributeExp (int exp)
	{
		int partyMembers = 1;

		if (PC2 != "null")
		{
			partyMembers ++;
		}
		
		if (PC3 != "null")
		{
			partyMembers ++;
		}
		
		if (PC4 != "null")
		{
			partyMembers ++;
		}
		
		if (PC5 != "null")
		{
			partyMembers ++;
			Debug.Log (partyMembers);
		}

		int experience = exp / partyMembers;
		int actualExp = DialogueLua.GetActorField ("Player", "experience").AsInt;
		int updateExp = actualExp + experience;

		DialogueLua.SetActorField ("Player", "experience", updateExp);

	//	Invoke ("DebugLogDelay", 0.01f);
	}

    public void ToggleOffControls()
    {
        GetComponent<PlayerControls>().enabled = false;
        
        if (rtsCameraImpr.enabled == true)
        {
            rtsCam = true;
            rtsCameraImpr.enabled = false;
        }
        else
        {
            rtsCam = false;
            mainCamera.GetComponent<MouseOrbitImp>().enabled = false;
            activePC.GetComponent<ThirdPersonUserControl>().enabled = false;
        }
    }

    public void ToggleOnControls()
    {
        GetComponent<PlayerControls>().enabled = true;

        if (rtsCam == true)
        {
            rtsCameraImpr.enabled = true;
        }
        else
        {
            mainCamera.GetComponent<MouseOrbitImp>().enabled = true;
            activePC.GetComponent<ThirdPersonUserControl>().enabled = true;
        }
    }

    void DebugLogDelay ()
	{
		Debug.Log (DialogueLua.GetActorField ("Player", "experience").AsInt);
	}

    void SetUpScripts()
    {
        if (GetComponent<LevelUp>().enabled == true)
        {
            GetComponent<LevelUp>().enabled = false;
        }

        if (GetComponent<GeneralWindow>().enabled == true)
        {
            GetComponent<GeneralWindow>().enabled = false;
        }

        if (GetComponent<DisplayItemScript>().enabled == true)
        {
            GetComponent<DisplayItemScript>().enabled = false;
        }

        if (GetComponent<DisplayCharacter>().enabled == true)
        {
            GetComponent<DisplayCharacter>().enabled = false;
        }

        if (GetComponent<DisplayPortraits>().enabled == true)
        {
            GetComponent<DisplayPortraits>().enabled = false;
        }

        if (GetComponent<DisplayToolBar>().enabled == true)
        {
            GetComponent<DisplayToolBar>().enabled = false;
        }

        if (GetComponent<CameraMap>().enabled == true)
        {
            GetComponent<CameraMap>().enabled = false;
        }

        if (GetComponent<Pause>().enabled == true)
        {
            GetComponent<Pause>().enabled = false;
        }


    }

    void PlayBark (GameObject go)
    {
        string path = "";
        int randomNo = Random.Range(1, 5);
        
        if (go.name != "Player")
        {
            path = "Audio/" + go.name + "/Barks/" + randomNo;

        }
        else
        {
            gender = go.GetComponent<PlayerStats>().gender;
     //       Debug.Log(gender);
            path = "Audio/Player/" + gender + "/Barks/" + randomNo;
        }
        //        barks = (AudioClip)(Resources.Load ("Audio/" + activePla))

    //    Debug.Log((AudioClip)(Resources.Load(path) ))  ;
        if (  (AudioClip)(Resources.Load(path)) != null )
        {
            barks.clip = (AudioClip)(Resources.Load(path));
            barks.Play();
        }
        else
        {
            Debug.LogError (go.name + " No Barks!");
        }

    }

    public void Heal (string healer, int points, int members)
    {
        healerVar = healer;
        pointsVar = points;
        membersVar = members;
        HealNow();
       
    }  


    void HealNow ()
    {

        if (GetComponent<DisplayPortraits>().enabled == true)
        {
            foreach (GameObject go in players)
            {
                GameObject healerGameObject = null;

                foreach (GameObject go2 in healers)
                {
                    if (go2.name == healerVar)
                    {
                        healerGameObject = go2;
                    }
                }

                PlayerStats ps = go.GetComponent<PlayerStats>();

                if (ps.curHealth <= 0)
                {
                    string realism = GetComponent<DisplayOptionsScript>().realism;
                    Debug.Log(realism);
                    if (realism == "Fairy Tale" || realism == "Epic Fantasy")
                    {
                        if (ps.curHealth < ps.totHealth)
                        {
                            ps.AddjustCurrentHealth(pointsVar, healerGameObject);
                        }
                        GetComponent<DisplayPortraits>().ChangeToAlive(go.name);
                        Vector3 position = new Vector3(go.transform.position.x, go.transform.position.y + 1.4f, go.transform.position.z);
                        Quaternion rot = Quaternion.identity;
                        GameObject healEffect = Instantiate(Resources.Load("Effects/FireEffect"), position, rot) as GameObject;

                        go.GetComponent<PlayerStats>().deadstate = false;
                        go.transform.position = activePC.transform.position;
                        
                    }
                    else
                    {
                        DialogueManager.ShowAlert(go.name + " is dead and may not be healed");
                    }
                }
                else
                {
                    if (ps.curHealth < ps.totHealth)
                    {
                        ps.AddjustCurrentHealth(pointsVar, healerGameObject);
                    }
                    GetComponent<DisplayPortraits>().ChangeToAlive(go.name);
                    Vector3 position = new Vector3(go.transform.position.x, go.transform.position.y + 1.4f, go.transform.position.z);
                    Quaternion rot = Quaternion.identity;
                    GameObject healEffect = Instantiate(Resources.Load("Effects/FireEffect"), position, rot) as GameObject;
                }
            }
        }
        else
        {
            Invoke("HealNow", 0.2f);
        }
    }

    public void ChangeCursorAttack ()
    {
        Cursor.SetCursor(cursorAttack, hotSpot, cursorMode);
        currentCursor = cursorAttack;
    }

    public void ChangeCursorNormal ()
    {
        cursorNormal = (Texture2D)(Resources.Load("Icons/Cursor/Normal"));
        Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);

        currentCursor = cursorNormal;
    }

    public void SetUpCameraToDialogue ()
    {
        inDialogue = true;
        ToggleOffControls();
        rtsCameraImpr.enabled = false;
        mainCamera.GetComponent<MouseOrbitImp>().enabled = false;
    }

    public void SetUpCameraBackNormal ()
    {
        inDialogue = false;
        ToggleOnControls();
        rtsCameraImpr.enabled = true;
    }






}