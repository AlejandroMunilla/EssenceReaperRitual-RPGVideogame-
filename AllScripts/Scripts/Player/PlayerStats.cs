using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

/*
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMoveAI))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(PlayerAI))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAICombat))]
[RequireComponent(typeof(TargetActivePC))]
[RequireComponent(typeof(PlayerEquippedItems))]
[RequireComponent(typeof(ThirdPersonUserControl))]
[RequireComponent(typeof(ThirdPersonCharacter))]*/
public class PlayerStats : MonoBehaviour 
{
	public List<EquippedItems> equipped = new List<EquippedItems>();
	public List <EquippedItems> EquippedItems
	{
		
		get { return equipped;}
	}

    //variables
    #region
    public string deathAnimation = "death";
    public bool loadEnded = false;          
    public string gender;					
	public string race;
	public string profession;
	public string _namePlayer;
	public string mainPlayer;
    public bool castingSpell = false;
	public int _strenght;					public int buffStr;				public int totalStr;
	public int _constitution;				public int buffCon;				public int totalCon;
	public int _agility;					public int buffAgi;				public int totalAgi;
	public int _inteligence;				public int buffInte;			public int totalInte;
	public int _willpower;					public int buffWill;			public int totalWill;
	public int _intuition;					public int buffIntu;			public int totalIntu;
	public int _charisma;					public int buffCha;				public int totalCha;

	public int strengthModifier;
    public int damageMod;
    public int damageModBuff = 0;
	public int defenseAttr;					public int buffDef;				public int totalDef;
	public int meleeAttr;					public int buffMel;				public int totalMel;
	public int rangedAttr;					public int buffRang;			public int totalRang;
	public int magicAttr;					public int buffMagic;			public int totalMagic;

	public int damage = 5;
	public int damCru;						public int buffDamCru;			public int totDamCru;
	public int damPier;						public int buffDamPier;			public int totDamPier;
	public int damSlas;						public int buffDamSlas;			public int totDamSlas;
	public int damFire;						public int buffDamFire;			public int totDamFire;
	public int damIce;						public int buffDamIce;			public int totDamIce;
	public int damElec;						public int buffDamElec;			public int totDamElec;
	public int damAcid;						public int buffDamAcid;			public int totDamAcid;
	public int damMagic;					public int buffDamMagic;		public int totDamMagic;
	public int damEntro;					public int buffDamEntro;		public int totDamEntro;
	public int damNecro;					public int buffDamNecro;		public int totDamNecro;
	public int damMind;						public int buffDamMind;			public int totDamMind;

	
	public int _health;						public int buffHealth;			public int totHealth;
	public int _endurance;					public int buffEndu;			public int totEndu;
	public int _mana;						public int buffMana;			public int totMana;
	
	public int attackSkill;					public int buffAttack;			public int totAttack;
	public int meleeOrRangedAttack;

	public int axeSkill;					public int buffAxe;				public int totAxe;
	public int bowSkill;					public int buffBow;				public int totBow;
	public int crossbowSkill;				public int buffCross;			public int totCross;
	public int knifeSkill ;					public int buffKnife;			public int totKnife;
	public int lanceSkill ;					public int buffLance;			public int totLance;
	public int bluntSkill ;					public int buffBlunt;			public int totBlunt;
	public int scyntheSkill ;				public int buffScyn;			public int totScyn;
	public int slingSkill ;					public int buffSling;			public int totSling;
	public int stickSkill ;					public int buffStick;			public int totStick;
	public int swordSkill ;					public int buffSword;			public int totSword;
	public int throwingSkill ;				public int buffThrow;			public int totThrow;	
	public int greatWeaponSkill ;			public int buffGreat;			public int totGreat;
	public int weaponAndShieldSkill ;		public int buffShield;			public int totShield;
	public int doubleWeaponSkill ;			public int buffDouble;			public int totDouble;
	public int unarmedSkill ;				public int buffUnarmed;			public int totUnarmed;
	public int arcabuzSkill ;				public int buffArcabuz;			public int totArcabuz;
	public int katanaSkill;					public int buffKatana;			public int totKatana;
	
	public int defenseSkill;				public int buffDefSkill;		public int totDefSkill;
    public int defenseRangeSkill;           public int buffDefRangeSkill;   public int totDefRangeSkill;
	public int meleeSkill ;					public int buffMeleeSkill;		public int totMeleeSkill;
	public int rangedSkill ;				public int buffRangedSkill;		public int totRangedSkill;		
	public int magicSkill;					public int buffMagicSkill;		public int totMagicSkill;
	
