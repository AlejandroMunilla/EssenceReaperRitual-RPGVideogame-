using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ExpController : MonoBehaviour 
{
	public int exp = 10000;
	public int level = 1;
    public int nextLevelExp = 22000;
    public int trueLevel;
	private GameObject gc;
	private GameController gameController;
    private DisplayPortraits displayPortraits;




	public void Start () 
	{
		gc = GameObject.FindGameObjectWithTag ("GameController");
		gameController = gc.GetComponent <GameController>();
        displayPortraits = GetComponent<DisplayPortraits>();
		InvokeRepeating ("Transit", 0.2f, 0.1f);
	}

	private void Transit ()
	{

        if (gc.GetComponent<DisplayToolBar>().enabled == true)
        {
            EndStart();
            CancelInvoke();
        }
    }

	private void EndStart ()
	{
       	level = DialogueLua.GetActorField ("Player", "level").AsInt;
    //   Debug.Log(level);
        if (level == 0)
        {
            Debug.Log(level);
            level = 1;
            DialogueLua.SetActorField("Player", "level", 1);
        }
        exp = DialogueLua.GetActorField("Player", "experience").AsInt;
     //   Debug.Log(level + "/" + exp);
        trueLevel = level;
        CheckLevel();
    }

	public void AdjustExp (int addExp, int enemyLevel)
	{
        exp = DialogueLua.GetActorField("Player", "experience").AsInt;
        level = DialogueLua.GetActorField("Player", "level").AsInt;
        if (level < 1)
        {
            Debug.Log(level);
            level = 1;
            DialogueLua.SetActorField("Player", "level", 1);
        }
        //    float ratio = (float)(enemyLevel / level);
        int partyNo = gameController.players.Count;
        float addjustedExpFloat = addExp / partyNo;
        int addjustedExp = (int)(addExp);
        FinalExp(addjustedExp);

    //    Debug.Log(partyNo + "/" + addjustedExpFloat + "/" + addjustedExp);
    }

    public void FinalExp (int finalExp)
    {
        exp = DialogueLua.GetActorField("Player", "experience").AsInt;
        exp = exp + finalExp;
        DialogueLua.SetActorField("Player", "experience", exp);
        gc.GetComponent<DisplayInfo>().AddText("Player has gained experience: " + finalExp + "\n");
     //   Debug.Log("Exp");
        CheckLevel();
    }

    public void CheckLevel ()
    {
        
        if  (exp < 22000)
        {
            nextLevelExp = 22000;
            trueLevel = 1;
            CheckLevelPartyMembers();
        }
        else if (exp >= 22000 && exp < 40000)
        {
            trueLevel = 2;
            nextLevelExp = 40000;
            CheckLevelPartyMembers();
        }
        else if (exp >= 40000 && exp < 70000)
        {
            trueLevel = 3;
            nextLevelExp = 70000;
            CheckLevelPartyMembers();
        }
        else if (exp >= 70000 && exp < 100000)
        {
            trueLevel = 4;
            nextLevelExp = 100000;
            CheckLevelPartyMembers();
        }
        else if (exp >= 100000 && exp < 135000)
        {
            trueLevel = 5;
            nextLevelExp = 135000;
            CheckLevelPartyMembers();
        }

    //    Debug.Log(exp + "/" + nextLevelExp);

    }

    void CheckLevelPartyMembers ()
    {
        
        foreach (GameObject go in GetComponent<GameController>().players)
        {
            
            int currentLevel = DialogueLua.GetActorField(go.name, "level").AsInt;
      //      Debug.Log(trueLevel + "/" + currentLevel + "/" + go.name);
            if (trueLevel > currentLevel)
            {
                DialogueLua.SetActorField(go.name, "level", trueLevel);
        //        
        //   LevelUp(go);
            }
        }
    }

    void LevelUp (GameObject partyMember)
    {
        Debug.Log("LevelUp");
        displayPortraits.LevelUpMember(partyMember);
    }
    /*
    void Update()
    {
        if (Input.GetKeyUp (KeyCode.E))
        {
            CheckLevel();
        }
    }*/
}
