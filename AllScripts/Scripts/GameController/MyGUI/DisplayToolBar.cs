using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DisplayToolBar : MonoBehaviour 
{
 //   public bool globalAI = true;
	private int numberButtons = 16;
	private int buttonWide;
	private int posX;
	private int posY;
	private int buttonHeight;
    private int guiDownHeight;
    private int positionGUIDownY;
    private int quickX1;
    private int quickX2;
    private int quickX3;
	private string ability1;
	private string ability2;
	private string ability3;
	private string spell = "no";
	private string abilitySpecial = "no";
	private string showChoice;
	private bool displayGenButtons = true;
    private bool loaded = false;
	private string [] arrayString;
	private Texture TextAbi1;
	private Texture TextAbi2;
	private Texture TextAbi3;
    public Texture quickAttack;                                    //This may be used to force an attack on certain NPCs to become enemies
    public Texture quick1;
    public Texture quick2;
    public Texture quick3;
    private Texture aiOn;
    private Texture aiOff;
    private Texture aiDisplay;
    private Texture aiIndividualOn;
    private Texture aiIndividualOff;
    private Texture aiIndividualDisplay;
	private GameObject gc;
	private GameController gameController;
    private PlayerAbilities playerAbilities;
	public GUISkin skin;
    public GUIStyle myStyle;
    private Texture downGUI;
    public Rect toolBarRect;                        //this is to be used on the GUI.Contai(Input.mouse) of TargetPlayerActive

	void Awake ()
	{
//		string speAbilityList = "Disarm Traps";
//		arrayString = speAbilityList.Split (new string [] {"*"}, System.StringSplitOptions.None);
	}
    /*
	void Start () 
	{
        OnEnable();  

	}*/

    void OnEnable ()
    {
   //     Debug.Log("Enable");
        if (loaded != true)
        {
            buttonHeight = (int)(Screen.height * 0.10f);
            toolBarRect = new Rect(0, 0, Screen.width, buttonHeight + 5);
            buttonWide = (int)(Screen.width / 16);
            posX = 0;
            posY = (int)(Screen.height * 0.91f);
            buttonHeight = (int)(Screen.height * 0.10f);
            quickX1 = posX + (10 * buttonWide);
            quickX2 = posX + (11 * buttonWide);
            quickX3 = posX + (12 * buttonWide);
            buttonHeight = (int)(Screen.height * 0.10f);
            guiDownHeight = (int)(Screen.height * 0.13f);
            positionGUIDownY = (int)(Screen.height * 0.88f);
            gc = GameObject.FindGameObjectWithTag("GameController");
            gameController = gc.GetComponent<GameController>();
            UpdateActivePC(gameController.activePC);
            spell = "no";
            abilitySpecial = "yes";
       //     skin = (GUISkin)(Resources.Load("GUI/FantasyT", typeof(GUISkin)));
      //      skin = GetComponent<GeneralWindow>().mySkin;
            downGUI = (Texture)(Resources.Load("GUI/GUIdown", typeof(Texture)));
            aiOn = (Texture)(Resources.Load("Icons/GUI/AI", typeof(Texture)));
            aiOff = (Texture)(Resources.Load("Icons/GUI/AIOff", typeof(Texture)));
            aiIndividualOn = (Texture)(Resources.Load("Icons/GUI/IndividualAI", typeof(Texture)));
            aiIndividualOff = (Texture)(Resources.Load("Icons/GUI/IndividualAIOff", typeof(Texture)));
            quickAttack = (Texture)(Resources.Load("GUI/Attack", typeof(Texture)));

            if (gameController.globalAI == false)
            {
                aiDisplay = aiOff;
            }
            else
            {
                aiDisplay = aiOn;
            }
            if (gameController.activePC.GetComponent<PlayerAI>().dontMove == false)
            {
                aiIndividualDisplay = aiIndividualOn;
            }
            else
            {
                aiIndividualDisplay = aiIndividualOff;
            }
            if (transform.Find("QuickItem") == null)
            {
                GameObject newQuickItem = Instantiate(Resources.Load("QuickItems/QuickItem"), transform.position, Quaternion.identity) as GameObject;
                newQuickItem.transform.parent = transform;
            }
            loaded = true;
        }        
    }

	void OnGUI ()
	{
   //     GUI.skin = skin;
        GUI.depth = 0;
        Background();
		DisplayBar();
	}

    void Background ()
    {
        GUI.DrawTexture(new Rect(0, Screen.height *0.8f, Screen.width, Screen.height *0.2f), downGUI);
    }

	public void DisplayBar ()
	{
		if (displayGenButtons == true)
		{
			DisplayGenButtons();
		}
		else if (displayGenButtons == false)
		{
			DisplaySpecialAbilities();
		}
	}

	private void DisplayGenButtons ()
	{
        //I added this because from Scene 04TheCrossCrypts to 05Caves gives an error for 5-6
        //frames telling gameController.activePC is null, I could not find what they problem is.
        //In other transits in between other scenes it does not happen. 
        if (gameController.activePC == null)
        {
            return;
        }

		playerAbilities = gameController.activePC.GetComponent<PlayerAbilities>();
        GameObject playerActive = gameController.activePC;
        int timeAbi1 = DialogueLua.GetActorField(playerActive.name, "timeAbi1").AsInt;

        if (timeAbi1 > 0)
        {
            GUI.Button(new Rect(posX, posY, buttonWide, buttonHeight), new GUIContent(timeAbi1.ToString(), playerAbilities.ability1));
        }
        else
        {
            if (GUI.Button(new Rect(posX, posY, buttonWide, buttonHeight), new GUIContent(playerAbilities.TextAbi1, playerAbilities.ability1)))
            {

                bool stateNo = false;
                foreach (string st in gameController.activePC.GetComponent<PlayerStats>().states)
                {
                    if (st == "Stun" || st == "Inactive")
                    {
                        stateNo = true;
                    }
                    
                }
                if (gameController.activePC.GetComponent<PlayerStats>().curHealth <= 0)
                {
                    stateNo = true;
                }

                if (Event.current.control)
                {

                }
                else if (stateNo == true)
                {
                    DialogueManager.ShowAlert("Character is not active");
                }
                else
                {
                    GameObject abilityGO = null;

                    Debug.Log(playerActive.name + "/" + playerAbilities.ability1);
                    if (playerActive.transform.Find("Ability/" + playerAbilities.ability1) != null)
                    {
                        abilityGO = playerActive.transform.Find("Ability/" + playerAbilities.ability1).gameObject;
                    }
                    else
                    {
                        Debug.LogWarning(playerActive.name + "/" + playerAbilities.ability1);

                    }

                    abilityGO.SetActive(true);
                    int coolDown = DialogueLua.GetActorField(playerActive.name, "coolDown1").AsInt;
                    DialogueLua.SetActorField(playerActive.name, "timeAbi1", coolDown);
                }
            }
        }

        int timeAbi2 = DialogueLua.GetActorField(playerActive.name, "timeAbi2").AsInt;

        if (timeAbi2 > 0)
        {
            GUI.Button(new Rect(posX + buttonWide, posY, buttonWide, buttonHeight), new GUIContent(timeAbi2.ToString(), playerAbilities.ability2));
        }
        else
        {
            if (GUI.Button(new Rect(posX + buttonWide, posY, buttonWide, buttonHeight), new GUIContent(playerAbilities.TextAbi2, playerAbilities.ability2)))
            {

                bool stateNo = false;
                foreach (string st in gameController.activePC.GetComponent<PlayerStats>().states)
                {
                    if (st == "Stun" || st == "Inactive")
                    {
                        stateNo = true;
                    }
                }

                if (gameController.activePC.GetComponent<PlayerStats>().curHealth <= 0)
                {
                    stateNo = true;
                }

                if (Event.current.control)
                {

                }
                else if (stateNo == true)
                {
                    DialogueManager.ShowAlert("Character is not active");
                }
                else
                {
                    GameObject abilityGO = playerActive.transform.Find("Ability/" + playerAbilities.ability2).gameObject;
                    abilityGO.SetActive(true);
                    int coolDown = DialogueLua.GetActorField(playerActive.name, "coolDown2").AsInt;
                    DialogueLua.SetActorField(playerActive.name, "timeAbi2", coolDown);
                    //    Debug.Log("timeAbi1" + timeAbi1);

                }

                //         Debug.Log(gameController.activePC.transform.Find("Ability/" + playerAbilities.ability1).gameObject);
            }
        }



        if (GUI.Button (new Rect(posX + (3 * buttonWide), posY , buttonWide , buttonHeight), new GUIContent ( quickAttack, "Attack!") ))
        {
            gameController.ChangeCursorAttack();
        }
		
		if ((new Rect(posX + (3 * buttonWide), posY , buttonWide , buttonHeight).Contains (Event.current.mousePosition) ))
		{
			if (Input.GetMouseButtonUp (0) )
			{
				Debug.Log ("0");
			}
			else if (Event.current.button == 1)
			{
				Debug.Log ("1");
			}
		}


		if (GUI.Button (new Rect(posX + (4 * buttonWide), posY , buttonWide , buttonHeight), new GUIContent (  (Texture)(Resources.Load ("Icons/Rhand")), "Quick Item") ))
		{
			
		}
		
		if (GUI.Button (new Rect(posX + (5 * buttonWide), posY , buttonWide , buttonHeight), new GUIContent (  playerAbilities.TextSpells, "Spells")     ))
		{
            if (gameController.activePC .GetComponent<PlayerAbilities>().spells != null)
            {

                bool stateNo = false;
                foreach (string st in gameController.activePC.GetComponent<PlayerStats>().states)
                {
                    if (st == "Stun" || st == "Inactive")
                    {
                        stateNo = true;
                    }
                }
                if (stateNo == false)
                {

                    GetComponent<DisplaySpells>().enabled = true;
                    this.enabled = false;
                }
                else
                {
                    DialogueManager.ShowAlert("Character may not be controlled");
                }

            }
            else
            {
                DialogueManager.ShowAlert("This character doesnt have any spell available");
            }

		}
		
		if (GUI.Button (new Rect(posX + (6 * buttonWide), posY , buttonWide , buttonHeight), new GUIContent (  playerAbilities.specialAbilities, "Special Abilities")    ))
		{
		//	displayGenButtons = false;
			showChoice = "SpecialAbilities";
		}
		
		if (GUI.Button (new Rect(posX + (9 * buttonWide), posY , buttonWide , buttonHeight), new GUIContent (  playerAbilities.specialObjects, "Special Objects Actions")    ))
		{
			
		}

        QuickButtons();

        if (GUI.Button(new Rect(posX + (14 * buttonWide), posY, buttonWide, buttonHeight), new GUIContent((Texture)(aiDisplay), "Group AI On/Off")))
        {
            if (gameController.globalAI == true)
            {
                foreach (GameObject go in gameController.players)
                {
                    if (go != gameController.activePC)
                    {
                        if (go.GetComponent<PlayerAI>() != null)
                        {
                            go.GetComponent<PlayerAI>()._state = PlayerAI.State.DontMove;
                            go.GetComponent<PlayerAI>().dontMove = true;
                        }
                        else if (go.GetComponent<PlayerAIBlackWolf>() != null)
                        {
                            go.GetComponent<PlayerAIBlackWolf>()._state = PlayerAIBlackWolf.State.DontMove;
                            go.GetComponent<PlayerAIBlackWolf>().dontMove = true;
                        }
                        else if (go.GetComponent<PlayerAIRose>() != null)
                        {
                            go.GetComponent<PlayerAIRose>()._state = PlayerAIRose.State.DontMove;
                            go.GetComponent<PlayerAIRose>().dontMove = true;
                        }
                    }                    
                }
                aiDisplay = aiOff;
                aiIndividualDisplay = aiIndividualOff;
                gameController.globalAI = false;
            }
            else
            {
                foreach (GameObject go in gameController.players)
                {
                    if (go != gameController.activePC)
                    {
                        if (go.GetComponent<PlayerAI>() != null)
                        {
                            go.GetComponent<PlayerAI>()._state = PlayerAI.State.Idle;
                            go.GetComponent<PlayerAI>().dontMove = false;
                        }
                        else if (go.GetComponent<PlayerAIBlackWolf>() != null)
                        {
                            go.GetComponent<PlayerAIBlackWolf>()._state = PlayerAIBlackWolf.State.Idle;
                            go.GetComponent<PlayerAIBlackWolf>().dontMove = false;
                        }
                        else if (go.GetComponent<PlayerAIRose>() != null)
                        {
                            go.GetComponent<PlayerAIRose>()._state = PlayerAIRose.State.Idle;
                            go.GetComponent<PlayerAIRose>().dontMove = false;
                        }

                    }

                }
                aiDisplay = aiOn;
                aiIndividualDisplay = aiIndividualOn;
                gameController.globalAI = true;
            }
        }

        if ( GUI.Button(new Rect(posX + (15 * buttonWide), posY, buttonWide, buttonHeight), new GUIContent((Texture)(aiIndividualDisplay), "Individual AI On/Off")))
        {
            if (gameController.activePC.GetComponent<PlayerAI>().dontMove == true)
            {
                gameController.activePC.GetComponent<PlayerAI>().dontMove = false;
                aiIndividualDisplay = aiIndividualOn;
            }
            else
            {
                gameController.activePC.GetComponent<PlayerAI>().dontMove = true;
                aiIndividualDisplay = aiIndividualOff;
            }
            
        }

        GUI.Button (new Rect(posX + (17 * buttonWide), posY , buttonWide , buttonHeight), "" ) ;
		
		GUI.Label (new Rect (posX, posY - buttonHeight, 500, buttonHeight), GUI.tooltip);
	}

	void DisplaySpecialAbilities ()
	{
        playerAbilities = gameController.activePC.GetComponent<PlayerAbilities>();
  //      GameObject player = gameController.activePC;
  //      Texture ability = 
  
        if (GUI.Button(new Rect(posX + (buttonWide), posY, buttonWide, buttonHeight), new GUIContent(playerAbilities.TextAbi1, playerAbilities.ability1)))
        {

    //        gameController.activePC.transform.Find("Ability").gameObject.SetActive(true);
            //      gameController.activePC.transform.Find("Ability").gameObject.SetActive(false);
        }

        if (GUI.Button (new Rect(posX + (buttonWide * arrayString.Length), posY , buttonWide , buttonHeight), new GUIContent ("X", "Close Special Abilities"  )))
		{
			displayGenButtons = true;
		}

		GUI.Label (new Rect (posX, posY - buttonHeight, 500, buttonHeight), GUI.tooltip);
	}

    void QuickButtons ()
    {

        if (GUI.Button(new Rect(quickX1, posY, buttonWide, buttonHeight), quick1))
        {
     //       Debug.Log(GetComponent<DisplayItemScript>().textureCursor);
            if (GetComponent<DisplayItemScript>().textureCursor == true)
            {
                if (Event.current.button == 0)
                {
                    quick1 = GetComponent<DisplayItemScript>().textureToQuickItems;                    
                    DialogueLua.SetActorField(gameController.activePC.name, "quick1", quick1.name);
                    Debug.Log(quick1.name);
                }
                else if (Event.current.button == 1)
                {

                }
                GetComponent<DisplayItemScript>().BackToNormalCursor();

            }
            else
            {
                if (Event.current.button == 1)
                {
                    quick1.name = "DPad";
                    quick1 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
                }
                else
                {
                    if (quick1.name != "DPad")
                    {
                        CallQuickItem(quick1.name);
                        Debug.Log(quick1.name);
                    }
                    else
                    {
                        Debug.Log("Nno");
                    }
                }
            }

        }

        if (GUI.Button(new Rect(quickX2, posY, buttonWide, buttonHeight), quick2))
        {
            //       Debug.Log(GetComponent<DisplayItemScript>().textureCursor);
            if (GetComponent<DisplayItemScript>().textureCursor == true)
            {
                quick2 = GetComponent<DisplayItemScript>().textureToQuickItems;
                GetComponent<DisplayItemScript>().BackToNormalCursor();
                DialogueLua.SetActorField(gameController.activePC.name, "quick2", quick2.name);
                Debug.Log(quick2.name);
            }
            else
            {
                if (quick2.name != "DPad")
                {
                    CallQuickItem(quick2.name);
                    Debug.Log(quick2.name);
                }
                else
                {
                    Debug.Log("Nno");
                }
            }
        }

        if (GUI.Button(new Rect(quickX3, posY, buttonWide, buttonHeight), quick3))
        {
            //       Debug.Log(GetComponent<DisplayItemScript>().textureCursor);
            if (GetComponent<DisplayItemScript>().textureCursor == true)
            {
                quick3 = GetComponent<DisplayItemScript>().textureToQuickItems;
                GetComponent<DisplayItemScript>().BackToNormalCursor();
                DialogueLua.SetActorField(gameController.activePC.name, "quick3", quick3.name);
                Debug.Log(quick3.name);
            }
            else
            {
                if (quick3.name != "DPad")
                {
                    CallQuickItem(quick3.name);
                    Debug.Log(quick3.name);
                }
                else
                {
      //              Debug.Log("Nno");
                }
            }
        }
    }

    public void UpdateActivePC (GameObject player)
    {
        string quick1String = DialogueLua.GetActorField(player.name, "quick1").AsString;
        if (quick1String == "" || quick1String == "nill")
        {
            quick1 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
        }
        else
        {
            quick1 = (Texture)(Resources.Load("Icons/" + quick1String, typeof(Texture)));
        }

        string quick2String = DialogueLua.GetActorField(player.name, "quick2").AsString;
        if (quick2String == "" || quick2String == "nill")
        {
            quick2 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
        }
        else
        {
            quick2 = (Texture)(Resources.Load("Icons/" + quick2String, typeof(Texture)));
        }

        string quick3String = DialogueLua.GetActorField(player.name, "quick3").AsString;
        if (quick3String == "" || quick3String == "nill" || quick3String == "DPad")
        {
            quick3 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
        }
        else
        {
            quick3 = (Texture)(Resources.Load("Icons/" + quick3String, typeof(Texture)));
        }
    }

    private void CallQuickItem (string callItem)
    {
        GameObject newQuickItem = Instantiate(Resources.Load("QuickItems/" + callItem), transform.position, Quaternion.identity) as GameObject;
        newQuickItem.transform.parent = transform;
        newQuickItem.SetActive(true);

    }


    
    public void CoolDownControl ()
    {
        foreach (GameObject go in gameController.players)
        {
            PlayerAbilities pa = go.GetComponent<PlayerAbilities>();
            
            int timeAbi1 = DialogueLua.GetActorField(go.name, "timeAbi1").AsInt;
        //    Debug.Log("TimeAbi1" + timeAbi1 + "/" + go.name);
            if (timeAbi1 > 0)
            {
                timeAbi1--;
                DialogueLua.SetActorField(go.name, "timeAbi1", timeAbi1);
            }

            int timeAbi2 = DialogueLua.GetActorField(go.name, "timeAbi2").AsInt;
            if (timeAbi2 > 0)
            {
                timeAbi2--;
                DialogueLua.SetActorField(go.name, "timeAbi2", timeAbi2);
            }
        }
    }
}
