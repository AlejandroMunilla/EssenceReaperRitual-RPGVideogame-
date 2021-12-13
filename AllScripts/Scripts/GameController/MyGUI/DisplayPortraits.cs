using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;

public class DisplayPortraits : MonoBehaviour {

    private WWW www;
    private GameController gameController;
    private GeneralWindow generalWindow;
    public LevelUp levelUp;
    public Rect portraitWindowRect = new Rect(Screen.width - (Screen.width * 0.10f), 1, (Screen.width * 0.10f), (Screen.height * 0.6f));
    private const int PORTRAIT_WINDOW_ID = 11;
    //**********Level Up***************************
//    public bool levelUp = false;
    private GameObject player;
    private GameObject PCObj2;
    private GameObject PCObj3;
    private GameObject PCObj4;
    private GameObject PCObj5;
    private GameObject dummyPlayer;
    private Texture levelUpImage;
    private Texture healthTexture;
    private Texture manaTexture;
    private Texture mana;
    private Texture framePortrait;
    public Texture levelUpTexture;
    private GUISkin skin;
    private int vitalWidth;
    private int textureX;
    private int healthX;
    private int healthY = 1;
    private int health2Y;
    private int health3Y;
    private int health4Y;
    private int health5Y;
    private int manaY;
    private int mana2Y;
    private int mana3Y;
    private int mana4Y;
    private int mana5Y;
    private int frameY = 1;
    private RtsCameraImp rtsCamera;
    
    // Use this for initialization
    void Start ()
    {
    //    Debug.Log(DialogueLua.GetVariable("Books").AsString);
        dummyPlayer = transform.Find("DummyPlayer").gameObject;
  //      Debug.Log(dummyPlayer);
        gameController = GetComponent<GameController>();
        generalWindow = GetComponent<GeneralWindow>();
        rtsCamera = Camera.main.GetComponent<RtsCameraImp>();
        LoadPortraits();
        int portraitWidth = (int)((Screen.height * 0.6f) / 5);
        portraitWindowRect = new Rect(Screen.width - (Screen.width * 0.10f), 1, portraitWidth, (Screen.height * 0.6f));
        skin = generalWindow.mySkin;
        vitalWidth = (int) (Screen.width * 0.047f);
        healthTexture = (Texture)(Resources.Load("GUI/health"));
        manaTexture = (Texture)(Resources.Load("GUI/mana"));
        levelUpTexture = (Texture)(Resources.Load("GUI/Skull4"));
 //       Debug.Log(transform.Find("LevelUp").gameObject);
        player = gameController.player;
        PCObj2 = gameController.PC2Obj;
        PCObj3 = gameController.PC3Obj;
        PCObj4 = gameController.PC4Obj;
        PCObj5 = gameController.PC5Obj;
        healthX = (int) (Screen.width - (Screen.width * 0.18f));
        textureX = (int)(Screen.width - (Screen.width * 0.13f));
        manaY = (int)(Screen.height * 0.04f);
        health2Y = (int)(Screen.height * 0.12f);
        mana2Y = (int)(Screen.height * 0.16f);
        health3Y = (int)(Screen.height * 0.24f);
        mana3Y = (int)(Screen.height * 0.28f);
        health4Y = (int)(Screen.height * 0.36f);
        mana4Y = (int)(Screen.height * 0.40f);
        health5Y = (int)(Screen.height * 0.48f);
        mana5Y = (int)(Screen.height * 0.52f);
    }

    private void OnEnable()
    {
        Start();
    }

    void OnGUI ()
    {
        GUI.skin = skin;
        portraitWindowRect = GUI.Window(PORTRAIT_WINDOW_ID, portraitWindowRect,
                                  PortraitWindow, "PARTY");
        Vitals();
    }
	
