using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour 
{
    WWW www;
	public delegate void SaveAllChests (string fileDirectory);
	public static event SaveAllChests OnSaveAllChests;
    public bool luaLoaded = false;
    public bool finishedLoadLua = false;
    public bool positionCorrection = true;
	public string mainPlayerName;
	private GameObject gc;
	private	string saveData;
	private string saveFileName = "Autosave";
	private string previousFileDirectory;

	public string loadPCList = "Yes";
	public string loadPosition = "N";
	public string scene;
	public string path;
    public bool setUpSceneLoaded = false;           //after setupscene so players can be instantiated in the right position
    public bool loadPosFromLastScene = true;

	
	//From setup script
	
	public GameObject mainPlayer;
	public GameObject PC2Obj;
	public GameObject PC3Obj;
	public GameObject PC4Obj;
	public GameObject PC5Obj;
    
	public Vector3 pos01 = new Vector3(140, 5.2f, 53.7f);
    public Vector3 pos02 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos03 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos04 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos05 = new Vector3 (23.10f, 3.43f, 29.93f);
    public Vector3 posBug = new Vector3(23.10f, 3.43f, 29.93f);
    public Quaternion rot = Quaternion.Euler (0, 0, 0);
	private GameController gameController;
	private GameObject mainCamera;
    private GameObject dummyPlayer;
    public string PC1;
	public string PC2;
	public string PC3;
	public string PC4;
	public string PC5;
	public string saveDirectory;
    public bool loaded = false;
    private bool endLoad = false;
    public bool ignoreLoadPosition = false;
    private Scene sceneCurrent;

    private bool GUIrootSetUp = false;
    ScaleFontSize sFZ;

    void Awake ()
	{
        if (this.enabled == true)
        {
            
            if (loaded == true)
            {
                return;
            }
    //        GameObject dm = (GameObject)(Resources.Load("Dialogue Managerv1", typeof(GameObject)));
            gc = GameObject.FindGameObjectWithTag("GameController");

            if (gc != null)
            {
                gameController = gc.GetComponent<GameController>();
            }
            else
            {
                Debug.Log("gc not found");
            }
            dummyPlayer = transform.Find("DummyPlayer").gameObject;
        }
        

    }

    void Start()
    {
    //    Debug.Log("Start");
        Time.timeScale = 1;
        if (loaded != true)
        {
            gc = GameObject.FindGameObjectWithTag("GameController");

            if (gc != null)
            {
                gameController = gc.GetComponent<GameController>();
            }
            else
            {
                Debug.Log("gc not found");
            }
           dummyPlayer = transform.Find("DummyPlayer").gameObject;
            LoadCurrentProfile();
            
            loaded = true;
        }

    }

    void OnEnable ()
    {
        if (loaded != true)
        {
            gc = GameObject.FindGameObjectWithTag("GameController");

            if (gc != null)
            {
                gameController = gc.GetComponent<GameController>();
            }
            else
            {
                Debug.Log("gc not found");
            }
            dummyPlayer = transform.Find("DummyPlayer").gameObject;
    //        Debug.Log(dummyPlayer);
            LoadCurrentProfile();
            
            loaded = true;
        }

    }

	public void LoadCurrentProfile ()
	{
  //      Debug.Log("Profile");
        string path = Application.persistentDataPath + "/Eternalia";
        string fileName = path + "/CurrentProfile.dat";
   //     Debug.Log(fileName);
		if(File.Exists( fileName ))
		{			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (fileName, FileMode.Open);
			CurrentProfile data = (CurrentProfile) bf.Deserialize(file);
			file.Close();
   			mainPlayerName =  data.currentProfile;
    //        mainPlayerName = "Ball";
			scene = data.scene;
            /*
            if (mainPlayerName == "")
            {
                mainPlayerName= "Luthien";
            }*/
            if (gc != null)
            {
                if (gameController != null)
                {
                    gameController.mainPlayerName = mainPlayerName;
                }
                
            }
            
            if (ignoreLoadPosition == false)
            {
                loadPosition = data.loadPosition;
            }
            else
            {
                loadPosition = "N";
            }
			
            
            saveFileName = data.saveFileName;

  //          Debug.Log(mainPlayerName);
		}
		else
		{
			
		}
   //     Debug.Log(fileName);
        Invoke("PreLoad", 0.1f);
	}

    private void PreLoad()
    {
   //     Debug.Log("Preload");
        LoadAllData();
    }

    public void LoadAllData ()
	{
        LoadQuests(saveFileName);
        //     LoadQuests("Ball");
        //LoadPCData; PlayerStats and EquippedItems are loaded at Start() on Script PlayerStats
    //    Debug.Log("LoadAllData");
    }

	public void LoadQuests (string fileDirectory)
	{
        string path = Application.persistentDataPath + "/Eternalia";
        string fileName = path + "/" + mainPlayerName + "/" + fileDirectory + ".dat";
   //     Debug.Log(fileName);
        if (File.Exists (fileName))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (fileName , FileMode.Open);
			saveQuest data = (saveQuest) bf.Deserialize(file);
			file.Close();			
			saveData = data.saveData;	
           	
			PersistentDataManager.ApplySaveData (saveData);
            //        Debug.Log(saveData);
            //    		DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);
            StartCoroutine("LoadData");
            if (this.enabled == true)
            {
                
            }
            
        }

	}
	
	IEnumerator LoadData()
	{
		yield return new WaitForSeconds (0);
        //      DialogueLua.SetActorField("Aurelius", "RHand", "4005");
        luaLoaded = true;
        if (this.enabled == true)
        {
            EndLoadData();
        }
        
	}
	
	private void EndLoadData()
	{
        StopCoroutine ("LoadData");
        
        //     Debug.Log(DialogueLua.GetActorField("Player", "stren").AsString);
        if (loadPCList == "Yes")
		{
            LoadPCList();
            SetUpSceneCurrentVar();
      //       DialogueLua.SetActorField("Rose", "RHand", "4002");
     //       DialogueLua.SetActorField("Kira", "RHand", "4010");
    //        DialogueLua.SetActorField("Oleg", "LHand", "4800");
      //      DialogueLua.SetActorField("4800", "name", "Morrigan Shield");
     //       DialogueLua.SetVariable("QuestItems", "9002*9003");
        }
		
	}

    public void LoadPCList()
    {
        //SETUPS


        gameController.players.Clear();
        if (transform.Find("LevelUp") == null)
        {
            GameObject goLevelUp = Instantiate(Resources.Load("Models/LevelUp"), transform.position, transform.rotation) as GameObject;
            goLevelUp.transform.parent = transform;
            goLevelUp.name = "LevelUp";
            GetComponent<DisplayCharacter>().levelUp = goLevelUp.GetComponent<LevelUp>();
            GetComponent<DisplayPortraits>().levelUp = goLevelUp.GetComponent<LevelUp>();
        }

        int reputationTotal = DialogueLua.GetActorField("Player", "reputationBenign").AsInt + DialogueLua.GetActorField("Player", "reputationEvil").AsInt + DialogueLua.GetActorField("Player", "reputationOrder").AsInt + DialogueLua.GetActorField("Player", "reputationChaos").AsInt;
        DialogueLua.SetActorField("Player", "reputationTotal", reputationTotal);
        DialogueLua.SetActorField("Player", "SpecialAbility2", "Flurry");
        DialogueLua.SetActorField("Fred", "SpecialAbility", "Set_Up_Traps");
        DialogueLua.SetActorField("Fred", "coolDown1", 0);
    //    DialogueLua.SetVariable("ShinTalk", "OK");
    //    DialogueLua.SetVariable("ShinRep", "NotAssigned");
        DialogueLua.SetActorField("Aurelius", "SpecialAbility", "Barrier");
        DialogueLua.SetActorField("Kira", "SpecialAbility", "nill");
    //    DialogueLua.SetActorField("Enora", "RHand", "4003");
        //    DialogueLua.SetActorField("Preyton", "thiefSkillPoints", 7);
        //   DialogueLua.SetVariable("units", "14000*14012*14202*14203");
        Debug.Log(reputationTotal  + " = benign " + "/" + DialogueLua.GetActorField("Player", "reputationBenign").AsInt + "/evil " + DialogueLua.GetActorField("Player", "reputationEvil").AsInt + "/order : " + DialogueLua.GetActorField("Player", "reputationOrder").AsInt + "/ chaos " + DialogueLua.GetActorField("Player", "reputationChaos").AsInt);
        if (reputationTotal > 25)
        {
    //        string audioChange = "FX/Trap_02";
   //         GetComponent<AudioController>().ChangeToOther(audioChange, 1);
            Debug.LogWarning("Rep");
            Debug.LogError("Rep");
        }

        posBug = pos02;

        if (DialogueLua.GetActorField("Position", "load").AsString == "Yes" && loadPosFromLastScene == true)
        {
            pos01 = new Vector3(DialogueLua.GetActorField("Position", "x1").AsInt, (DialogueLua.GetActorField("Position", "y1").AsInt + 0.5f), DialogueLua.GetActorField("Position", "z1").AsInt);
            pos02 = new Vector3(DialogueLua.GetActorField("Position", "x2").AsInt, (DialogueLua.GetActorField("Position", "y2").AsInt + 0.5f), DialogueLua.GetActorField("Position", "z2").AsInt);
            pos03 = new Vector3(DialogueLua.GetActorField("Position", "x3").AsInt, (DialogueLua.GetActorField("Position", "y3").AsInt + 0.5f), DialogueLua.GetActorField("Position", "z3").AsInt);
            pos04 = new Vector3(DialogueLua.GetActorField("Position", "x4").AsInt, (DialogueLua.GetActorField("Position", "y4").AsInt + 0.5f), DialogueLua.GetActorField("Position", "z4").AsInt);
            pos05 = new Vector3(DialogueLua.GetActorField("Position", "x5").AsInt, (DialogueLua.GetActorField("Position", "y5").AsInt + 0.5f), DialogueLua.GetActorField("Position", "z5").AsInt);
     //       Debug.Log(pos01);
        }
     

        string pcList = DialogueLua.GetVariable("PCList").AsString;
   //     Debug.Log(pcList);
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;
        if (currentSceneName == "07HeartBreakCastleV1")
        {
         //   Debug.Log(DialogueLua.GetQuestField("Look_for_help", "State").AsString);
            if (DialogueLua.GetQuestField("Look_for_help", "State").AsString == "done")
            {
                pcList = "Player*null*null*null*null";
            }            
        }
        string[] arrayPC = pcList.Split(new string[] { "*" }, System.StringSplitOptions.None);
        string young = DialogueLua.GetActorField("Player", "young").AsString;
        string PCchoice = "";
        string portrait;

        Debug.Log(DialogueLua.GetActorField("Player", "portraitPath").AsString);
        if (young == "No")
        {
            path = "Models/MainPlayer/" + DialogueLua.GetActorField("Player", "race").AsString + "/" + DialogueLua.GetActorField("Player", "gender").AsString + "/Player";
            PCchoice = "PC";
            portrait = "Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString;

      

        }
        else
        {
            path = "Models/MainPlayer/" + DialogueLua.GetActorField("Player", "race").AsString + "/" + DialogueLua.GetActorField("Player", "gender").AsString + "/Young/Player";
            PCchoice = "PCYoung";
            portrait = DialogueLua.GetActorField("Player", "portraitPath").AsString;


        }

        DialogueManager.SetPortrait("Player", portrait);
        DialogueManager.SetPortrait("Preyton", "Portraits/Preyton");

        //       Debug.Log(path);
        /*
               if (DialogueLua.GetActorField("Player", "race").AsString == "")
               {
                   path = "Models/mainPlayer/Human/Female/Young/Player";
                   Debug.Log("Warning!!");
               }*/
     //   Debug.Log(pos01);
        mainPlayer = Instantiate(Resources.Load(path), pos01, rot) as GameObject;
        mainPlayer.name = "Player";
        gameController.activePC = mainPlayer;
        gameController.player = mainPlayer;
        gameController.activePC = mainPlayer;
        mainPlayer.SetActive(true);
        gameController.mainPlayerName = DialogueLua.GetActorField("Player", "playerName").AsString; 
        mainPlayer.GetComponent<PlayerAbilities>().enabled = true;
        gameController.players.Add(mainPlayer);
        Camera mainCamera = Camera.main;
        PC2 = arrayPC[1];
 //       Debug.Log(PC2);
        PC3 = arrayPC[2];
 //       Debug.Log(PC3);
        PC4 = arrayPC[3];
        PC5 = arrayPC[4];
     //   AdjustLevel();

        if (PC2 != "null")
        {
            gameController.PC2 = PC2;
            PC2Obj = Instantiate(Resources.Load("Models/" + PCchoice + "/" + PC2), pos02, rot) as GameObject;
            PC2Obj.name = PC2;
            gameController.PC2Obj = PC2Obj;
            PC2Obj.SetActive(true);
            LoadEachMember(PC2Obj);
        }
        else
        {
            PC2Obj = dummyPlayer;
            gameController.PC2 = "null";
        }

        if (PC3 != "null")
        {
            gameController.PC3 = PC3;
            PC3Obj = Instantiate(Resources.Load("Models/" + PCchoice + "/" + PC3), pos03, rot) as GameObject;
            PC3Obj.name = PC3;
            gameController.PC3Obj = PC3Obj;
            PC3Obj.SetActive(true);
            LoadEachMember(PC3Obj);
        //    trapController.players.Add(PC3Obj);
        }
        else
        {
            PC3Obj = dummyPlayer;
   //         Debug.Log(PC3Obj);
            gameController.PC3 = "null";
        }

        if (PC4 != "null")
        {
            gameController.PC4 = PC4;
            PC4Obj = Instantiate(Resources.Load("Models/" + PCchoice + "/" + PC4), pos04, rot) as GameObject;
            PC4Obj.name = PC4;
            gameController.PC4Obj = PC4Obj;
            PC4Obj.SetActive(true);
            LoadEachMember(PC4Obj);
        //    trapController.players.Add(PC4Obj);
        }
        else
        {
            PC4Obj = dummyPlayer;
            gameController.PC4 = "null";
        }


        if (PC5 != "null")
        {
            gameController.PC5 = PC5;
            PC5Obj = Instantiate(Resources.Load("Models/" + PCchoice + "/" + PC5), pos05, rot) as GameObject;
            PC5Obj.name = PC5;
            gameController.PC5Obj = PC5Obj;
            PC5Obj.SetActive(true);
            LoadEachMember(PC5Obj);
        //    trapController.players.Add(PC5Obj);
        }
        else
        {
            PC5Obj = dummyPlayer;
            gameController.PC5 = "null";
        }

        /*
        foreach (GameObject character in gameController.players)
        {
            if (character.GetComponent<PlayerStats>().curHealth < Mathf.CeilToInt(character.GetComponent<PlayerStats>().totHealth * 0.2f))
            {
                character.GetComponent<PlayerStats>().curHealth = Mathf.CeilToInt(character.GetComponent<PlayerStats>().totHealth * 0.2f);
            }

            if (DialogueLua.GetActorField (character.name, "curHealth").AsInt <= 0)
            {
                DialogueLua.SetActorField(character.name, "curHealth", Mathf.CeilToInt(character.GetComponent<PlayerStats>().totHealth * 0.2f));
            }

            PlayerStats playerstats = character.GetComponent<PlayerStats>();

            if (playerstats.deadstate == true)
            {
                playerstats.deadstate = false;
            }
        }*/

        if (loadPosition == "Y")
        {
    //            Debug.Log(pos01);
            //         Debug.Log("loaded");
            mainPlayer.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            mainPlayer.transform.position = new Vector3(DialogueLua.GetActorField("Player", "X").AsFloat, DialogueLua.GetActorField("Player", "Y").AsFloat, DialogueLua.GetActorField("Player", "Z").AsFloat);
            mainPlayer.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            if (PC2 != "null")
            {
                PC2Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                pos02 = PC2Obj.transform.position = new Vector3(DialogueLua.GetActorField(PC2, "X").AsFloat, DialogueLua.GetActorField(PC2, "Y").AsFloat, DialogueLua.GetActorField(PC2, "Z").AsFloat);
                PC2Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
            if (PC3 != "null")
            {
                PC3Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                pos03 = PC3Obj.transform.position = new Vector3(DialogueLua.GetActorField(PC3, "X").AsFloat, DialogueLua.GetActorField(PC3, "Y").AsFloat, DialogueLua.GetActorField(PC3, "Z").AsFloat);
                PC3Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
            if (PC4 != "null")
            {
                PC4Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                pos04 = PC4Obj.transform.position = new Vector3(DialogueLua.GetActorField(PC4, "X").AsFloat, DialogueLua.GetActorField(PC4, "Y").AsFloat, DialogueLua.GetActorField(PC4, "Z").AsFloat);

                PC4Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
            if (PC5 != "null")
            {

                PC5Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                pos05 = PC5Obj.transform.position = new Vector3(DialogueLua.GetActorField(PC5, "X").AsFloat, DialogueLua.GetActorField(PC5, "Y").AsFloat, DialogueLua.GetActorField(PC5, "Z").AsFloat);
                PC5Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }

        }

        foreach (string st in arrayPC)
        {
            DialogueLua.SetActorField(st, "inParty", "Yes");
            DialogueLua.SetActorField(st, "vanguard", "Yes");
        }

        endLoad = true;
        StartCoroutine("BeforeEndLoad");
    }

    //This function works together with the above LoadPCList()
    private void LoadEachMember(GameObject member)
    {
        if (member.name == "Lycaon")
        {
            member.GetComponent<PlayerAIBlackWolf>().enabled = true;
            member.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
        }
        else if (member.name == "Rose")
        {
            if (member.GetComponent<PlayerMoveAIRose>() != null)
            {
                member.GetComponent<PlayerMoveAIRose>().enabled = true;
            }
            
            member.GetComponent<PlayerAIRose>().enabled = true;
        }
        else if (member.name == "Kira")
        {
       //     member.GetComponent<PlayerMoveAI>().enabled = true;
            member.GetComponent<PlayerAIKira>().enabled = true;
        }
        else
        {
     //       Debug.Log(member);
            member.GetComponent<PlayerMoveAI>().enabled = true;
            member.GetComponent<PlayerAI>().enabled = true;           
        }

        if (loadPosition == "Y")
        {
            member.transform.position = new Vector3(DialogueLua.GetActorField(member.name, "X").AsFloat, DialogueLua.GetActorField(member.name, "Y").AsFloat, DialogueLua.GetActorField(member.name, "Z").AsFloat);
        }
    //    Debug.Log(member);
        member.GetComponent<PlayerAbilities>().enabled = true;
        gameController.players.Add(member);
        DialogueLua.SetActorField(member.name, "inParty", "Yes");
        DialogueLua.SetActorField(member.name, "vanguard", "Yes");
        //    StartCoroutine("BeforeEndLoad");
    }
	
    private void SetUpSceneCurrentVar ()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        DialogueLua.SetVariable("sceneCurrent", sceneName);
    }
   
		
	void ChangeDirectory(string fileDirectory)
	{
		GetComponent<GameController>().saveDirectory = fileDirectory;
	}

    IEnumerator BeforeEndLoad ()
    {

        yield return new WaitForSeconds(0);
        if (gameController.activePC != null && gameController.activePC.GetComponent<PlayerAbilities>())
        {
            EndLoadFinal();
            StopCoroutine("BeforeEndLoad");
            yield return new WaitForSeconds(0);

        }
        else
        {
            StartCoroutine("BeforeEndLoad");
            yield return new WaitForSeconds(0);
        }


        yield return new WaitForSeconds(0);

        //     EndLoadFinal();
    }

    private void EndLoadFinal ()
    {
        gameController.player.name = "Player";
        if (PC2 != "null")
        {
            gameController.PC2Obj.name = PC2;
            
        }
        else
        {
            gameController.PC2Obj = dummyPlayer;
        }
        if (PC3 != "null")
        {
            gameController.PC3Obj.name = PC3;
        }
        else
        {
            gameController.PC3Obj = dummyPlayer;
        }
        if (PC4 != "null")
        {
            gameController.PC4Obj.name = PC4;
        }
        else
        {
            gameController.PC4Obj = dummyPlayer;
        }
        if (PC5 != "null")
        {
            gameController.PC5Obj.name = PC5;
        }
        else
        {
            gameController.PC5Obj = dummyPlayer;
        }
        gameController.inDialogue = true;
        GetComponent<DisplayItemScript>().enabled = true;
        GetComponent<DisplayItemScript>().UpDateInventory();
        GetComponent<GeneralWindow>().activePCObj = mainPlayer;
        GetComponent<DisplayToolBar>().enabled = true;
        GetComponent<CombatController>().enabled = true;        
        Camera mainCamera = Camera.main;
        gameController.ChangeActivePlayer(mainPlayer);
        mainCamera.transform.position = mainPlayer.transform.position;
        //   Debug.Log(gameController.player.transform);
        /*
           mainCamera.GetComponent<MouseOrbitImp>().target = gameController.player.transform;
           mainCamera.GetComponent<MouseOrbitImp>().enabled = true;
           mainCamera.GetComponent<MouseOrbitImp>().FindActivePlayer();*/
        mainCamera.GetComponent<RtsCameraImp>().Follow(gameController.player, true);
        StopCoroutine("BeforeEndLoad");
        DialogueLua.SetActorField("The Cross", "visible", "Yes");        
        Invoke("TurnOffMap", 0);
        foreach (GameObject go in gameController.players)
        {
   //         Debug.Log(go);
            go.GetComponent<PlayerStats>().enabled = true;
            if (go.name == "Kira")
            {
                go.GetComponent<PlayerAIKira>().enabled = true;
            }

            else if (go.name != "Rose" && go.name != "Lycaon")
            {
                go.GetComponent<PlayerAI>().enabled = true;
            }
            else if (go.name == "Rose")
            {
                go.GetComponent<PlayerAIRose>().enabled = true;
            }
            else if (go.name == "Lycaon")
            {
                go.GetComponent<PlayerAIBlackWolf>().enabled = true;
            }
            
        }                
    }

    void TurnOffMap ()
    {
        GetComponent<WorldMap>().cameraHolder.transform.root.gameObject.SetActive(false);        
   //     GetComponent<GeneralWindow>().CallBackground();
        GetComponent<DisplayPortraits>().enabled = true;
        gameController.player.GetComponent<TargetActivePC>().enabled = true;
   //     Debug.Log("Map");
        if (GetComponent<FadeScreen>().enabled == true)
        {
            Invoke("TurnOffMap", 0);
        }
        else
        {
            GetComponent<PlayerControls>().enabled = true;
            gameController.inDialogue = false;
            GetComponent<CameraMap>().CheckActiveLocations();
            GetComponent<CameraMap>().CheckActiveQuests();
        }
        GetComponent<ExpController>().Start();
        GetComponent<DisplayInfo>().Start();
        GetComponent<DisplayCharacter>().enabled = true;
        GetComponent<GeneralWindow>().StartManual();
        GetComponent<DisplayToolBar>().UpdateActivePC(gameController.activePC);
        finishedLoadLua = true;

        if (positionCorrection == true)
        {
            Invoke ("PositionCorrection", 0.6f);
        }


        
    }

    private void PositionCorrection()
    {
        mainPlayer.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        mainPlayer.transform.position = pos01;
     //   Debug.Log(pos01);
        mainPlayer.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        mainPlayer.GetComponent<TargetActivePC>().target.transform.position = pos01;
        
        if (PC2 != "null")
        {
            PC2Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            PC2Obj.transform.position = pos02;
            PC2Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        if (PC3 != "null")
        {
            PC3Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            PC3Obj.transform.position = pos03;
            PC3Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        if (PC4 != "null")
        {
            PC4Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            PC4Obj.transform.position = pos04;
            PC4Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        if (PC5 != "null")
        {
            PC5Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            PC5Obj.transform.position = pos05;
            PC5Obj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        positionCorrection = false;


       //      GUIDMSkin();

        
    }



    private void GUIDMSkin ()
    {
        GameObject dm = GameObject.FindGameObjectWithTag("DialogueManager");
        GameObject GUIDS = null;
        foreach (Transform ta in dm.transform)
        {
            if (ta.name != "JRPG Unity GUI Quest Log Window")
            {
                Debug.Log(ta.name);
                GUIDS = ta.gameObject;
                if (GUIDS.AddComponent<ScaleFontSize>() == null)
                {
                    GUIDS.AddComponent<ScaleFontSize>();
                    sFZ = GUIDS.GetComponent<ScaleFontSize>();
                    GUIrootSetUp = true;
                }
                    
                GUIrootSetUp = true;
                sFZ.styles[1].scaleFactor = 0.01f;
                sFZ.styles[2].scaleFactor = 0.01f;
                GUIDS.transform.Find("GUIRoot").GetComponent<GUIRoot>().ManualRefresh();
            }
        }
    }



}