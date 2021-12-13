/// <summary>
/// AMG, Nov 15th, 2016
/// this class is responsible to Save games, either Autosaves, quicksaves or player named saves. 
/// </summary>
using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour 
{
	public delegate void SaveAllChests (string fileDirectory);
	public static event SaveAllChests OnSaveAllChests;
    public bool savePostion = true;
	public string loadPosition = "N";
	public string scene;
	public string path;
	public GameObject mainPlayer;
	public GameObject PC2Obj;
	public GameObject PC3Obj;
	public GameObject PC4Obj;
	public GameObject PC5Obj;	
	public Vector3 pos01 = new Vector3 (23.90f, 3.43f, 28.93f);
	public Vector3 pos02 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos03 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos04 = new Vector3 (23.10f, 3.43f, 29.93f);
	public Vector3 pos05 = new Vector3 (23.10f, 3.43f, 29.93f);	
	public string PC1;
	public string PC2;
	public string PC3;
	public string PC4;
	public string PC5;
	public string saveDirectory;
    public int x1;
    public int y1;
    public int z1;
    public int x2;
    public int y2;
    public int z2;
    public int x3;
    public int y3;
    public int z3;
    public int x4;
    public int y4;
    public int z4;
    public int x5;
    public int y5;
    public int z5;
    

    private string fileDirectoryVar;
    private string mainPlayerName;
    private GameObject gc;
    private string saveData;
    private string saveFileName;
    private string previousFileDirectory;
    private Quaternion rot = new Quaternion(0, 180, 0, 0);
    private GameController gameController;
    private GameObject mainCamera;
    public bool saveDone = false;


	void Start ()
	{
		gameController = GetComponent <GameController>();
	}

    void OnDisable ()
    {
        if (GetComponent<TrapController>())
        {
            GetComponent<TrapController>().StopAllCoroutines();
            foreach (GameObject trap in GetComponent<TrapController>().traps)
            {
                Destroy(trap);
            }
        }
    }

    public void NotAutoSave (string fileDirectory, string sceneToSave)
    {
 //       Debug.Log(fileDirectory +"/" + sceneToSave);
        fileDirectoryVar = fileDirectory;
        scene = sceneToSave;
        SaveCurrentProfile(fileDirectory);
        SaveScene();
        SavePosition();
        SaveInventory();
        ChangeDirectory(fileDirectory);
        Invoke("TransitSaveData", 0.1f);

    }

	public void SaveAllData (string fileDirectory, string sceneToSave)
	{
   //     Debug.Log("Savealldata");
        fileDirectoryVar = fileDirectory;
		scene = sceneToSave;
//		mainPlayerName = DialogueLua.GetActorField ("Player", "playerName").AsString;
		SaveCurrentProfile(fileDirectory);
		SaveScene ();
		SavePosition();
		SaveInventory();
   //     Invoke("TransitSaveData", 0.05f);       
		ChangeDirectory(fileDirectory);		
	}
	
	void SaveCurrentProfile(string fileDirectory)
	{
        string path = Application.persistentDataPath + "/Eternalia";
 //       Debug.Log(path);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        BinaryFormatter bf2 = new BinaryFormatter();
		string fileName = path + "/CurrentProfile.dat";
   //     Debug.Log(fileName);
        FileStream file = File.Create(fileName);
		BinaryFormatter bf = new BinaryFormatter();
		
		CurrentProfile data = new CurrentProfile ();

		mainPlayerName = DialogueLua.GetActorField ("Player", "playerName").AsString;
//		Debug.Log (mainPlayerName);
	
		data.currentProfile = mainPlayerName;
		data.saveFileName = fileDirectory;
		data.loadPosition = loadPosition;
		data.scene = scene;	

		bf.Serialize (file, data);			
		file.Close ();

        Invoke("TransitSaveData", 0.1f);
	}

	private void SaveScene ()
	{
        Scene sceneCurrent = SceneManager.GetActiveScene();
 //       Debug.Log(sceneCurrent.name);
		DialogueLua.SetVariable ("sceneSave", sceneCurrent.name);

        if (gameController != null)
        {
            foreach (GameObject go in gameController.players)
            {
                DialogueLua.SetActorField(go.name, "curHealth", go.GetComponent<PlayerStats>().curHealth);
            }
        }

	}
	
	private void SaveInventory ()
	{
		string genInventory = "";
        string miscItems = "";
        DisplayItemScript displayItemScript = GetComponent<DisplayItemScript>();

        if(displayItemScript)
        {
            if (displayItemScript.inventoryItems.Count > 0)
            {
                for (int cnt = 0; cnt < displayItemScript.inventoryItems.Count; cnt++)
                {
                    if (cnt == 0)
                    {
                        genInventory = displayItemScript.inventoryItems[cnt];
                    }
                    else if (cnt > 0)
                    {
                        genInventory = genInventory + "*" + displayItemScript.inventoryItems[cnt];
                    }
                }
            }

            if (displayItemScript.miscItemsList.Count > 0)
            {
                for (int cnt = 0; cnt < displayItemScript.miscItemsList.Count; cnt++)
                {
                    if (cnt == 0)
                    {
                        miscItems = displayItemScript.miscItemsList[cnt];
                    }
                    else if (cnt > 0)
                    {
                        miscItems = miscItems + "*" + displayItemScript.miscItemsList[cnt];
                    }
                }
            }

            DialogueLua.SetVariable("GeneralInventory", genInventory);
            DialogueLua.SetVariable("miscInventory", miscItems);
    //        Debug.Log(genInventory);
        }

        DisplayAssets assets = GetComponent<DisplayAssets>();
        string unitVariable = null;
        if (GetComponent<DisplayAssets>() != null)
        {
            if (assets.units.Count > 0)
            {
                for (int cnt = 0; cnt < assets.units.Count; cnt++)
                {
                    if (cnt == 0)
                    {
                        unitVariable = assets.units[cnt];
                    }
                    else if (cnt > 0)
                    {
                        unitVariable = unitVariable + "*" + assets.units[cnt];
                    }
                }
        //        Debug.Log(unitVariable);
                DialogueLua.SetVariable("units", unitVariable);
            }

            string buildings = null;
            if (assets.buildings.Count > 0)
            {
                for (int cnt = 0; cnt < assets.buildings.Count; cnt++)
                {
                    if (cnt == 0)
                    {
                        buildings = assets.buildings[cnt];
                    }
                    else if (cnt > 0)
                    {
                        buildings = unitVariable + "*" + assets.buildings[cnt];
                    }
                }
       //         Debug.Log(buildings);
                DialogueLua.SetVariable("buildings", buildings);
            }

            string allies = null;
            if (assets.allies.Count > 0)
            {
                for (int cnt = 0; cnt < assets.allies.Count; cnt++)
                {
                    if (cnt == 0)
                    {
                        allies = assets.allies[cnt];
                    }
                    else if (cnt > 0)
                    {
                        allies = allies + "*" + assets.allies[cnt];
                    }
                }
                Debug.Log(allies);
                DialogueLua.SetVariable("allies", allies);
            }
        }
        else
        {
            Debug.LogWarning("No display Aseets!!");
        }



    }


    public void TransitSaveData ()
    {
    //    Debug.Log(fileDirectoryVar);
        SaveData(fileDirectoryVar);
        SavePosition();
    }
	
	void SaveData (string fileDirectory)
	{
   //     Debug.Log(DialogueLua.GetActorField ("Player", "stren").AsString);
        string path = Application.persistentDataPath + "/Eternalia";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string mainPlayerName = DialogueLua.GetActorField ("Player", "playerName").AsString;

        if (mainPlayerName == "" || mainPlayerName == null)
        {
            string nameEmergency = gameController.player.name;
            if (nameEmergency == "" || nameEmergency == null)
            {
                mainPlayerName = "Balder2";
                Debug.Log("Balder2");
            }
            else
            {
                mainPlayerName = nameEmergency;
                Debug.Log("Emergency: " + mainPlayerName);
            }
        }
        //	string fileName = @"C:/SaveGame/" + mainPlayerName + "/" + fileDirectory + ".dat";	
        string fileName = path + "/" + mainPlayerName + "/" + fileDirectory + ".dat";
  //      Debug.Log(fileName);
        if (!Directory.Exists (path + "/" + mainPlayerName) )
		{
			Directory.CreateDirectory (path + "/" + mainPlayerName); //+ "/" + fileDirectory);
		}

        saveData = PersistentDataManager.GetSaveData();
		BinaryFormatter bf = new BinaryFormatter();		
		FileStream file = File.Create(fileName);		
		saveQuest data = new saveQuest();		
		data.saveData = saveData;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentScene = currentScene;
		bf.Serialize (file, data);			
		file.Close ();


        saveDone = true; 
    }
	
	void ChangeDirectory(string fileDirectory)
	{
        Scene scene = SceneManager.GetActiveScene();
        
		if (scene.name != "02CharCreation")
		{
            if (scene.name != "RTSPreBattle")
            {
                if (GetComponent<GameController>() != null)
                {
                    GetComponent<GameController>().saveDirectory = fileDirectory;
                }
            }			
		}
	}

    //This is used only when "Save" is used in game. 
	void SavePosition ()
	{
        if (savePostion == true)
        {
            if (gameController != null)
            {
                if (gameController.player != null)
                {

                    GameObject mainPlayer = gameController.player;
                    if (gameController.PC2Obj != null)
                    {
                        PC2Obj = gameController.PC2Obj;
                    }
                    if (gameController.PC3Obj != null)
                    {
                        PC3Obj = gameController.PC3Obj;
                    }
                    Vector3 positionSave = mainPlayer.transform.position;
                    DialogueLua.SetActorField("Player", "X", mainPlayer.transform.position.x);
                    DialogueLua.SetActorField("Player", "Y", mainPlayer.transform.position.y);
                    DialogueLua.SetActorField("Player", "Z", mainPlayer.transform.position.z);
                    DialogueLua.SetActorField(gameController.PC2, "X", PC2Obj.transform.position.x);
                    DialogueLua.SetActorField(gameController.PC2, "Y", PC2Obj.transform.position.y);
                    DialogueLua.SetActorField(gameController.PC2, "Z", PC2Obj.transform.position.z);
                    DialogueLua.SetActorField(gameController.PC3, "X", mainPlayer.transform.position.x);
                    DialogueLua.SetActorField(gameController.PC3, "Y", mainPlayer.transform.position.y);
                    DialogueLua.SetActorField(gameController.PC3, "Z", mainPlayer.transform.position.z);
                    DialogueLua.SetActorField(gameController.PC4, "X", mainPlayer.transform.position.x);
                    DialogueLua.SetActorField(gameController.PC4, "Y", mainPlayer.transform.position.y);
                    DialogueLua.SetActorField(gameController.PC4, "Z", mainPlayer.transform.position.z);
                    DialogueLua.SetActorField(gameController.PC5, "X", mainPlayer.transform.position.x);
                    DialogueLua.SetActorField(gameController.PC5, "Y", mainPlayer.transform.position.y);
                    DialogueLua.SetActorField(gameController.PC5, "Z", mainPlayer.transform.position.z);
                    //		Debug.Log (transform.position.x + "/"+ transform.position.y);
                }
            }            
        }
	}

    void SaveCharacterData ()
    {
        if (gameController != null)
        {
            foreach (GameObject go in gameController.players)
            {
                DialogueLua.SetActorField(go.name, "curHealth", go.GetComponent<PlayerStats>().curHealth);
            }
        }

    }

    //This is used when the next level might have more than one entry point, and provide with the right initial position
    public void PositionNextLevel (int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3, int x4, int y4, int z4, int x5, int y5, int z5)
    {
   //     Debug.Log(x1 + "/" + y1 + "/" + z1);
        DialogueLua.SetActorField ("Position", "load", "Yes");
        DialogueLua.SetActorField("Position", "x1", x1);
        DialogueLua.SetActorField("Position", "y1", y1);
        DialogueLua.SetActorField("Position", "z1", z1);
        DialogueLua.SetActorField("Position", "x2", x2);
        DialogueLua.SetActorField("Position", "y2", y2);
        DialogueLua.SetActorField("Position", "z2", z2);
        DialogueLua.SetActorField("Position", "x3", x3);
        DialogueLua.SetActorField("Position", "y3", y3);
        DialogueLua.SetActorField("Position", "z3", z3);
        DialogueLua.SetActorField("Position", "x4", x4);
        DialogueLua.SetActorField("Position", "y4", y4);
        DialogueLua.SetActorField("Position", "z4", z4);
        DialogueLua.SetActorField("Position", "x5", x5);
        DialogueLua.SetActorField("Position", "y5", y5);
        DialogueLua.SetActorField("Position", "z5", z5);
    }
}

[Serializable]
class SaveNames
{
	public string saveName;
	public string playerName;
	public string scene;
	public string loadPosition;
}

[Serializable]
class saveQuest
{
	public string saveData;
	public string currentScene;
	public string saveName;
}