    void PortraitWindow (int id)
	{
        //Player
        #region
        if (GUI.Button(new Rect(1, 1, portraitWindowRect.width, portraitWindowRect.height / 5), generalWindow.img1))
        {
        //          Debug.Log(gameController.abilityInUse);
            if (gameController.abilityInUse == false)
            {


      //          Debug.Log("Hit");
                if (generalWindow.activePCObj == generalWindow.mainPlayer)
                {
                    return;
                }
                
                else if (Event.current.button == 0)
                {
                    bool stateNo = false;
                    foreach (string st in player.GetComponent<PlayerStats>().states)
                    {
                        if (st == "Stun")
                        {
                            stateNo = true;
                        }
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
                        GetComponent<GameController>().ChangeActivePlayer(gameController.player);
                        generalWindow.activePortrait = generalWindow.char1;
                        generalWindow.activePCObj = gameController.player;
                        frameY = 1;
                        GetComponent<GameController>().playerActive = true;
                    }
                }

                if (Event.current.button == 1)
                {
                    if (generalWindow.img1 == levelUpTexture)
                    {
                        levelUp.gameObject.SetActive(true);
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    if (generalWindow.activePortrait != generalWindow.char1)
                    {
                        GetComponent<GameController>().playerActive = !GetComponent<GameController>().playerActive;
                    }
                }
            }
        }

        #endregion

        //PC2
        if (GUI.Button(new Rect(1, portraitWindowRect.height * 0.20f, portraitWindowRect.width, portraitWindowRect.height / 5), generalWindow.img2))
        {
            
            if (gameController.abilityInUse == false && gameController.PC2 != "null")
            {
          //      Debug.Log("Hit2");
                if (generalWindow.activePCObj == generalWindow.PC2Obj)
                {
                    return;
                }
                else
                {
                    generalWindow.activePCObj = generalWindow.PC2Obj;
                }

                //	if (Event.current.button == 0 || activePortrait != char2 )
                if (Event.current.button == 0)
                {

                    if (generalWindow.activePCObj == generalWindow.mainPlayer)
                    {
                        return;
                    }

                    else if (Event.current.button == 0)
                    {
                        bool stateNo = false;
                        foreach (string st in gameController.PC2Obj.GetComponent<PlayerStats>().states)
                        {
                            if (st == "Stun" || st == "Inactive")
                            {
                                stateNo = true;
                            }
                        }

                        if (Event.current.control)
                        {

                        }
                        else if (stateNo == true)
                        {
                            DialogueManager.ShowAlert("Player may not be controlled");
                        }
                        else
                        {
                            GetComponent<GameController>().ChangeActivePlayer(gameController.PC2Obj);
                            GetComponent<GameController>().OnlyOneActive(generalWindow.char2);
                            GetComponent<GameController>().PC2Active = true;
                            generalWindow.activePortrait = generalWindow.char2;
                            generalWindow.activePCObj = gameController.PC2Obj;
                            frameY = (int)(portraitWindowRect.height * 0.20f);
                        }
                    }
                }

                if (Event.current.button == 1)
                {
                    if (generalWindow.img2 == levelUpTexture)
                    {
                        levelUp.gameObject.SetActive(true);
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");


                    if (generalWindow.activePortrait != generalWindow.char2)
                    {
                        GetComponent<GameController>().PC2Active = !GetComponent<GameController>().PC2Active;

                    }
                }
            }

        }

        //PC3
        if (GUI.Button(new Rect(1, portraitWindowRect.height * 0.40f, portraitWindowRect.width, portraitWindowRect.height / 5), generalWindow.img3))
        {
            if (gameController.abilityInUse == false && gameController.PC3 != "null")
            {
                if (generalWindow.activePCObj == gameController.PC3Obj)
                {
                    return;
                }

                else if (Event.current.button == 0 )
                {

                    if (gameController.activePC == gameController.PC3Obj)
                    {
                        return;
                    }

                    bool stateNo = false;
                    foreach (string st in gameController.PC3Obj.GetComponent<PlayerStats>().states)
                    {
                        if (st == "Stun" || st == "Inactive")
                        {
                            stateNo = true;
                        }
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
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC3Obj);
                        //                      Debug.Log(gameController.PC3Obj);
                        GetComponent<GameController>().OnlyOneActive(generalWindow.char3);
                        GetComponent<GameController>().PC3Active = true;
                        generalWindow.activePortrait = generalWindow.char3;
                        generalWindow.activePCObj = generalWindow.PC3Obj = gameController.PC3Obj;
                        gameController.activePC = gameController.PC3Obj;
                        frameY = (int)(portraitWindowRect.height * 0.40f);
                    }
                }

               if (Event.current.button == 1)
                {
                    if (generalWindow.img3 == levelUpTexture)
                    {
                        levelUp.gameObject.SetActive(true);
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");

                    if (generalWindow.activePortrait != generalWindow.char3)
                    {
                        GetComponent<GameController>().PC3Active = !GetComponent<GameController>().PC3Active;

                    }
                }
            }
        }

        //	GUI.Button (new Rect (1, _portraitWindowRect.height * 0.30f ,_portraitWindowRect.width, _portraitWindowRect.height /6 ), img3);
        //PC4
        if (GUI.Button(new Rect(1, portraitWindowRect.height * 0.60f, portraitWindowRect.width, portraitWindowRect.height / 5), generalWindow.img4))
        {
            if (gameController.abilityInUse == false && gameController.PC4 != "null")
            {
                if (generalWindow.activePCObj == gameController.PC4Obj)
                {
                    return;
                }
                //|| generalWindow.activePortrait != generalWindow.char4
                else if (Event.current.button == 0 )
                {
                    if (gameController.activePC == gameController.PC4Obj)
                    {
                        return;
                    }

                    bool stateNo = false;
                    foreach (string st in gameController.PC4Obj.GetComponent<PlayerStats>().states)
                    {
                        if (st == "Stun" || st == "Inactive")
                        {
                            stateNo = true;
                        }
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
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC4Obj);
                        //                      Debug.Log(gameController.PC3Obj);
                        GetComponent<GameController>().OnlyOneActive(generalWindow.char4);
                        GetComponent<GameController>().PC4Active = true;
                        generalWindow.activePortrait = generalWindow.char4;
                        generalWindow.activePCObj = generalWindow.PC4Obj = gameController.PC4Obj;
                        gameController.activePC = gameController.PC4Obj;
                        frameY = (int)(portraitWindowRect.height * 0.60f);
                    }
                }

                if (Event.current.button == 1)
                {
                    if (generalWindow.img4 == levelUpTexture)
                    {
                        levelUp.gameObject.SetActive(true);
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");

                    if (generalWindow.activePortrait != generalWindow.char4)
                    {
                        GetComponent<GameController>().PC4Active = !GetComponent<GameController>().PC4Active;

                    }
                }
            }
        }

        //PC5
        if (GUI.Button(new Rect(1, portraitWindowRect.height * 0.80f, portraitWindowRect.width, portraitWindowRect.height / 5), generalWindow.img5))
        {
            if (gameController.abilityInUse == false && gameController.PC5 != "null")
            {
                if (generalWindow.activePCObj == gameController.PC5Obj)
                {
                    return;
                }

                else if (Event.current.button == 0 )
                {
                    if (gameController.activePC == gameController.PC5Obj)
                    {
                        return;
                    }

                    bool stateNo = false;
                    foreach (string st in gameController.PC5Obj.GetComponent<PlayerStats>().states)
                    {
                        if (st == "Stun" || st == "Inactive")
                        {
                            stateNo = true;
                        }
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
                        GetComponent<GameController>().ChangeActivePlayer(gameController.PC5Obj);
                        GetComponent<GameController>().OnlyOneActive(generalWindow.char5);
                        GetComponent<GameController>().PC5Active = true;
                        generalWindow.activePortrait = generalWindow.char5;
                        generalWindow.activePCObj = generalWindow.PC5Obj = gameController.PC5Obj;
                        frameY = (int)(portraitWindowRect.height * 0.80f);
                        gameController.activePC = gameController.PC5Obj;

                    }
                }
                if (Event.current.button == 1)
                {
                    if (generalWindow.img5 == levelUpTexture)
                    {
                        levelUp.gameObject.SetActive(true);
                    }
                }

                if (Event.current.button == 0 && Event.current.control)
                {
                    Debug.Log("Button 1 and Ctrl");

                    if (generalWindow.activePortrait != generalWindow.char5)
                    {
                        GetComponent<GameController>().PC5Active = !GetComponent<GameController>().PC5Active;

                    }
                }
            }
        }

        GUI.DrawTexture(new Rect(1, frameY, portraitWindowRect.width, portraitWindowRect.height / 5), framePortrait);
    }

    
   public void LoadPortraits()
    {
        string gender = DialogueLua.GetActorField("Player", "gender").AsString;
       string young = DialogueLua.GetActorField("Player", "young").AsString;
        
        //      string young = "No";
        //     Debug.Log(young);

        bool leveledUp =  levelUp.CheckLevelUp("Player", false);
   //     Debug.Log(leveledUp);
        if (leveledUp == true)
        {
            GetComponent<DisplayInfo>().AddText(DialogueLua.GetActorField("Player", "playerName").AsString + ": skill points available!");
        }
        //    Debug.Log(leveledUp);
        if (DialogueLua.GetActorField("Player", "portraitPath").AsString != "Default"  & DialogueLua.GetActorField("Player", "portraitPath").AsString != "")
        {
            generalWindow.img1 = (Texture2D)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString + "Young", typeof(Texture)));
            LoadCustomPortrait();
        }
        else
        {
            if (young == "No")
            {
                generalWindow.img1 = (Texture2D)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString, typeof(Texture)));
            }
            else
            {
                generalWindow.img1 = (Texture2D)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString + "Young", typeof(Texture)));

            }
        }

        generalWindow.char1 = gameController.player.name;
        //     levelUpImage = (Texture)(Resources.Load("Portraits/Narrator", typeof(Texture)));
        generalWindow.char2 = gameController.PC2;
        generalWindow.char3 = gameController.PC3;
        generalWindow.char4 = gameController.PC4;
        generalWindow.char5 = gameController.PC5;
        //        player = gameController.player;
        //	string gender = player.GetComponent<PlayerStats>().gender;
        generalWindow.activePortrait = generalWindow.char1;
        generalWindow.activePCObj = gameController.player;
        

        if (generalWindow.char2 != "null")
        {
            generalWindow.PC2Obj = gameController.PC2Obj;

            leveledUp = levelUp.CheckLevelUp(gameController.PC2, false);
            if (leveledUp == true)
            {
                GetComponent<DisplayInfo>().AddText(gameController.PC2 + ": skill points available!");
            }
      //      Debug.Log(gameController.PC2 + "/" + leveledUp + "/" + gameController.PC2);
            generalWindow.img2 = (Texture)(Resources.Load("Portraits/" + gameController.PC2, typeof(Texture)));


        }
        else
        {
            
            generalWindow.PC2Obj = dummyPlayer;
            generalWindow.img2 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (generalWindow.char3 != "null")
        {
            generalWindow.PC3Obj = gameController.PC3Obj;



            leveledUp = levelUp.CheckLevelUp(gameController.PC3, false);

            if (levelUp == true)
            {
                GetComponent<DisplayInfo>().AddText(gameController.PC3 + ": skill points available!");
            }

            if (gameController.PC3Obj.name == "Preyton")
            {
                if (young == "Yes")
                {
                    generalWindow.img3 = (Texture)(Resources.Load("Portraits/PreytonYoung", typeof(Texture)));
                }
                else
                {
                    generalWindow.img3 = (Texture)(Resources.Load("Portraits/" + gameController.PC3, typeof(Texture)));
                }
            }
            else
            {
                generalWindow.img3 = (Texture)(Resources.Load("Portraits/" + gameController.PC3, typeof(Texture)));
            }

        }
        else
        {
            generalWindow.PC3Obj = dummyPlayer;
            generalWindow.img3 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (generalWindow.char4 != "null")
        {
            generalWindow.PC4Obj = gameController.PC4Obj;

            leveledUp = levelUp.CheckLevelUp(gameController.PC4, false);

            if (levelUp == true)
            {
                GetComponent<DisplayInfo>().AddText(gameController.PC4 + ": skill points available!");
            }
            generalWindow.img4 = (Texture)(Resources.Load("Portraits/" + gameController.PC4, typeof(Texture)));


        }
        else
        {
            generalWindow.PC4Obj = dummyPlayer;
            generalWindow.img4 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        if (generalWindow.char5 != "null")
        {
            generalWindow.PC5Obj = gameController.PC5Obj;

            leveledUp = levelUp.CheckLevelUp(gameController.PC5, false);

            if (levelUp == true)
            {
                GetComponent<DisplayInfo>().AddText(gameController.PC5 + ": skill points available!");
            }

            generalWindow.img5 = (Texture)(Resources.Load("Portraits/" + gameController.PC5, typeof(Texture)));

        }
        else
        {
            generalWindow.PC5Obj = dummyPlayer;
            generalWindow.img5 = (Texture)(Resources.Load("Portraits/Empty", typeof(Texture)));
        }

        framePortrait = (Texture)(Resources.Load("Portraits/FramePortrait", typeof(Texture)));

    }
    
    void Vitals ()
    {
        GUI.DrawTexture(new Rect(textureX, healthY, vitalWidth, vitalWidth ), healthTexture);
        GUI.Label (new Rect(healthX, healthY, vitalWidth, vitalWidth), player.GetComponent<PlayerStats>().curHealth.ToString() + "/" + player.GetComponent<PlayerStats>().totHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, manaY, vitalWidth, vitalWidth), manaTexture);
        GUI.Label(new Rect(healthX, manaY, vitalWidth, vitalWidth), player.GetComponent<PlayerStats>().curMana.ToString() + "/" + player.GetComponent<PlayerStats>().totMana.ToString());

    //    Debug.Log(PCObj3.name + "/" + PCObj3.GetComponent<PlayerStats>().curHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, health2Y , vitalWidth, vitalWidth), healthTexture);
        GUI.Label(new Rect(healthX, health2Y, vitalWidth, vitalWidth), gameController.PC2Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PCObj2.GetComponent<PlayerStats>().totHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, mana2Y, vitalWidth, vitalWidth), manaTexture);
        GUI.Label(new Rect(healthX, mana2Y, vitalWidth, vitalWidth), gameController.PC2Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PCObj2.GetComponent<PlayerStats>().totMana.ToString());

        GUI.DrawTexture(new Rect(textureX, health3Y, vitalWidth, vitalWidth), healthTexture);
        GUI.Label(new Rect(healthX, health3Y, vitalWidth, vitalWidth), gameController.PC3Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PCObj3.GetComponent<PlayerStats>().totHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, mana3Y, vitalWidth, vitalWidth), manaTexture);
        GUI.Label(new Rect(healthX, mana3Y, vitalWidth, vitalWidth), gameController.PC3Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PCObj3.GetComponent<PlayerStats>().totMana.ToString());

        GUI.DrawTexture(new Rect(textureX, health4Y, vitalWidth, vitalWidth), healthTexture);
        GUI.Label(new Rect(healthX, health4Y, vitalWidth, vitalWidth), gameController.PC4Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PCObj4.GetComponent<PlayerStats>().totHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, mana4Y, vitalWidth, vitalWidth), manaTexture);
        GUI.Label(new Rect(healthX, mana4Y, vitalWidth, vitalWidth), gameController.PC4Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PCObj4.GetComponent<PlayerStats>().totMana.ToString());

        GUI.DrawTexture(new Rect(textureX, health5Y, vitalWidth, vitalWidth), healthTexture);
        GUI.Label(new Rect(healthX, health5Y, vitalWidth, vitalWidth), gameController.PC5Obj.GetComponent<PlayerStats>().curHealth.ToString() + "/" + PCObj5.GetComponent<PlayerStats>().totHealth.ToString());
        GUI.DrawTexture(new Rect(textureX, mana5Y, vitalWidth, vitalWidth), manaTexture);
        GUI.Label(new Rect(healthX, mana5Y, vitalWidth, vitalWidth), gameController.PC5Obj.GetComponent<PlayerStats>().curMana.ToString() + "/" + PCObj5.GetComponent<PlayerStats>().totMana.ToString());

    }

    public void UpdatePCs ()
    {      
        if (PCObj2 == null)
        {
            PCObj2 = dummyPlayer;
        }
        else
        {
            PCObj2 = gameController.PC2Obj;
        }
        if (PCObj3 == null)
        {
            PCObj3 = dummyPlayer;
        }
        else
        {
            PCObj3 = gameController.PC3Obj;
        }
        if (PCObj4 == null)
        {
            PCObj4 = dummyPlayer;
        }
        else
        {
            PCObj4 = gameController.PC4Obj;
        }
        if (PCObj5 == null)
        {
            PCObj5 = dummyPlayer;
        }
        else
        {
            PCObj5 = gameController.PC5Obj;
        } 
        
    }

    public void ChangeToDead (string dead)
    {
  //      Debug.Log(dead);
        if (dead == "Player")
        {
            string gender = DialogueLua.GetActorField("Player", "gender").AsString;
            string young = DialogueLua.GetActorField("Player", "young").AsString;
            string path = "Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString + "Dead";
         //   Debug.Log(path);
            generalWindow.img1 = (Texture2D)(Resources.Load(path, typeof(Texture)));

        }
        else if (generalWindow.img2.name == dead)
        {
            generalWindow.img2 = (Texture)(Resources.Load("Portraits/" + gameController.PC2 + "Dead", typeof(Texture)));
        }
        else if (generalWindow.img2.name == dead)
        {
            generalWindow.img3 = (Texture)(Resources.Load("Portraits/" + gameController.PC2 + "Dead", typeof(Texture)));
        }
        else if (generalWindow.img2.name == dead)
        {
            generalWindow.img4 = (Texture)(Resources.Load("Portraits/" + gameController.PC2 + "Dead", typeof(Texture)));
        }
        else if (generalWindow.img2.name == dead)
        {
            generalWindow.img5 = (Texture)(Resources.Load("Portraits/" + gameController.PC2 + "Dead", typeof(Texture)));
        }
    }

    public void ChangeToAlive (string dead)
    {
   //     Debug.Log(dead);
        if (generalWindow.img1.name == dead)
        {
            string gender = DialogueLua.GetActorField("Player", "gender").AsString;
            string young = DialogueLua.GetActorField("Player", "young").AsString;
            generalWindow.img1 = (Texture2D)(Resources.Load("Portraits/Player/Human/" + DialogueLua.GetActorField("Player", "gender").AsString, typeof(Texture)));

        }
        else if (generalWindow.img2.name == dead)
        {
            generalWindow.img2 = (Texture)(Resources.Load("Portraits/" + gameController.PC2, typeof(Texture)));
        }
        else if (generalWindow.img3.name == dead)
        {
            generalWindow.img3 = (Texture)(Resources.Load("Portraits/" + gameController.PC3, typeof(Texture)));
        }
        else if (generalWindow.img4.name == dead)
        {
            generalWindow.img4 = (Texture)(Resources.Load("Portraits/" + gameController.PC4, typeof(Texture)));
        }
        else if (generalWindow.img5.name == dead)
        {
            generalWindow.img5 = (Texture)(Resources.Load("Portraits/" + gameController.PC5, typeof(Texture)));
        }
    }


    /*
    public bool CheckLevelUp (string character, bool levelUp)
    {
        int level = DialogueLua.GetActorField("Player", "level").AsInt;
        int spentExpPoints = DialogueLua.GetActorField(character, "spentExpPoints").AsInt;
        int spentThiefPoints = DialogueLua.GetActorField(character, "spentThiefPoints").AsInt;
        int expPointsTotal = (DialogueLua.GetActorField(character, "battleSkillPoints").AsInt * level) +  DialogueLua.GetActorField(character, "expPointsBuff").AsInt - DialogueLua.GetActorField(character, "battleSkillPoints").AsInt;
        int thiefPointsTotal = (DialogueLua.GetActorField(character, "thiefSkillPoints").AsInt * level) + DialogueLua.GetActorField(character, "thiefPointsBuff").AsInt - DialogueLua.GetActorField(character, "thiefSkillPoints").AsInt;
        Debug.Log(character + "/" + level + "/" + expPointsTotal + "/" + spentExpPoints);
        if (spentExpPoints < expPointsTotal)
        {
            levelUp = true; 
        }

        return levelUp;

        //      if (expPoints )
    }*/



    public void LevelUpMember (GameObject member)
    {
        /*
        if (member == player)
        {
            generalWindow.img1 = levelUpTexture;
        }
        else if (member == PCObj2)
        {
            generalWindow.img2 = levelUpTexture;
        }
        else if (member == PCObj3)
        {
            generalWindow.img3 = levelUpTexture;
        }
        else if (member == PCObj4)
        {
            generalWindow.img4 = levelUpTexture;

        }
        else if (member == PCObj5)
        {
            generalWindow.img5 = levelUpTexture;

        }*/
    }

    private void LoadCustomPortrait()
    {
        string mySaveDirectory = DialogueLua.GetActorField("Player", "portraitPath").AsString;
        www = new WWW(mySaveDirectory);

        StartCoroutine("LoadTexture");
    }
    private IEnumerator LoadTexture()
    {
        while (!www.isDone)
            yield return null;
        StopCoroutine("LoadTexture");
        generalWindow.img1 = www.texture;
        generalWindow.img1.name = "Player";

        //    DialogueManager.ConversationModel.ActorInfo.Name = "Player";
        //     DialogueManager.ConversationModel.ActorInfo.portrait = www.texture;
        //     DialogueLua.SetActorField("Player", "Pictures", www.texture);

        //    DialogueManager.SetPortrait("Player", "portraitPath");
    }



}
