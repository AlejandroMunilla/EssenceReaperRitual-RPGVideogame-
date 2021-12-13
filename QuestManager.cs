using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class QuestManager : MonoBehaviour 
{
	string saveData;

	void Update () 
	{
		if (Input.GetKeyUp(KeyCode.T))
		{
			Save ();
		}

		if (Input.GetKeyUp(KeyCode.G))
		{
			Load ();
		}
	}


	void Save ()
	{
		saveData = PersistentDataManager.GetSaveData();
		
		Debug.Log (saveData);
		
		BinaryFormatter bf = new BinaryFormatter();
		
		string fileName = @"C:/SaveGame/Quests"  ;
		
		FileStream file = File.Create(fileName);
		
		saveQuest data = new saveQuest();
		
		data.saveData = saveData;
		
		bf.Serialize (file, data);		
		
		file.Close ();
	}

	void Load ()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open (@"C:/SaveGame/Quests" , FileMode.Open);
		saveQuest data = (saveQuest) bf.Deserialize(file);
		file.Close();

		saveData = data.saveData;

		DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);

		Debug.Log (saveData);
	}
}





