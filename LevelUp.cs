using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

public class LevelUp : MonoBehaviour {
    private string _namePlayer;
    private ExpController expController;
    private GameController gameController;
    private CharSkills cs;
    private PlayerStats ps;
    private GUISkin mySkin;
    private GameObject activePC;
    private GameObject gc;
    private Camera mainCamera;
    private Texture backgroundTexture;
    private Texture portrait;
    private Texture rightButtonTexture;
    private Texture leftButtonTexture;
    private bool loaded = false;
    private int exp;
    private int spentExpPoints = 0;
    private int spentThiefPoints = 0;
    private Vector2 scrollPosition = Vector2.zero;
    private List<string> pcs = new List<string>();
    public int weaponsPointsLeft = 0;
    public int thiefPointsLeft = 0;

    /*
    private void Start()
    {
        OnEnable();
    }*/

    void OnEnable ()
    {
        spentExpPoints = 0;
        spentThiefPoints = 0;
        if (loaded == false)
        {
            gc = transform.root.gameObject;
            expController = gc.GetComponent<ExpController>();
            gameController = gc.GetComponent<GameController>();
            mainCamera = Camera.main;
    //        Debug.Log(gc);
            activePC = gameController.player;
            activePC = gc.GetComponent<DisplayCharacter>().activePC;
            _namePlayer = activePC.name;
            cs = GetComponent<CharSkills>();
            cs.UpDateRects();
            //         int weaponBase = DialogueLua.GetActorField (activePC.name, )
            //        mySkin = (GUISkin)(Resources.Load("GUI/Fantasy", typeof(GUISkin)));
            mySkin = gc.GetComponent<GeneralWindow>().mySkin;
            backgroundTexture = (Texture)(Resources.Load("GUI/Bigboard", typeof(Texture)));
            leftButtonTexture = (Texture)(Resources.Load("GUI/Left button", typeof(Texture)));
            rightButtonTexture = (Texture)(Resources.Load("GUI/Right button", typeof(Texture)));

            loaded = true;
        }
        UpdatePCs();
        UpdateSkills();
        gameController.Pause();
        gc.GetComponent<DisplayPortraits>().enabled = false;
        gc.GetComponent<PlayerControls>().CheckToggle();
        
    }


    private void OnDisable()
    {
        gameController.Continue();
        gc.GetComponent<DisplayPortraits>().enabled = true;
        gc.GetComponent<PlayerControls>().CheckToggle();
        gameController.ChangeActivePlayer(gameController.player);
        

    }

    void OnGUI ()
    {
        GUI.skin = mySkin;
        Background();
        GUI.skin.button.wordWrap = true;
        DisplayPC();
        if (GUI.Button(new Rect(Screen.width * 0.75f, Screen.height * 0.01f, Screen.width * 0.15f, Screen.height * 0.09f), "EXIT"))
        {
            SaveAndExit();
        }
        scrollPosition = GUI.BeginScrollView(new Rect(Screen.width * 0.01f, Screen.height * 0.20f, Screen.width * 0.99f, Screen.height * 0.70f), scrollPosition, new Rect(0, 0, Screen.width * 0.9f, Screen.height * 1.2f));
        cs.CombatSkills();
        cs.ThiefSkills();
        GUI.EndScrollView();
    }

