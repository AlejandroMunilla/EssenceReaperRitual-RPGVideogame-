using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class DisplayItemScript : MonoBehaviour
{
    #region
    public GUIStyle myStyle;
    private bool dataLoaded = false;
    private bool buttonSelected = false;
    private bool buttonSelected2 = false;
    private string buttonType;
    private string pathChild;
    private string itemSelected;
    private string itemOrigin;
    private string weaponDisplay;
    private int itemPos;
    private int tempCNT;
    private int buttonWidth = 40;
    private int buttonHeight = 40;
    private int _offset = 10;
    private int windowLenght;
    public string[] description;
    private GameObject activePCObj;
    private GameObject player;
    private GameObject activePlayer;
    private ItemManager itemManager;
    private GameController gameController;
    private GeneralWindow generalWindow;
    //	private MyGUI myGUI;
    private Texture button;
    private Texture slots;
    public List<Item2> _lootItems;
    public List<string> inventoryItems = new List<string>();
    //	public Hashtable lootItems;


    //*************************************************
    //              EQUIPPED ITEMS
    //*************************************************
    private bool existItemEquipped = false;
    private bool displayInfoItem = false;
    public bool dalilaEquipped = false;
    public bool dalilaLeftEquipped;
    private int equippedButtonWidth = 45;
    private int equippedButtonHeight = 45;
    private int headYPos; private int headXPos;
    private int neckYPos; private int neckXPos;
    private int breastYPos; private int breastXpos;
    private int armsYPos; private int armsXPos;
    private int waistYPos; private int waistXPos;
    private int legsYPos; private int legsXPos;
    private int fingerYPos; private int fingerXPos;
    private int fingerYPos2; private int fingerXPos2;
    private int rightHandY; private int rightHandX;
    private int leftHandY; private int leftHandX;
    private List<Item2> equippedItems;
    List<EquippedItems> equipped = new List<EquippedItems>();
    List<Item2> equippedWithObj = new List<Item2>();
    public string[] descriptionSpecial;
    public string dalilaHand = "RHand";
    private Texture2D armour;

    //***********************************************
    //  		 GENERAL INVENTORY WINDOW
    //***********************************************
    private bool weaponsBool = true;
    private bool potionBool = false;
    private bool miscBool = false;
    private string[] miscInventoryArray = null;
    private string[] genPanelNames = new string[]
{
    "Weapons",
    "Potions",
    "Misc"
};
    private const int INVENTORYALL_WINDOW_ID = 13;
    private Rect genInventoryRect;
    private int genInventoryPanel = 0;         //we need to assgin a number to the switch panel
    private Rect _ItemDesWindowRect = new Rect(1, 1, 400, 1000);  // Change it as % of screen
    public GUISkin skin;
    public GUISkin mySkin;
    private GameObject inventory;
    private const int INVENTORY_WINDOW_ID = 12;
    private Rect _InventoryWindowRect = new Rect(10, 10, 250, 200);
    private bool nullGenInventory = false;
    private bool nullMiscInventory = false;
    private Vector2 _InventoryWindowSlider = Vector2.zero;
    private Vector2 _ItemDesSlider = Vector2.zero;
    private int _inventoryRows = 50;
    private int _inventoryCols = 1;
    private int inventoryButtonWidth;
    private int inventoryButtonHeight = 25;
    private int inventoryWindowXStart;
    private int inventoryWindowXStart2;
    private int inventoryWindowYStart;
    private int inventoryWindowWidth;
    private int inventoryWindowHeight;
    //    private int chestX;
    private int chestY;
    //   private int chestWidth;
    private Rect inventoryWindownSliderRect;
    private Rect itemDescripSliderRect;
    private ItemManager infoObject;
    private Texture chestTexture;
    private Texture black;

    //Variables for Inventory Rect when General Window is not open, only when loot window is open to allow us 
    // to loot and place back items from the inventory to Chests to store them there. 
    private int inventoryLootXStart;
    private int inventoryLootYStart;
    private string[] arrayInventory;
    private TextAsset textAssetInventory;
    private string valueInventory;
    private string[] arrayStringInventory;
    public string[] arrayInfoItem;
    private Rect InventoryRect;
    private bool readInventory = false;
    private Vector2 InventoryRectSlider;
    private DisplayCharacter displayChar;

    //****************************************************************************
    // *****************  DISPLAY SPECIAL PROPERTIES *****************************
    //****************************************************************************
    public string displayPath;
    private string displayPath2;
    private Texture background;


    //QUEST ITEMS VARIABLE
    //	private int questItemYPos;
    public List<string> arrayQuestItems = new List<string>();
    public TextAsset textAsset;
    public string textAssetString;
    private string value;
    private string[] arrayString;
    private Rect questItemsRect;
    public bool readQuestItem = false;
    public bool nullQuestItem = false;
    private Vector2 questItemRectSlider;
    private int posX;
    private int posY;
    private int questItemsWidth;
    private int questItemsHeight;
    private int questXPos;
    private int questYPos;
    private Texture2D textureToDisplay;
    #endregion


    //POTIONS
    private string[] potionsNames = new string[]
    {
        "15000",
        "15002"
    };
    public List<string> potions = new List<string>();
    public List<string> miscItemsList = new List<string>();
    public bool textureCursor;
    public Texture2D textureToQuickItems;

    //COUROTINE FOR CURSOR TEXTURE CHANGE
    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04,
        Seq05
    }
    private State state;

    public void Start()
    {
        itemManager = GetComponent<ItemManager>();
        generalWindow = GetComponent<GeneralWindow>();
        inventoryButtonWidth = (int)(Screen.width - (Screen.width * 0.8f));
  /*      inventoryWindowXStart = (int)(Screen.width * 0.45f);
        inventoryWindowXStart2 = (int)(Screen.width * 0.50f);
        inventoryWindowYStart = (int)(Screen.height - (Screen.height * 0.86f));
        inventoryWindowWidth = (int)(Screen.width * 0.44f);
        inventoryWindowHeight = (int)(Screen.height - (Screen.height * 0.25f));
        inventoryWindownSliderRect = new Rect(0, 0, buttonWidth, (buttonWidth * 53) + _offset);*/
        inventoryWindowXStart = (int)(Screen.width * 0.45f);
        inventoryWindowXStart2 = (int)(Screen.width * 0.02f);
        inventoryWindowYStart = (int)(Screen.height *0.18f);
        inventoryWindowWidth = (int)(Screen.width * 0.44f);
        inventoryWindowHeight = (int)(Screen.height * 0.50f);

        inventoryWindownSliderRect = new Rect(0, 0, buttonWidth, (equippedButtonWidth * inventoryItems.Count/ 6) + equippedButtonWidth);
        itemDescripSliderRect = new Rect(0, 0, 1000, 600);
        inventoryLootXStart = (int)(Screen.width - (Screen.width * 0.49f));
        inventoryLootYStart = (int)(Screen.height - (Screen.height * 0.85f));
        equippedButtonHeight = (int)(Screen.height * 0.10f);  //just added test
        equippedButtonWidth = equippedButtonHeight;
        headXPos = 0;
        headYPos = (int)(Screen.height * 0.15f);
        breastYPos = headYPos + equippedButtonHeight; breastXpos = headXPos;
        rightHandY = breastYPos + equippedButtonHeight; rightHandX = headXPos;
        waistYPos = rightHandY + equippedButtonHeight; waistXPos = headXPos;
        fingerYPos = waistYPos + equippedButtonHeight; fingerXPos = headXPos;
        neckYPos = headYPos;
        neckXPos = (int)(Screen.width * 0.33f);
        armsYPos = neckYPos + equippedButtonHeight; armsXPos = neckXPos;
        leftHandY = armsYPos + equippedButtonHeight; leftHandX = neckXPos;
        legsYPos = leftHandY + equippedButtonHeight; legsXPos = neckXPos;
        fingerYPos2 = legsYPos + equippedButtonHeight; fingerXPos2 = neckXPos;
        chestY = fingerYPos + equippedButtonHeight;
        //        questItemYPos = (int) (Screen.height * 0.2f);
        gameController = GetComponent<GameController>();
        player = gameController.player;
        activePCObj = gameController.activePC;
        buttonWidth = (int)(Screen.width * 0.10f);
        //	button = (Texture)(Resources.Load ("GUI/Button", typeof(Texture)));
        //	skin = (GUISkin)(Resources.Load ("GUI/Skin", typeof(GUISkin)));
        slots = (Texture)(Resources.Load("GUI/Slots", typeof(Texture)));
        background = (Texture)(Resources.Load("GUI/Wall", typeof(Texture)));
        chestTexture = (Texture)(Resources.Load("GUI/Chest", typeof(Texture)));
        black = (Texture)(Resources.Load("Textures/Black", typeof(Texture)));
        armour = (Texture2D)(Resources.Load("GUI/armour", typeof(Texture2D)));
        //     myGUI = GetComponent<MyGUI>();
        questItemsRect = new Rect(0, 0, 1, 1000);
        posX = (int)(Screen.width * 0.05f);
        posY = (int)(Screen.width * 0.20f);
        questItemsWidth = (int)(Screen.width * 0.08f);
        questItemsHeight = questItemsWidth;
        questXPos = (int)(Screen.width * 0.1f);
        questYPos = (int)(Screen.height * 0.25f);

        genInventoryRect = new Rect(Screen.width * 0.42f, Screen.height * 0.14f, Screen.width * 0.46f, Screen.height * 0.60f);

        if (dataLoaded == false)
        {
            GetQuestItemsList();
            UpDateInventory();
            dataLoaded = true;
        }
    }

    void OnEnable()
    {
        GetQuestItemsList();
        UpDateInventory();
        potionBool = false;
        miscBool = false;
        weaponsBool = true;

        dataLoaded = true;
    }

    public void DisplaySlots()
    {
        GUI.DrawTexture(new Rect(Screen.width * 0.01f, Screen.height * 0.15f, Screen.width * 0.4f, Screen.height * 0.70f), slots);
    }

    public void DisplayEquippedItems()
    {
        if (displayInfoItem == false)
        {
            activePCObj = GetComponent<GeneralWindow>().activePCObj;
            string _namePlayer = activePCObj.name;
            string mainPlayerNameGC = gameController.GetComponent<GameController>().mainPlayerName;
            if (_namePlayer == mainPlayerNameGC)
            {
                _namePlayer = "Player";
            }
            DisplayHead(_namePlayer);
            DisplayBreast(_namePlayer);
            DisplayNeck(_namePlayer);
            DisplayRHand(_namePlayer);
            DisplayLHand(_namePlayer);
            DisplayArms(_namePlayer);
            DisplayWaist(_namePlayer);
            DisplayLegs(_namePlayer);
            DisplayFingerL(_namePlayer);
            DisplayFingerR(_namePlayer);
            DisplayArmour();
            //      GUI.Label(new Rect(fingerXPos, fingerYPos + equippedButtonWidth, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);
            GUI.Label(new Rect(Input.mousePosition.x + (0.5f * equippedButtonWidth), Screen.height - Input.mousePosition.y, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);

        }
    }

    private void DisplayHead(string _namePlayer)
    {
        if (_namePlayer == "Player" || _namePlayer == "Dylan")
        {
            string weaponID = DialogueLua.GetActorField(_namePlayer, "Head").AsString;
            string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));


            //         Debug.Log(weaponID + "/" + weaponName + "/" + weaponTexture);

            if (GUI.Button(new Rect(headXPos, headYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName), myStyle))
            {


            }
        }

    }
    private void DisplayBreast(string _namePlayer)
    {
        if (_namePlayer == "Player" || _namePlayer == "Dylan")
        {
            string weaponID = DialogueLua.GetActorField(_namePlayer, "Breast").AsString;
            string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
            string destroyName = "";
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
            if (GUI.Button(new Rect(breastXpos, breastYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName), myStyle))
            {
                if (_namePlayer == "Enora" || _namePlayer == "Weirum" || _namePlayer == "Lycaon" || _namePlayer == "Ecumius" || _namePlayer == "Oleg")
                {
                    DialogueManager.ShowAlert("This character can not change armour");
                    return;
                }

                if (Event.current.button == 1 && "Breast" != DialogueLua.GetActorField(weaponName, "name").AsString)
                {
                    #region
                    if (DialogueLua.GetActorField(weaponName, "displayInfoItem").AsString == "Yes")
                    {
                        displayInfoItem = true;
                        displayPath = (weaponName);
                        displayPath2 = displayPath;

                        if (gameObject.transform.Find(displayPath) != null)
                        {
                            Transform tdisplay2 = gameObject.transform.Find(displayPath);
                            tdisplay2.gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                            goToName.transform.parent = gameObject.transform;
                            goToName.name = weaponName;
                        }
                    }
                    else
                    {
                        Debug.Log(weaponID);
                        textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponID, typeof(TextAsset)));
                        description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                        generalWindow.displayInfo = true;
                        //              myGUI.displayInfoItem = true;
                    }
                    #endregion
                }

                // -- equip and control inventory
                else if (Event.current.button == 0)
                {
                    //          Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);
                    if (buttonType == "general" && buttonSelected == true)
                    {
                        //general, if an item has already been selected from general inventory so
                        // this item can be passed into equipped items. 
                        #region

                        if (DialogueLua.GetActorField(weaponID, "type").AsString == "armour")
                        {

                        }

                        string profession = activePCObj.GetComponent<PlayerStats>().profession;
                        bool usable = false;
                        string ID = activePCObj.name;
                        string usableVar = DialogueLua.GetActorField(itemSelected, "usable").AsString;

                        if (usableVar == "Everyone")
                        {
                            usable = true;
                        }
                        else
                        {
                            string[] usableArray = usableVar.Split(new string[] { "*" }, System.StringSplitOptions.None);
                            foreach (string l in usableArray)
                            {
                                if (l == profession || l == ID || l == "Everyone")
                                {
                                    usable = true;
                                }
                            }
                        }

                        if (usable != true)
                        {
                            DialogueManager.ShowAlert("No usable by this character");
                            return;
                        }

                        if (weaponID == "6001")
                        //it means there is not already a weapon equipped
                        {
                            existItemEquipped = false;
                            destroyName = "";
                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            existItemEquipped = true;
                            destroyName = DialogueLua.GetActorField(_namePlayer, "Breast").AsString;
                        }

                        //              Debug.Log(DialogueLua.GetActorField(itemSelected, "type").AsString == "armour");
                        #endregion
                        if (DialogueLua.GetActorField(itemSelected, "type").AsString == "armour")
                        {

                            #region
                            inventoryItems.RemoveAt(tempCNT);
                            //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                            if (existItemEquipped == true)
                            {
                                inventoryItems.Add(weaponID);
                                destroyName = DialogueLua.GetActorField(weaponID, "name").AsString;
                                activePCObj.transform.Find("Armour/" + weaponID).gameObject.SetActive(false);

                            }

                            DialogueLua.SetActorField(_namePlayer, "Breast", itemSelected);
                            //             Debug.Log(DialogueLua.GetActorField(_namePlayer, "Breast").AsString);
                            buttonSelected = false;
                            BackToNormalCursor();
                            #endregion
                        }
                    }
                    //		else if (buttonType == "general" && buttonSelected == false)                
                    else if (buttonSelected == false)
                    {
                        //there is not an item selected from general inventory, so if we click on the
                        // equipped item, it will be sent to general inventory
                        #region
                        if (weaponID == "6001")
                        //it means there is not already a weapon equipped
                        {
                            destroyName = "";
                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {

                            inventoryItems.Add(weaponID);
                            activePCObj.transform.Find("Armour/" + weaponID).gameObject.SetActive(false);
                            DialogueLua.SetActorField(_namePlayer, "Breast", "6001");
                            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpBreast", 0);
                            buttonSelected = false;
                        }
                        //           activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = weaponID;
                        activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpBreast", 0);
                        BackToNormalCursor();
                        #endregion
                    }

                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpBreast", 0);
                    //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
                }
            }
            //      GUI.Label(new Rect(breastXpos, breastYPos, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);

        }
        else
        {
            DialogueManager.ShowAlert("This character can not change armour");
            return;
        }

    }
    private void DisplayNeck(string _namePlayer)
    {

        string weaponID = DialogueLua.GetActorField(_namePlayer, "Neck").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(neckXPos, neckYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {

            //Only to display info of the item, not to equip or do anything
            if (Event.current.button == 1 && "Neck" != DialogueLua.GetActorField(weaponName, "name").AsString)
            {
                Debug.Log(weaponID);
                //           Debug.Log(DialogueLua.GetActorField(weaponID, "name").AsString);                
                string displayLua = DialogueLua.GetActorField(weaponID, "displayInfoItem").AsString;
                #region
                if (displayLua == "Yes")
                {
                    displayInfoItem = true;
                    generalWindow.displayInfo = true;
                    displayPath = (weaponName);
                    displayPath2 = displayPath;
                    if (gameObject.transform.Find(displayPath) != null)
                    {
                        Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                        tdisplay2.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                        Transform displayTran = transform.Find("Display");
                        goToName.transform.parent = displayTran;
                        goToName.name = weaponName;
                    }

                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                else
                {
                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponID, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                #endregion
            }

            // Equip and control inventory
            else if (Event.current.button == 0)
            {

                Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);

                
                if (buttonType == "general" && buttonSelected == true)
                {
                    //general, if an item has already been selected from general inventory so
                    // this item can be passed into equipped items. 
                    #region
                    string typeObject = DialogueLua.GetActorField(itemSelected, "type").AsString;

                    if (typeObject == "Neck")
                    {
                        string profession = activePCObj.GetComponent<PlayerStats>().profession;
                        bool usable = false;
                        usable = true;

                        if (usable != true)
                        {
                            return;
                        }

                        if (DialogueLua.GetActorField(_namePlayer, "Neck").AsString == "6009")
                        //it means there is not already a weapon equipped
                        {
                            existItemEquipped = false;
                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            existItemEquipped = true;
                        }

                        inventoryItems.RemoveAt(tempCNT);
                        //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                        if (existItemEquipped == true)
                        {
                            inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "Neck").AsString);

                        }
                        DialogueLua.SetActorField(_namePlayer, "Neck", itemSelected);
                        buttonSelected = false;
                    }
                    else
                    {
                        string languageLocation = DialogueLua.GetVariable("language").AsString;
                        string wrongItem = "";
                        wrongItem = DialogueLua.GetActorField("Dictionary", "wrongItem " + languageLocation).AsString;
                        Debug.Log(wrongItem);
                        GetComponent<AlertTrigger>().message = wrongItem;
                        GetComponent<AlertTrigger>().OnUse();
                    }

                    
                }
                //		else if (buttonType == "general" && buttonSelected == false)                
                else if (buttonSelected == false)
                {
                    //there is not an item selected from general inventory, so if we click on the
                    // equipped item, it will be sent to general inventory
                    Debug.Log("buttonselecter false");
                    string neck = DialogueLua.GetActorField(_namePlayer, "Neck").AsString;
                    string itemRHand = neck;

                    #region
                    if (neck == "6009")
                    //it means there is not already a weapon equipped
                    {

                    }
                    else
                    //it means there is already a weapon and we need to send it to general inventory.
                    {
                        Debug.Log("buttonselecter false");
                        inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "Neck").AsString);
                        //             GeneralInventory.GenInventory.Add(new Item2(DialogueLua.GetActorField(_namePlayer, "RHand").AsString, "001"));
                        DialogueLua.SetActorField(_namePlayer, "Neck", "6009");
                        //            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                        buttonSelected = false;
                    }
                    //     activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = itemRHand;
                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpNeck", 0.05f);
                    BackToNormalCursor();
                    #endregion
                }
                BackToNormalCursor();
                activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpNeck", 0.05f);
                //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
            }
        }

    }
    private void DisplayRHand(string _namePlayer)
    {

        if (_namePlayer == "Player" || _namePlayer == "Dylan")
        {
            string weaponID = DialogueLua.GetActorField(_namePlayer, "RHand").AsString;
            string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
            //      Texture weaponTexture = (Texture)(Resources.Load("Icons/"+ weaponName, typeof(Texture)));
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));




            if (GUI.Button(new Rect(rightHandX, rightHandY, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName), myStyle))
            {

                if (_namePlayer == "Rose" || _namePlayer == "Lilith" || _namePlayer == "Weirum" || _namePlayer == "Lycaon")
                {
                    DialogueManager.ShowAlert("This character can not change weapon");
                    return;
                }
                //RHand -- only to display info of the item, not to equip or do anything
                else if (Event.current.button == 1 && "RHand" != DialogueLua.GetActorField(weaponName, "name").AsString)
                {
                    Debug.Log(weaponID);
                    //           Debug.Log(DialogueLua.GetActorField(weaponID, "name").AsString);                
                    string displayLua = DialogueLua.GetActorField(weaponID, "displayInfoItem").AsString;
                    //          Debug.Log(displayLua);
                    #region
                    if (displayLua == "Yes")
                    {
                        //            Debug.Log(displayLua);
                        if (weaponName == "Dalila")
                        {
                            GetComponent<DalilaController>().DescriptionUpdate();
                        }
                        displayInfoItem = true;
                        generalWindow.displayInfo = true;
                        displayPath = (weaponName);
                        displayPath2 = displayPath;

                        if (gameObject.transform.Find(displayPath) != null)
                        {
                            Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                            tdisplay2.gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                            Transform displayTran = transform.Find("Display");
                            goToName.transform.parent = displayTran;
                            goToName.name = weaponName;
                        }

                        Debug.Log(weaponName);
                        textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString, typeof(TextAsset)));
                        textAssetString = textAsset.text;
                        description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                        UpdateInfoItem(textAsset);
                        generalWindow.displayInfo = true;
                    }
                    else
                    {
                        Debug.Log(weaponName);
                        textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponID, typeof(TextAsset)));
                        textAssetString = textAsset.text;
                        description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                        UpdateInfoItem(textAsset);
                        generalWindow.displayInfo = true;
                    }
                    #endregion
                }

                // RHand -- equip and control inventory
                else if (Event.current.button == 0 && _namePlayer != "Rose")
                {
                    //          Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);
                    if (buttonType == "general" && buttonSelected == true)
                    {
                        //general, if an item has already been selected from general inventory so
                        // this item can be passed into equipped items. 
                        #region

                        if (itemSelected == "DalilaSword" || itemSelected == "DalilaStaff" || itemSelected == "DalilaSceptre")
                        {
                            itemSelected = "Dalila";
                        }

                        if (itemSelected == "Dalila" && gameController.mainPlayerName != activePCObj.name)
                        {
                            DialogueManager.ShowAlert("Dalila may only be used by " + DialogueLua.GetActorField("Player", "playerName").AsString);
                            return;
                        }
                        string profession = activePCObj.GetComponent<PlayerStats>().profession;
                        bool usable = false;
                        usable = true;

                        if (itemSelected == "Dalila" && gameController.mainPlayerName == activePCObj.name)
                        {
                            usable = true;
                            dalilaEquipped = true;
                        }
                        else if (itemSelected == "Dalila" && gameController.mainPlayerName != activePCObj.name)
                        {
                            usable = false;
                        }

                        if (usable != true)
                        {
                            return;
                        }

                        if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "6002")
                        //it means there is not already a weapon equipped
                        {
                            existItemEquipped = false;
                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            existItemEquipped = true;
                        }

                        #endregion
                        if (DialogueLua.GetActorField(itemSelected, "type").AsString == "1Hand")
                        {
                            //If item is for one hand
                            #region
                            if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "Dalila")
                            {
                                dalilaEquipped = false;
                            }

                            inventoryItems.RemoveAt(tempCNT);

                            if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "Full")
                            {
                                DialogueLua.SetActorField(_namePlayer, "LHand", "6008");
                            }
                            //           GeneralInventory.GenInventory.RemoveAt(tempCNT);



                            if (existItemEquipped == true)
                            {
                                inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "RHand").AsString);

                            }
                            DialogueLua.SetActorField(_namePlayer, "RHand", itemSelected);
                            buttonSelected = false;
                            if (itemSelected == "Dalila")
                            {
                                dalilaEquipped = true;
                                string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
                            }


                            #endregion
                        }
                        else if (DialogueLua.GetActorField(itemSelected, "type").AsString == "2Hand")
                        {
                            //If item is for two hand
                            #region
                            if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "Dalila")
                            {
                                dalilaEquipped = false;
                            }

                            inventoryItems.RemoveAt(tempCNT);
                            //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                            if (existItemEquipped == true)
                            {
                                inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "RHand").AsString);

                            }
                            DialogueLua.SetActorField(_namePlayer, "RHand", itemSelected);
                            DialogueLua.SetActorField(_namePlayer, "LHand", "Full");
                            buttonSelected = false;
                            if (itemSelected == "Dalila")
                            {
                                dalilaEquipped = true;
                                string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
                            }


                            #endregion
                        }
                    }
                    //		else if (buttonType == "general" && buttonSelected == false)                
                    else if (buttonSelected == false)
                    {
                        //there is not an item selected from general inventory, so if we click on the
                        // equipped item, it will be sent to general inventory
                        Debug.Log("buttonselecter false");
                        string RHand = DialogueLua.GetActorField(_namePlayer, "RHand").AsString;
                        string itemRHand = RHand;

                        #region
                        if (RHand == "6002")
                        //it means there is not already a weapon equipped
                        {

                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            Debug.Log("buttonselecter false");

                            if (RHand == "Dalila")
                            {
                                RHand = "Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString;
                                dalilaEquipped = false;
                            }
                            inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "RHand").AsString);
                            //             GeneralInventory.GenInventory.Add(new Item2(DialogueLua.GetActorField(_namePlayer, "RHand").AsString, "001"));
                            DialogueLua.SetActorField(_namePlayer, "RHand", "6002");
                            //            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                            if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "Full")
                            {
                                //it means there is a 2Hand weapon equipped and we free LHand slot
                                DialogueLua.SetActorField(_namePlayer, "LHand", "6008");
                            }

                            buttonSelected = false;
                        }

                        if (itemRHand == "Dalila")
                        {
                            itemRHand = "Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString;
                            dalilaEquipped = false;
                        }
                        //     activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = itemRHand;
                        activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                        BackToNormalCursor();
                        #endregion
                    }
                    BackToNormalCursor();
                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                    //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
                }
                else if (_namePlayer == "Rose")
                {
                    DialogueManager.ShowAlert("Rose will not accept any other weapon");
                }
            }
        }
    }


    private void DisplayLHand(string _namePlayer)
    {
        if (_namePlayer == "Player" || _namePlayer == "Dylan")
        {
            string weaponID = DialogueLua.GetActorField(_namePlayer, "LHand").AsString;
            string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));

            if (weaponID == "Full")
            {

                weaponTexture = (Texture)(Resources.Load("Icons/" + DialogueLua.GetActorField(_namePlayer, "RHand").AsString, typeof(Texture)));
            }



            if (GUI.Button(new Rect(leftHandX, leftHandY, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)) && _namePlayer == "Rose")
            {
                //RHand -- only to display info of the item, not to equip or do anything

                if (_namePlayer == "Rose" || _namePlayer == "Lilith" || _namePlayer == "Weirum" || _namePlayer == "Lycaon")
                {
                    DialogueManager.ShowAlert("This character can not change weapon");
                    return;
                }

                else if (Event.current.button == 1 && "LHand" != DialogueLua.GetActorField(weaponName, "name").AsString)
                {
                    Debug.Log(DialogueLua.GetActorField(weaponID, "name").AsString);
                    #region
                    if (DialogueLua.GetActorField(weaponID, "displayInfoItem").AsString == "Yes")
                    {
                        if (weaponName == "Dalila")
                        {
                            GetComponent<DalilaController>().DescriptionUpdate();
                        }
                        displayInfoItem = true;
                        generalWindow.displayInfo = true;
                        displayPath = (weaponName);
                        displayPath2 = displayPath;

                        if (gameObject.transform.Find(displayPath) != null)
                        {
                            Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                            tdisplay2.gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                            Transform displayTran = transform.Find("Display");
                            goToName.transform.parent = displayTran;
                            goToName.name = weaponName;
                        }
                    }
                    else
                    {
                        TextAsset textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponName, typeof(TextAsset)));
                        description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                        generalWindow.displayInfo = true;
                    }
                    #endregion
                }

                // RHand -- equip and control inventory
                else if (Event.current.button == 0 && _namePlayer != "Rose")
                {
                    Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);
                    if (buttonType == "general" && buttonSelected == true)
                    {
                        //general, if an item has already been selected from general inventory so
                        // this item can be passed into equipped items. 
                        #region

                        if (itemSelected == "DalilaSword" || itemSelected == "DalilaStaff" || itemSelected == "DalilaSceptre")
                        {
                            itemSelected = "Dalila";
                        }

                        if (itemSelected == "Dalila" && gameController.mainPlayerName != activePCObj.name)
                        {
                            DialogueManager.ShowAlert("Dalila may only be used by " + DialogueLua.GetActorField("Player", "playerName").AsString);
                            return;
                        }
                        string profession = activePCObj.GetComponent<PlayerStats>().profession;
                        bool usable = false;
                        usable = true;
                        /*
                        string ID = activePCObj.name;
                        string usableVar = DialogueLua.GetActorField(itemSelected, "usable").AsString;

            //            Debug.Log("A");
                        string[] usableArray = usableVar.Split(new string[] { "*" }, System.StringSplitOptions.None);

                        foreach (string l in usableArray)
                        {
                   //         Debug.Log(l + "/" + ID + "/" + profession);
                            if (l == profession || l == ID || l == "Everyone" || DialogueLua.GetActorField(activePCObj.name, "race").AsString == l)
                            {
                                usable = true;
                                Debug.Log(l + "/" + ID + "/" + profession);
                            }
                        }*/


                        if (itemSelected == "Dalila" && gameController.mainPlayerName == activePCObj.name)
                        {
                            usable = true;
                            dalilaEquipped = true;
                        }
                        else if (itemSelected == "Dalila" && gameController.mainPlayerName != activePCObj.name)
                        {
                            usable = false;
                        }

                        if (usable != true)
                        {
                            return;
                        }

                        if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "6008")
                        //it means there is not already a weapon equipped
                        {
                            existItemEquipped = false;
                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            existItemEquipped = true;
                        }

                        #endregion
                        if (DialogueLua.GetActorField(itemSelected, "type").AsString == "1Hand")
                        {
                            //If item is for one hand
                            #region
                            if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "Dalila")
                            {
                                dalilaLeftEquipped = false;
                            }

                            inventoryItems.RemoveAt(tempCNT);
                            //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                            if (existItemEquipped == true)
                            {
                                inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "LHand").AsString);

                            }
                            DialogueLua.SetActorField(_namePlayer, "LHand", itemSelected);
                            buttonSelected = false;
                            if (itemSelected == "Dalila")
                            {
                                dalilaLeftEquipped = true;
                                string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
                            }
                            #endregion
                        }
                        else if (DialogueLua.GetActorField(itemSelected, "type").AsString == "2Hand")
                        {

                        }
                    }
                    //		else if (buttonType == "general" && buttonSelected == false)                
                    else if (buttonSelected == false)
                    {
                        //there is not an item selected from general inventory, so if we click on the
                        // equipped item, it will be sent to general inventory
                        Debug.Log("buttonselecter false");
                        string LHand = DialogueLua.GetActorField(_namePlayer, "LHand").AsString;
                        string itemLHand = LHand;

                        #region
                        if (LHand == "6008")
                        //it means there is not already a weapon equipped
                        {

                        }
                        else
                        //it means there is already a weapon and we need to send it to general inventory.
                        {
                            Debug.Log("buttonselecter false");

                            if (LHand == "Dalila")
                            {
                                LHand = "Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString;
                                dalilaEquipped = false;
                            }
                            inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "LHand").AsString);
                            //             GeneralInventory.GenInventory.Add(new Item2(DialogueLua.GetActorField(_namePlayer, "RHand").AsString, "001"));
                            DialogueLua.SetActorField(_namePlayer, "LHand", "6008");
                            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpLeftHand", 0.05f);
                            if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "Full")
                            {
                                //it means there is a 2Hand weapon equipped and we free LHand slot
                                DialogueLua.SetActorField(_namePlayer, "RHand", "6002");
                            }

                            buttonSelected = false;
                        }

                        if (itemLHand == "Dalila")
                        {
                            itemLHand = "Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString;
                            dalilaEquipped = false;
                        }
                        //     activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = itemLHand;
                        activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpLeftHand", 0.05f);
                        BackToNormalCursor();
                        #endregion
                    }
                    BackToNormalCursor();
                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpLeftHand", 0.05f);
                    //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
                }
                else if (_namePlayer == "Rose")
                {
                    DialogueManager.ShowAlert("Rose will not accept any other weapon");
                }
            }
        }
    }

    private void DisplayArms(string _namePlayer)
    {
        string weaponID = DialogueLua.GetActorField(_namePlayer, "Arms").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(armsXPos, armsYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {
            if ( _namePlayer == "Weirum" || _namePlayer == "Lycaon")
            {
                DialogueManager.ShowAlert("This character can not change this equipment");
                return;
            }
        }
    }
    private void DisplayWaist(string _namePlayer)
    {
        string weaponID = DialogueLua.GetActorField(_namePlayer, "Waist").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(waistXPos, waistYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {

        }
    }
    private void DisplayLegs(string _namePlayer)
    {
        string weaponID = DialogueLua.GetActorField(_namePlayer, "Legs").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(legsXPos, legsYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {
        }
    }
    private void DisplayFingerL(string _namePlayer)
    {
        string weaponID = DialogueLua.GetActorField(_namePlayer, "FingerL").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(leftHandX, fingerYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {
            if ( _namePlayer == "Weirum" || _namePlayer == "Lycaon")
            {
                DialogueManager.ShowAlert("This character can not change this equipment");
                return;
            }


            //Only to display info of the item, not to equip or do anything
            else if (Event.current.button == 1 && "FingerL" != DialogueLua.GetActorField(weaponName, "name").AsString)
            {
                Debug.Log(weaponID);
                //           Debug.Log(DialogueLua.GetActorField(weaponID, "name").AsString);                
                string displayLua = DialogueLua.GetActorField(weaponID, "displayInfoItem").AsString;
                #region
                if (displayLua == "Yes")
                {
                    displayInfoItem = true;
                    generalWindow.displayInfo = true;
                    displayPath = (weaponName);
                    displayPath2 = displayPath;
                    if (gameObject.transform.Find(displayPath) != null)
                    {
                        Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                        tdisplay2.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                        Transform displayTran = transform.Find("Display");
                        goToName.transform.parent = displayTran;
                        goToName.name = weaponName;
                    }

                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                else
                {
                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponID, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                #endregion
            }

            // Equip and control inventory
            else if (Event.current.button == 0)
            {
                Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);
                if (buttonType == "general" && buttonSelected == true)
                {
                    //general, if an item has already been selected from general inventory so
                    // this item can be passed into equipped items. 

                    string profession = activePCObj.GetComponent<PlayerStats>().profession;
                    bool usable = false;
                    usable = true;

                    if (usable != true)
                    {
                        return;
                    }

                    if (DialogueLua.GetActorField(_namePlayer, "FingerL").AsString == "6005")
                    //it means there is not already a weapon equipped
                    {
                        existItemEquipped = false;
                    }
                    else
                    //it means there is already a weapon and we need to send it to general inventory.
                    {
                        existItemEquipped = true;
                    }

                    inventoryItems.RemoveAt(tempCNT);
                    //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                    if (existItemEquipped == true)
                    {
                        inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "FingerL").AsString);

                    }
                    DialogueLua.SetActorField(_namePlayer, "FingerL", itemSelected);
                    buttonSelected = false;
                }
                //		else if (buttonType == "general" && buttonSelected == false)                
                else if (buttonSelected == false)
                {
                    //there is not an item selected from general inventory, so if we click on the
                    // equipped item, it will be sent to general inventory
                    Debug.Log("buttonselecter false");
                    string neck = DialogueLua.GetActorField(_namePlayer, "FingerL").AsString;
                    string itemRHand = neck;

                    #region
                    if (neck == "6005")
                    //it means there is not already a weapon equipped
                    {

                    }
                    else
                    //it means there is already a weapon and we need to send it to general inventory.
                    {
                        Debug.Log("buttonselecter false");
                        inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "FingerL").AsString);
                        //             GeneralInventory.GenInventory.Add(new Item2(DialogueLua.GetActorField(_namePlayer, "RHand").AsString, "001"));
                        DialogueLua.SetActorField(_namePlayer, "FingerL", "6005");
                        //            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                        buttonSelected = false;
                    }
                    //     activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = itemRHand;
                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpFingerL", 0.05f);
                    BackToNormalCursor();
                    #endregion
                }
                BackToNormalCursor();
                activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpFingerL", 0.05f);
                //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
            }
        }
    }
    private void DisplayFingerR(string _namePlayer)
    {
        string weaponID = DialogueLua.GetActorField(_namePlayer, "FingerR").AsString;
        string weaponName = DialogueLua.GetActorField(weaponID, "name").AsString;
        Texture weaponTexture = (Texture)(Resources.Load("Icons/" + weaponID, typeof(Texture)));
        if (GUI.Button(new Rect(fingerXPos, fingerYPos, equippedButtonWidth, equippedButtonHeight), new GUIContent(weaponTexture, weaponName)))
        {
            if (_namePlayer == "Weirum" || _namePlayer == "Lycaon")
            {
                DialogueManager.ShowAlert("This character can not change this equipment");
                return;
            }

            //Only to display info of the item, not to equip or do anything
            else if (Event.current.button == 1 && "FingerR" != DialogueLua.GetActorField(weaponName, "name").AsString)
            {
                Debug.Log(weaponID);
                //           Debug.Log(DialogueLua.GetActorField(weaponID, "name").AsString);                
                string displayLua = DialogueLua.GetActorField(weaponID, "displayInfoItem").AsString;
                #region
                if (displayLua == "Yes")
                {
                    displayInfoItem = true;
                    generalWindow.displayInfo = true;
                    displayPath = (weaponName);
                    displayPath2 = displayPath;
                    if (gameObject.transform.Find(displayPath) != null)
                    {
                        Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                        tdisplay2.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject goToName = (Instantiate(Resources.Load("Display/" + displayPath), transform.position, transform.rotation) as GameObject);
                        Transform displayTran = transform.Find("Display");
                        goToName.transform.parent = displayTran;
                        goToName.name = weaponName;
                    }

                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + DialogueLua.GetActorField("Player", "dalila").AsString, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                else
                {
                    Debug.Log(weaponName);
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + weaponID, typeof(TextAsset)));
                    textAssetString = textAsset.text;
                    description = textAsset.text.Split(new string[] { "*" }, System.StringSplitOptions.None);
                    UpdateInfoItem(textAsset);
                    generalWindow.displayInfo = true;
                }
                #endregion
            }

            // Equip and control inventory
            else if (Event.current.button == 0)
            {
                Debug.Log(buttonSelected + "/" + buttonSelected2 + "/" + buttonType);
                if (buttonType == "general" && buttonSelected == true)
                {
                    //general, if an item has already been selected from general inventory so
                    // this item can be passed into equipped items. 
                    
                    string profession = activePCObj.GetComponent<PlayerStats>().profession;
                    bool usable = false;
                    usable = true;

                    if (usable != true)
                    {
                        return;
                    }

                    if (DialogueLua.GetActorField(_namePlayer, "FingerR").AsString == "6004")
                    //it means there is not already a weapon equipped
                    {
                        existItemEquipped = false;
                    }
                    else
                    //it means there is already a weapon and we need to send it to general inventory.
                    {
                        existItemEquipped = true;
                    }

                    inventoryItems.RemoveAt(tempCNT);
                    //           GeneralInventory.GenInventory.RemoveAt(tempCNT);

                    if (existItemEquipped == true)
                    {
                        inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "FingerR").AsString);

                    }
                    DialogueLua.SetActorField(_namePlayer, "FingerR", itemSelected);
                    buttonSelected = false;
                }
                //		else if (buttonType == "general" && buttonSelected == false)                
                else if (buttonSelected == false)
                {
                    //there is not an item selected from general inventory, so if we click on the
                    // equipped item, it will be sent to general inventory
                    Debug.Log("buttonselecter false");
                    string neck = DialogueLua.GetActorField(_namePlayer, "FingerR").AsString;
                    string itemRHand = neck;

                    #region
                    if (neck == "6004")
                    //it means there is not already a weapon equipped
                    {

                    }
                    else
                    //it means there is already a weapon and we need to send it to general inventory.
                    {
                        Debug.Log("buttonselecter false");
                        inventoryItems.Add(DialogueLua.GetActorField(_namePlayer, "FingerR").AsString);
                        //             GeneralInventory.GenInventory.Add(new Item2(DialogueLua.GetActorField(_namePlayer, "RHand").AsString, "001"));
                        DialogueLua.SetActorField(_namePlayer, "FingerR", "6004");
                        //            activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpRightHand", 0.05f);
                        buttonSelected = false;
                    }
                    //     activePCObj.GetComponent<PlayerEquippedItems>().itemRHand = itemRHand;
                    activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpFingerR", 0.05f);
                    BackToNormalCursor();
                    #endregion
                }
                BackToNormalCursor();
                activePCObj.GetComponent<PlayerEquippedItems>().Invoke("SetUpFingerR", 0.05f);
                //      activePCObj.GetComponent<PlayerEquippedItems>().SetUpRightHand();
            }




        }
    }

    public void DisplayGeneralInventory()
    {
        if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.12f, buttonWidth, Screen.width * 0.06f), "WEAPONS"))
        {
            weaponsBool = true;
            potionBool = false;
            miscBool = false;
        }

        if( GUI.Button(new Rect(Screen.width * 0.45f + buttonWidth, Screen.height * 0.12f, buttonWidth, Screen.width * 0.06f), "POTIONS"))
        {
            weaponsBool = false;
            potionBool = true;
            miscBool = false;
        }

        if (GUI.Button(new Rect(Screen.width * 0.45f + (2 * buttonWidth), Screen.height * 0.12f, buttonWidth, Screen.width * 0.06f), "MISC"))
        {
            weaponsBool = false;
            potionBool = false;
            miscBool = true;
        }

        if (weaponsBool == true)
        {
            InventoryWeapons();
        }
        else if (potionBool == true)
        {
            InventoryPotions();
        }
        else if (miscBool == true)
        {
   //         Debug.Log("mis");
            InventoryMisc();
        }
        DisplayCoins();
        GUI.Label(new Rect(Input.mousePosition.x + (0.5f * equippedButtonWidth), Screen.height - Input.mousePosition.y, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);

        //      GUI.Label(new Rect(fingerXPos, fingerYPos + equippedButtonWidth, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);

    }

    public void DisplayInfoItems()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height * 0.91f), generalWindow.background);
        if (generalWindow.displayInfo == true)
        {
            if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.12f, 100, 30), "CLOSE"))
            {
                generalWindow.displayInfo = false;
                displayInfoItem = false;
                Transform tdisplay2 = gameObject.transform.Find("Display/" + displayPath);
                tdisplay2.gameObject.SetActive(false);
            }

            _ItemDesSlider = GUI.BeginScrollView(new Rect(10, 125, Screen.width * 0.98f, Screen.height * 0.7f), _ItemDesSlider,
                                                          _ItemDesWindowRect);

            GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.1f, Screen.width * 0.5f, Screen.height), textAssetString);
            /*
            for (int cnt = 0; cnt < arrayInfoItem.Length; cnt++)
            {
                //			GUI.Label (new Rect (10, 20 + (cnt * 25), 700, 25), arrayInfoItem[cnt]);

            }*/

            GUI.EndScrollView();
        }
    }

    public void GetQuestItemsList()
    {
        arrayQuestItems.Clear();
        string questItemsList = DialogueLua.GetVariable("QuestItems").AsString;
        //      Debug.Log(questItemsList);
        if (questItemsList == null || questItemsList == "")
        {
            nullQuestItem = true;
        }
        else
        {
            nullQuestItem = false;
            string[] tempArrayQuestItems = questItemsList.Split(new string[] { "*" }, System.StringSplitOptions.None);
            for (int cnt = 0; cnt < tempArrayQuestItems.Length; cnt++)
            {
                arrayQuestItems.Add(tempArrayQuestItems[cnt]);
            }
        }

    }

    public void DisplayQuestItems()
    {
        if (readQuestItem == true)
        {
            if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.11f, Screen.width * 0.12f, Screen.height * 0.07f), "CLOSE"))
            {
                readQuestItem = false;
            }

            GUI.DrawTexture(new Rect(Screen.width * 0.70f, Screen.height * 0.14f, Screen.width * 0.23f, Screen.width * 0.23f), textureToDisplay);

            questItemRectSlider = GUI.BeginScrollView(new Rect(posX, posY, 800, 400), questItemRectSlider,
                                                  questItemsRect);
            GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.1f, Screen.width * 0.5f, Screen.height), value);
            /*
            for (int cnt = 0; cnt < arrayString.Length; cnt++)
            {
                GUI.Label(new Rect(1, 20 + (cnt * 25), 600, buttonHeight), arrayString[cnt]);
            }*/


            GUI.EndScrollView();
        }
        else
        {

            if (nullQuestItem == false)
            {
                for (int cnt = 0; cnt < arrayQuestItems.Count; cnt++)
                {
                    if (GUI.Button(new Rect(questXPos, questYPos + (cnt * questItemsHeight), questItemsWidth, questItemsHeight), new GUIContent((Texture)(Resources.Load("Icons/" + arrayQuestItems[cnt])), DialogueLua.GetActorField(arrayQuestItems[cnt], "name").AsString)))
                    {
                        SetPath(arrayQuestItems[cnt]);
                        textureToDisplay = (Texture2D)(Resources.Load("Icons/" + arrayQuestItems[cnt]));
                        readQuestItem = true;
                    }
                    GUI.Label(new Rect(Input.mousePosition.x + (equippedButtonWidth * 0.5f), Screen.height - Input.mousePosition.y, equippedButtonWidth, equippedButtonHeight), GUI.tooltip);


                }
            }
        }
    }

    private void SetPath(string pathResources)
    {
     //   Debug.Log(pathResources);
        textAsset = (TextAsset)(Resources.Load("QuestItems/" + pathResources, typeof(TextAsset)));
        value = textAsset.text;
        Chop(value);
    }

    public void Chop(string value)
    {
        arrayString = value.Split(new string[] { "*" }, System.StringSplitOptions.None);
        int lines = arrayString.Length + 2;
        questItemsRect = new Rect(0, 0, 1, (lines * buttonHeight));
    }

    //	END QUEST ITEMS ************************************************************************

    // START GEN INVENTORY **********************************************************************

    public void UpDateInventory()
    {
        string inventory = DialogueLua.GetVariable("GeneralInventory").AsString;
        string miscInventory = DialogueLua.GetVariable("miscInventory").AsString;
   //     Debug.Log(inventory);
   //     Debug.Log(miscInventory);

        if (inventory == null || inventory == "")
        {
            nullGenInventory = true;
            //          Debug.Log(nullGenInventory);
        }
        else
        {
            arrayInventory = inventory.Split(new string[] { "*" }, System.StringSplitOptions.None);
            nullGenInventory = false;
        }

        if (miscInventory == null || miscInventory == "")
        {
            nullMiscInventory = true;
    //        Debug.Log(nullGenInventory);
        }
        else
        {
            miscInventoryArray = miscInventory.Split(new string[] { "*" }, System.StringSplitOptions.None);
           nullMiscInventory = false;
        }

        GetInventory();
    }


    void GetInventory()
    {
        if (nullGenInventory == false)
        {
            inventoryItems.Clear();
            for (int cnt = 0; cnt < arrayInventory.Length; cnt++)
            {
                inventoryItems.Add(arrayInventory[cnt]);
            }
        }

        if (nullMiscInventory == false)
        {
            miscItemsList.Clear();
            for (int cnt = 0; cnt < miscInventoryArray.Length; cnt++)
            {
                miscItemsList.Add(miscInventoryArray[cnt]);
     //           Debug.Log(arrayInventory[cnt]);
            }
        }

    }


    void UpdateInfoItem(TextAsset textAsset)
    {
        string text = textAsset.text;
        ChopInfoItem(text);
    }

    void ChopInfoItem(string text)
    {
        arrayInfoItem = text.Split(new string[] { "*" }, System.StringSplitOptions.None);
        int lines = arrayInfoItem.Length + 2;

        _ItemDesWindowRect = new Rect(0, 0, 500, (lines * buttonHeight));
    }

    void DisplayCoins()
    {
        GUI.Label(new Rect(leftHandX, chestY, equippedButtonHeight, equippedButtonHeight), new GUIContent(chestTexture, "Treasure (Coins)"));
        GUI.DrawTexture(new Rect(leftHandX, chestY + equippedButtonHeight, equippedButtonHeight, equippedButtonHeight / 2), black);
        GUI.Label(new Rect(leftHandX, chestY + equippedButtonHeight, equippedButtonHeight, equippedButtonHeight / 2), new GUIContent(DialogueLua.GetVariable("coins").AsInt.ToString(), "Treasure (Coins)"));
    }

    void DisplayArmour()
    {
        GUI.Label(new Rect(breastXpos, chestY, equippedButtonHeight, equippedButtonHeight), new GUIContent(armour, "Armour"));
        //     GUI.DrawTexture(new Rect(leftHandX, chestY + equippedButtonHeight, equippedButtonHeight, equippedButtonHeight / 2), black);
        GUI.Label(new Rect(breastXpos + (0.5f * equippedButtonWidth), chestY + equippedButtonHeight, equippedButtonHeight, equippedButtonHeight / 2), new GUIContent(activePCObj.GetComponent<PlayerStats>().armourTotal.ToString(), "Armour"));
    }
    #endregion

    void InventoryPanel(int id)
    {
        

        
        genInventoryPanel = GUI.Toolbar(new Rect(0, 0, Screen.width * 0.48f, Screen.height * 0.06f), genInventoryPanel, genPanelNames);
        switch (genInventoryPanel)
        {
            //INVENTORY
            case 0:
       //         Debug.Log("weapons");
                InventoryWeapons();
                break;

            //CHARACTER
            case 1:
                InventoryPotions();
                break;

            //QUEST ITEMS
            case 2:
                InventoryMisc();
                break;
        }
    }


    void InventoryWeapons ()
    {
        int rowLen = 5;

        _InventoryWindowSlider = GUI.BeginScrollView(new Rect(inventoryWindowXStart, inventoryWindowYStart, inventoryWindowWidth, inventoryWindowHeight), _InventoryWindowSlider,
                                              inventoryWindownSliderRect);
        //      PlayerEquippedItems go = (PlayerEquippedItems)activePCObj.GetComponent("PlayerEquippedItems");

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            string id = inventoryItems[i];
            string weaponName = DialogueLua.GetActorField(id, "name").AsString;
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + id, typeof(Texture)));

            if (GUILayout.Button(new GUIContent(weaponTexture, weaponName), GUILayout.Width(equippedButtonWidth), GUILayout.Height(equippedButtonHeight)))
            {
                if (id == itemSelected)
                {
                    buttonType = "null";
                    buttonSelected = false;
                    itemSelected = null;
                }

                else if (buttonSelected2 == true & buttonType == "equipped")
                {
                    inventoryItems.Add(itemSelected);
                    buttonSelected2 = false;
                }

                if (Event.current.button == 0)
                {
                    buttonType = "general";
                    buttonSelected = true;
                    itemSelected = id;
                    tempCNT = i;

                    textureCursor = true;
                    Texture2D cursorTexture = (Texture2D)(Resources.Load("Icons/Cursor/" + id, typeof(Texture2D)));
                    textureToQuickItems = (Texture2D)(Resources.Load("Icons/Cursor/" + id, typeof(Texture2D)));
                    Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                    gameController.currentCursor = (Texture2D)(Resources.Load("Icons/" + id, typeof(Texture)));
                    state = State.Seq01;
                    StartCoroutine("FSM");
                }
                    



                if (buttonSelected == true)
                {
                    GUI.Label(new Rect(3, 20 + (i * inventoryButtonHeight), inventoryButtonWidth, inventoryButtonHeight), weaponName);
                }

                // DISPLAY INFO ONLY ************************************************************
                if (Event.current.button == 1)
                {
                    //      Debug.Log("info");
                    if (weaponName == "DalilaSceptre" || weaponName == "DalilaSword" || weaponName == "DalilaStaff")
                    {
                        weaponName = "Dalila";
                    }

                    if (DialogueLua.GetActorField(id, "displayInfoItem").AsString == "Yes")
                    {
                        if (weaponName == "Dalila")
                        {
                            GetComponent<DalilaController>().DescriptionUpdate();
                        }


                    }
                    generalWindow.displayInfo = true;
                    textAsset = null;
                    if (weaponName != "Dalila")
                    {
                        textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + id));
                        Debug.Log(id);
                        textAssetString = textAsset.text;
                        UpdateInfoItem(textAsset);
                    }
                    else
                    {
                        string dalilaType = DialogueLua.GetActorField("Player", "dalila").AsString;
                        textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + dalilaType));
                        textAssetString = textAsset.text;
                        UpdateInfoItem(textAsset);
                    }

                }
            }
            if ((i + 1) % rowLen == 0)
            { //if the next object needs to be on a new row
                GUILayout.EndHorizontal(); //end row
                GUILayout.BeginHorizontal(); //create new row
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.EndScrollView();

        
    }

    void InventoryPotions()
    {
        int rowLen = 5;

        _InventoryWindowSlider = GUI.BeginScrollView(new Rect(inventoryWindowXStart, inventoryWindowYStart, inventoryWindowWidth, inventoryWindowHeight), _InventoryWindowSlider,
                                              inventoryWindownSliderRect);
        //      PlayerEquippedItems go = (PlayerEquippedItems)activePCObj.GetComponent("PlayerEquippedItems");

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < potions.Count; i++)
        {
            string id = potions[i];
            string weaponName = DialogueLua.GetActorField(id, "name").AsString;
            int numberPotions = DialogueLua.GetActorField(id, "amount").AsInt; 
            Texture2D weaponTexture = (Texture2D)(Resources.Load("Icons/" + id, typeof(Texture)));
      //      Debug.Log(id);
            GUIContent content = new GUIContent();
            content.image = weaponTexture;
            content.text = numberPotions.ToString();
            content.tooltip = weaponName;            

            if (GUILayout.Button(content, GUILayout.Width(equippedButtonWidth), GUILayout.Height(equippedButtonHeight)))
            {
                textureCursor = true;
                Texture2D cursorTexture = (Texture2D)(Resources.Load("Icons/Cursor/" + id, typeof(Texture2D)));
                textureToQuickItems = (Texture2D)(Resources.Load("Icons/Cursor/"+ id, typeof(Texture2D)));
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                gameController.currentCursor = weaponTexture;
                state = State.Seq01;
                StartCoroutine("FSM");

                // DISPLAY INFO ONLY ************************************************************
                if (Event.current.button == 1)
                {
    
                    if (DialogueLua.GetActorField(id, "displayInfoItem").AsString == "Yes")
                    {

                    }
                    generalWindow.displayInfo = true;
                    textAsset = null;
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + id));
                    textAssetString = textAsset.text;
                    UpdateInfoItem(textAsset);
                }
            }

                if ((i + 1) % rowLen == 0)
            { //if the next object needs to be on a new row
                GUILayout.EndHorizontal(); //end row
                GUILayout.BeginHorizontal(); //create new row
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.EndScrollView();


    }

    void InventoryMisc ()
    {
        int rowLen = 5;

        _InventoryWindowSlider = GUI.BeginScrollView(new Rect(inventoryWindowXStart, inventoryWindowYStart, inventoryWindowWidth, inventoryWindowHeight), _InventoryWindowSlider,
                                              inventoryWindownSliderRect);
        //      PlayerEquippedItems go = (PlayerEquippedItems)activePCObj.GetComponent("PlayerEquippedItems");

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < miscItemsList.Count; i++)
        {
            string id = miscItemsList[i];
            Debug.Log(id);
            string weaponName = DialogueLua.GetActorField(id, "name").AsString;
            Texture weaponTexture = (Texture)(Resources.Load("Icons/" + id, typeof(Texture)));

            if (GUILayout.Button(new GUIContent(weaponTexture, weaponName), GUILayout.Width(equippedButtonWidth), GUILayout.Height(equippedButtonHeight)))
            {
                string quickItem = DialogueLua.GetActorField(id, "quickItem").AsString;

                if (quickItem == "Yes")
                {
                    if (id == itemSelected)
                    {
                        buttonType = "null";
                        buttonSelected = false;
                        itemSelected = null;
                    }
                    else if (buttonSelected2 == true & buttonType == "equipped")
                    {
                        //         go.equipped.RemoveAt(tempCNT);
                        inventoryItems.Add(itemSelected);
                        //         GeneralInventory.GenInventory.Add(new Item2(itemSelected, "001"));
                        //         go.equipped.Add(new EquippedItems(itemOrigin, itemPos, itemOrigin));
                        buttonSelected2 = false;
                    }
                    buttonType = "general";
                    buttonSelected = true;
                    itemSelected = id;
                    tempCNT = i;

                    textureCursor = true;
                    Texture2D cursorTexture = (Texture2D)(Resources.Load("Icons/Cursor/" + id, typeof(Texture2D)));
                    textureToQuickItems = (Texture2D)(Resources.Load("Icons/Cursor/" + id, typeof(Texture2D)));
                    Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                    gameController.currentCursor = (Texture2D)(Resources.Load("Icons/" + id, typeof(Texture)));
                    state = State.Seq01;
                    StartCoroutine("FSM");

                    if (buttonSelected == true)
                    {
                        GUI.Label(new Rect(3, 20 + (i * inventoryButtonHeight), inventoryButtonWidth, inventoryButtonHeight), weaponName);
                    }
                }
                //    Debug.Log(id);




                // DISPLAY INFO ONLY ************************************************************
                if (Event.current.button == 1)
                {
                    generalWindow.displayInfo = true;
                    textAsset = (TextAsset)(Resources.Load("WeaponDescription/" + id));
                    textAssetString = textAsset.text;
                    UpdateInfoItem(textAsset);
                }
            }
            if ((i + 1) % rowLen == 0)
            { //if the next object needs to be on a new row
                GUILayout.EndHorizontal(); //end row
                GUILayout.BeginHorizontal(); //create new row
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.EndScrollView();
    }


    public void UpdatePotions ()
    {
        potions.Clear();
        foreach (string st in potionsNames)
        {
            if (DialogueLua.GetActorField (st, "amount").AsInt > 0)
            {
  //              Debug.Log(st);
                potions.Add(st);
            }
        }
    }


    private IEnumerator FSM()
    {
        while (true)
        {
            switch (state)
            {
                case State.Seq01:
                    Seq01();
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(0);
                 
                    yield return new WaitForSeconds(0);
                    break;
                case State.Seq03:
                    yield return new WaitForSeconds(0);
                  
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq04:
                    yield return new WaitForSeconds(0);
                   
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq05:
                    yield return new WaitForSeconds(0);
                  
                    yield return new WaitForSeconds(0);
                    break;
            }
            yield return null;
        }
    }


    void Seq01 ()
    {
        if (Input.GetMouseButton(1))
        {
            BackToNormalCursor();
        }
    }

    public void BackToNormalCursor ()
    {
        textureCursor = false;
        Cursor.SetCursor(gameController.cursorNormal, Vector2.zero, CursorMode.Auto);
        gameController.currentCursor = gameController.cursorNormal;
        buttonType = "null";
        buttonSelected = false;
        itemSelected = null;
        StopCoroutine("FSM");

    }

    public void AddQuestItem (string itemToAdd)
    {
        string currentQuestItems = DialogueLua.GetVariable("QuestItems").AsString;
        currentQuestItems = currentQuestItems + "*" + itemToAdd;
        DialogueLua.SetVariable("QuestItems", currentQuestItems);
    }
   
}
