using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PlayerAttack : MonoBehaviour 
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
	public int maxDamage = 2;
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
    public int damageStrength;

    public string audioWeapon = "Hands";
    public string animController = "Sword";
	public float audioLength = 0.5f;
	public int audioTracks = 1;

	private bool invokeOn = false;
	private int dice;
	private int dice2;
	private int dam;
	private float attackTimer;
	private int damageTotal;
    private int enemyDefense;

	private Transform targetTransform;
	private Animator anim;
	private PlayerStats playerStats;
	private PlayerAI playerAI;
	private TargetActivePC targetActivePC;
	private GameObject gc;
	private GameController gameController;
    private CombatController combatController;
    private DisplayInfo displayInfo;
	private float startTime;

	void OnEnable () 
	{
        target = null;
        if (this.enabled == false)
        {
            this.enabled = true;
        }
        anim = GetComponent <Animator>();		
		targetActivePC = GetComponent<TargetActivePC>();
		playerAI = GetComponent <PlayerAI>();
		gc = GameObject.FindGameObjectWithTag ("GameController");
		gameController = gc.GetComponent <GameController>();
        combatController = gameController.GetComponent<CombatController>();
        displayInfo = gc.GetComponent<DisplayInfo>();
        //because playerattack is shared by NPC gameobjects
        playerStats = GetComponent<PlayerStats>();
        playerStats.CalculateStrengthMod();

    }

    public void Attack()
	{
    //    Debug.Log(gameObject.name + " Attacks");

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

        if (combatController.inCombat == false)
        {
            combatController.ChangeToBattle();
        }

        if (gameObject.name != "Lycaon" && gameObject.name != "Rose")
        {
            anim.SetFloat("Forward", 0);
            int ranNumber = Random.Range(1, 5);
            anim.SetTrigger("Attack" + ranNumber);
        }

        RandomNumber();
        attack = playerStats.totAttack;        
        if (waitForEffect == false)
        {
            DiceRolls();
        }
        
    }

    public void DiceRolls ()
    {
        if (dice <= playerStats.totAttack)
        {
            EnemyStats es = (EnemyStats)target.GetComponent("EnemyStats");

            if (enemyDodge == true)
            {
                if (ranged == false)
                {
                    enemyDefense = es.defence;
                }
                else
                {
                    enemyDefense = es.defenceRange;
                }
                
            }
            else
            {
                enemyDefense = 0;
            }
            string enemyName = es.name;
            RandomNumber();

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
                    crusPer = 100;
                }
                int realDamCrus = Mathf.RoundToInt((crusPer * (dice2 + playerStats.damageModBuff)) / 100 * es.resCrushing);
                int realDamPier = Mathf.RoundToInt((pierPer * (dice2 + playerStats.damageModBuff)) / 100 * es.resPiercing);
                int realDamSlas = Mathf.RoundToInt((slasPer * (dice2 + playerStats.damageModBuff)) / 100 * es.resSlashing);     //           
                int highestDamage = Mathf.Max(realDamCrus, realDamPier, realDamSlas);
                //             Debug.Log("Piercing: " + realDamCrus + "/Crus:" + realDamPier+ "/Slash" + realDamSlas + "/Highest " + highestDamage);
                damagePhysical = highestDamage;
                if (ranged != false)
                {
                    damagePhysical = damagePhysical + playerStats.damageMod;
                    Debug.Log(damagePhysical + "/Damage Strength: " + playerStats.damageMod);
                }  
                    
                dam = damagePhysical - armour;
     //           Debug.Log(armour + "/" + damagePhysical + "/" + gameObject.name);
                if (armour < damagePhysical)
                {
                    es.AddjustCurrentHealth(-dam, gameObject);
                    displayInfo.AddText(gameObject.name + " caused " + dam + " damage to " + target.name);
                    if (target.transform.Find ("DamageWeapon") != null)
                    {
           //             Debug.Log("activate");
                        target.transform.Find("DamageWeapon").gameObject.SetActive(true);
                    }

                    if (target.transform.Find ("Blood") != null)
                    {
                        target.transform.Find("Blood").GetComponent<PlayerBlood>().ActivateBlood();
                    }
                    else if (target.GetComponent<EnemyStats>().instantiateBlood == true)
              
                    {
                        GameObject blood = Instantiate(Resources.Load("Models/Blood"), target.transform.position, target.transform.rotation) as GameObject;
                        blood.name = "Blood";
                        blood.transform.parent = target.transform;
                        blood.transform.localPosition = new Vector3(0, 1, 0);

                    }

                }
                //armour is superior to physical damage inflicted, therefore weapon has not effect. 
                else
                {

                    if (target.transform.Find("DamageArmour") != null)
                    {
                        Debug.Log("activate");
                        target.transform.Find("DamageArmour").gameObject.SetActive(true);
                        displayInfo.AddText(gameObject.name + " : " + target.name + " armour has blocked my attack");
                    }
                    else
                    {
                        GameObject damageWeapon = Instantiate(Resources.Load("Effects/DamageWeapon"), target.transform.position, target.transform.rotation) as GameObject;
                        damageWeapon.name = "DamageWeapon";
                        damageWeapon.transform.parent = target.transform;
                        damageWeapon.transform.localPosition = new Vector3(0, 1, 0);
                    }

                    //			Debug.Log ("Player Weapon has not physical effect on " + es.name);
                }


                //OTHER DAMAGES AND RESISTANCES
                #region
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

                #endregion

                if (gameObject.name == "Player")
                {
        //            Debug.Log("Player");
                    if (DialogueLua.GetActorField("Player", "dalilaEquipped").AsString == "Yes")
                    {
                        int levelPlayer = DialogueLua.GetActorField("Player", "level").AsInt;
                        int dalilaNecroDamage = 1;

                        if (levelPlayer < 5)
                        {
                            dalilaNecroDamage = 1;
                        }
                        else if (levelPlayer < 10)
                        {
                            dalilaNecroDamage = 2;
                        }
                        else if (levelPlayer < 15)
                        {
                            dalilaNecroDamage = 3;
                        }
                        else if (levelPlayer <20)
                        {
                            dalilaNecroDamage = 4;
                        }
                        else
                        {
                            dalilaNecroDamage = 5;
                        }
            //            Debug.Log(dalilaNecroDamage);
                        GetComponent<PlayerStats>().AddjustCurrentHealth(dalilaNecroDamage, gameObject);

                        displayInfo.AddText("Dalila steals " + dalilaNecroDamage + "health from " + target.name);
                        es.AddjustCurrentHealth(-dalilaNecroDamage, gameObject);
                        if (target.transform.Find("DamageDalila") != null)
                        {
            //                Debug.Log("activate");
                            target.transform.Find("DamageDalila").gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject damageDalila = Instantiate(Resources.Load("Effects/DamageDalila"), target.transform.position, target.transform.rotation) as GameObject;
                            damageDalila.name = "DamageDalila";
                            damageDalila.transform.parent = target.transform;
                            damageDalila.transform.localPosition = new Vector3(0, 1, 0);
                        }

                    }
                }

                if (gameObject.name == "Weirum")
                {
                    int magicDamageWeirum = GetComponent<Weirum>().magicDamage;
                    es.AddjustCurrentHealth(- magicDamageWeirum , gameObject);
                    displayInfo.AddText("Weirum caused " + magicDamageWeirum + " divine magical damage to " + target.name);
                }

                if (es.curHealth == 0)
                {
                    target = null;

                    if (targetActivePC.enabled == true)
                    {
                        targetActivePC.hitGameObject = null;
                    }
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
		dice2 = Random.Range (minDamage, (maxDamage + 1)) + addDamage;
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