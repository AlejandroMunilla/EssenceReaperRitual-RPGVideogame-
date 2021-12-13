using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;
using UnityEngine.SceneManagement;


public class DisplayOptionsScript : MonoBehaviour 
{
    public List<string> files = new List<string>();
    private Rect _optionsWindowRect = new Rect (1, 1, Screen.width - (Screen.width * 0.1f), Screen.height *0.90f);
	private int _optionPanel = 30;
	private string mainPlayerName;
	private string mySaveDirectory;
	private string chosenFileToLoad;
	public string nameSave = "";
	private string scene;
    private string fileToDelete;
	private string [] _optionPanelNames = new string[] 
	{
		"Save",
		"Load",
		"Dificulty",
        "Exit Game",        
        "Return Game"
    };
	
	private bool displayOverwrite = false;
	private bool displayLoadWarning = false;
	private bool displaySaveDone = false;
    private bool deleteFilesOn = false;
	private int saveXPos = 50;
	private int saveNameXpos = 150;
	private int saveButton = 200;
	private int saveYPos;
	private int savesButtonLength;
    private int savesButtonHeight;
	private int savesButtonY;
	private int enterNameButton ;
	private int overwriteYPos;
	private int overwriteXPos;
	private int labelXPos;
	private GameController gamecontroller;
	private Texture button;
    private GUISkin mySkin;
	private string saveDirectory ;
	private string loadPosition ;
    private bool counter = false;
    private float initialTime;
    private Vector2 loadRectSlide;
    private Rect loadRect;
    private Rect internalLoadRect;

    #region
    //For Display Dificulty
    public string difficulty = "Normal";
    public string realism = "Epic Fantasy";
    private bool showDifficulty = false;
    private bool showRealism = false;
    private int buttonLength;
    private int buttonHeight;
    private string easy = "Enemies has a basic AI with 30% lesser health and 20% lower chances to hit, lower armour and lower stats";
    private string normal = "This is the normal set up for the game. Basic AI, save some bosses with improved AI to track weaker party members";
    private string hard = "Enemies has improved attacks and damage, and some of them will target weak party members or follow other unusal tactics";
    private string fairyTale = "Unrealistic even for fantasy worlds. Fallen party members in combat will raise back with 30% health at the end of combat. Game is over if all party members are defeated";
    private string epicfantasy = "Party members who fall in battle can be brought back with a resurrection spell. Party members without helmet protection might suffer grave damage and resurrection will not be option, especially if playable character has fallen as a result of a critical hit. If main player is dead, it is the end of your adventure and you will have to reload the game from last saved game";
    private string lifeIsHard = "Party members can not be brought back to life. Death is the end. If main character is dead, game is over. You may reload your last save to recover your party member.";
    private string textDifficulty = "xx";
    private string textRealism = "XXY";

    //Display ExitGame
    private bool displayExitWarning;

    //Display Graphics

    private float fontSizeDialog = 0.02f;
    private GameObject dialogueManager;
    private ScaleFontSize scaleFont;
    private GUIRoot guiRoot;
    private float rateAdjust;

    #endregion

    void Start () 
	{

    }

