using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DalilaController : MonoBehaviour 
{
	public string type = "Sceptre";		// such as Sword, Bow, etc
	public int level = 1;				// player level, at higher levels, Dalila becomes more powerful
	public int necroDamage = 1;

	private PlayerStats playerStats;
	private Dalila dalila;

	private string damage;
	private string skill;
	private string range;
	private string description;
	private GameController gameController;
	private string weaponName;


	void Awake ()
	{
        level = DialogueLua.GetActorField("Player", "level").AsInt;
        necroDamage = (int) (level / 3);	
        if (necroDamage < 1)
        {
            necroDamage = 1;
        }

	}

	void Start () 
	{
		gameController = GetComponent<GameController>();
		Invoke ("StartUp", 2);
	}

	public void StartUp()
	{
		weaponName = DialogueLua.GetActorField ("Player", "dalila").AsString;
		ChangeType (weaponName);
	}


	public void DescriptionUpdate()
	{
        string dalilaType = DialogueLua.GetActorField("Player", "dalila").AsString;
        TextAsset textAsset = (TextAsset)(Resources.Load("WeaponDescription/Dalila" + dalilaType, typeof(TextAsset)));
        string textAssetString = textAsset.text;
        GetComponent<DisplayItemScript>().textAssetString = textAssetString;

        /*
		string playerName = gameController.mainPlayerName;

		description = "DALILA" +
			"" +
				" Type: " + type +
				" Skill: " + skill +
				" Range: "+ range +
				" Damage: " + damage + 
				" Minimum Strenght = 4" +
				" Optimal Strenght = 4" +
				" Dalila causes Necro damage and ignore any Necro Resistance target might" +
				" have. Dalila uses the necro damage caused to vital esence to heal " + playerName + "." +
				"" +
				" DESCRIPTION"+
				" This object was with you when you were found with your brother as orphan babies, " +
				" with a note supposedly written by your mother - Dalila shall take care of you -." +
				" You have always thought this sceptre is a gift from your mother." +
				"" +
				" The sceptre is a complete mistery. It always stay with "  + playerName + " not" + 
				" matter what you do; you may throw it away or even into the fire, bury it, ..." +
				" at the end it does not matter: the sceptre will always find his way back, and" +
				" appear unexpectly by your side, especially if you are in troubles. " +
				"" +
				"" +
				" USABLE BY " +
				"" + playerName;
        GetComponent<DisplayItemScript>().textAssetString = description;
	//	string [] arrayString = description.Split (new string [] {"*"}, System.StringSplitOptions.None);
	//	GetComponent<DisplayItemScript>().arrayInfoItem = arrayString;	*/
    }	

	public void ChangeType (string weaponType)
	{
		if (weaponType == "Axe") {Axe();}
		else if (weaponType == "Bow") { Bow();}
		else if (weaponType == "Crossbow") {Crossbow ();}
		else if (weaponType == "Lance") {Lance ();}
		else if (weaponType == "Sword") {Sword (); } 
		else if (weaponType == "Scynthe") {Scynthe ();}
		else if (weaponType == "Knife") {Knife ();}
		else if (weaponType == "Maze") {Maze ();}
		else if (weaponType == "Sling") {Sling ();}
		else if (weaponType == "Stick") {Stick ();}
		else if (weaponType == "GreatSword") {GreatSword ();}
		else if (weaponType == "Sceptre") {Sceptre();}
		else if (weaponType == "sceptre")
		{
			Sceptre();
			type = "Sceptre";
			DialogueLua.SetActorField ("Player", "dalila", "Sceptre");
		}
	}

	public void ChangeObj (string weaponType)
	{

	}

	void Axe ()
	{
		damage = "1D8 + 1 (Slash / Piercing) + Necro Damage";
		type = "1Hand";
		skill = "Axe";
		range = "0.4m";
		damage = "1D8 + 1  (Slash / Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Bow ()
	{
		damage = "1D6 (Piercing) + Necro Damage";
		type = "2Hand";
		skill = "Bow";
		range = "20m";
		damage = "1D8 + 1  (Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Crossbow ()
	{
		damage = "1D4 + 3 (Piercing) + Necro Damage";
		type = "2Hand";
		skill = "Crossbow";
		range = "12m";
		damage = "1D4 + 3  (Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Lance ()
	{
		damage = "2D6 + 2 (Piercing) + Necro Damage";
		type = "2Hand";
		skill = "Lance";
		range = "1.5m";
		damage = "2D6 + 2  (Piercing) + Necro Damage";
		DescriptionUpdate();
	}
	

	void Sword ()
	{
		damage = "1D8 + 1 (Slash / Piercing) + Necro Damage";
		type = "1Hand";
		skill = "Sword";
		range = "0.5m";
		damage = "1D8 + 1 (Slash / Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Scynthe()
	{
		damage = "2D6 + 1 (Slash / Piercing) + Necro Damage";
		type = "2Hand";
		skill = "Scynthe";
		range = "1.2m";
		damage = "2D6 + 1 (Slash / Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Knife ()
	{
		damage = "1D6  (Piercing) + Necro Damage";
		type = "1Hand";
		skill = "Knife";
		range = "0.2m";
		damage = "1D4 + 2 (Slash / Piercing) + Necro Damage";
		DescriptionUpdate();
	}

	void Maze ()
	{
		damage = "1D8  (Crushing) + Necro Damage";
		type = "1Hand";
		skill = "Maze";
		dalila.range = "0.5m";
		damage = "1D8 (Crushing) + Necro Damage";
		DescriptionUpdate();
	}

	void Sling ()
	{
		damage = "1D4+1  (Piercing) + Necro Damage";
		type = "1Hand";
		skill = "Sling";
		range = "12m";
		damage = "1D8 (Crushing) + Necro Damage";
		DescriptionUpdate();
	}

	void Stick ()
	{
		damage = "1D4  (Crushing) + Necro Damage";
		type = "2Hand";
		skill = "Stick";
		range = "12m";
		damage = "1D4 (Crushing) + Necro Damage";
		DescriptionUpdate();
	}

	void GreatSword()
	{
		damage = "2D6+4  (Slashing/Crushing) + Necro Damage";
		type = "2Hand";
		skill = "GreatSword";
		range = "0.8m";
		damage = "2D6+4 (Crushing) + Necro Damage";
		DescriptionUpdate();
	}

	void Sceptre ()
	{
		damage = "1D6 (Crushing) + Necro Damage";
		type = "1Hand";
		skill = "Unarmed";
		range = "0.4m";
		damage = "1D6  (Slash / Piercing) + Necro Damage";
		DescriptionUpdate();
	}
}
