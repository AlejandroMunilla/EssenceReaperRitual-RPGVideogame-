using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadMainMenu : MonoBehaviour {

    private List<string> files = new List<string>();
    private List<string> folders = new List<string>();
    private string mySaveDirectory;
    private string myProfile;
    private bool displayProfile = true;
    private int buttonWidth;
    private int buttonHeight;
    private int posX;
    private int posY;
    private Vector2 loadRectSlide;
    private Rect loadRect;
    private Rect internalLoadRect;
    private Vector2 loadRectSlideProfile;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(Application.persistentDataPath);
        buttonWidth = (int)(Screen.width * 0.20f);
        buttonHeight = (int)(Screen.height * 0.06f);
        posX = (int)(Screen.width * 0.12f);
        posY = (int)((Screen.height * 0.25f));
        loadRect = new Rect(0, Screen.height * 0.22f, (Screen.width - posX), Screen.height * 0.76f);
        //  ( ,  ,  , lo que se ve en pantalla );
        internalLoadRect = new Rect(0, Screen.height * 0.18f, Screen.width * 0.45f, Screen.height * 1.5f);
        mySaveDirectory = Application.persistentDataPath + "/Eternalia/";

        if (Directory.Exists(mySaveDirectory))
        {
            folders.Clear();
            string[] tempfiles = Directory.GetDirectories (mySaveDirectory);
            foreach (string t in tempfiles)
            {
    //            Debug.Log(Path.GetDirectoryName (t));
                string[] arrayPC = t.Split(new string[] { "/" }, System.StringSplitOptions.None);
                for (int cnt = 0; cnt< arrayPC.Length; cnt ++)
                {
                    if (cnt == arrayPC.Length -1)
                    {
                        folders.Add(arrayPC[cnt]);
  //                      Debug.Log(arrayPC[cnt]);
                    }
                }
                
            }
        }
        else
        {
            Directory.CreateDirectory(mySaveDirectory);
            folders.Clear();
        }

        UpdateInternalRect(folders.Count);
    }

    public void DisplayAll ()
    {
        if (displayProfile == true)
        {
            DisplayProfiles();
        }
        else
        {
            DisplaySaves();
        }
        DisplayBackButton();
    }

    void DisplayProfiles()
    {
        GUI.Label (new Rect(posX, posY - buttonHeight, buttonWidth, buttonHeight), "PROFILES");

        loadRectSlide = GUI.BeginScrollView(loadRect, loadRectSlide, internalLoadRect);

        for (int cnt = 0; cnt < folders.Count; cnt++)
        {
            if (GUI.Button(new Rect(posX, posY + (cnt * buttonHeight), buttonWidth, buttonHeight), folders[cnt]))
            {
                myProfile = Application.persistentDataPath + "/Eternalia/" + folders[cnt];
                GetSavesInProfile();
                displayProfile = false;
                UpdateInternalRect(files.Count);
            }
        }

        GUI.EndScrollView();
    }

    void DisplaySaves()
    {
        GUI.Label(new Rect(posX, (Screen.height * 0.25f) - buttonHeight, buttonWidth, buttonHeight), "SAVED GAMES");

        loadRectSlide = GUI.BeginScrollView(loadRect, loadRectSlide, internalLoadRect);

        for (int cnt = 0; cnt < files.Count; cnt++)
        {
            if (GUI.Button(new Rect(posX, posY + (cnt * buttonHeight), buttonWidth, buttonHeight), files[cnt]))
            {
                Load(files[cnt]);
            }
        }
        GUI.EndScrollView();
    }

    void DisplayBackButton ()
    {
        if (GUI.Button (new Rect (Screen.width * 0.75f, Screen.height * 0.20f, Screen.width * 0.15f, Screen.height * 0.08f), "BACK TO MENU"))
        {
            GetComponent<MainMenu>().displayLoad = false;
            displayProfile = true;
            this.enabled = false;
        }
    }

    void GetSavesInProfile()
    {
        if (Directory.Exists(myProfile))
        {
            files.Clear();
            string[] tempfiles = Directory.GetFiles(myProfile);
            foreach (string t in tempfiles)
            {
                files.Add(Path.GetFileNameWithoutExtension (t));
     //           Debug.Log(Path.GetFileNameWithoutExtension(t));
            }
        }
        else
        {
            Directory.CreateDirectory(myProfile);
        }
    }

    private void Load(string file)
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

        string mainPlayerName = Path.GetFileNameWithoutExtension(myProfile);
        //	Debug.Log (mainPlayerName);

        data2.currentProfile = mainPlayerName;
        data2.saveFileName = file;
        data2.loadPosition = "Y";
        data2.scene = "MainMenu";

        bf2.Serialize(file2, data2);
        file2.Close();

        //Second get the right scene from saved file
        string fileName3 = Application.persistentDataPath + "/Eternalia/" + mainPlayerName + "/" + file + ".dat";
        Debug.Log(mainPlayerName);
        if (File.Exists(fileName3))
        {
            BinaryFormatter bf3 = new BinaryFormatter();
            FileStream file3 = File.Open(fileName3, FileMode.Open);
            saveQuest data3 = (saveQuest)bf3.Deserialize(file3);
            file3.Close();
            string sceneToLoad = data3.currentScene;
   //         Debug.Log(sceneToLoad);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void UpdateInternalRect (int cnt)
    {
        int internalHeight = (buttonHeight * cnt) + (2 * buttonHeight);
        internalLoadRect = new Rect(0, Screen.height * 0.18f, Screen.width * 0.45f, internalHeight);
    }

}
