using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class DisplayCharacter : MonoBehaviour 
{
    public LevelUp levelUp;
    //X/Y coordinates for display attributes and other
    #region
    private int startNameXPos;
    private int startNameField;
	private int startNameYPos;
	private int startGenderYPos;
	private int startRaceYpos;
	private int startProfYPos;
	private int startBenignYPos;
	private int startOrderYPos;
	private int startFriendYpos;
    private int startAttrYpos;
	private int startStrYPos;
	private int startAgiYPos;
	private int startConYPos;
	private int startWillPos;
	private int startIntePos;
	private int startIntuPos;
	private int startCharPos;
    private int XScreenHalf;
    private int XScreenField;
    private int levelPos;
    private int expPos;
    private int party;
    private int benign;
    private int order;
    private int evil;
    private int chaos;
    private int totalRep;
    private int totalReputation;
    private int battleSkills;
    private int axe;
    private int bow;
    private int crossbow;
    private int knife;
    private int lance;
    private int blunt;
    private int scynthe;
    private int sling;
    private int stick;
    private int sword;
    private int throwing;
    private int great;
    private int shield;
    private int doubleWeapon;
    private int unarmed;
    private int arcabuz;
    private int katana;
    private int detect;
    private int tramp;
    private int hide;
    private int backstab;
    private int resistences;
    private int crush;
    private int slash;
    private int pierce;
    private int fire;
    private int acid;
    private int ice;
    private int elec;
    private int magic;
    private int necro;
    private int mind;
    private int entropy;

    #endregion

    private Vector2 scrollPosition = Vector2.zero;
    private bool showInfo = false;
    public bool skillPoints = false;
    private int buttonHeight= 25;
	private int buttonWidth = 100;
    private int longButtonWidth;
    private int buttonAbility;
    private int optionPanel = 30;
    private int toolBarY;
    private int toolBarHeight;
    private int charBarHeight;
    private int weaponsPointsLeft;
    private int thiefPointsLeft;
    private string namePlayer;
	private string benignDisplay;
	private string orderDisplay;
	private string friendshipDisplay;
	private string relationship;
	private string orderRepDisplay;
	private string benignRepDisplay;
    private string evilRepDisplay;
    private string chaosRepDisplay;
    private string totalRepDisplay;
    private string info;
    private string labelInfo;
    private string abilityActive;
    private string ability1;
    private string ability2;
	private GameObject gameController;
	private GameObject player;
	private GameObject PC2Obj;
	private GameObject PC3Obj;
	private GameObject PC4Obj;
	private GameObject PC5Obj;
	public GameObject activePC;
	private DisplayAI displayAI;
	private DisplayBooks displayBooks;
    private PlayerAbilities pa;
    private ExpController expController;
    
	private Rect _optionsWindowRect = new Rect (1, 1, Screen.width *0.85f, Screen.height *0.80f);
    private Vector2 characterRectSlider = Vector2.zero;
    private Rect characterWindowSliderRect;
    private string [] optionsPanelNames = new string[] 
	{
		"Attributes",
        "Spells",
		"States",
		"AI",
        "Bio"
	};

    private void Start()
    {
        OnEnable();
    }

    public void OnEnable () 
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController");
        expController = GetComponent<ExpController>();
    //    levelUp = transform.Find("LevelUp").GetComponent<LevelUp>();        
		player = gameController.GetComponent<GameController>().player;
		PC2Obj = gameController.GetComponent<GameController>().PC2Obj;
		PC3Obj = gameController.GetComponent<GameController>().PC3Obj;
		PC4Obj = gameController.GetComponent<GameController>().PC4Obj;
		PC5Obj = gameController.GetComponent<GameController>().PC5Obj;
    //    Debug.Log("Onenable");
            
		activePC = player;
		ChangePC ();
        GetReputationData();
        // X/Y coordinates to display labels of attributes
        #region
        longButtonWidth = (int)(Screen.width * 0.20f);
        startNameXPos = (int) (Screen.width * 0.02f);
        startNameField = startNameXPos + longButtonWidth;
		startNameYPos = (int) (Screen.height * 0.25f);
        buttonAbility = (int)(Screen.width * 0.07f);
        GUISkin myskin = (GUISkin)(Resources.Load("GUI/FantasyT"));
        int fontSize = myskin.GetStyle("label").fontSize;
        buttonHeight = (int)(Screen.height * 0.035f);
        buttonWidth = (int)(Screen.width * 0.2f);
        startGenderYPos = startNameYPos + buttonHeight;
		startRaceYpos = startGenderYPos + buttonHeight;
		startProfYPos = startRaceYpos + buttonHeight;
        startBenignYPos = startProfYPos + buttonHeight;
		startOrderYPos = startBenignYPos + buttonHeight;
		startFriendYpos = startOrderYPos + buttonHeight;
		XScreenHalf = (int)(Screen.width * 0.45f);
        XScreenField = XScreenHalf + longButtonWidth;
        levelPos = startNameYPos;
        expPos = levelPos + buttonHeight;
        party = expPos + buttonHeight;
        benign = party + buttonHeight;
        order = benign + buttonHeight;
        evil = order + buttonHeight;
        chaos = evil + buttonHeight;
        totalRep = chaos + buttonHeight;
        startAttrYpos = totalRep + (2 * buttonHeight);
		startStrYPos = startAttrYpos + buttonHeight;
		startConYPos =	startStrYPos + buttonHeight;
		startAgiYPos =  startConYPos + buttonHeight;
		startWillPos = startAgiYPos + buttonHeight;
		startIntuPos = startWillPos + buttonHeight;
		startIntePos = startIntuPos + buttonHeight;
		startCharPos = startIntePos + buttonHeight;
        toolBarHeight = (int) (Screen.height * 0.06f);
        toolBarY = (int)(Screen.height * 0.09f);
        #endregion
        // coordinates to display battle skills
        #region
        battleSkills = startCharPos + (2 * buttonHeight);
        axe = battleSkills + buttonHeight;
        bow = axe + buttonHeight;
        crossbow = bow + buttonHeight;
        knife = crossbow + buttonHeight;
        lance = knife + buttonHeight;
        blunt = lance + buttonHeight;
        scynthe = blunt + buttonHeight;
        sling = scynthe + buttonHeight;
        stick = sling + buttonHeight;
        sword = stick + buttonHeight;
        throwing = sword + buttonHeight;
        great = throwing + buttonHeight;
        shield = great + buttonHeight;
        doubleWeapon = shield + buttonHeight;
        unarmed = doubleWeapon + buttonHeight;
        arcabuz = unarmed + buttonHeight;
        katana = arcabuz + buttonHeight;
        #endregion
        // coordinates for resistences
        #region
        resistences = katana + (2 * buttonHeight);
        crush = resistences + ( buttonHeight);
        slash = crush + buttonHeight;
        pierce = slash + buttonHeight;
        fire = pierce + buttonHeight;
        acid = fire + buttonHeight;
        ice = acid + buttonHeight;
        elec = ice + buttonHeight;
        magic = elec + buttonHeight;
        necro = magic + buttonHeight;
        mind = necro + buttonHeight;
        entropy = mind + buttonHeight;
        #endregion
        characterWindowSliderRect = new Rect(Screen.width * 0.005f, toolBarHeight + toolBarY, Screen.width * 0.96f, Screen.height * 3);
        if (GetComponent<DisplayAI>())
        {
            displayAI = GetComponent<DisplayAI>();
        }
        else
        {
            Debug.LogError("displayAI missing");
        }
		displayBooks = GetComponent <DisplayBooks>();
        GetAllValues();
        abilityActive = ability1;


       
	}    

    private void DisplayPersonalData()
    {
        //	optionPanel = GUI.Toolbar (new Rect (10, 102, _optionsWindowRect.width, 50), optionPanel, optionsPanelNames);
        optionPanel = GUI.Toolbar(new Rect(Screen.width * 0.005f, toolBarY, _optionsWindowRect.width, toolBarHeight), optionPanel, optionsPanelNames);

        switch (optionPanel)
        {
            case 0:
                DisplayData();
                break;
            case 1:
                DisplaySpells();
                break;

            case 2:

                break;

            case 3:
                displayAI.DisplayAIOptions();
                break;

            case 4:
                
                break;
        }
    }

    public void ChangePC ()
	{
        skillPoints = levelUp.CheckLevelUp(activePC.name, false);
        
		if (activePC.name == gameController.GetComponent<GameController>().player.name)
		{
			relationship = "";
			friendshipDisplay = "";
		}
		else
		{
			relationship = "Partnership";
			int friendship = DialogueLua.GetActorField (activePC.name, "friendship").AsInt;
    //        Debug.Log(activePC.name);
			if (friendship >= 0 && friendship <= 25)
			{
				friendshipDisplay = friendship.ToString() + " / Acquaintance";
			}
			else if (friendship > 25 && friendship <= 50)
			{
				friendshipDisplay = friendship.ToString() + " / Casual Friendship";
			}
			else if (friendship > 50 && friendship <= 75)
			{
				friendshipDisplay = friendship.ToString() + " / Fellowship";
			}
			else if (friendship > 75 && friendship <= 100)
			{
				friendshipDisplay = friendship.ToString() + " / Intimate Friends";
			}
			else if (friendship < 0 && friendship >= -25)
			{
				friendshipDisplay = friendship.ToString() + " / Acquaintance";
			}
			else if (friendship < -25 && friendship >= -50)
			{
				friendshipDisplay = friendship.ToString() + " / Bad relationship";
			}
			else if (friendship < -25 && friendship >= -75)
			{
				friendshipDisplay = friendship.ToString() + " / Horrible relationship";
			}
			else if (friendship < -75 && friendship >= -100)
			{
				friendshipDisplay = friendship.ToString() + " / Sworn Enemy";
			}

		}
		GetAllValues();
        int levelActivePC = DialogueLua.GetActorField(activePC.name, "level").AsInt;
    //    Debug.Log(levelActivePC);
        if (levelActivePC < expController.trueLevel)
        {
            skillPoints = true;
        }

        int level = DialogueLua.GetActorField("Player", "level").AsInt;
        if (level < 1)
        {
            level = 1;
        }
        int spentExpPoints = DialogueLua.GetActorField(activePC.name, "spentExpPoints").AsInt;
        int spentThiefPoints = DialogueLua.GetActorField(activePC.name, "spentThiefPoints").AsInt;
        if (DialogueLua.GetActorField(activePC.name, "battleSkillPoints").AsInt == 0)
        {
            Debug.LogError(activePC.name + " battleSkillPoints not set up");
        }
        int expPointsTotal = (DialogueLua.GetActorField(activePC.name, "battleSkillPoints").AsInt * level) + DialogueLua.GetActorField(activePC.name, "expPointsBuff").AsInt - DialogueLua.GetActorField(activePC.name, "battleSkillPoints").AsInt;
        int thiefPointsTotal = (DialogueLua.GetActorField(activePC.name, "thiefSkillPoints").AsInt * level) + DialogueLua.GetActorField(activePC.name, "thiefPointsBuff").AsInt - DialogueLua.GetActorField(activePC.name, "thiefSkillPoints").AsInt;


        weaponsPointsLeft = expPointsTotal - spentExpPoints;
        thiefPointsLeft = thiefPointsTotal - spentThiefPoints;

   //     levelUp.thiefPointsLeft = thiefPointsLeft;
   //     levelUp.weaponsPointsLeft = weaponsPointsLeft;

        if (weaponsPointsLeft > 0 || thiefPointsLeft > 0)
        {
            skillPoints = true;
      //      Debug.Log(weaponsPointsLeft + "/" + activePC.name + "/" + expPointsTotal + "/" + spentExpPoints);
        }
        //	GetReputationData ();
    }

	private void GetAllValues ()
	{
		int benign = DialogueLua.GetActorField (activePC.name, "benignPath").AsInt;
		int order = DialogueLua.GetActorField (activePC.name, "orderPath").AsInt;
   //     Debug.Log(activePC.name + "/" + benign + "/"+ order);
        if (benign >= -25 && benign <= 25)
		{
			benignDisplay = (benign.ToString()) + " / Unaligned";
		}
		else if (benign > 25 && benign < 50)
		{
			benignDisplay = (benign.ToString()) + " / Moderately Benign";
		}
		else if (benign >= 50 && benign < 75 )
		{
			benignDisplay = (benign.ToString()) + " / Benign";
		}
		else if (benign >= 75)
		{
			benignDisplay = (benign.ToString()) + " / Strongly Good";
		}
		else if (benign < -25 && benign >= -50)
		{
			benignDisplay = (benign.ToString()) + " / Moderately Evil";
		}
		else if (benign < -50 && benign >= -75)
		{
			benignDisplay = (benign.ToString()) + " / Evil";
		}
		else if (benign <= -75 && benign >= -97)
		{
			benignDisplay = (benign.ToString()) + " / Strongly Evil";
		}
        else if (benign <= -98)
        {
            benignDisplay = (benign.ToString()) + " / The Essence of Evil";
        }

		if (order >= -25 && order <= 25)
		{
			orderDisplay = (order.ToString()) + " / Unaligned";
		}
		else if (order > 25 && order < 50)
		{
			orderDisplay = (order.ToString()) + " / Moderately Order";
		}
		else if (order >= 50 && order < 75 )
		{
			orderDisplay = (order.ToString()) + " / Order";
		}
		else if (order >= 75)
		{
			orderDisplay = (order.ToString()) + " / Strongly Order";
		}
		else if (order < -25 && order > -50)
		{
			orderDisplay = (order.ToString()) + " / Moderately Chaos";
		}
		else if (order <= -50 && order > -75 )
		{
			orderDisplay = (order.ToString()) + " / Chaos";
		}
		else if (order <= -75 && order >= -97)
		{
			orderDisplay = (order.ToString()) + " / Strongly Chaos";
		}
        else if (order <= -98)
        {
            orderDisplay = (order.ToString()) + " / A truly force of Entropy";
        }
        pa = activePC.GetComponent<PlayerAbilities>();
        TextAsset path = (TextAsset)(Resources.Load("Text/Abilities/"+ pa.ability1));
        if (path != null)
        {
            ability1 = path.text;
        }
        else
        {
            ability1 = "";
        }
        
        TextAsset path2 = (TextAsset)(Resources.Load("Text/Abilities/" + pa.ability2));
        if (path2 != null)
        {
            ability2 = path2.text;
        }
        else
        {
            ability2 = "";
        }
        abilityActive = ability1;
     //   ability1 = path2.text;
    }

	public void GetReputationData ()
	{
		int benign = DialogueLua.GetActorField ("Player", "reputationBenign").AsInt;
		int order = DialogueLua.GetActorField ("Player", "reputationOrder").AsInt;
        int evil = DialogueLua.GetActorField("Player", "reputationEvil").AsInt;
        int chaos = DialogueLua.GetActorField("Player", "reputationChaos").AsInt;
        //  int totalReputation = DialogueLua.GetActorField("Player", "reputationTotal").AsInt;
        int totalReputation = benign + order + evil + chaos;

        if (benign >= -25 && benign <= 25)
		{
			benignRepDisplay = (benign.ToString()) + " / Unknown";
		}
		else if (benign > 25 && benign < 50)
		{
			benignRepDisplay = (benign.ToString()) + " / Some people heard of you";
		}
		else if (benign >= 50 && benign < 75 )
		{
			benignRepDisplay = (benign.ToString()) + " / Well-known";
		}
		else if (benign >= 75)
		{
			benignRepDisplay = (benign.ToString()) + " / Hero (Benign)";
		}

		
		if (order >= -25 && order <= 25)
		{
			orderRepDisplay = (order.ToString()) + " / Unknown";
		}
		else if (order > 25 && order  < 50)
		{
			orderRepDisplay = (order.ToString()) + " / Some people heard of you";
		}
		else if (order >= 50 && order < 75 )
		{
			orderRepDisplay = (order.ToString()) + " / Well-known";
		}
		else if (order >= 75)
		{
			orderRepDisplay = (order.ToString()) + " / Hero (Order)";
		}

        //Evil

        if (evil >= -25 && evil <= 25)
        {
            evilRepDisplay = (evil.ToString()) + " / Unknown";
        }
        else if (evil > 25 && evil < 50)
        {
            evilRepDisplay = (evil.ToString()) + " / Some people heard of you";
        }
        else if (evil >= 50 && evil < 75)
        {
            evilRepDisplay = (evil.ToString()) + " / Well-known";
        }
        else if (evil >= 75)
        {
            evilRepDisplay = (evil.ToString()) + " / Hero (Evil)";
        }


        if (chaos >= -25 && chaos <= 25)
        {
            chaosRepDisplay = (chaos.ToString()) + " / Unknown";
        }
        else if (chaos > 25 && chaos < 50)
        {
            chaosRepDisplay = (chaos.ToString()) + " / Some people heard of you";
        }
        else if (chaos >= 50 && chaos < 75)
        {
            chaosRepDisplay = (chaos.ToString()) + " / Well-known";
        }
        else if (chaos >= 75)
        {
            chaosRepDisplay = (chaos.ToString()) + " / Hero (Entropy)";
        }



        if (totalReputation >= -25 && totalReputation <= 25)
        {
            totalRepDisplay = (totalReputation.ToString()) + " / Unknown";
        }
        else if (totalReputation > 25 && totalReputation < 50)
        {
            totalRepDisplay = (totalReputation.ToString()) + " / Some people heard of you";
        }
        else if (totalReputation >= 50 && totalReputation < 75)
        {
            totalRepDisplay = (totalReputation.ToString()) + " / Well-known";
        }
        else if (totalReputation >= 75)
        {
            totalRepDisplay = (totalReputation.ToString()) + " / Hero (Total)";
        }

    }

	public void DisplayAllData ()
	{
		DisplayPersonalData();
	}

    void DisplayData ()
    {
    //    Debug.Log(skillPoints);
        if (skillPoints == true)
        {
            if (GUI.Button(new Rect(Screen.width * 0.38f, Screen.height * 0.15f, Screen.width * 0.24f, Screen.height * 0.06f), "SKILL POINTS"))
            {
                levelUp.gameObject.SetActive(true);
                GetComponent<DisplayPortraits>().enabled = false;
            }
        }
        characterRectSlider = GUI.BeginScrollView(new Rect(Screen.width * 0.005f, toolBarHeight + toolBarY + (Screen.width * 0.05f), Screen.width * 0.88f, Screen.height * 0.8f), characterRectSlider,
                                      characterWindowSliderRect);
        DisplayAttributes();
        DisplaySkills();
        DisplayResistences();
        DisplayAbilities();
        GUI.EndScrollView();
    }

	void DisplayAttributes()
	{
   //     Debug.Log(activePC.name);
        GUI.Label(new Rect(startNameXPos, startNameYPos, buttonWidth, buttonHeight), "Name:");
		if (activePC.name == "Player")
		{
			GUI.Label(new Rect(startNameField, startNameYPos, buttonWidth, buttonHeight), DialogueLua.GetActorField ("Player", "playerName").AsString );
		}
		else
		{
			GUI.Label(new Rect(startNameField, startNameYPos, buttonWidth, buttonHeight), activePC.name);
		}
        GUI.Label(new Rect(startNameXPos, startGenderYPos, buttonWidth, buttonHeight), "Gender:");
		GUI.Label(new Rect(startNameField, startGenderYPos, buttonWidth, buttonHeight), DialogueLua.GetActorField (activePC.name, "gender").AsString);
		
		GUI.Label(new Rect(startNameXPos, startRaceYpos, buttonWidth, buttonHeight), "Race");
		GUI.Label(new Rect(startNameField, startRaceYpos, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().race);
		
		GUI.Label(new Rect(startNameXPos, startProfYPos, buttonWidth, buttonHeight), "Profesion");
		GUI.Label(new Rect(startNameField, startProfYPos, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().profession);

        GUI.Label(new Rect(startNameXPos, startBenignYPos, buttonWidth, buttonHeight), "Benign/Evil");
		GUI.Label(new Rect(startNameField, startBenignYPos,longButtonWidth, buttonHeight), benignDisplay.ToString());
		
		GUI.Label(new Rect(startNameXPos, startOrderYPos, buttonWidth, buttonHeight), "Order/Entropy");
		GUI.Label(new Rect(startNameField, startOrderYPos, longButtonWidth, buttonHeight), orderDisplay.ToString());
		
		GUI.Label(new Rect(startNameXPos, startFriendYpos, buttonWidth, buttonHeight), relationship);
		GUI.Label(new Rect(startNameField, startFriendYpos, longButtonWidth, buttonHeight), friendshipDisplay);
		
		
		GUI.Label(new Rect(startNameXPos, startAttrYpos, buttonWidth, buttonHeight), "ATTRIBUTES");
		
		GUI.Label(new Rect(startNameXPos, startStrYPos, buttonWidth, buttonHeight), "Strength");
		GUI.Label(new Rect(startNameField, startStrYPos, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totalStr.ToString() );

		GUI.Label(new Rect(startNameXPos, startConYPos, buttonWidth, buttonHeight), "Constitution");
		GUI.Label(new Rect(startNameField, startConYPos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._constitution + activePC.GetComponent<PlayerStats>().buffCon).ToString());

		GUI.Label(new Rect(startNameXPos, startAgiYPos, buttonWidth, buttonHeight), "Agility");
		GUI.Label(new Rect(startNameField, startAgiYPos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._agility + activePC.GetComponent<PlayerStats>().buffAgi).ToString() );

		GUI.Label(new Rect(startNameXPos, startWillPos, buttonWidth, buttonHeight), "Willpower");
		GUI.Label(new Rect(startNameField, startWillPos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._willpower + activePC.GetComponent<PlayerStats>().buffWill).ToString());

		GUI.Label(new Rect(startNameXPos, startIntePos, buttonWidth, buttonHeight), "Inteligence");
		GUI.Label(new Rect(startNameField, startIntePos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._inteligence + activePC.GetComponent<PlayerStats>().buffInte).ToString());

		GUI.Label(new Rect(startNameXPos, startIntuPos, buttonWidth, buttonHeight), "Intuition");
		GUI.Label(new Rect(startNameField, startIntuPos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._intuition+ activePC.GetComponent<PlayerStats>().buffIntu).ToString());

		GUI.Label(new Rect(startNameXPos, startCharPos, buttonWidth, buttonHeight), "Charisma");
		GUI.Label(new Rect(startNameField, startCharPos, buttonWidth, buttonHeight), (activePC.GetComponent<PlayerStats>()._charisma + activePC.GetComponent<PlayerStats>().buffCha).ToString());

        GUI.Label(new Rect(XScreenHalf, levelPos, buttonWidth, buttonHeight), "Level:");
        GUI.Label(new Rect(XScreenField, levelPos, buttonWidth, buttonHeight), DialogueLua.GetActorField (activePC.name, "level").AsInt.ToString());

        GUI.Label(new Rect(XScreenHalf, expPos, buttonWidth, buttonHeight), "Experience:");
        GUI.Label(new Rect(XScreenField, expPos, buttonWidth, buttonHeight), DialogueLua.GetActorField("Player", "experience").AsInt.ToString() + " / " + GetComponent<ExpController>().nextLevelExp);

        GUI.Label(new Rect(XScreenHalf, party, longButtonWidth, buttonHeight), "PARTY REPUTATION");
    
        GUI.Label(new Rect(XScreenHalf, benign, longButtonWidth, buttonHeight), "Benign Reputation:");
		GUI.Label(new Rect(XScreenField, benign, buttonWidth, buttonHeight), benignRepDisplay);
		
		GUI.Label(new Rect(XScreenHalf, order, longButtonWidth, buttonHeight), "Order Reputation");
		GUI.Label(new Rect(XScreenField, order, buttonWidth, buttonHeight), orderRepDisplay);

        GUI.Label(new Rect(XScreenHalf, evil, longButtonWidth, buttonHeight), "Evil Reputation");
        GUI.Label(new Rect(XScreenField, evil, buttonWidth, buttonHeight), evilRepDisplay);

        GUI.Label(new Rect(XScreenHalf, chaos, longButtonWidth, buttonHeight), "Chaos Reputation");
        GUI.Label(new Rect(XScreenField, chaos, buttonWidth, buttonHeight), chaosRepDisplay);

        GUI.Label(new Rect(XScreenHalf, totalRep, longButtonWidth, buttonHeight), "Total Reputation");
        GUI.Label(new Rect(XScreenField, totalRep, buttonWidth, buttonHeight), totalRepDisplay);

        GUI.Label(new Rect(XScreenHalf, startStrYPos, longButtonWidth, buttonHeight), "Melee Bonus");
		GUI.Label(new Rect(XScreenField, startStrYPos, buttonWidth, buttonHeight), activePC.GetComponent <PlayerStats>().totMeleeSkill.ToString());

		GUI.Label(new Rect(XScreenHalf, startConYPos, longButtonWidth, buttonHeight), "Defense bonus");
		GUI.Label(new Rect(XScreenField, startConYPos, buttonWidth, buttonHeight), activePC.GetComponent <PlayerStats>().totDefSkill.ToString());

        GUI.Label(new Rect(XScreenHalf, startAgiYPos, longButtonWidth, buttonHeight), "Defense Ranged bonus");
        GUI.Label(new Rect(XScreenField, startAgiYPos, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totDefRangeSkill.ToString());

        GUI.Label(new Rect(XScreenHalf, startWillPos, longButtonWidth, buttonHeight), "Ranged bonus");
		GUI.Label(new Rect(XScreenField, startWillPos, buttonWidth, buttonHeight), activePC.GetComponent <PlayerStats>().totRangedSkill.ToString());

		GUI.Label(new Rect(XScreenHalf, startIntuPos, longButtonWidth, buttonHeight), "Magical attack");
		GUI.Label(new Rect(XScreenField, startIntuPos, buttonWidth, buttonHeight), activePC.GetComponent <PlayerStats>().totMagicSkill.ToString());

    }

    void DisplaySkills ()
    {
        GUI.Label(new Rect(startNameXPos, battleSkills, longButtonWidth, buttonHeight), "COMBAT SKILLS");

        GUI.Label(new Rect(startNameXPos, axe, longButtonWidth, buttonHeight), "Axe:");
        GUI.Label(new Rect(startNameField, axe, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totAxe.ToString());
        GUI.Label(new Rect(startNameXPos, bow, longButtonWidth, buttonHeight), "Bow:");
        GUI.Label(new Rect(startNameField, bow, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totBow.ToString());
        GUI.Label(new Rect(startNameXPos, crossbow, longButtonWidth, buttonHeight), "Crossbow:");
        GUI.Label(new Rect(startNameField, crossbow, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totCross.ToString());
        GUI.Label(new Rect(startNameXPos, knife, longButtonWidth, buttonHeight), "Knife:");
        GUI.Label(new Rect(startNameField, knife, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totKnife.ToString());
        GUI.Label(new Rect(startNameXPos, lance, longButtonWidth, buttonHeight), "Lance:");
        GUI.Label(new Rect(startNameField, lance, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totLance.ToString());
        GUI.Label(new Rect(startNameXPos, blunt, longButtonWidth, buttonHeight), "Blunt:");
        GUI.Label(new Rect(startNameField, blunt, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totBlunt.ToString());
        GUI.Label(new Rect(startNameXPos, scynthe, longButtonWidth, buttonHeight), "Scynthe:");
        GUI.Label(new Rect(startNameField, scynthe, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totScyn.ToString());
        GUI.Label(new Rect(startNameXPos, sling, longButtonWidth, buttonHeight), "Sling:");
        GUI.Label(new Rect(startNameField, sling, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totSling.ToString());
        GUI.Label(new Rect(startNameXPos, stick, longButtonWidth, buttonHeight), "Stick:");
        GUI.Label(new Rect(startNameField, stick, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totStick.ToString());
        GUI.Label(new Rect(startNameXPos, sword, longButtonWidth, buttonHeight), "Sword:");
        GUI.Label(new Rect(startNameField, sword, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totSword.ToString());
        GUI.Label(new Rect(startNameXPos, throwing, longButtonWidth, buttonHeight), "Throwing:");
        GUI.Label(new Rect(startNameField, throwing, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totThrow.ToString());
        GUI.Label(new Rect(startNameXPos, great, longButtonWidth, buttonHeight), "Great Weapons:");
        GUI.Label(new Rect(startNameField, great, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totGreat.ToString());
        GUI.Label(new Rect(startNameXPos, shield, longButtonWidth, buttonHeight), "Shield:");
        GUI.Label(new Rect(startNameField, shield, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totShield.ToString());
        GUI.Label(new Rect(startNameXPos, doubleWeapon, longButtonWidth, buttonHeight), "Double Weapons:");
        GUI.Label(new Rect(startNameField, doubleWeapon, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totDouble.ToString());
        GUI.Label(new Rect(startNameXPos, unarmed, longButtonWidth, buttonHeight), "Fists:");
        GUI.Label(new Rect(startNameField, unarmed, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totUnarmed.ToString());
        GUI.Label(new Rect(startNameXPos, arcabuz, longButtonWidth, buttonHeight), "Arcabuz:");
        GUI.Label(new Rect(startNameField, arcabuz, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totArcabuz.ToString());
        GUI.Label(new Rect(startNameXPos, katana, longButtonWidth, buttonHeight), "Katana:");
        GUI.Label(new Rect(startNameField, katana, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totKatana.ToString());

        GUI.Label(new Rect(XScreenHalf, battleSkills, longButtonWidth, buttonHeight), "THIEF SKILLS:");
        GUI.Label(new Rect(XScreenHalf, axe, longButtonWidth, buttonHeight), "Trap detection:");
        GUI.Label(new Rect(XScreenField, axe, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totDetect.ToString());
        GUI.Label(new Rect(XScreenHalf, bow, longButtonWidth, buttonHeight), "Trap disarm:");
        GUI.Label(new Rect(XScreenField, bow, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totTramp.ToString());
        GUI.Label(new Rect(XScreenHalf, crossbow, longButtonWidth, buttonHeight), "Hide:");
        GUI.Label(new Rect(XScreenField, crossbow, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totHide.ToString());
        GUI.Label(new Rect(XScreenHalf, knife, longButtonWidth, buttonHeight), "Backstab:");
        GUI.Label(new Rect(XScreenField, knife, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totBackstab.ToString());


        /*
        defenseSkill = DialogueLua.GetActorField(_namePlayer, "defenseSkill").AsInt;
        meleeSkill = DialogueLua.GetActorField(_namePlayer, "meleeSkill").AsInt;
        rangedSkill = DialogueLua.GetActorField(_namePlayer, "meleeSkill").AsInt;
        magicSkill = DialogueLua.GetActorField(_namePlayer, "magicSkill").AsInt;
        axeSkill = DialogueLua.GetActorField(_namePlayer, "axeSkill").AsInt;
        bowSkill = DialogueLua.GetActorField(_namePlayer, "bowSkill").AsInt;
        crossbowSkill = DialogueLua.GetActorField(_namePlayer, "crossbowSkill").AsInt;
        knifeSkill = DialogueLua.GetActorField(_namePlayer, "knifeSkill").AsInt;
        lanceSkill = DialogueLua.GetActorField(_namePlayer, "lanceSkill").AsInt;
        bluntSkill = DialogueLua.GetActorField(_namePlayer, "bluntSkill").AsInt;
        scyntheSkill = DialogueLua.GetActorField(_namePlayer, "scyntheSkill").AsInt;
        slingSkill = DialogueLua.GetActorField(_namePlayer, "slingSkill").AsInt;
        stickSkill = DialogueLua.GetActorField(_namePlayer, "stickSkill").AsInt;
        swordSkill = DialogueLua.GetActorField(_namePlayer, "swordSkill").AsInt;
        throwingSkill = DialogueLua.GetActorField(_namePlayer, "throwingSkill").AsInt;
        greatWeaponSkill = DialogueLua.GetActorField(_namePlayer, "greatWeaponSkill").AsInt;
        weaponAndShieldSkill = DialogueLua.GetActorField(_namePlayer, "weaponAndShieldSkill").AsInt;
        doubleWeaponSkill = DialogueLua.GetActorField(_namePlayer, "doubleWeaponSkill").AsInt;
        unarmedSkill = DialogueLua.GetActorField(_namePlayer, "unarmedSkill").AsInt;
        arcabuzSkill = DialogueLua.GetActorField(_namePlayer, "arcabuzSkill").AsInt;
        katanaSkill = DialogueLua.GetActorField(_namePlayer, "katanaSkill").AsInt;
        detectTrampsSkill = DialogueLua.GetActorField(_namePlayer, "detectTrampSkill").AsInt;
        trampAbilitySkill = DialogueLua.GetActorField(_namePlayer, "trampAbilitySkill").AsInt;
        hideSkill = DialogueLua.GetActorField(_namePlayer, "hideSkill").AsInt;
        backstabSkill = DialogueLua.GetActorField(_namePlayer, "backstabSkill").AsInt;
        */
    }

    void DisplayResistences ()
    {
        GUI.Label(new Rect(startNameXPos, resistences, longButtonWidth, buttonHeight), "RESISTENCES");
        GUI.Label(new Rect(startNameXPos, crush, longButtonWidth, buttonHeight), "Crushing:");
        GUI.Label(new Rect(startNameField, crush, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totCruRes.ToString());
        GUI.Label(new Rect(startNameXPos, slash, longButtonWidth, buttonHeight), "Slashing:");
        GUI.Label(new Rect(startNameField, slash, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totSlaRes.ToString());
        GUI.Label(new Rect(startNameXPos, pierce, longButtonWidth, buttonHeight), "Piercing:");
        GUI.Label(new Rect(startNameField, pierce, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totPierRes.ToString());
        GUI.Label(new Rect(startNameXPos, fire, longButtonWidth, buttonHeight), "Fire:");
        GUI.Label(new Rect(startNameField, fire, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totFireRes.ToString());
        GUI.Label(new Rect(startNameXPos, acid, longButtonWidth, buttonHeight), "Acid:");
        GUI.Label(new Rect(startNameField, acid, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totAcidRes.ToString());
        GUI.Label(new Rect(startNameXPos, ice, longButtonWidth, buttonHeight), "Ice:");
        GUI.Label(new Rect(startNameField, ice, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totIceRes.ToString());
        GUI.Label(new Rect(startNameXPos, elec, longButtonWidth, buttonHeight), "Electricity:");
        GUI.Label(new Rect(startNameField, elec, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totElecRes.ToString());
        GUI.Label(new Rect(startNameXPos, magic, longButtonWidth, buttonHeight), "Magic:");
        GUI.Label(new Rect(startNameField, magic, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totMagicRes.ToString());
        GUI.Label(new Rect(startNameXPos, necro, longButtonWidth, buttonHeight), "Necro:");
        GUI.Label(new Rect(startNameField, necro, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totNecroRes.ToString());
        GUI.Label(new Rect(startNameXPos, mind, longButtonWidth, buttonHeight), "Mind:");
        GUI.Label(new Rect(startNameField, mind, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totMindRes.ToString());
        GUI.Label(new Rect(startNameXPos, entropy, longButtonWidth, buttonHeight), "Entropy:");
        GUI.Label(new Rect(startNameField, entropy, buttonWidth, buttonHeight), activePC.GetComponent<PlayerStats>().totEntroRes.ToString());
    }

    void DisplaySpells ()
    {

        if (showInfo == false)
        {
            PlayerAbilities pa = activePC.GetComponent<PlayerAbilities>();
   //         Debug.Log(activePC);
            if (pa.spells != null)
            {
                for (int cnt = 0; cnt < pa.spells.Count; cnt++)
                {
                    scrollPosition = GUI.BeginScrollView(new Rect(Screen.width * 0.01f, Screen.height * 0.20f, Screen.width * 0.99f, Screen.height * 0.70f), scrollPosition, new Rect(0, 0, Screen.width * 0.9f, Screen.height * 1.2f));
                    if (GUI.Button(new Rect(0, 0 + (Screen.width * 0.06f * cnt), Screen.width * 0.06f, Screen.width * 0.06f), (Texture2D)(Resources.Load("Icons/Spells/" + pa.spells[cnt]))))
                    {
                        showInfo = true;
                        labelInfo = DialogueLua.GetActorField(pa.spells[cnt], name).AsString;
                        Debug.Log(pa.spells[cnt]); 
                        GetInfo(pa.spells[cnt]);
                    }
                    GUI.Label (new Rect(0 + (Screen.width * 0.08f), 0 + (Screen.width * 0.06f * cnt), Screen.width * 0.20f, Screen.height * 0.07f), DialogueLua.GetActorField (pa.spells[cnt], "name").AsString);
                    GUI.EndScrollView();
                }
            }
        }
        else
        {
            Debug.Log(labelInfo);
            if (GUI.Button(new Rect(Screen.width * 0.03f, (Screen.width * 0.15f), Screen.width * 0.08f, Screen.width * 0.08f), "BACK"))
            {
                showInfo = false;
            }
            GUI.Label(new Rect(Screen.width * 0.20f, (Screen.height * 0.20f), Screen.width * 0.10f, Screen.height * 0.08f), labelInfo);

            scrollPosition = GUI.BeginScrollView(new Rect(Screen.width * 0.01f, Screen.height * 0.30f, Screen.width * 0.99f, Screen.height * 0.55f), scrollPosition, new Rect(0, 0, Screen.width * 0.9f, Screen.height * 1.3f));

            GUI.Label (new Rect(Screen.width * 0.03f, (Screen.height * 0.02f), Screen.width * 0.75f, Screen.height * 0.75f), info);


            GUI.EndScrollView();
        }

    }

    void DisplayAbilities ()
    {
        if (GUI.Button (new Rect(startNameXPos, entropy + (2 * buttonAbility), buttonAbility, buttonAbility), new GUIContent (pa.TextAbi1, pa.TextAbi1.name)))
        {
            Debug.Log(pa.TextAbi1.name);
            abilityActive = ability1;
        }
        if (GUI.Button(new Rect(startNameXPos + buttonAbility, entropy + (2 * buttonAbility), buttonAbility, buttonAbility), new GUIContent(pa.TextAbi2, pa.TextAbi2.name)))
        {
            abilityActive = ability2;
        }
        GUI.Label (new Rect(startNameXPos, entropy + (3 * buttonAbility), Screen.width * 0.75f, Screen.height * 0.8f), abilityActive);


    }

    private void GetInfo (string pathInfo)
    {
        TextAsset path = (TextAsset)(Resources.Load("Text/Spells/" + pathInfo));
        info = path.text;
    }
}