	public int detectTrampsSkill ;			public int buffDetect;			public int totDetect;
	public int trampAbilitySkill ;			public int buffTramp;			public int totTramp;
	public int hideSkill ;					public int buffHide;			public int totHide;
	public int backstabSkill ;				public int buffBackstab;		public int totBackstab;
	
	public int crushingSkill ;				public int buffCruRes;			public int totCruRes;
	public int slashSkill ;					public int buffSlaRes;			public int totSlaRes;
	public int pierceSkill ;				public int buffPierRes;			public int totPierRes;
	public int fireSkill ;					public int buffFireRes;			public int totFireRes;
	public int acidSkill ;					public int buffAcidRes;			public int totAcidRes;
	public int iceSkill ;					public int buffIceRes;			public int totIceRes;
	public int electricitySkill ;			public int buffElecRes;			public int totElecRes;
	public int magicResSkill ;				public int buffMagicRes;		public int totMagicRes;
	public int necroSkill ;					public int buffNecroRes;		public int totNecroRes;
	public int mindSkill ;					public int buffMindRes;			public int totMindRes;
	public int entropySkill ;				public int buffEntroRes;		public int totEntroRes;
	
	public bool spellCaster;				
	public bool thiefProf;
	public int restrictionBase;
	public int restrictionLevel;
	public int level;
	public int experience;
	public int restrictionThiefBase;
	public int restrictionThiefLevel;
	public int battleSkillPoints;
	public int thiefSkillPoints;

	public int armour;
    public int armourBuff;
    public int armourTotal;
	public int curHealth;
	public int curEndurance;
	public int curMana;
	public float attackRange = 1.4f;

	public int orderPath;
	public int benignPath;

	public int friendship;
	public int romance;

	public string dalila;

	public string behaviour = "Aggressive";
	public string invisible = "No";
    public List<string> states = new List<string>();
    public string posibleTarget = "Yes";


    //SETTING UP HEALTH ADJUSTMENT AND DEATH FUNCTION
    private Animator anim;
    private Animation anima;
	private PlayerAICombat playerAICombat;
    private PlayerAttack playerAttack;
	private int dying = Animator.StringToHash("Dying");
	private string mainPlayerName;
	private GameController gameController;
	public GameObject gc;
	public GameObject attacker;
	public float posX; public float posY; public float posZ;
	public bool deadstate = false;
	private GameObject player;
    // Change cursor
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    //STATES
    public bool stunned = false;
    #endregion  

    //GENERAL
    private bool instantiatedBlood = false;