    void Background ()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    }

    void DisplayPC ()
    {
        GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.01f, Screen.width * 0.10f, Screen.width * 0.10f), portrait);
        if (GUI.Button(new Rect(Screen.width * 0.35f, Screen.height * 0.06f, Screen.width * 0.10f, Screen.height * 0.05f), leftButtonTexture))
        {
            ChangePC(false);
        }
        
        if (GUI.Button(new Rect(Screen.width * 0.55f, Screen.height * 0.06f, Screen.width * 0.10f, Screen.height * 0.05f), rightButtonTexture))
        {
            ChangePC(true);
        }
    }

    void UpdateSkills ()
    {        
        if (_namePlayer == "Player")
        {
            string gender = DialogueLua.GetActorField("Player", "gender").AsString;
            string young = DialogueLua.GetActorField("Player", "young").AsString;
            //      string young = "No";
            //     Debug.Log(young);
            if (young == "No")
            {
                portrait = (Texture)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString, typeof(Texture)));
            }
            else
            {
                portrait = (Texture)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString + "Young", typeof(Texture)));

            }
        }
        else
        {
            portrait = (Texture)(Resources.Load("Portraits/" + _namePlayer, typeof(Texture)));
        }
        
        cs.gender = DialogueLua.GetActorField(_namePlayer, "gender").AsString;    
        cs.race = DialogueLua.GetActorField(_namePlayer, "race").AsString;
        cs.profession = DialogueLua.GetActorField(_namePlayer, "profession").AsString;
        cs.axeSkill = DialogueLua.GetActorField(_namePlayer, "axeSkill").AsInt;                  cs.axeInitial = cs.axeSkill;
        cs.bowSkill = DialogueLua.GetActorField(_namePlayer, "bowSkill").AsInt;                     cs.bowInitial = cs.bowSkill;
        cs.crossbowSkill = DialogueLua.GetActorField(_namePlayer, "crossbowSkill").AsInt;           cs.crossbowInitial = cs.crossbowSkill;
        cs.knifeSkill = DialogueLua.GetActorField(_namePlayer, "knifeSkill").AsInt;                 cs.knifeInitial = cs.knifeSkill;
        cs.lanceSkill = DialogueLua.GetActorField(_namePlayer, "lanceSkill").AsInt;                 cs.lanceInitial = cs.lanceSkill;
        cs.bluntSkill = DialogueLua.GetActorField(_namePlayer, "bluntSkill").AsInt;                 cs.bluntInitial = cs.bluntSkill;
        cs.scyntheSkill = DialogueLua.GetActorField(_namePlayer, "scyntheSkill").AsInt;             cs.scyntheInitial = cs.scyntheSkill;
        cs.slingSkill = DialogueLua.GetActorField(_namePlayer, "slingSkill").AsInt;                 cs.slingInitial = cs.slingSkill;
        cs.stickSkill = DialogueLua.GetActorField(_namePlayer, "stickSkill").AsInt;                 cs.stickInitial = cs.stickSkill;
        cs.swordSkill = DialogueLua.GetActorField(_namePlayer, "swordSkill").AsInt;                 cs.swordInitial = cs.swordSkill;
        cs.throwingSkill = DialogueLua.GetActorField(_namePlayer, "throwingSkill").AsInt;           cs.throwingInitial = cs.throwingSkill;
        cs.greatWeaponSkill = DialogueLua.GetActorField(_namePlayer, "greatWeaponSkill").AsInt;     cs.greatWeaponInitial = cs.greatWeaponSkill;
        cs.weaponAndShieldSkill = DialogueLua.GetActorField(_namePlayer, "weaponAndShieldSkill").AsInt;         cs.weaponAndShieldInitial = cs.weaponAndShieldSkill;
        cs.doubleWeaponSkill = DialogueLua.GetActorField(_namePlayer, "doubleWeaponSkill").AsInt;               cs.doubleWeaponInitial = cs.doubleWeaponSkill;
        cs.unarmedSkill = DialogueLua.GetActorField(_namePlayer, "unarmedSkill").AsInt;                         cs.unarmedInitial = cs.unarmedSkill;
        cs.arcabuzSkill = DialogueLua.GetActorField(_namePlayer, "arcabuzSkill").AsInt;             cs.arcabuzInitial = cs.arcabuzSkill;
        cs.katanaSkill = DialogueLua.GetActorField(_namePlayer, "katanaSkill").AsInt;               cs.katanaInitial = cs.katanaSkill;
        cs.detectTrampsSkill = DialogueLua.GetActorField(_namePlayer, "detectTrampSkill").AsInt;    cs.detectTrampsInitial = cs.detectTrampsSkill;
        cs.trampAbilitySkill = DialogueLua.GetActorField(_namePlayer, "trampAbilitySkill").AsInt;   cs.trampAbilityInitial = cs.trampAbilitySkill;
        cs.hideSkill = DialogueLua.GetActorField(_namePlayer, "hideSkill").AsInt;                   cs.hideInitial = cs.hideSkill;
        cs.backstabSkill = DialogueLua.GetActorField(_namePlayer, "backstabSkill").AsInt;           cs.backstabInitial = cs.backstabSkill;
        cs.restrictionBase = DialogueLua.GetActorField(_namePlayer, "restrictionBase").AsInt;
        cs.restrictionLevel = DialogueLua.GetActorField(_namePlayer, "restrictionLevel").AsInt;
        cs.level = DialogueLua.GetActorField(_namePlayer, "level").AsInt;        
        cs.restrictionThiefBase = DialogueLua.GetActorField(_namePlayer, "restrictionThiefBase").AsInt;
        cs.restrictionThiefLevel = DialogueLua.GetActorField(_namePlayer, "restrictionThielLevel").AsInt;
        CheckLevelUp(_namePlayer, false);
        cs.weaponsPointsLeft = weaponsPointsLeft;
        cs.thiefPointsLeft = thiefPointsLeft;


       Debug.Log(weaponsPointsLeft);
    //    bool noLeveledUp = CheckLevelUp (_namePlayer, false)
    }

    void UpdatePCs ()
    {
        pcs.Clear();
        string listMembers = DialogueLua.GetVariable("listMembers").AsString;
        //     string listMembers = "Oleg";
        string listInParty = DialogueLua.GetVariable("PCList").AsString;
    //    Debug.Log(listInParty);
        string[] arrayMembers = listMembers.Split(new string[] { "*" }, System.StringSplitOptions.None);
        string[] arrayInParty = listInParty.Split(new string[] { "*" }, System.StringSplitOptions.None);

        pcs.Add("Player");

        foreach (string st in arrayInParty)
        {
            if (st != "null" && st != "")
            {
                if (st != "Player")
                {
                    pcs.Add(st);
                }
            }
    //              Debug.Log(st);


        }

        if (arrayMembers.Length > 0)
        {
            if (arrayMembers.Length == 1 && arrayMembers[0] == "null")
            {

            }
            else
            {
                for (int cnt = 0; cnt < arrayMembers.Length; cnt++)
                {
                    if (arrayMembers[cnt] != "null")
                    {
                        if (arrayMembers[cnt] == "Player" || arrayMembers[cnt] == "Dylan")
                        {

                        }
                        else
                        {
                            pcs.Add(arrayMembers[cnt]);
                        }
                    }
                }
            }
        }
    }

    public bool CheckLevelUp(string character, bool levelUp)
    {
        int level = DialogueLua.GetActorField("Player", "level").AsInt;
        if (level <1)
        {
            level = 1;
        }
        int levelCharacter = DialogueLua.GetActorField(character, "level").AsInt;

        if (levelCharacter < level)
        {
            DialogueLua.SetActorField(character, "level", level);
        }

        int levelHealUpdate = DialogueLua.GetActorField(_namePlayer, "levelHealUpdate").AsInt;
        int healAdded = DialogueLua.GetActorField(_namePlayer, "healAdded").AsInt;
        
    //    Debug.Log(level + "/" + levelHealUpdate);
        if (level > levelHealUpdate)
        {
            Invoke("UpdateHeal", 0.1f);
        }


        int spentExpPoints = DialogueLua.GetActorField(character, "spentExpPoints").AsInt;
        int spentThiefPoints = DialogueLua.GetActorField(character, "spentThiefPoints").AsInt;
        if (DialogueLua.GetActorField(character, "battleSkillPoints").AsInt == 0)
        {
            Debug.LogError(character + " battleSkillPoints not set up");
        }
        if (gc == null)
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
        }
        gc.GetComponent<ExpController>();
        int trueLevel = gc.GetComponent<ExpController>().trueLevel;

        int expPointsTotal = (DialogueLua.GetActorField(character, "battleSkillPoints").AsInt * trueLevel) + DialogueLua.GetActorField(character, "expPointsBuff").AsInt - DialogueLua.GetActorField(character, "battleSkillPoints").AsInt;
        int thiefPointsTotal = (DialogueLua.GetActorField(character, "thiefSkillPoints").AsInt * trueLevel) + DialogueLua.GetActorField(character, "thiefPointsBuff").AsInt - DialogueLua.GetActorField(character, "thiefSkillPoints").AsInt;

        
        weaponsPointsLeft = expPointsTotal - spentExpPoints;
        thiefPointsLeft = thiefPointsTotal - spentThiefPoints;
    //    Debug.Log(spentExpPoints + "/" + expPointsTotal + "/" +  weaponsPointsLeft + "/" + thiefPointsLeft);
            

        if (weaponsPointsLeft > 0)
        {
            levelUp = true;
    //        Debug.Log(weaponsPointsLeft + "/" + character + "/" + expPointsTotal + "/" + spentExpPoints);
        }
        return levelUp;        
    }  

    private void ChangePC (bool right)
    {
        SaveAndChange();
    //    bool next = false;

        if (right == true)
        {
            int cntTemp = 1000;
            string pcTemp = _namePlayer;
            for (int cnt = 0; cnt < pcs.Count; cnt++)
            {
                if (pcs[cnt] == pcTemp)
                {
                    cntTemp = cnt + 1;
                }
            }

            if (cntTemp >= pcs.Count)
            {
                _namePlayer = gameController.player.name;
            }
            else
            {
                _namePlayer = pcs[cntTemp];
            }
        }
        else
        {
            int cntTemp = 1000;
            string pcTemp = _namePlayer;
            for (int cnt = 0; cnt < pcs.Count; cnt++)
            {
                if (pcs[cnt] == pcTemp)
                {
                    cntTemp = cnt - 1;
                }
            }

            if (cntTemp < 0)
            {

                _namePlayer = pcs[pcs.Count - 1];
            }
            else
            {
                _namePlayer = pcs[cntTemp];
            }
        }



        UpdateSkills();
    }

    private void SaveAndChange ()
    {
        DialogueLua.SetActorField(_namePlayer, "axeSkill", cs.axeSkill);
        DialogueLua.SetActorField(_namePlayer, "bowSkill", cs.bowSkill);
        DialogueLua.SetActorField(_namePlayer, "crossbowSkill", cs.crossbowSkill);
        DialogueLua.SetActorField(_namePlayer, "knifeSkill", cs.knifeSkill);
        DialogueLua.SetActorField(_namePlayer, "lanceSkill", cs.lanceSkill);
        DialogueLua.SetActorField(_namePlayer, "bluntSkill", cs.bluntSkill);
        DialogueLua.SetActorField(_namePlayer, "scyntheSkill", cs.scyntheSkill);
        DialogueLua.SetActorField(_namePlayer, "slingSkill", cs.slingSkill);
        DialogueLua.SetActorField(_namePlayer, "stickSkill", cs.stickSkill);
        DialogueLua.SetActorField(_namePlayer, "swordSkill", cs.swordSkill);
        DialogueLua.SetActorField(_namePlayer, "throwingSkill", cs.throwingSkill);
        DialogueLua.SetActorField(_namePlayer, "greatWeaponSkill", cs.greatWeaponSkill);
        DialogueLua.SetActorField(_namePlayer, "weaponAndShieldSkill", cs.weaponAndShieldSkill);
        DialogueLua.SetActorField(_namePlayer, "doubleWeaponSkill", cs.doubleWeaponSkill);
        DialogueLua.SetActorField(_namePlayer, "unarmedSkill", cs.unarmedSkill);
        DialogueLua.SetActorField(_namePlayer, "arcabuzSkill", cs.arcabuzSkill);
        DialogueLua.SetActorField(_namePlayer, "katanaSkill", cs.katanaSkill);
        DialogueLua.SetActorField(_namePlayer, "detectTrampSkill", cs.detectTrampsSkill);
        DialogueLua.SetActorField(_namePlayer, "trampAbilitySkill", cs.trampAbilitySkill);
        DialogueLua.SetActorField(_namePlayer, "hideSkill", cs.hideSkill);
        DialogueLua.SetActorField(_namePlayer, "backstabSkill", cs.backstabSkill);
        int tempExpPointsLeft = cs.weaponsPointsLeft;
        int tempThiefPointsLeft = cs.thiefPointsLeft;
        CheckLevelUp(_namePlayer, false);
        int spentExpPointsToSave = weaponsPointsLeft - tempExpPointsLeft;
        int spentExpPoints = DialogueLua.GetActorField(_namePlayer, "spentExpPoints").AsInt;
        spentExpPointsToSave = spentExpPoints + spentExpPointsToSave;
        DialogueLua.SetActorField(_namePlayer, "spentExpPoints", spentExpPointsToSave );

        int spentThiefPointsToSave = thiefPointsLeft - tempThiefPointsLeft;
        int spentThiefPoints = DialogueLua.GetActorField(_namePlayer, "spentThiefPoints").AsInt;
        spentThiefPointsToSave = spentThiefPoints + spentThiefPointsToSave;
        DialogueLua.SetActorField(_namePlayer, "spentThiefPoints", spentThiefPointsToSave);


   //     Debug.Log(_namePlayer);
        activePC.GetComponent<PlayerStats>().LoadLua();
        activePC.GetComponent<PlayerStats>().CalculateEverything();

     //   this.enabled = false;
        //    DialogueLua.SetActorField(_namePlayer, "spentExpPoints", cs.weaponsPointsLeft);
        //    DialogueLua.SetActorField(_namePlayer, "thiefSkillPointsSaved", cs.thiefPointsLeft);
    }

    private void SaveAndExit()
    {
        SaveAndChange();
        
        gameObject.SetActive(false);
        Invoke("UpdateSkillPoints", 0);

        //    DialogueLua.SetActorField(_namePlayer, "spentExpPoints", cs.weaponsPointsLeft);
        //    DialogueLua.SetActorField(_namePlayer, "thiefSkillPointsSaved", cs.thiefPointsLeft);
    }

    private void UpdateSkillPoints ()
    {
        string activePCDisplayCharacter = gc.GetComponent<DisplayCharacter>().activePC.name;
        bool skillPointsLeft = CheckLevelUp(activePCDisplayCharacter, false);
        if (skillPointsLeft == false)
        {
            gc.GetComponent<DisplayCharacter>().skillPoints = skillPointsLeft;
        }
    }


    /*
    private void UpdateTotalHealth (string character, int amountToSum)
    {
        int diceRoll = Random.Range(1, 4) + amountToSum;
        
        int healAdded =  DialogueLua.GetActorField(character, "healAdded").AsInt;
        healAdded = healAdded + diceRoll;
        Debug.Log(character + "/" + diceRoll + "/" + amountToSum + "/" + healAdded);
        DialogueLua.SetActorField(character, "healAdded", healAdded);
        int constitution = DialogueLua.GetActorField(character, "level").AsInt;
        int totalHeal = constitution + healAdded;
        Debug.Log(totalHeal);
        DialogueLua.SetActorField(character, "curHealth", totalHeal );
        DialogueLua.SetActorField(character, "heal", totalHeal);

    }*/

    

}
