using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class MyGUI : MonoBehaviour 
{
	public bool conversationON = false; 
	private DisplayItemScript displayItemScript;
    private bool loaded = false;
	private float headXPos;
    private float nativeWidth = 735;
    private float nativeHeight = 551;
    //**********************************************
    //  VITALS
    //**********************************************
    #region
    private Texture healthTexture;
	private Texture enduranceTexture;
	private Texture manaTexture;
	private int vitalsX;
	private int vitalButtonWidth = 50;
	private int textureWidth = 20;
	private int vitalButtonHeight = 20;
	private int vitalY1;
	private int vitalY2;
	private int vitalY3;
	private int vitalY4;
	private int vitalY5;
    #endregion

    //***********************************************
    // LOOT WINDOW
    //***********************************************
    #region
    public List <Item2> _lootItems;
	public Hashtable lootItems;
	public GameObject chest;
	public bool _displayLootWindow = false;
	private int buttonWidth = 40;
	private int buttonHeight = 40;
	private int _offset = 10;
	private int lootWindowHeight = 100;
    private int ylootWind = 500;
	private int closeButtonWidth = 20;
	private int closeButtonHeight = 20;
	private const int LOOT_WINDOW_ID = 0;
	private Rect _lootWindowRect = new Rect (0,0,0,0);
	private Vector2 _lootWindowSlider = Vector2.zero;
    #endregion

    //************************************************
    // GENERAL WINDOW (Inventory, Character, Map, etc)
    // ***********************************************
    #region

    private int rate;               // % of screen shared by different elements 14,28%
    /*
//	private Rect _generalWindowRect = new Rect (1, 1, Screen.width - (Screen.width * 0.1f), Screen.height *0.90f);
	private Rect _generalWindowRect = new Rect (1, 1, Screen.width , Screen.height * 0.91f);
	private int _generalPanel = 0;
	private string [] _generalPanelNames = new string[] 
	{
		"Character",
		"Inventory",
		"Quest Items",
		"Map",
		"Quests",
		"Options",
		"Exit Game"
	};  */
	private DisplayCharacter displayChar;
    public bool _displayGeneralWindow = false;
    private int _generalPanel = 0;
    private const int GENERAL_WINDOW_ID = 10;
    //	private Rect _itemWindowWindowRect = new Rect (0, 0, (int) (Screen.width), (int) (Screen.height));


    #endregion
    //************************************************
    // PORTRAITS 
    // ***********************************************
    #region
    public GameObject player;
	public GameObject[] players; 
	private bool _displayPortraits = true;
	private const int PORTRAIT_WINDOW_ID = 11;
	private Rect _portraitWindowRect = new Rect (Screen.width - (Screen.width * 0.10f), 1, (Screen.width * 0.10f), (Screen.height *0.6f));
    public Rect portraitRect;
    private int _portraitPanel = 0;
	private GameObject [] _portraitObjects;
	private string char1;
	private string char2;
	private string char3;
	private string char4;
	private string char5;
	public Texture img1;
	public Texture img2;
	public Texture img3;
	public Texture img4;
	public Texture img5;
	private Texture[] portraitImages;
	public GameObject mainPlayer;
	public GameObject PC2Obj;
	public GameObject PC3Obj;
	public GameObject PC4Obj;
	public GameObject PC5Obj;
	public string activePortrait;
	public GameObject activePCObj;
	private string [] _portraitPanelNames ; 
	private List<string> playerNames = new List<string>();
	private GameController gameController;
    private LevelUp levelUp;
    #endregion

    //**********Level Up***************************
    public bool levelUpPC1 = false;
    public bool levelUpPC2 = false;
    public bool levelUpPC3 = false;
    public bool levelUpPC4 = false;
    public bool levelUpPC5 = false;
    private Texture levelUpImage;

    //***********************************************
    //  		 GENERAL INVENTORY WINDOW
    //***********************************************
    public bool displayInfoItem = false;
	public GUISkin skin;           // ???????????????????????? Remove??????????????????????

	//************************************************
	//             RENDER MAP
	//************************************************
	private GameObject secondCamera;
	private RenderTexture cameraTexture;
	private Camera mapCamera;
	private int inventoryMapYStart;

	//***********************************************
	//   	QUESTS WINDOW
	//***********************************************
	private GameObject dialogueManager;
	private Transform guiRootTransform;
	private GameObject guiRootGameObject;

    //***********************************************
    //         DISPLAY OPTIONS
    //***********************************************
    #region
    private DisplayOptionsScript displayOptions;
	private Rect _optionsWindowRect = new Rect (1, 1, Screen.width - (Screen.width * 0.1f), Screen.height *0.90f);
	private int _optionPanel = 30;
	private string mySaveDirectory;
	private string nameSave = "";
	private string [] _optionPanelNames = new string[] 
	{
		"Save",
		"Load",
		"Audio",
		"Graphics",
		"Controls",
		"Dificulty"
	};
	private bool displayOverwrite = false;
	private int saveXPos;
	private int saveNameXpos;
	private int saveButton;
    #endregion

    //*********************************************************
    //           DISPLAY TOOL BAR
    //*********************************************************
    private DisplayToolBar displayToolBar;

	/// HERE FINISH DECLARING VARIABLE AND START SCRIPTING.....
	/// *******************************************************
    
	void Awake ()
	{
		gameController = GetComponent<GameController>();
        levelUp = GetComponent<LevelUp>();
        
    }

	void OnEnable () 
	{
        if (loaded == false)
        {
            portraitRect = new Rect (_portraitWindowRect.x, Screen.height - _portraitWindowRect.height, _portraitWindowRect.width, _portraitWindowRect.height); //this is so TargetActivePC script could get the value of portraitWindowRect. If I changed portraitwindowRect to public the portraits stop showing up
            ylootWind = (int)(Screen.height * 0.8f);
            vitalsX = (int)(Screen.width * 0.88f);
            vitalY2 = (int)(Screen.height * 0.14f);
            vitalY3 = (int)(Screen.height * 0.28f);
            vitalY4 = (int)(Screen.height * 0.42f);
            vitalY5 = (int)(Screen.height * 0.56f);
            GUI.depth = 1;
            displayItemScript = GetComponent<DisplayItemScript>();
            _lootItems = new List<Item2>();
            lootItems = new Hashtable();
            _generalPanel = 1;
            skin = (GUISkin)(Resources.Load("GUI/Skin", typeof(GUISkin)));    //delete?
            mainPlayer = gameController.player;
            inventoryMapYStart = (int)(Screen.height - (Screen.height * 0.86f));
            healthTexture = (Texture)(Resources.Load("Icons/Health", typeof(Texture)));
            enduranceTexture = (Texture)(Resources.Load("Icons/Endurance", typeof(Texture)));
            manaTexture = (Texture)(Resources.Load("Icons/Mana", typeof(Texture)));
            LoadPortraits();
            skin = (GUISkin)(Resources.Load("GUI/Skin", typeof(GUISkin)));

            //Get component of secondary camera 
            secondCamera = GameObject.FindGameObjectWithTag("SecondCamera");
            if (secondCamera.GetComponent<Camera>())
            {
                mapCamera = secondCamera.GetComponent<Camera>();
                secondCamera.GetComponent<Camera>().enabled = false;
                cameraTexture = mapCamera.targetTexture;
            }

            //	mapCamera.enabled = false;

            //QUESTS ITEMS and Get component of Dialogue Manager 
            dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager");
            guiRootTransform = dialogueManager.transform.Find("JRPG Unity GUI Quest Log Window/GUIRoot");
            guiRootGameObject = guiRootTransform.gameObject;

            //Options display
            mySaveDirectory = @"C:\SaveGame\Saves\";

            //DISPLAYCHARACTER
            displayChar = GetComponent<DisplayCharacter>();
            displayOptions = GetComponent<DisplayOptionsScript>();
            displayToolBar = GetComponent<DisplayToolBar>();
            Messenger.AddListener("ToggleCharacterWindow", ToggleCharacterWindow);
            //*************************************************
            loaded = true;
            rate = (int) (Screen.width * 0.1428f);
        }
    }

    public void LoadPortraits ()
    {
        string gender = DialogueLua.GetActorField("Player", "gender").AsString;
        img1 = (Texture)(Resources.Load("Portraits/Player/Human/" + gender, typeof(Texture)));

        levelUpImage = (Texture)(Resources.Load("Portraits/Narrator", typeof(Texture)));
        char2 = gameController.PC2;
        char3 = gameController.PC3;
        char4 = gameController.PC4;
        char5 = gameController.PC5;
        player = gameController.player;
        //	string gender = player.GetComponent<PlayerStats>().gender;
        activePortrait = char1;
        activePCObj = mainPlayer;


        if (char2 != "null")
        {
            PC2Obj = gameController.PC2Obj;
            img2 = (Texture)(Resources.Load("Portraits/" + gameController.PC2, typeof(Texture)));
        }
        else
        {
            img2 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (char3 != "null")
        {
            PC3Obj = gameController.PC3Obj;
            img3 = (Texture)(Resources.Load("Portraits/" + gameController.PC3, typeof(Texture)));
        }
        else
        {
            img3 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (char4 != "null")
        {
            PC4Obj = gameController.PC4Obj;
            img4 = (Texture)(Resources.Load("Portraits/" + gameController.PC4, typeof(Texture)));
        }
        else
        {
            img4 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (char5 != "null")
        {
            PC5Obj = gameController.PC5Obj;
            img5 = (Texture)(Resources.Load("Portraits/" + gameController.PC5, typeof(Texture)));
        }
        else
        {
            img5 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        _portraitPanelNames = new string[]
{
			//char0, char1, char2, char3, char4
			char1, char2, char3, char4, char5
};

        portraitImages = new Texture[]
        {
            img1, img2, img3, img4, img5
        };
    }

    void OnGUI ()
	{
        float rx = Screen.width / nativeWidth;
        float ry = Screen.height / nativeHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUI.skin = skin;
		Cursor.visible = true;
        
        if (_displayLootWindow == true)
        {
            _lootWindowRect = GUI.Window(LOOT_WINDOW_ID, new Rect(_offset, ylootWind - (lootWindowHeight), Screen.width - (Screen.width * 0.5f), lootWindowHeight),
                                          LootWindow, "LOOT WINDOW");
    //        displayItemScript.DisplayInventoryLooting();
        }

        if (_displayGeneralWindow == true && displayInfoItem == false)
        {
            /*
            _generalWindowRect = GUI.Window(GENERAL_WINDOW_ID, _generalWindowRect,
                                             GeneralWindow, "GENERAL WINDOW");*/
        }
        else if (_displayGeneralWindow == false)
        {
            guiRootGameObject.SetActive(false);
            _portraitWindowRect = GUI.Window(PORTRAIT_WINDOW_ID, _portraitWindowRect,
                                              PortraitWindow, "PARTY");
            Vitals();
        }

        if (displayInfoItem == true && _displayGeneralWindow == true)
        {
            /*
            _itemWindowWindowRect = GUI.Window(50, _itemWindowWindowRect, ItemDescriptionWindow, "DESCRIPTION");
        */
        }
    }

    //This controls the portraits while in normal game (not when GUI Inventory and other with all the options are active)
	void PortraitWindow (int id)
	{
		if (GUI.Button (new Rect (1, 1,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img1))
		{
            if (gameController.abilityInUse == false)
            {
                if (levelUpPC1 == true)
                {
                    levelUp.enabled = true;
                    this.enabled = false;
                }

                if (activePCObj == player)
                {
                    return;
                }

                if (Event.current.button == 0)
                {
                    if (Event.current.control)
                    {

                    }
                    else
                    {
                        GetComponent<GameController>().ChangeActivePlayer(gameController.player);
                        activePortrait = char1;
                        activePCObj = gameController.player;
                        GetComponent<GameController>().playerActive = true;
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    if (activePortrait != char1)
                    {
                        GetComponent<GameController>().playerActive = !GetComponent<GameController>().playerActive;
                    }
                }
            }
        }

 

		if (GUI.Button (new Rect (1, _portraitWindowRect.height * 0.20f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img2))
		{
            if (gameController.abilityInUse == false && gameController.PC2 != "null")
            {
                if (levelUpPC2 == true)
                {

                    levelUp.enabled = true;
                    this.enabled = false;
                }

                if (activePCObj == PC2Obj)
                {
                    return;
                }
                else
                {
                    activePCObj = PC2Obj;
                }

                //	if (Event.current.button == 0 || activePortrait != char2 )
                if (Event.current.button == 0)
                {
                    if (gameController.activePC == gameController.PC2Obj)
                    {
                        return;
                    }

                    if (Event.current.control)
                    {

                    }
                    else
                    {
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC2Obj);
                        GetComponent<GameController>().OnlyOneActive(char2);
                        GetComponent<GameController>().PC2Active = true;
                        activePortrait = char2;
                        activePCObj = gameController.PC2Obj;
                        if (levelUpPC2 == true)
                        {
                            Debug.Log("LevelUp" + PC2Obj);
                        }
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");


                    if (activePortrait != char2)
                    {
                        GetComponent<GameController>().PC2Active = !GetComponent<GameController>().PC2Active;

                    }
                }
            }

		}

		if (GUI.Button (new Rect (1, _portraitWindowRect.height * 0.40f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img3))
		{
            if (gameController.abilityInUse == false && gameController.PC3 != "null")
            {
                if (levelUpPC3 == true)
                {
                    levelUp.enabled = true;
                    this.enabled = false;
                }
                if (activePCObj == gameController.PC3Obj)
                {
                    return;
                }

                if (Event.current.button == 0 || activePortrait != char3)
                {
                    if (gameController.activePC == gameController.PC3Obj)
                    {
                        return;
                    }

                    if (Event.current.control)
                    {

                    }
                    else
                    {
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC3Obj);
  //                      Debug.Log(gameController.PC3Obj);
                        GetComponent<GameController>().OnlyOneActive(char3);
                        GetComponent<GameController>().PC3Active = true;
                        activePortrait = char3;
                        activePCObj = PC3Obj = gameController.PC3Obj;
                        gameController.activePC = gameController.PC3Obj;
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");

                    if (activePortrait != char3)
                    {
                        GetComponent<GameController>().PC3Active = !GetComponent<GameController>().PC3Active;

                    }
                }
            }
		}

        //	GUI.Button (new Rect (1, _portraitWindowRect.height * 0.30f ,_portraitWindowRect.width, _portraitWindowRect.height /6 ), img3);
        if (GUI.Button(new Rect(1, _portraitWindowRect.height * 0.60f, _portraitWindowRect.width, _portraitWindowRect.height / 5), img4))
        {
            if (gameController.abilityInUse == false && gameController.PC4 != "null")
            {
                if (levelUpPC4 == true)
                {

                    levelUp.enabled = true;
                    this.enabled = false;
                }
                if (activePCObj == PC4Obj)
                {
                    return;
                }

                if (Event.current.button == 0 || activePortrait != char4)
                {
                    if (Event.current.control)
                    {

                    }
                    else
                    {
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC4Obj);
                        GetComponent<GameController>().OnlyOneActive(char4);
                        GetComponent<GameController>().PC4Active = true;
                        activePortrait = char4;
                        activePCObj = gameController.PC4Obj;
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    if (activePortrait != char4)
                    {
                        GetComponent<GameController>().playerActive = !GetComponent<GameController>().playerActive;

                    }
                }
            }
        }


        //		GUI.Button (new Rect (1, _portraitWindowRect.height * 0.45f ,_portraitWindowRect.width, _portraitWindowRect.height /6 ), img4);
        //		GUI.Button (new Rect (1, _portraitWindowRect.height * 0.80f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img5);
        if (GUI.Button(new Rect(1, _portraitWindowRect.height * 0.80f, _portraitWindowRect.width, _portraitWindowRect.height / 5), img5))
        {
            if (gameController.abilityInUse == false && gameController.PC5 != "null")
            {
                if (levelUpPC5 == true)
                {

                    levelUp.enabled = true;
                    this.enabled = false;
                }
                if (activePCObj == PC5Obj)
                {
                    return;
                }

                if (Event.current.button == 0 || activePortrait != char5)
                {
                    if (Event.current.control)
                    {

                    }
                    else
                    {
                        GetComponent<GameController>().ChangeActivePlayer(PC5Obj);
                        Debug.Log(PC5Obj);
                        GetComponent<GameController>().OnlyOneActive(char5);
                        GetComponent<GameController>().PC4Active = true;
                        activePortrait = char5;
                        activePCObj = PC5Obj;
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    if (activePortrait != char5)
                    {
                        GetComponent<GameController>().playerActive = !GetComponent<GameController>().playerActive;

                    }
                }
            }
        }
    }

	private void LootWindow (int id)
	{
		if (GUI.Button (new Rect (_lootWindowRect.width -20, 0, closeButtonWidth, closeButtonHeight ), "x") || _displayGeneralWindow == true)
		{
			chest.GetComponent<Chest>().CloseChest();
			_displayLootWindow = false;
		}

		_lootWindowSlider = GUI.BeginScrollView (new Rect (5, 20, _lootWindowRect.width - 10, 70), _lootWindowSlider, 
		                                         new Rect(0, 0, _lootItems.Count * buttonWidth, buttonWidth + _offset));

		for (int cnt = 0; cnt < _lootItems.Count ; cnt++)
		{
			if (GUI.Button (new Rect (buttonWidth * cnt, _offset, buttonWidth, buttonHeight), _lootItems[cnt].name ))
			{
	//			Debug.Log (_lootItems[cnt].name + "/" + _lootItems[cnt].value);
                string go5 = _lootItems[cnt].name;

                if (_lootItems[cnt].value == "Quest")
				{
                    Debug.Log(_lootItems[cnt].name + "/" + _lootItems[cnt].value);
                    //	QuestItems.QuestItem.Add (new Item2(_lootItems[cnt].name, cnt.ToString()    ));
                    string questItemsList = DialogueLua.GetVariable ("QuestItems").AsString;
                    ShowAlert("Quest Item Added: " + _lootItems[cnt].name);
                    
                    if (questItemsList == null || questItemsList == "")
					{
						string questItemsAdded =  _lootItems[cnt].name;
						
						DialogueLua.SetVariable ("QuestItems", questItemsAdded);
			//			GetComponent<DisplayItemScript>().arrayQuestItems =  questItemsAdded.Split (new string [] {"*"}, System.StringSplitOptions.None);
			//			GetComponent<DisplayItemScript>().nullQuestItem = false;

                    }
					else
					{
                        Debug.Log(_lootItems[cnt].name + "/" + _lootItems[cnt].value);
                        string questItemsAdded = questItemsList + "*" + _lootItems[cnt].name;
                        
						DialogueLua.SetVariable ("QuestItems", questItemsAdded);
			//			GetComponent<DisplayItemScript>().arrayQuestItems =  questItemsAdded.Split (new string [] {"*"}, System.StringSplitOptions.None);
                        //	GetComponent<DisplayItemScript>().nullQuestItem = false;
                    }
					_lootItems.RemoveAt (cnt);
                    
                }
				else
				{
					GeneralInventory.GenInventory.Add(new Item2(_lootItems[cnt].name, _lootItems[cnt].value));
					_lootItems.RemoveAt (cnt);
				}
            }
		}
		GUI.EndScrollView();
	}

	private void PopulateChest (int x, GameObject go)
	{
		chest = go;
		_displayLootWindow = true;
	//	Test ();
	}

	public void ToggleCharacterWindow ()
	{
		_displayGeneralWindow =! _displayGeneralWindow;
        if (_displayGeneralWindow == true)
        {
            Time.timeScale = 0;
      //      this.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
     //       this.enabled = false;
        }
	}

	void GeneralWindow (int id)
	{
        GUI.Button(new Rect(0, Screen.width * 0.07f, rate, Screen.width * 0.07f), "Inventory");

        /*
		_generalPanel = GUI.Toolbar (new Rect (1, 25, _generalWindowRect.width - 10, 50), _generalPanel, _generalPanelNames);
	
		switch (_generalPanel)
		{
		case 0:
			DisplayCharacter();
			break;
		case 1:
			displayItemScript.DisplayEquippedItems();
			displayItemScript.DisplayGeneralInventory();
			
			guiRootGameObject.SetActive(false); 
			Portraits();
			break;

		case 2:
			displayItemScript.DisplayQuestItems();
			guiRootGameObject.SetActive(false); 
			break;

		case 3:
			DisplayMap();
			break;
		case 4:
			DisplayQuests();

			break;
		case 5:
			DisplayOptions();
			break;
		case 6:
			DisplayExitGame();
			break;		
		}
        */
	}

	void DisplayCharacter ()
	{
		guiRootGameObject.SetActive(false); 
		displayChar.DisplayAllData();
		Portraits();
	}

	void ItemDescriptionWindow (int d)
	{
		displayItemScript.DisplayInfoItems();
	}
	
	public void Portraits ()
	{
		if (GUI.Button (new Rect (Screen.width * 0.89f, Screen.height * 0.14f,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img1))
		{
            activePCObj = gameController.player; ;
            GetComponent<DisplayCharacter>().activePC = gameController.player;
            GetComponent<GameController>().ChangeActivePlayer(gameController.player);
             GetComponent<DisplayCharacter>().ChangePC();
        }
		
		if (GUI.Button (new Rect (Screen.width * 0.89f, Screen.height * 0.29f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img2) && char2 != "null")
		{
            activePCObj = gameController.PC2Obj;
            GetComponent<DisplayCharacter>().activePC = gameController.PC2Obj;
            GetComponent<GameController>().ChangeActivePlayer(gameController.PC2Obj);
            GetComponent<DisplayCharacter>().ChangePC();
        }

		if (GUI.Button (new Rect (Screen.width * 0.89f, Screen.height * 0.44f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img3) && char3 != "null")
		{
            activePCObj = gameController.PC3Obj;
			GetComponent<DisplayCharacter>().activePC = gameController.PC3Obj;
            GetComponent<GameController>().ChangeActivePlayer(gameController.PC3Obj);
            GetComponent<DisplayCharacter>().ChangePC();
        }

		if (GUI.Button (new Rect (Screen.width * 0.89f, Screen.height * 0.59f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img4) && char4 != "null")
		{
			activePCObj = gameController.PC4Obj;
			GetComponent<DisplayCharacter>().activePC = gameController.PC4Obj;
            GetComponent<GameController>().ChangeActivePlayer(gameController.PC4Obj);
           GetComponent<DisplayCharacter>().ChangePC();
        }

		if (GUI.Button (new Rect (Screen.width * 0.89f, Screen.height * 0.74f ,_portraitWindowRect.width, _portraitWindowRect.height /5 ), img5) && char5 != "null")
		{
			activePCObj = gameController.PC5Obj;
			GetComponent<DisplayCharacter>().activePC = gameController.PC5Obj;
            GetComponent<GameController>().ChangeActivePlayer(gameController.PC5Obj);
            GetComponent<DisplayCharacter>().ChangePC();
        }
	}

	void DisplayMap ()
	{
		mapCamera.enabled = true;

		GUI.DrawTexture(new Rect(150, inventoryMapYStart, 450, 450), cameraTexture);
		guiRootGameObject.SetActive(false); 
	}

	void DisplayQuests ()
	{
		guiRootGameObject.SetActive(true); 
	}

	void DisplayOptions()
	{
		guiRootGameObject.SetActive(false); 

//		displayOptions.DisplayOptions();
	}

	void DisplayExitGame ()
	{
		//	GUI.DrawTexture (new Rect(overwriteXPos, saveYPos, Screen.width * 0.50f, Screen.height*0.4f), button);
		GUI.Label (new Rect(Screen.width * 0.2f, Screen.height * 0.35f, Screen.width * 0.50f, Screen.height*0.5f), "All unsaved progress will be lost");
		GUI.Label (new Rect(Screen.width * 0.2f, (Screen.height * 0.35f) + 25, Screen.width * 0.50f, Screen.height*0.5f), "Would you like to exit the game?");
		//	GUI.Button(new Rect(saveButton, saveYPos, Screen.width * 0.75f, Screen.height*0.6f), "File already exist. Would you like to overwrite file?");
		if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.45f, 150, 50), "Yes"))
		{
			Debug.Log ("Exit");
			Application.Quit();
			
		}
		if (GUI.Button(new Rect((Screen.width * 0.2f) + 150, Screen.height * 0.45f, 150, 50), "No"))
		{
			_generalPanel = 0;
		}
	}

	void Vitals ()
	{
		GUI.Label (new Rect (vitalsX, 5, textureWidth, vitalButtonHeight), healthTexture );
		GUI.Label (new Rect (vitalsX - vitalButtonWidth, 5, vitalButtonWidth, vitalButtonHeight), mainPlayer.GetComponent<PlayerStats>().curHealth.ToString() + "/" + mainPlayer.GetComponent<PlayerStats>().totHealth   );
		GUI.Label (new Rect (vitalsX, 5 + vitalButtonHeight, textureWidth, vitalButtonHeight), enduranceTexture );
		GUI.Label (new Rect (vitalsX - vitalButtonWidth, 5 + vitalButtonHeight, vitalButtonWidth, vitalButtonHeight), mainPlayer.GetComponent<PlayerStats>().curEndurance.ToString() + "/" + mainPlayer.GetComponent<PlayerStats>().totEndu  );
		GUI.Label (new Rect (vitalsX, 5 + vitalButtonHeight + vitalButtonHeight, textureWidth, vitalButtonHeight), manaTexture );
		GUI.Label (new Rect (vitalsX - vitalButtonWidth, 5 + vitalButtonHeight + vitalButtonHeight, vitalButtonWidth, vitalButtonHeight), mainPlayer.GetComponent<PlayerStats>().curMana.ToString() + "/" + mainPlayer.GetComponent<PlayerStats>().totMana   );

		if (char2 != "null")
		{
			GUI.Label (new Rect (vitalsX, vitalY2, textureWidth, vitalButtonHeight), healthTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalY2, vitalButtonWidth, vitalButtonHeight), PC2Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PC2Obj.GetComponent<PlayerStats>().totHealth   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalY2, textureWidth, vitalButtonHeight), enduranceTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalY2, vitalButtonWidth + 15, vitalButtonHeight), PC2Obj.GetComponent<PlayerStats>().curEndurance.ToString() + "/" + PC2Obj.GetComponent<PlayerStats>().totEndu   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalButtonHeight + vitalY2, textureWidth, vitalButtonHeight), manaTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalButtonHeight + vitalY2, vitalButtonWidth + 15, vitalButtonHeight), PC2Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PC2Obj.GetComponent<PlayerStats>().totMana  );
		}

		if (char3 != "null")
		{
			GUI.Label (new Rect (vitalsX, vitalY3, textureWidth, vitalButtonHeight), healthTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalY3, vitalButtonWidth, vitalButtonHeight), PC3Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PC3Obj.GetComponent<PlayerStats>().totHealth   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalY3, textureWidth, vitalButtonHeight), enduranceTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalY3, vitalButtonWidth + 15, vitalButtonHeight), PC3Obj.GetComponent<PlayerStats>().curEndurance.ToString() + "/" + PC3Obj.GetComponent<PlayerStats>().totEndu   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalButtonHeight + vitalY3, textureWidth, vitalButtonHeight), manaTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalButtonHeight + vitalY3, vitalButtonWidth + 15, vitalButtonHeight), PC3Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PC3Obj.GetComponent<PlayerStats>().totMana  );
		}

		if (char4 != "null")
		{
			GUI.Label (new Rect (vitalsX, vitalY4, textureWidth, vitalButtonHeight), healthTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalY4, vitalButtonWidth, vitalButtonHeight), PC4Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PC4Obj.GetComponent<PlayerStats>().totHealth   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalY4, textureWidth, vitalButtonHeight), enduranceTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalY4, vitalButtonWidth + 15, vitalButtonHeight), PC4Obj.GetComponent<PlayerStats>().curEndurance.ToString() + "/" + PC4Obj.GetComponent<PlayerStats>().totEndu   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalButtonHeight + vitalY4, textureWidth, vitalButtonHeight), manaTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalButtonHeight + vitalY4, vitalButtonWidth + 15, vitalButtonHeight), PC4Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PC4Obj.GetComponent<PlayerStats>().totMana  );

		}

		if (char5 != "null")
		{
			GUI.Label (new Rect (vitalsX, vitalY5, textureWidth, vitalButtonHeight), healthTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalY5, vitalButtonWidth, vitalButtonHeight), PC5Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PC5Obj.GetComponent<PlayerStats>().totHealth   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalY5, textureWidth, vitalButtonHeight), enduranceTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalY5, vitalButtonWidth + 15, vitalButtonHeight), PC5Obj.GetComponent<PlayerStats>().curEndurance.ToString() + "/" + PC5Obj.GetComponent<PlayerStats>().totEndu   );
			GUI.Label (new Rect (vitalsX, vitalButtonHeight + vitalButtonHeight + vitalY5, textureWidth, vitalButtonHeight), manaTexture );
			GUI.Label (new Rect (vitalsX - vitalButtonWidth, vitalButtonHeight + vitalButtonHeight + vitalY5, vitalButtonWidth + 15, vitalButtonHeight), PC5Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PC5Obj.GetComponent<PlayerStats>().totMana  );

		}
	}

	public void ChangePC ()
	{
		char2 = gameController.PC2;	
		char3 = gameController.PC3;
		char4 = gameController.PC4;
		char5 = gameController.PC5;
		activePortrait = char1;
		activePCObj = mainPlayer;

		if (char2 != "null")
		{
			PC2Obj = gameController.PC2Obj;
			img2 = (Texture)(Resources.Load ("Portraits/"+gameController.PC2, typeof(Texture)));
		}
		else
		{
			img2 = (Texture)(Resources.Load ("Portraits/Empty", typeof(Texture)));
		}
		
		if (char3 != "null")
		{
			PC3Obj = gameController.PC3Obj;
			img3 = (Texture)(Resources.Load ("Portraits/"+gameController.PC3, typeof(Texture)));
		}
		else
		{
			img3 = (Texture)(Resources.Load ("Portraits/Empty", typeof(Texture)));
		}
		
		if (char4 != "null")
		{
			PC4Obj = gameController.PC4Obj;
            img4 = (Texture)(Resources.Load("Portraits/" + gameController.PC4, typeof(Texture)));
        }
		else
		{
			img4 = (Texture)(Resources.Load ("Portraits/Empty", typeof(Texture)));
		}
		
		if (char5 != "null")
		{
			PC5Obj = gameController.PC5Obj;
            img5 = (Texture)(Resources.Load("Portraits/" + gameController.PC5, typeof(Texture)));
        }
		else
		{
			img5 = (Texture)(Resources.Load ("Portraits/Empty", typeof(Texture)));
		}

		_portraitPanelNames  = new string[] 
		{
			//char0, char1, char2, char3, char4
			char1, char2, char3, char4, char5
		};
		
		portraitImages = new Texture[] 
		{
			img1, img2, img3, img4, img5
		};
	}

	void OnConversationStart ()
	{
		Debug.Log ("Conversation started");
	}

    void ShowAlert (string alert)
    {
  //      Debug.Log(alert);
        DialogueManager.ShowAlert(alert);
    }

    public void LevelUpMember (GameObject member)
    {
        if (member == mainPlayer)
        {
            levelUpPC1 = true;
            img1 = levelUpImage;
        }
        else 
        {
            if ((PC2Obj != null))
            {
                if (PC2Obj == member)
                {
                    levelUpPC2 = true;
                    img2 = levelUpImage;
                }
            }
            if ((PC3Obj != null))
            {
                if (PC3Obj == member)
                {
                    levelUpPC3 = true;
                    img3 = levelUpImage;
                }
            }
        }
    }

}