    void Awake ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController");
        if (gc)
        {
            if (gc.GetComponent<GameController>())
            {
                gameController = gc.GetComponent<GameController>();
            }
        }
        
    }

    void Start ()
    {
        OnEnable();
    }

    void OnEnable ()
    {
        if (gameObject.name == "DummyPlayer")
        {
            totHealth = 0;
            curHealth = 0;
            totMana = 0;
            curMana = 0;
            return;
        }
        else
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
            if (gc)
            {
                if (gc.GetComponent<GameController>())
                {
                    gameController = gc.GetComponent<GameController>();
                    mainPlayerName = gameController.mainPlayerName;
                    player = gc.GetComponent<GameController>().activePC;
                }
            }
            
            if (GetComponent<PlayerAICombat>() != null)
            {
                playerAICombat = GetComponent<PlayerAICombat>();
            }
            playerAttack = GetComponent<PlayerAttack>();
            if (GetComponent<Animator>())
            {
                anim = GetComponent<Animator>();
            }

            if (GetComponent<Animation>())
            {
                anima = GetComponent<Animation>();
            }
            
            _namePlayer = gameObject.name;
            LoadLua();
            StartCoroutine("Transit");
            if (instantiatedBlood == false)
            {
                InstantiateBlood();
            }
            
            SetUpScripts();
        }
    }


	IEnumerator Transit ()
	{
		yield return new WaitForSeconds (0);

        int healthLua = DialogueLua.GetActorField(gameObject.name, "health").AsInt;
        if (healthLua == 00)
        {
            _health = _constitution;
            DialogueLua.SetActorField(gameObject.name, "health", _health);
        }
        else
        {
            _health = healthLua;
        }
        		
		AdjustAttributeValues();
		CalculateTotals();
		CalculateStrengthMod();
    //    curHealth = _health;
        curHealth = DialogueLua.GetActorField (gameObject.name, "curHealth").AsInt;
        curMana = totMana;
		curEndurance = totEndu;
		StopCoroutine ("Transit");
        loadEnded = true;

        if (instantiatedBlood == false)
        {
            InstantiateBlood();
        }
        //     Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            Debug.Log(gameObject.name + "dead");
            Death();
        }
		//********************************************FOR DEBUGGING PURPOSES
	//	curHealth = 1;
	}

    public void CalculateEverything ()
    {
        CalculateTotals();
        AdjustAttributeValues();
        
    }

	public void AdjustAttributeValues()
	{
	//	attackSkill = 50;
		//	totAttack = attackSkill + meleeAttr;
		totalCon =  _constitution +buffCon;
		totalInte =  _inteligence + buffInte;			
		totalWill = _willpower + buffWill;			 
		totalIntu =  _intuition +  buffIntu;
		_endurance = (totalCon * 3)  + (level * 1);
		int magicBaseAttack = (int)(totalIntu + totalInte + totalWill)/2; 
		magicAttr = magicBaseAttack + (level*4);
		_mana = magicAttr;
        totalAgi = _agility + buffAgi;
        defenseAttr = totalAgi;
        float strenghtBonus = ((_strenght - 10) / 4);

        if (_strenght >= 20)
        {
            strenghtBonus = 5;
        }
        else if (_strenght == 19)
        {
            strenghtBonus = 4;
        }
        else if (_strenght == 18)
        {
            strenghtBonus = 3;
        }
        else if (_strenght >=16)
        {
            strenghtBonus = 2;
        }
        else if (_strenght >=14)
        {
            strenghtBonus = 1;
        }
        else if (_strenght <10)
        {
            strenghtBonus = -1;
        }
        


        int roundBonus = Mathf.FloorToInt(strenghtBonus);
        damageMod = roundBonus;
        

    //    Debug.Log(gameObject.name + "dam mod: " + damageMod);
	}

	public void CalcultateAttributeOnly()
	{
		totHealth = buffHealth + _health;
		totEndu = buffEndu + _endurance;
		totMana = buffMana + _mana;
	}

	public void CalculateTotals()
	{
		totalStr =  _strenght + buffStr;	
		totalCon =  _constitution +buffCon;	 		
	//	totalAgi =  _agility + buffAgi;				 calculated in AdjustAttributes
		totalInte =  _inteligence + buffInte;			
		totalWill = _willpower + buffWill;			 
		totalIntu =  _intuition +  buffIntu;		
		totalCha =  _charisma + buffCha;
        armourTotal = armour + armourBuff;

		totDamCru = buffDamCru + damCru;
		totDamPier = buffDamPier+ damPier;
		totDamSlas = buffDamSlas + damSlas;
		totDamFire = buffDamFire + damFire;
		totDamIce = buffDamIce + damIce;
		totDamElec = buffDamElec + damElec;
		totDamAcid = buffDamAcid + damAcid;
		totDamMagic = buffDamMagic + damMagic;
		totDamEntro =  buffDamEntro + damEntro;
		totDamNecro = buffDamNecro + damNecro;
		totDamMind = buffDamMind + damMind;

		totHealth = buffHealth + _health;
		totEndu = buffEndu + _endurance;
		totMana = buffMana + _mana;
		totAttack = attackSkill + buffAttack;
		totAxe = buffAxe + axeSkill;
		totBow = buffBow + bowSkill;
		totCross = buffCross + crossbowSkill;
		totKnife = buffKnife + knifeSkill;
		totLance = buffLance + lanceSkill;
		totBlunt = buffBlunt + bluntSkill;
		totScyn = buffScyn + scyntheSkill;
		totSling = buffSling + slingSkill;
		totStick = buffStick + stickSkill;
		totSword = buffSword + swordSkill;
		totThrow = buffThrow + throwingSkill;
		totGreat = buffGreat + greatWeaponSkill;
		totShield = buffShield + weaponAndShieldSkill;
		totDouble = buffDouble + doubleWeaponSkill;
		totUnarmed = buffUnarmed + unarmedSkill;
		totArcabuz = buffArcabuz + arcabuzSkill;
		totKatana = buffKatana + katanaSkill;

		totDefSkill = buffDefSkill + defenseSkill + defenseAttr;
        totDefRangeSkill = buffDefRangeSkill + defenseSkill + defenseAttr;
		totMeleeSkill = buffMeleeSkill + meleeSkill + meleeAttr;
		totRangedSkill = buffRangedSkill + rangedSkill + rangedAttr;
		totMagicSkill = buffMagicSkill + magicSkill + magicAttr;

		totDetect = buffDetect + detectTrampsSkill + totalIntu;
		totTramp = buffTramp + trampAbilitySkill;
		totHide = buffHide + hideSkill;
		totBackstab = buffBackstab + backstabSkill;
        CalculateOnlyResistances();
	}

	public void CalculateStrengthMod ()
	{
		strengthModifier = 0;
		int cal = totalStr - 10;
		
		if (cal <= 0 ) 
		{
			strengthModifier = 0;
		}
		else
		{
			strengthModifier = (int) (cal / 2);
		}
	}

	//This is used when in combat, damage suffered is reduced from Health. 
	public void AddjustCurrentHealth(int dam, GameObject go)
	{
        curHealth = DialogueLua.GetActorField(gameObject.name, "curHealth").AsInt;
        
		
        
        if (dam > 0 && curHealth < totHealth)
        {
            gc.GetComponent<DisplayInfo>().AddText(gameObject.name + " healed " + dam);
        }


        curHealth += dam;
        DialogueLua.SetActorField(gameObject.name, "curHealth", curHealth);
        if (curHealth <= 0) 
		{
            curHealth = 0;
            DialogueLua.SetActorField(gameObject.name, "curHealth", 0);
            deadstate = true;
            Death();
        }
		
		if (curHealth > totHealth) 
		{
            curHealth = totHealth;

            DialogueLua.SetActorField(gameObject.name, "curHealth", totHealth);
        }
        
        attacker = go;
	}
	
	public void Death()
	{
		transform.gameObject.tag = "PlayerDead";

        if (GetComponent<Animator>())
        {
            if (anim.GetBool("Dead") == false)
            {
                anim.SetBool("Dead", true);
                anim.SetTrigger("Die");
            }
        }
        else if (GetComponent<Animation>())
        {
            
        }

        DisableCharacter();
		Rigidbody ridBody = GetComponent<Rigidbody>();
		ridBody.isKinematic = true;
		ridBody.constraints = RigidbodyConstraints.FreezeAll;
		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        if (gc.GetComponent<DisplayPortraits>().enabled == true)
        {
            gc.GetComponent<DisplayPortraits>().ChangeToDead(gameObject.name);
        }
        else
        {
            Invoke("CheckDeadPortrait", 0.5f);
        }
        if (gameObject == gameController.player)
        {
            if (gc.GetComponent<DisplayOptionsScript>().realism != "Fairy Tale" && gameController.sequenceOn == false)
            {
                MainCharacterDead();
            }
            
        }

        deadstate = true;
	}	

    public void CalculateOnlyResistances ()
    {
        totCruRes = buffCruRes + crushingSkill;
        totSlaRes = buffSlaRes + slashSkill;
        totPierRes = buffPierRes + pierceSkill;
        totFireRes = buffFireRes + fireSkill;
        totIceRes = buffIceRes + iceSkill;
        totAcidRes = buffAcidRes + acidSkill;
        totElecRes = buffElecRes + electricitySkill;
        totMagicRes = buffMagicRes + magicResSkill;
        totNecroRes = buffNecroRes + necroSkill;
        totMindRes = buffMindRes + mindSkill;
        totEntroRes = buffEntroRes + entropySkill;
    }

	public void LoadLua()
	{
	//	StopCoroutine ("Transit");
		_namePlayer = gameObject.name;
        string mainPlayerNameGC = "";
        if (gc.GetComponent<GameController>() != null)
        {
            mainPlayerNameGC = gc.GetComponent<GameController>().mainPlayerName;
        }
        else
        {
            mainPlayerNameGC = DialogueLua.GetActorField("Player", "name").AsString;
        }
		
		string mainPlayerIs;

		if (_namePlayer == "Player")
		{
			mainPlayerIs = "Yes";
			DialogueLua.SetActorField ("Player", "IsPlayer", true);
		}
		else
		{
			mainPlayerIs = "No";
		}
		gender = DialogueLua.GetActorField (_namePlayer, "gender").AsString;
		race = DialogueLua.GetActorField (_namePlayer, "race").AsString;
		profession = DialogueLua.GetActorField (_namePlayer, "profession").AsString;
		_strenght = DialogueLua.GetActorField (_namePlayer, "stren").AsInt;
 //       Debug.Log(_strenght + "/" + _namePlayer);
		_constitution = DialogueLua.GetActorField (_namePlayer, "constitution").AsInt;
		_agility = DialogueLua.GetActorField (_namePlayer, "agility").AsInt;
		_willpower = DialogueLua.GetActorField (_namePlayer, "willpower").AsInt;
		_inteligence = DialogueLua.GetActorField (_namePlayer, "intelligence").AsInt;
		_intuition = DialogueLua.GetActorField (_namePlayer, "intuition").AsInt;
		_charisma = DialogueLua.GetActorField (_namePlayer, "charisma").AsInt;
		defenseSkill = DialogueLua.GetActorField (_namePlayer, "defenseSkill").AsInt;
		meleeSkill = DialogueLua.GetActorField (_namePlayer, "meleeSkill").AsInt;
		rangedSkill = DialogueLua.GetActorField (_namePlayer, "meleeSkill").AsInt;
		magicSkill = DialogueLua.GetActorField (_namePlayer, "magicSkill").AsInt;
		axeSkill = DialogueLua.GetActorField (_namePlayer, "axeSkill").AsInt;
		bowSkill = DialogueLua.GetActorField (_namePlayer, "bowSkill").AsInt;
		crossbowSkill = DialogueLua.GetActorField (_namePlayer, "crossbowSkill").AsInt;
		knifeSkill = DialogueLua.GetActorField (_namePlayer, "knifeSkill").AsInt;
		lanceSkill = DialogueLua.GetActorField (_namePlayer, "lanceSkill").AsInt;
		bluntSkill = DialogueLua.GetActorField (_namePlayer, "bluntSkill").AsInt;
		scyntheSkill = DialogueLua.GetActorField (_namePlayer, "scyntheSkill").AsInt;
		slingSkill = DialogueLua.GetActorField (_namePlayer, "slingSkill").AsInt;
		stickSkill = DialogueLua.GetActorField (_namePlayer, "stickSkill").AsInt;
		swordSkill = DialogueLua.GetActorField (_namePlayer, "swordSkill").AsInt;
		throwingSkill = DialogueLua.GetActorField (_namePlayer, "throwingSkill").AsInt;
		greatWeaponSkill = DialogueLua.GetActorField (_namePlayer, "greatWeaponSkill").AsInt;
		weaponAndShieldSkill = DialogueLua.GetActorField (_namePlayer, "weaponAndShieldSkill").AsInt;
		doubleWeaponSkill = DialogueLua.GetActorField (_namePlayer, "doubleWeaponSkill").AsInt;
		unarmedSkill = DialogueLua.GetActorField (_namePlayer, "unarmedSkill").AsInt;
		arcabuzSkill = DialogueLua.GetActorField (_namePlayer, "arcabuzSkill").AsInt;
		katanaSkill = DialogueLua.GetActorField (_namePlayer, "katanaSkill").AsInt;
		detectTrampsSkill = DialogueLua.GetActorField (_namePlayer, "detectTrampSkill").AsInt;
		trampAbilitySkill = DialogueLua.GetActorField (_namePlayer, "trampAbilitySkill").AsInt;
		hideSkill = DialogueLua.GetActorField (_namePlayer, "hideSkill").AsInt;
		backstabSkill = DialogueLua.GetActorField (_namePlayer, "backstabSkill").AsInt;
		crushingSkill = DialogueLua.GetActorField (_namePlayer, "crushingSkill").AsInt;
		slashSkill = DialogueLua.GetActorField (_namePlayer, "slashSkill").AsInt;
		pierceSkill = DialogueLua.GetActorField (_namePlayer, "pierceSkill").AsInt;
		fireSkill = DialogueLua.GetActorField (_namePlayer, "fireSkill").AsInt;
		acidSkill = DialogueLua.GetActorField (_namePlayer, "acidSkill").AsInt;
		iceSkill =  DialogueLua.GetActorField (_namePlayer, "iceSkill").AsInt;
		electricitySkill = DialogueLua.GetActorField (_namePlayer, "electricitySkill").AsInt;
		magicResSkill = DialogueLua.GetActorField (_namePlayer, "magicResSkill").AsInt;
		necroSkill = DialogueLua.GetActorField (_namePlayer, "necroSkill").AsInt;
		mindSkill = DialogueLua.GetActorField (_namePlayer, "mindSkill").AsInt;
		entropySkill = DialogueLua.GetActorField (_namePlayer, "entropySkill").AsInt;
		spellCaster = DialogueLua.GetActorField (_namePlayer, "spellcaster").AsBool;
		thiefProf  = DialogueLua.GetActorField (_namePlayer, "thiefProf").AsBool;
		restrictionBase = DialogueLua.GetActorField (_namePlayer, "restrictionBase").AsInt;
		restrictionLevel = DialogueLua.GetActorField (_namePlayer, "restrictionLevel").AsInt;
		level = DialogueLua.GetActorField (_namePlayer, "level").AsInt;
		restrictionThiefBase = DialogueLua.GetActorField (_namePlayer, "restrictionThiefBase").AsInt;
		restrictionThiefLevel  = DialogueLua.GetActorField (_namePlayer, "restrictionThielLevel").AsInt;
		battleSkillPoints  = DialogueLua.GetActorField (_namePlayer, "battleSkillPoints").AsInt;
		thiefSkillPoints = DialogueLua.GetActorField (_namePlayer, "thiefSkillPoints").AsInt;
		curHealth = DialogueLua.GetActorField (_namePlayer, "curHealth").AsInt;
        if (curHealth <= 0)
        {
            Debug.Log(_namePlayer + "/" + _constitution);
            curHealth = _constitution;
            DialogueLua.SetActorField(_namePlayer, "curHealth", curHealth);
        }
		attackRange = DialogueLua.GetActorField (_namePlayer, "attackRange").AsFloat;
		orderPath = DialogueLua.GetActorField (_namePlayer, "orderPath").AsInt;
		benignPath = DialogueLua.GetActorField (_namePlayer, "benignPath").AsInt;
        /*
        if (_namePlayer == "Fred")
        {
            Debug.Log(DialogueLua.GetActorField(_namePlayer, "detectTrampSkill").AsInt);
        }*/
        if (_namePlayer == "Dylan")
        {
            DialogueLua.SetActorField(_namePlayer, "knifeSkill", 30);
            DialogueLua.SetActorField(_namePlayer, "swordSkill", 30);
        }

		if (mainPlayerIs == "No")
		{
			friendship  = DialogueLua.GetActorField (_namePlayer, "friendship").AsInt;
			romance = DialogueLua.GetActorField (_namePlayer, "romance").AsInt;
		}

		if (mainPlayerIs == "Yes")
		{
			dalila = DialogueLua.GetActorField (_namePlayer, "dalila").AsString;
			experience = DialogueLua.GetActorField ("Player", "Experience").AsInt;
		}
		else
		{
			dalila = "null";
		}
		
		behaviour = DialogueLua.GetActorField (_namePlayer, "behaviour").AsString;

		posX = DialogueLua.GetActorField (_namePlayer, "X").AsFloat;
		posY = DialogueLua.GetActorField (_namePlayer, "Y").AsFloat;
		posZ = DialogueLua.GetActorField (_namePlayer, "Z").AsFloat;

        string AI = DialogueLua.GetActorField(_namePlayer, "AI").AsString;
        if (AI == null || AI == "")
        {
            DialogueLua.SetActorField(_namePlayer, "AI", "Yes");
            if (GetComponent<PlayerAI>() != null)
            {
                GetComponent<PlayerAI>().AI = true;
            }
            
        }
        else if (AI == "Yes")
        {
            if (GetComponent<PlayerAI>() != null)
            {
                GetComponent<PlayerAI>().AI = true;
            }
        }
        else if (AI == "Attack")
        {
            GetComponent<PlayerAI>().AI = true;
        }
        else if (AI == "Defensive")
        {
            GetComponent<PlayerAI>().AI = true;
        }
        else if (AI == "Guard")
        {
            GetComponent<PlayerAI>().AI = true;
        }
        else
        {
            DialogueLua.SetActorField(_namePlayer, "AI", "Yes");
            GetComponent<PlayerAI>().AI = true;
        }
        curHealth = DialogueLua.GetActorField(gameObject.name, "curHealth").AsInt;
        DialogueLua.SetActorField(gameObject.name, "inParty", "Yes");

    }

	void OnConversationStart ()
	{
        if (gameObject.name == "DummyPlayer")
        {
            return;
        }
        else
        {
            //        player = gc.GetComponent<GameController>().activePC;
            gameController.ChangeActivePlayer(gameController.player);
            player = gameController.player;
            gc.GetComponent<GameController>().inDialogue = true;
            if (gc.GetComponent<GameController>().sequenceOn == false)
            {
                if (player != gameObject)
                {
                    gc.GetComponent<GeneralWindow>().conversationON = true;
                    gc.GetComponent<DisplayToolBar>().enabled = false;
                    player.GetComponent<ThirdPersonCharacter>().enabled = false;
                    player.GetComponent<ThirdPersonUserControl>().enabled = false;
                    player.GetComponent<TargetActivePC>().enabled = false;
                    player.GetComponent<Animator>().SetFloat("Forward", 0);
                    player.GetComponent<Animator>().SetFloat("Turn", 0);
                }

            }
        }

	}
	void OnConversationEnd ()
	{
        if (gameObject.name == "DummyPlayer")
        {
            return;
        }
        else
        {
            gc.GetComponent<GameController>().inDialogue = false;
            if (gc.GetComponent<GameController>().sequenceOn == false)
            {
                if (player != gameObject)
                {
                    gc.GetComponent<GeneralWindow>().conversationON = false;
                    gc.GetComponent<DisplayToolBar>().enabled = true;
                    player.GetComponent<ThirdPersonCharacter>().enabled = true;
                    player.GetComponent<ThirdPersonUserControl>().enabled = true;
                    player.GetComponent<TargetActivePC>().enabled = true;
                }

            }
        }
	}

    void SetUpScripts()
    {
        if (GetComponent<ThirdPersonCharacter>())
        {
            if (GetComponent<ThirdPersonCharacter>().enabled == true)
            {
                if (gameObject.name != "Player")
                {
                    GetComponent<ThirdPersonCharacter>().enabled = false;
                }
            }
        }

        if (GetComponent<ThirdPersonUserControl>())
        {
            if (GetComponent<ThirdPersonUserControl>().enabled == true)
            {
                if (gameObject.name != "Player")
                {
                    GetComponent<ThirdPersonUserControl>().enabled = false;
                }
            }
        }
    }


    void OnMouseEnter()
    {
        if (gameController)
        {
            if (gameController.currentCursor == gameController.cursorNormal)
            {
                Cursor.SetCursor(gameController.cursorPoint, hotSpot, cursorMode);
            }            
        }        
    }

    void OnMouseExit()
    {
        if (gameController)
        {
            if (gameController.currentCursor == gameController.cursorPoint)
            {
                Cursor.SetCursor(gameController.cursorNormal, Vector2.zero, cursorMode);
            }
        }
    }

    void CheckDeadPortrait ()
    {
        if (gc.GetComponent<DisplayPortraits>().enabled == true)
        {
            gc.GetComponent<DisplayPortraits>().ChangeToDead(gameObject.name);
        }
        else
        {
            Invoke("CheckDeadPortrait", 0.5f);
        }
    }

    public void DisableCharacter ()
    {
        if (gameObject.name == "Lycaon")
        {
            if (gameController.activePC == gameObject)
            {
                gameController.MainControllerDead();
            }

            PlayerAIBlackWolf playerAI = GetComponent<PlayerAIBlackWolf>();
            playerAI.SetDeath();
            anima.Play("Death");
        }
        else if (gameObject.name == "Rose")
        {
            if (gameController.activePC == gameObject)
            {
                gameController.MainControllerDead();
            }

            PlayerAIRose playerAI = GetComponent<PlayerAIRose>();
            playerAI.ChangeToDead();
            anima.Play("death");
        }
        else
        {

            if (anim != null)
            {
                if (anim.enabled == false)
                {
                    anim.enabled = true;
                }

                if (anim.GetBool("Attacking") == true)
                {
                    anim.SetBool("Attacking", false);
                }
            }


            if (GetComponent<ThirdPersonUserControl>() != null)
            {
                if (GetComponent<ThirdPersonUserControl>().enabled == true)
                {
                    gameController.MainControllerDead();
                }
            }
            else if (GetComponent<ThirdPersonUserControlWolf>() != null)
            {
                if (GetComponent<ThirdPersonUserControlWolf>().enabled == true)
                {
                    gameController.MainControllerDead();
                }
            }
            else if (GetComponent<ThirdPersonUserBlackWolf>() != null)
            {
                if (GetComponent<ThirdPersonUserBlackWolf>().enabled == true)
                {
                    gameController.MainControllerDead();
                }
            }
            else if (GetComponent<ThirdPersonUserRose>() != null)
            {
                if (GetComponent<ThirdPersonUserRose>().enabled == true)
                {
                    gameController.MainControllerDead();
                }
            }

            if (GetComponent<PlayerAI>() != null)
            {
                PlayerAI playerAI = GetComponent<PlayerAI>();
                if (playerAI.enabled == true)
                {
                    Debug.Log(gameObject + "/PlayerAI was enabled");
                    playerAI.StopAllCoroutines();
                    playerAI.enabled = false;
                }
            }


            PlayerAICombat playerAICombat = GetComponent<PlayerAICombat>();

            if (playerAICombat != null)
            {
                if (playerAICombat.enabled == true)
                {
                    playerAICombat.Die();
                    GetComponent<PlayerMoveAI>().enabled = false;
                }
            }

        }
        stunned = true;

    }

    private void MainCharacterDead ()
    {
        Debug.Log("MaincharacterDead");
        if (gameController.dontKillPlayer == false)
        {
            DialogueManager.ShowAlert("Main character dead. Game over. Be more careful next time");
            gameController.GetComponent<FadeScreen>().enabled = true;
            Invoke("ExitGame", 4);
        }


    }

    private void ExitGame ()
    {
        Debug.Log("ExitGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene("01MainMenu");
    }

    private void InstantiateBlood ()
    {
        if (transform.Find ("Blood") == null)
        {
        //    Debug.Log(gameObject.name);
            GameObject blood = Instantiate(Resources.Load("Models/Blood"), new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
            blood.name = "Blood";
            blood.transform.parent = transform;
            blood.transform.position = new Vector3(0, 1, 0);
            instantiatedBlood = true;
            GameObject effects = Instantiate(Resources.Load("Effects/Effects"), new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
            blood.name = "Effects";
            blood.transform.parent = transform;
        }
    }

    private void UpdateHealth()
    {
        string character = gameObject.name;
        int level = DialogueLua.GetActorField("Player", "level").AsInt;
        int levelHealUpdate = DialogueLua.GetActorField(_namePlayer, "levelHealUpdate").AsInt;
        int healAdded = DialogueLua.GetActorField(_namePlayer, "healAdded").AsInt;


        if (levelHealUpdate < level)
        {
            int sumToHealDice = 0;
            if (character == "Player")
            {
                sumToHealDice = 6;
            }
            else if (character == "Fred")
            {
                sumToHealDice = 3;
            }
            else if (character == "Kira")
            {
                sumToHealDice = 4;
            }
            else if (character == "Aurelius")
            {
                sumToHealDice = 1;
            }
            else if (character == "Oleg")
            {
                sumToHealDice = 6;
            }
            else if (character == "Rose")
            {
                sumToHealDice = 1;
            }
            else if (character == "Weirum")
            {
                sumToHealDice = 6;
            }
            else if (character == "Enora")
            {
                sumToHealDice = 5;
            }
            else if (character == "Grugk")
            {
                sumToHealDice = 1;
            }
            else if (character == "Preyton")
            {
                sumToHealDice = 3;
            }
            else if (character == "Ashak")
            {
                sumToHealDice = 6;
            }
            else if (character == "Lilith")
            {
                sumToHealDice = 3;
            }
            else if (character == "Lycaon")
            {
                sumToHealDice = 6;
            }
            else if (character == "Ecumius")
            {
                sumToHealDice = 5;
            }

            int diceRoll = Random.Range(1, 4) + sumToHealDice;
            healAdded = healAdded + diceRoll;

            int constitution = DialogueLua.GetActorField(character, "constitution").AsInt;
            int totalHeal = constitution + healAdded;
            Debug.Log(character + "/" + level + "/" + levelHealUpdate + "/" + sumToHealDice + "/" + constitution + "/" + totalHeal);



            //   UpdateTotalHealth(character, sumToHealDice);

            //    levelHealUpdate++;

            //    DialogueLua.SetActorField(_namePlayer, "levelHealUpdate", levelHealUpdate);


        }
    }
}