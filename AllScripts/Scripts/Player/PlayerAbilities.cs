using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour 
{
    public string abilitiesList;
	public string ability1 = null;
	public string ability2 = null;
	public string ability3 = null;
	public string spell = "no";
	public string abilitySpecial = "no";
    public List<string> specialList = new List<string>();
    public List<string> spells = new List<string>();
    public Texture TextAbi1;
	public Texture TextAbi2;
	public Texture TextAbi3;

	public Texture TextSpells;
	public Texture specialAbilities;
	public Texture specialObjects;

    public float timeAbi1;
    public float timeAbi2;
    public float timeAbi3;

	private GameObject gc;
	private GameController gameController;


	void Start () 
	{
        LoadLua();
		spell = "No";
		abilitySpecial = "Yes";
        specialList = new List<string>();
        spells = new List<string>();
        SetUpSpells();
    }    

    private void LoadLua ()
    {
        ability1 = DialogueLua.GetActorField(gameObject.name, "SpecialAbility").AsString;
        
        if (ability1 == null || ability1 == "nill" || ability1 == "")
        {
            ability1 = "nill";
            TextAbi1 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
        }
        else
        {
            TextAbi1 = (Texture)(Resources.Load("Icons/Abilities/" + ability1));
        }


        ability2 = DialogueLua.GetActorField(gameObject.name, "SpecialAbility2").AsString;

        if (ability2 == null || ability2 == "nill" || ability2 == "")
        {
            ability2 = "nill";
            TextAbi2 = (Texture)(Resources.Load("Icons/GUI/DPad", typeof(Texture)));
        }
        else
        {
            TextAbi2 = (Texture)(Resources.Load("Icons/Abilities/" + ability2));
            int cooldown2 = DialogueLua.GetActorField(gameObject.name, "coolDown2").AsInt;
        }

        /*
        if (ability2 != null && ability2 != "nill" && ability2 != "")
        {
            //     Debug.Log(ability2);
            TextAbi2 = (Texture)(Resources.Load("Icons/Abilities/" + ability2));
            int cooldown2 = DialogueLua.GetActorField(gameObject.name, "coolDown2").AsInt;

        }*/

        DialogueLua.SetActorField(gameObject.name, "abiTime1", 0);
   //     Debug.Log(TextAbi1);
        int cooldown1 = DialogueLua.GetActorField(gameObject.name, "coolDown1").AsInt;
     //   Debug.Log(cooldown1);

        /*
     if (cooldown1 == 0)
        {
            DialogueLua.SetActorField(gameObject.name, "coolDown1", 15);
        }*/

     if (gameObject.name == "Fred")
        {
            DialogueLua.SetActorField("Fred", "coolDown1", 0);
        }

       if (gameObject.name == "Rose")
        {
            DialogueLua.SetActorField("Rose", "coolDown1", 0);
        }

        if (gameObject.name == "Aurelius")
        {
            DialogueLua.SetActorField("Aurelius", "coolDown1", 20);
        }

        if (gameObject.name == "Player")
        {
            DialogueLua.SetActorField("Player", "coolDown1", 15);
            DialogueLua.SetActorField("Player", "coolDown2", 15);
        }




        /*
        ability3 = DialogueLua.GetActorField(gameObject.name, "SpecialAbility3").AsString;
        TextAbi3 = (Texture)(Resources.Load("Icons/Abilities/" + ability3));*/
    //    Debug.Log(ability3);

        Invoke("SetUpTextures", 0);
     //   SetUpTextures();
    }



    public void SetUpSpells ()
    {
        spells.Clear();
        string spellsList = DialogueLua.GetActorField(gameObject.name, "spells").AsString;
     //   Debug.Log(spellsList);
        if (spellsList == "nil" || spellsList == "none" || spellsList == "")
        {
            spells = null;
        }
        else
        {          

            string[] spellsArray = spellsList.Split(new string[] { "*" }, System.StringSplitOptions.None);

            foreach (string st in spellsArray)
            {
       //         Debug.Log(st);
                spells.Add(st);
            }
        }


    }

	void SetUpTextures ()
	{
        StopAllCoroutines();
		TextSpells = (Texture)(Resources.Load ("Icons/Spells/Spells"+spell) );
		specialAbilities = (Texture)(Resources.Load ("Icons/Abilities/SpecialAbilities") );
		specialObjects = (Texture)(Resources.Load ("Icons/Abilities/SpecialObjectsYes") );
        SetUpAbilities();
        
	}

    void SetUpAbilities ()
    {
        if (ability1 == null || ability1 == "nill" || ability1 == "")
        {
        //    Debug.Log("null");

        }
        else
        {
            if (transform.Find("Ability/" + ability1) == null)
            {
            //        Debug.Log(ability1);
                GameObject ability2GO = Instantiate(Resources.Load("Abilities/" + ability1), transform.position, transform.rotation) as GameObject;
                ability2GO.name = ability1;
                ability2GO.transform.parent = transform.Find("Ability");

            }
        }

        if (ability2 == null || ability2 == "nill" || ability2 == "")
        {

        }
        else
        {
            if (transform.Find("Ability/" + ability2) == null)
            {
            //    Debug.Log(ability2);
                GameObject ability2GO = Instantiate(Resources.Load("Abilities/" + ability2), transform.position, transform.rotation) as GameObject;
                ability2GO.transform.parent = transform.Find("Ability");
                ability2GO.name = ability2;
                     
            }
        }
    }

}
