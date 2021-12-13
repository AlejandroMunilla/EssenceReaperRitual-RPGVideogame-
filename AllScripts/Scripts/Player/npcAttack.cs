using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class npcAttack : MonoBehaviour 
{
    public bool rangedAttack = false;
	public int attack;
	public int skill;
	public int damagePhysical = 3;
	public float cooldown;
    public float distance = 2.5f;
	public GameObject target;
    public GameObject weapon;
	public bool vampiricSkill = false;
	public bool dalilaEquipped = false;
    public bool inCombat = false;
    public bool ranged = false;
    public bool effect = false;
    public bool waitForEffect = false;
    public bool enemyDodge = true;
	public int minDamage = 1;
	public int maxDamage = 1;
	public int addDamage = 0;
	public int crusPer = 0;
	public int pierPer = 0;
	public int slasPer = 0;
    public int fireDamage;
    public int iceDamage;
    public int acidDamage;
    public int elecDamage;
    public int magicDamage;
    public int necroDamage;
    public int mindDamage;
    public int entropyDamage;

    public string audioWeapon = "Hands";
    public string animController = "Sword";
	public float audioLength = 0.5f;
	public int audioTracks = 1;

	private bool invokeOn = false;
    private bool started = false;
	private int dice;
	private int dice2;
	private int dam;
	private float attackTimer;
	private int damageTotal;
    private int enemyDefense;

	private Transform targetTransform;
	private Animator anim;
	private npcStats playerStats;
	private npcAI playerAI;
//	private TargetActivePC targetActivePC;
	private GameObject gameController;
	private GameController gc;
    private CombatController combatController;
    private DisplayInfo displayInfo;
    private npcStats npcStats;
	private float startTime;


	void Start () 
	{
        target = null;
        if (started == false)
        {
            SetUpVariables();
        }
        
        
        if (this.enabled == false)
        {
            this.enabled = true;
        }
 
            npcStats.CalculateStrengthMod();
									
	}

    private void OnEnable()
    {
        Start();
    }

    private void SetUpVariables ()
    {
        started = true;
        npcStats = GetComponent<npcStats>();
        anim = GetComponent<Animator>();
        //		targetActivePC = GetComponent<TargetActivePC>();
        playerAI = GetComponent<npcAI>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gc = gameController.GetComponent<GameController>();
        combatController = gameController.GetComponent<CombatController>();
        displayInfo = gc.GetComponent<DisplayInfo>();
        //because playerattack is shared by NPC gameobjects
        npcStats = GetComponent<npcStats>();
    }

    public void Attack()
	{
     //   Debug.Log(gameObject.name + " Attacks");
        attack = GetComponent<npcStats>().attackSkill + GetComponent<npcStats>().buffAttack;
        if (target == null)
		{
			return;
		}

        if (weapon)
        {
            if (weapon.GetComponent<AudioSource>())
            {
                weapon.GetComponent<AudioSource>().Play();
            }

            if (effect == true)
            {
                if (waitForEffect == true)
                {
                    return;
                }
                else
                {
                    GameObject effectObj = weapon.transform.Find("Effect").gameObject;
                    effectObj.SetActive(true);
                }
            }
        }

        
        transform.LookAt(target.transform.position);
        targetTransform = target.transform;

        if (combatController == null)
        {
            SetUpVariables();
      //      Debug.Log(gameController);

        }

        if (combatController.inCombat == false)
        {
            combatController.ChangeToBattle();
        }

        anim.SetFloat("Forward", 0);
        int ranNumber = Random.Range(1, 5);
        anim.SetTrigger("Attack" + ranNumber);
        RandomNumber();
 //       attack = playerStats.totAttack;
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
 //       transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        //     Debug.Log(gameObject.name + "/" + dice + "/" + attack);
        if (rangedAttack == false && waitForEffect == false)
        {
            DiceRolls();

        }
        
    }

    public void DiceRolls ()
    {
    //    Debug.Log(gameObject.name + "/" + dice + "/" + attack);
        if (dice <= attack)
        {
            EnemyStats es = (EnemyStats)target.GetComponent("EnemyStats");
            if (enemyDodge == true)
            {
                enemyDefense = es.defence;
            }
            else
            {
                enemyDefense = 0;
            }
            string enemyName = es.name;
            RandomNumber();
    //        Debug.Log("dice: " + dice + "/defense " + enemyDefense);
            if (dice > enemyDefense)
            {
       //         Debug.Log("dice: " + dice + "/defense " + enemyDefense);
                int armour = es.armour;
                //Getting Resistences values of the enemy target; Res = 0 means 100% resistence, while 1 means 0% resistence
                float crushingRes = es.resCrushing;
                float slashingRes = es.resSlashing;
                float piercingRes = es.resPiercing;
                float fireRes = es.resFire;
                float acidRes = es.resAcid;
                float iceRes = es.resIce;
                float electricityRes = es.resElectricity;
                float magicRes = es.resMagic;
                float necroRes = es.resNecro;
                float mindRes = es.resMind;
                float entropyRes = es.resEntropy;
       //			int damagePhysical = (int)(crushingDamage * crushingRes) + (int)(slashingDamage * slashingRes) + (int) (piercingDamage * piercingRes);
                if (crusPer == 0 & pierPer == 0 & slasPer == 0)
                {
          //          crusPer = 100;
                }
                int realDamCrus = Mathf.RoundToInt((npcStats.damCru * dice2) / 100 * es.resCrushing);
                int realDamPier = Mathf.RoundToInt((npcStats.damPier * dice2) / 100 * es.resPiercing);
                int realDamSlas = Mathf.RoundToInt((npcStats.damSlas * dice2) / 100 * es.resSlashing);     //           
                int highestDamage = Mathf.Max(realDamCrus, realDamPier, realDamSlas);

                damagePhysical = highestDamage;
                dam = damagePhysical - armour;

                if (armour < damagePhysical)
                {
                    es.AddjustCurrentHealth(-dam, gameObject);
                    displayInfo.AddText(gameObject.name + " caused " + dam + " damage to " + target.name);
                }
                //armour is superior to physical damage inflicted, therefore weapon has not effect. 
                else
                {
                    //			Debug.Log ("Player Weapon has not physical effect on " + es.name);
                }

                int fireFinal = (int)(fireDamage * fireRes); if (fireFinal > 0) { es.AddjustCurrentHealth(-fireFinal, gameObject); }
                int iceFinal = (int)(iceDamage * iceRes); if (iceFinal > 0) { es.AddjustCurrentHealth(-iceFinal, gameObject); }
                int acidFinal = (int)(acidDamage * acidRes); if (acidFinal > 0) { es.AddjustCurrentHealth(-acidFinal, gameObject); }
                int elecFinal = (int)(elecDamage * electricityRes); if (elecFinal > 0) { es.AddjustCurrentHealth(-elecFinal, gameObject); }
                int magicFinal = (int)(magicDamage * magicRes); if (magicFinal > 0) { es.AddjustCurrentHealth(-magicFinal, gameObject); }
                int mindFinal = (int)(mindDamage * mindRes); if (mindFinal > 0) { es.AddjustCurrentHealth(-mindFinal, gameObject); }
                int entropyFinal = (int)(entropyDamage * entropyRes); if (entropyFinal > 0) { es.AddjustCurrentHealth(-entropyFinal, gameObject); }
                int necroFinal = (int)(necroDamage * necroRes); if (necroFinal > 0) { es.AddjustCurrentHealth(-necroFinal, gameObject); }
                //         Debug.Log(magicFinal + "/" + magicDamage);

                if (magicFinal > 0)
                {
                    displayInfo.AddText(gameObject.name + ": has caused " + magicFinal + "magic damage to " + target.name + "\n");
                }
                else if (magicDamage > 0 && magicFinal == 0)
                {
                    displayInfo.AddText(gameObject.name + ": " + target.name + " is inmune to magic damage" + "\n");
                }

                if (necroFinal > 0)
                {
                    Debug.Log(gameObject + " caused necrodamage " + necroFinal + " to " + target.name);
                }
                if (necroDamage > 0 && necroFinal == 0)
                {
                    Debug.Log(target.name + " is inmune to " + gameObject.name + " necro damage");
                }

                if (vampiricSkill == true)
                {
                    GetComponent<PlayerStats>().AddjustCurrentHealth(necroFinal, gameObject);
                }

                if (es.curHealth == 0)
                {
                    target = null;

                    /*
                    if (targetActivePC.enabled == true)
                    {
                        targetActivePC.hitGameObject = null;
                    }*/
                }
            }

            else
            {
                displayInfo.AddText(gameObject.name + ": " + target.name + " has dodge my attack" + "\n");
            }
        }

        if (effect == true)
        {
            //        GameObject effectObj = weapon.transform.Find("Effect").gameObject;
            //        effectObj.SetActive(false);

        }
    }

	void RandomNumber ()
	{
		dice = Random.Range (1, 101);
		dice2 = Random.Range (npcStats.minDamage, (npcStats.maxDamage + 1)) + addDamage;
	}

	void CheckEnemyIsAlive ()
	{
		if (target != null)
		{
			invokeOn = true;
//			Debug.Log ("target != null");
		}
		else
		{
			CancelInvoke ("CheckEnemyIsAlive");
		}
	}
}