    void OnEnable ()
    {
        GetComponent<DisplayInfo>().enabled = false;
        gamecontroller = GetComponent<GameController>();
        mainPlayerName = DialogueLua.GetActorField("Player", "playerName").AsString;
        mySaveDirectory = Application.persistentDataPath + "/Eternalia/" + mainPlayerName;
  //      Debug.Log(mySaveDirectory);
        saveXPos = (int)(Screen.width * 0.10f);
        saveNameXpos = (int)(Screen.width * 0.12f);
        saveButton = (int)(Screen.width * 0.25f);
        saveYPos = (int)(Screen.height * 0.17f);
        savesButtonLength = (int)(Screen.width * 0.3f);
        savesButtonHeight = (int)(Screen.height * 0.09f);
        savesButtonY = (int)(Screen.height * 0.3f);
        enterNameButton = (int)(Screen.width * 0.40f);
        overwriteYPos = (int)(Screen.height * 0.55f);
        overwriteXPos = (int)(Screen.width * 0.40f);
        labelXPos = (int)(Screen.width * 0.50f);
        buttonLength = (int)(Screen.width * 0.18f);
        buttonHeight = (int)(Screen.height * 0.05f);
        textDifficulty = normal;
        textRealism = epicfantasy;
        button = (Texture)(Resources.Load("GUI/Button", typeof(Texture)));
        mySkin = GetComponent<GeneralWindow>().mySkin;
        GetFiles();
        loadRect = new Rect(0, Screen.height * 0.26f, (Screen.width - ((int)(Screen.width * 0.12f))), Screen.height * 0.70f);
        //  ( ,  ,  , lo que se ve en pantalla );
        float internalHeight = (int)(savesButtonHeight * (files.Count + 6));
        internalLoadRect = new Rect(0, Screen.height * 0.1f, Screen.width * 0.45f, internalHeight);
        float rateTo = Screen.width / 1024 * 1.5f;
    //    rateAdjust = 1024 / Screen.width * 1.3f;
    //    DialogueLua.SetVariable("rateAdjust", rateTo);
    //    guiRoot.ManualRefresh();
    }

   

    private void OnDisable()
    {
        GetComponent<DisplayInfo>().enabled = true;
    }

    void OnGUI ()
    {
        GUI.skin = mySkin;
        DisplayOptions();
    }

