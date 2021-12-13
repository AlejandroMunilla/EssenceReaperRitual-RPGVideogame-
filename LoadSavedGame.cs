/// This class handles loading a saved game when using "Load" option.
/// by getting the right scene from Dialogue Manager. Once right scene is loaded, 
/// class "LoadGame" load all data automatically. 
/// AMG, March 20th, 2016

using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class LoadSavedGame : MonoBehaviour {

    private string mainPlayerName = null;
    private string saveData = null;


    public void Load(string fileToLoad)
    {
        LoadCurrentProfile(fileToLoad);
    }

    //We need to know current profile as it tells us the right character data to use
    private void LoadCurrentProfile(string fileToLoad)
    {
        string fileName = @"C:/SaveGame/CurrentProfile.dat";

        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            CurrentProfile data = (CurrentProfile)bf.Deserialize(file);
            file.Close();
            mainPlayerName = data.currentProfile;
            LoadQuests(fileToLoad);
        }
        else
        {

        }
    }

    //...once profile is known (which is mainPlayerName) we load the file, by serialization. 
    public void LoadQuests(string fileDirectory)
    {
        //	string mainPlayerName = DialogueLua.GetActorField ("Player", "playerName").AsString;		
        string fileName = @"C:/SaveGame/" + mainPlayerName + "/" + fileDirectory + ".dat";
        Debug.Log(fileName);

        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            saveQuest data = (saveQuest)bf.Deserialize(file);
            file.Close();

            saveData = data.saveData;         

           PersistentDataManager.ApplySaveData(saveData);
            //	DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);
            Debug.Log(fileName);
            StartCoroutine("Transit");	
        }
    }

    // We need at least one frame for data to be available on Dialogue Manager. Yield 0 seconds = 1 frame. 
    private IEnumerator Transit()
    {        
        yield return new WaitForSeconds(0);
        StopAllCoroutines();
        EndLoad();

    }

    // Load the right scene
    private void EndLoad()
    {
        string sceneToLoad = DialogueLua.GetVariable("sceneSave").AsString;
        Debug.Log(sceneToLoad);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
