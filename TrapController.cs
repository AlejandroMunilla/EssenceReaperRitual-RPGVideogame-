///This script will get the info sent by each trap to keep a list of traps
///in the escene. Every x second, it will check distance with trap with each
///PC and if within distance -> do a dice roll to detect traps. 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class TrapController : MonoBehaviour {

    public List<GameObject> traps = new List<GameObject>();
    public List<GameObject> secrets = new List<GameObject>();
    

    void Start ()
    {
	    //We need to add players, but from LoadGame!!
        foreach (GameObject go in traps)
        {
            Debug.Log(go);
        }
               
	}

    public void CheckPerPlayer (GameObject player)
    {
        foreach (GameObject trap in traps)
        {
            if (trap.GetComponent<Trap>().discovered == false)
            {
                float distance = Vector3.Distance(player.transform.position, trap.transform.position);
                if (distance <= 12.0f)
                {
                    CheckSkill(player, trap);
                }
            }
        }

        foreach (GameObject secret in secrets)
        {
            if (secret.GetComponent<SecretDoor>().discovered == false)
            {
                float distance = Vector3.Distance(player.transform.position, secret.transform.position);
                if (distance <= 12.0f)
                {
                    CheckSkillSecret(player, secret);
                }
            }
        }
    }

    void CheckSkill (GameObject player, GameObject trap)
    {
        if (trap.GetComponent<Trap>() != null)
        {
            if (trap.GetComponent<Trap>().numberAttempt > 16)
            {

                return;
            }
            else
            {
                CheckDificulty(trap);
                trap.GetComponent<Trap>().numberAttempt++;
                int skill = player.GetComponent<PlayerStats>().totDetect;
                if (skill <= 0)
                {
                    skill = -20;
                }
                int difficulty = trap.GetComponent<Trap>().detectDifficulty;
                int dice = Random.Range(1, 101);


                if (dice < (skill - difficulty))
                {
                    //     disarmed = true;
                    string nameObj = trap.name + "Trap";
                    string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    //       DialogueLua.SetActorField(scene, nameObj, "No");

                    if (trap.GetComponent<Trap>().discovered == false)
                    {
                        trap.GetComponent<Trap>().Discover();
                        GetComponent<DisplayInfo>().AddText(player.name + " discovered a trap! Skill :" + skill + "/dice roll : " + dice + "/Difficulty: " + difficulty);
                        if (player.name == "Player")
                        {
                            string nameToDisplay = DialogueLua.GetActorField("Player", "playerName").AsString;
                            DialogueManager.ShowAlert(nameToDisplay + " discovered a trap!");
                        }
                        else
                        {
                            DialogueManager.ShowAlert(player.name + " discovered a trap!");
                        }

                    }
                }
            }
        }
        else if (trap.GetComponent<TrapFloor>() != null)
        {

        }

    }

    void CheckSkillSecret (GameObject player, GameObject secret)
    {        
        int skill = player.GetComponent<PlayerStats>().totDetect;
        int difficulty = secret.GetComponent<SecretDoor>().detectDifficulty;
        int dice = Random.Range(1, 101);
//        GetComponent<DisplayInfo>().AddText("Skill :" + skill + "/dice roll : " + dice + "/Difficulty: " + difficulty);
        
        if (dice < (skill - difficulty))
        {
            //     disarmed = true;
            string nameObj = secret.name + "Secret";
            string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            DialogueManager.ShowAlert(player.name + " discovered a secret!");
            GetComponent<DisplayInfo>().AddText(player.name + " discovered a secret! Skill :" + skill + "/dice roll : " + dice + "/Difficulty: " + difficulty);
            DialogueLua.SetActorField(scene, nameObj, "Revealed");

            if (secret.GetComponent<SecretDoor>().discovered == false)
            {
              secret.GetComponent<SecretDoor>().Discover();
            }
        }
    }

    void CheckDificulty(GameObject trap)
    {
        if (trap.GetComponent<Trap>().numberAttempt == 6)
        {
            trap.GetComponent<Trap>().difficulty = trap.GetComponent<Trap>().difficulty - 5;
        }
        else if (trap.GetComponent<Trap>().numberAttempt == 11)
        {
            trap.GetComponent<Trap>().difficulty = trap.GetComponent<Trap>().difficulty - 5;
        }

    }
}