	void  DisplayOptions ()
	{ 		
        GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), GetComponent<GeneralWindow>().background  );
		_optionPanel = GUI.Toolbar (new Rect (0, 0, Screen.width, Screen.height * 0.08f), _optionPanel, _optionPanelNames);
		switch (_optionPanel)
		{
            case 0:
                DisplaySave();
                break;
            case 1:
                DisplayLoad();
                break;
            case 2:
                DisplayDifficulty();
                break;
            case 3:
                DisplayExitGame();
                break;
            case 4:
                GetComponent<PlayerControls>().toggleIsOn = false;
                GetComponent<PlayerControls>().ToggleOff();
                GetComponent<SaveGame>().savePostion = false;
                if (GetComponent<PlayerControls>().enabled == false)
                {
                    GetComponent<PlayerControls>().enabled = true;
                }

                if (GetComponent<DisplayToolBar>().enabled == false)
                {
                    GetComponent<DisplayToolBar>().enabled = true;
                }
                /*
                GetComponent<GeneralWindow>().enabled = true;
                GetComponent<PlayerControls>().enabled = true;
                GetComponent<DisplayPortraits>().enabled = true;
                GetComponent<DisplayToolBar>().enabled = true;
                /*
                GetComponent<PlayerControls>().ToggleOff();
                GetComponent<GameController>().activePC.GetComponent<TargetActivePC>().enabled = true;
                GetComponent<DisplayToolBar>().enabled = true;
                GetComponent<DisplayPortraits>().enabled = true;
    //            GetComponent<DisplayInfo>().enabled = true;*/
                _optionPanel = 0;
                this.enabled = false;
                break;

        }
	}

	void DisplaySave()
	{
        GUI.Label(new Rect(Screen.width * 0.05f , saveYPos, Screen.width * 0.1f, Screen.height * 0.06f), "Name:");
        nameSave = GUI.TextArea(new Rect(saveNameXpos, saveYPos, Screen.width * 0.18f, Screen.height * 0.06f), nameSave, 15);

        loadRectSlide = GUI.BeginScrollView(loadRect, loadRectSlide, internalLoadRect);
        for (int cnt = 0; cnt < files.Count; cnt++)
        {
            if (GUI.Button(new Rect(saveXPos, savesButtonY + ((cnt + 2) * savesButtonHeight), savesButtonLength, savesButtonHeight), Path.GetFileNameWithoutExtension(files[cnt].ToString())) && displayOverwrite == false)
            {

            }
        }
        GUI.EndScrollView();


        if (displaySaveDone == false)
        {
            
            if (string.IsNullOrEmpty(nameSave) || nameSave == "")
            {
                GUI.Label(new Rect(enterNameButton, saveYPos, 200, 25), "Enter name for a new save");
            }
            else
            {
            //    Debug.Log(nameSave);
                if (GUI.Button(new Rect(Screen.width * 0.35f, saveYPos, Screen.width * 0.12f, Screen.height * 0.07f), "Save"))
                {
                    string mySaveToFile = Application.persistentDataPath + "/Eternalia/" + mainPlayerName + "/" + nameSave + ".dat";
                    //	Debug.Log (mySaveToFile);
                    if (File.Exists(mySaveToFile))
                    {
         //               Debug.Log(mySaveToFile);
                        displayOverwrite = true;
                    }
                    else
                    {
                        initialTime = Time.realtimeSinceStartup;
                //        Debug.Log(initialTime);
                        Save();
                        counter = true;
                        
                    }
                }
            }
        }

        CheckSave();
    }

    void CheckSave()
    {
        if (counter == true)
        {
   //         Debug.Log(Time.realtimeSinceStartup);
            if (Time.realtimeSinceStartup - initialTime > 0.05f)
            {
                GetComponent<SaveGame>().savePostion = true;
                GetComponent<SaveGame>().TransitSaveData();
                GetFiles();
                counter = false;
            }
        }
    }

	void DisplayOverwrite()
	{
        
		GUI.DrawTexture (new Rect(overwriteXPos, saveYPos, Screen.width * 0.50f, Screen.height*0.4f), button);
		GUI.Label (new Rect(labelXPos, savesButtonY, Screen.width * 0.50f, Screen.height*0.5f), "File already exist. Would you like to overwrite file?");
	//	GUI.Button(new Rect(saveButton, saveYPos, Screen.width * 0.75f, Screen.height*0.6f), "File already exist. Would you like to overwrite file?");
		if (GUI.Button(new Rect(labelXPos, overwriteYPos, 150, 50), "Yes"))
		{
			Debug.Log (fileToDelete);
            files.Remove(fileToDelete);
            nameSave = fileToDelete;
			Save();
            GetFiles();
        }
		if (GUI.Button(new Rect(labelXPos + 150, overwriteYPos, 150, 50), "No"))
		{
			nameSave = "";
			displayOverwrite = false;
		}
	}

    void GetFiles ()
    {
        if (Directory.Exists(mySaveDirectory))
        {
            files.Clear();
            string[] tempfiles = Directory.GetFiles(mySaveDirectory);
            /*
            foreach (string t in tempfiles)
            {
                files.Add(t);

            }*/

            for (int cnt = 0; cnt < tempfiles.Length; cnt++)
            {
                
                files.Add(tempfiles[cnt]);
            }

            for (int cnt = 0; cnt < files.Count; cnt++)
            {

                DialogueLua.SetActorField("File" + cnt.ToString(), "delete", "no");
            }

        }
        else
        {
             Directory.CreateDirectory(mySaveDirectory);
        }
    }

	void DisplayDone()
	{
        /*
		GUI.Label (new Rect(labelXPos, savesButtonY, Screen.width * 0.50f, Screen.height*0.5f), "Saved Successfully");
		//	GUI.Button(new Rect(saveButton, saveYPos, Screen.width * 0.75f, Screen.height*0.6f), "File already exist. Would you like to overwrite file?");
		if (GUI.Button(new Rect(labelXPos, overwriteYPos, 150, 50), "Close"))
		{
			displaySaveDone = false;
		}*/
	}

	public void Save()
	{
        Scene sceneCurrent = SceneManager.GetActiveScene();
        GetComponent<SaveGame>().loadPosition = "Y";
    //    Debug.Log(nameSave + "/" + sceneCurrent.name);
		GetComponent<SaveGame>().NotAutoSave (nameSave, sceneCurrent.name);
   //     GetComponent<SaveGame>().loadPosition = "N";
    }
	
	void DisplayLoad()
	{
        GUI.Label(new Rect(saveXPos, savesButtonY - savesButtonHeight, savesButtonLength, savesButtonHeight), "SAVED GAMES");
        if (GUI.Button(new Rect(Screen.width * 0.55f, savesButtonY, savesButtonLength, savesButtonHeight), "DELETE FILES"))
        {
            Debug.Log(deleteFilesOn);
            deleteFilesOn =! deleteFilesOn;
            if (deleteFilesOn == true)
            {
                Debug.Log("DeleteFilesOn");

                GetFiles();
            }
        }

        loadRectSlide = GUI.BeginScrollView(loadRect, loadRectSlide, internalLoadRect);
        for (int cnt = 0; cnt < files.Count; cnt++)
        {
            if (GUI.Button (new Rect (saveXPos, savesButtonY + (cnt * savesButtonHeight), savesButtonLength, savesButtonHeight), Path.GetFileNameWithoutExtension(files[cnt].ToString())) && displayLoadWarning == false)
			{
                mySaveDirectory = Application.persistentDataPath + "/Eternalia/" + mainPlayerName;
                if (displayLoadWarning == false)
				{
					chosenFileToLoad = Path.GetFileNameWithoutExtension(files[cnt].ToString());
                    Debug.Log(chosenFileToLoad);
                    //Display Overwrite?
                    displayLoadWarning = true;
				}
			}
		}

        if (deleteFilesOn == true)
        {
            DeleteFileOn();
        }

        GUI.EndScrollView();

        if (deleteFilesOn == true)
        {
            if (GUI.Button(new Rect((int)(Screen.width * 0.55f), savesButtonY + (savesButtonHeight), 100, 100), "DELETE!!"))
            {
                Debug.Log("Delete");
                for (int cnt = 0; cnt < files.Count; cnt++)
                {
                    if (DialogueLua.GetActorField("File" + cnt.ToString(), "delete").AsString == "yes")
                    {
                        //            Debug.Log("delete" + files[cnt]);
                        File.Delete(files[cnt]);
                        deleteFilesOn = false;
                        GetFiles();
                    }
                    else
                    {
                        Debug.Log("do not delete" + files[cnt]);
                    }
                }
            }
        }


        if (displayLoadWarning == true)
		{
			DisplayLoadWarning();
		}



	}

    void DeleteFileOn ()
    {
        for (int cnt = 0; cnt < files.Count; cnt++)
        {
            Texture textureMarked =  (Texture)(Resources.Load("GUI/Marked" + DialogueLua.GetActorField("File" + cnt.ToString(), "delete").AsString, typeof(Texture)));
            if (GUI.Button(new Rect((int)(Screen.width * 0.04f), savesButtonY + (cnt * savesButtonHeight), 50, 50), textureMarked))
            {
                if (DialogueLua.GetActorField("File" + cnt.ToString(), "delete").AsString == "no")
                {
                    DialogueLua.SetActorField("File" + cnt.ToString(), "delete", "yes");
                }
                else
                {
                    DialogueLua.SetActorField("File" + cnt.ToString(), "delete", "no");
                }
            }
        }
    }

	void DisplayLoadWarning()
	{
		GUI.DrawTexture (new Rect(overwriteXPos, saveYPos, Screen.width * 0.50f, Screen.height*0.4f), button);
		GUI.Label (new Rect(labelXPos, savesButtonY, Screen.width * 0.50f, Screen.height*0.5f), "All unsaved progress will be lost");
		GUI.Label (new Rect(labelXPos, savesButtonY + 25, Screen.width * 0.50f, Screen.height*0.5f), "Would you like to continue loading this game?");
		//	GUI.Button(new Rect(saveButton, saveYPos, Screen.width * 0.75f, Screen.height*0.6f), "File already exist. Would you like to overwrite file?");
		if (GUI.Button(new Rect(labelXPos, overwriteYPos, 150, 50), "Yes"))
		{
			Debug.Log ("Load");
            Time.timeScale = 1;
			Load ();
		}
		if (GUI.Button(new Rect(labelXPos + 150, overwriteYPos, 150, 50), "No"))
		{
            if (chosenFileToLoad != null && chosenFileToLoad != "")
            {
                displayLoadWarning = false;
            }			
		}
	}

    private void Load()
    {
        //First Save the right path
        string path = Application.persistentDataPath + "/Eternalia";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        BinaryFormatter bf2 = new BinaryFormatter();
        string fileName2 = path + "/CurrentProfile.dat";
        FileStream file2 = File.Create(fileName2);
         CurrentProfile data2 = new CurrentProfile();

        mainPlayerName = DialogueLua.GetActorField("Player", "playerName").AsString;
        //	Debug.Log (mainPlayerName);

        data2.currentProfile = mainPlayerName;
        data2.saveFileName = chosenFileToLoad;
        data2.loadPosition = loadPosition;
        data2.scene = scene;

        bf2.Serialize(file2, data2);
        file2.Close();

        //Second get the right scene from saved file
        string fileName3 = path + "/" + mainPlayerName + "/" + chosenFileToLoad + ".dat";
        Debug.Log(fileName3);
        if (File.Exists(fileName3))
        {
            BinaryFormatter bf3 = new BinaryFormatter();
            FileStream file3 = File.Open(fileName3, FileMode.Open);
            saveQuest data3 = (saveQuest)bf3.Deserialize(file3);
            file3.Close();
            string sceneToLoad = data3.currentScene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }
	
	void DisplayDifficulty()
	{
        GUI.Label(new Rect(Screen.width * 0.07f, Screen.height * 0.25f, buttonLength, buttonHeight), "DIFFICULTY");

        if (showDifficulty == false)
        {
            if (GUI.Button(new Rect(Screen.width * 0.07f, Screen.height * 0.25f + buttonHeight, buttonLength, buttonHeight), difficulty))
            {
                showDifficulty = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width * 0.07f, Screen.height * 0.25f + buttonHeight, buttonLength, buttonHeight), "Easy"))
            {
                difficulty = "Easy";
                textDifficulty = easy;
                showDifficulty = false;
            }
            if (GUI.Button(new Rect(Screen.width * 0.07f, Screen.height * 0.25f + (2 * buttonHeight), buttonLength, buttonHeight), "Average"))
            {
                difficulty = "Average";
                textDifficulty = normal;
                showDifficulty = false;
            }
            if (GUI.Button(new Rect(Screen.width * 0.07f, Screen.height * 0.25f + (3 * buttonHeight), buttonLength, buttonHeight), "Hard"))
            {
                difficulty = "Hard";
                textDifficulty = hard;
                showDifficulty = false;
            }
        }
        GUI.Label(new Rect(Screen.width * 0.07f, Screen.height * 0.45f, Screen.width * 0.35f, Screen.height * 0.3f), textDifficulty);


        GUI.Label(new Rect(Screen.width * 0.47f, Screen.height * 0.25f, buttonLength, buttonHeight), "REALISM");
        if (showRealism == false)
        {
            if (GUI.Button(new Rect(Screen.width * 0.47f, Screen.height * 0.25f + buttonHeight, buttonLength, buttonHeight), realism))
            {
                showRealism = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width * 0.47f, Screen.height * 0.25f + buttonHeight, buttonLength, buttonHeight), "Fairy Tale"))
            {
                realism = "Fairy Tale";
                textRealism = fairyTale;
                showRealism = false;
            }
            if (GUI.Button(new Rect(Screen.width * 0.47f, Screen.height * 0.25f + (2 * buttonHeight), buttonLength, buttonHeight), "Epic Fantasy"))
            {
                realism = "Epic Fantasy";
                textRealism = epicfantasy;
                showRealism = false;
            }
            if (GUI.Button(new Rect(Screen.width * 0.47f, Screen.height * 0.25f + (3 * buttonHeight), buttonLength, buttonHeight), "Life Is Hard"))
            {
                realism = "Life Is Hard";
                textRealism = lifeIsHard;
                showRealism = false;
            }
        }
        GUI.Label(new Rect(Screen.width * 0.47f, Screen.height * 0.45f, Screen.width * 0.35f, Screen.height * 0.3f), textRealism);
    }

    void DisplayExitGame ()
    {
        if (GUI.Button (new Rect (Screen.width * 0.40f, Screen.height* 0.5f, Screen.width * 0.20f, Screen.height * 0.10f), "EXIT GAME"))
        {
            Application.Quit();
        }
    }

   

    void LoadDeleteFileOption ()
    {

    }
}
