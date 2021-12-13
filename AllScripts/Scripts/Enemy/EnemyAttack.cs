using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class EnemyAttack : MonoBehaviour 
{	
	public int attack;
    public float attackRange;
	public float cooldown; 
	public int damagePhysical = 2;
    public int attackNo = 1;
	public GameObject target;	
	public int dice;
    public string specialAttack = null;
	private int dice2;
	private int dam;
	private float attackTimer;
    private float startTime;
    private int damageTotal;

	private Transform targetTransform;
	private Animator anim;
	private EnemyStats enemyStats;
	private EnemyAI enemyAI;
	private EnemyMove enemyMove;
    private DisplayInfo displayInfo;
	private bool AddPC = false;
    private bool loaded = false;

	//VARIABLES FOR Target Next Enemy functions, which is in charge of choosing next PC alive
	public List <GameObject> targets;
	private GameObject selectedTarget;
	private Transform myTransform;

	void OnEnable () 
	{
        if (loaded == false)
        {
            loaded = true;
            cooldown = 5.0f;

            anim = GetComponent<Animator>();
            enemyStats = GetComponent<EnemyStats>();
            enemyAI = GetComponent<EnemyAI>();
            attack = enemyStats.melee;
            attackRange = enemyStats.attackRange;
            startTime = Time.time;
            myTransform = transform;
            gameObject.tag = "Enemy";
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            displayInfo = gc.GetComponent<DisplayInfo>();
        }


    }

    private void Start()
    {
        if (loaded == false)
        {
            loaded = true;
            cooldown = 5.0f;

            anim = GetComponent<Animator>();
            enemyStats = GetComponent<EnemyStats>();
            enemyAI = GetComponent<EnemyAI>();
            attack = enemyStats.melee;
            attackRange = enemyStats.attackRange;
            startTime = Time.time;
            myTransform = transform;
            gameObject.tag = "Enemy";
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            displayInfo = gc.GetComponent<DisplayInfo>();
        }
    }

    public void Attack(GameObject go)
	{
    //    Debug.Log("Attack");
        target = go;
		if ((Time.time - startTime) >= cooldown)
		{
			startTime = Time.time;
		}
		else
		{
			return;
		}

        if (target != null)
		{
                     
            if (target.GetComponent<PlayerStats>() != null || target.GetComponent<npcStats>() != null)
            {
                PlayerStats ps = null;
                npcStats npcS = null;
                #region  
                if (target.GetComponent<PlayerStats>() != null)
                {
                    ps = target.GetComponent<PlayerStats>();
                }
                else if (target.GetComponent<PlayerStats>() != null)
                {
                    npcS = target.GetComponent<npcStats>();
                }


                if (enemyStats == null)
                {
                    enemyStats = GetComponent<EnemyStats>();
                }
                attack = enemyStats.melee;
                attackTimer = cooldown;
               	int attackChosen = Random.Range (1, attackNo);
                if (anim)
                {
                    //     int attackNo = Random.Range(1, 5);


                    Debug.Log(attackChosen);
                    anim.SetTrigger("Attack" + attackChosen.ToString());
                    anim.SetBool("Attacking", false);
                }
                targetTransform = target.transform;
                transform.LookAt(targetTransform);
                RandomNumber(0, 100);

                if (dice <= enemyStats.melee)
                {
                    //     Debug.Log(dice);
                    //         PlayerStats ps = (PlayerStats)target.GetComponent("PlayerStats");

                    int def = 0;
                    int skill = 0;
                    string playerName = target.name;

                    if (ps != null)
                    {
                        def = ps.defenseSkill;
                        skill = ps.totAttack;
                    }
                    else if (npcS != null)
                    {
                        def = npcS.defenseSkill;
                        skill = npcS.totAttack;
                    }
                    else
                    {
                        Debug.LogWarning("No Stats for target!!!!!!!!!!!");
                    }

                    
                    int modSkill = (int)(skill * 0.5f);

                    RandomNumber(0, 100);
                    if (def >= modSkill)
                    {
                     //                 Debug.Log(def + "> modSkill");
                    }
                    else
                    {
                        def = modSkill;
                    }

                    if (dice > def)
                    {
                        RandomDamage(enemyStats.minDamage, enemyStats.maxDamage);

                        int totalDamage = 0;
                        //Getting Resistences values of the player target; Res = 0 means 0% resistence, while 100 means 100% resistence
                        int realDamCrushing = Mathf.RoundToInt((enemyStats.crushingDamage * dice2) / 100 * 1);
                        int realDamPiercing = Mathf.RoundToInt((enemyStats.piercingDamage * dice2) / 100 * 1);
                        int realDamSlash = Mathf.RoundToInt((enemyStats.slashingDamage * dice2) / 100 * 1);
                        int highestDamage = Mathf.Max(realDamCrushing, realDamPiercing, realDamSlash);
                        damagePhysical = highestDamage;
                        //         Debug.Log(dice2 + "/" + (enemyStats.slashingDamage * dice2) / 100 + "/" + ((100 - ps.totCruRes) / 100));
                        if (ps != null)
                        {

                            dam = damagePhysical - ps.armour;
                        }
                        else if (npcS != null)
                        {
                            dam = damagePhysical - npcS.armour;
                        }


                        
                        //          Debug.Log("dam: " + dam + "/DamagePhysical: " + damagePhysical + "/Armour: " + ps.armour + " /Target: " + target);

                        if (dam > 0)
                        {
                            //    ps.AddjustCurrentHealth(-dam, gameObject);
                            totalDamage = dam;
                //       		Debug.Log (gameObject.name +  " Attack sucessful: " + dice + " physical damage: " + dam + " received by " + playerName + "defense " + dice2);
                            displayInfo.AddText(gameObject.name + " has caused " + dam + "damage to " + target.name);

                            if (target.transform.Find("Blood") != null)
                            {
                   //             Debug.Log("Blood");
                                target.transform.Find("Blood").GetComponent<PlayerBlood>().ActivateBlood();
                            }
                            else 

                            {
                                GameObject blood = Instantiate(Resources.Load("Models/Blood"), target.transform.position, target.transform.rotation) as GameObject;
                                blood.name = "Blood";
                                blood.transform.parent = target.transform;
                                blood.transform.localPosition = new Vector3(0, 1, 0);

                            }

                            if (specialAttack != null)
                            {
                                transform.Find(specialAttack).gameObject.SetActive(true);
                            }
                            
                        }
                        //armour is superior to physical damage inflicted, therefore weapon has not effect. 
                        else
                        {
                            //             Debug.Log(" Weapon has not physical effect on " + ps.name + "/" + damagePhysical);

                            if (target.transform.Find("DamageArmour") != null)
                            {
                                Debug.Log("activate");
                                target.transform.Find("DamageArmour").gameObject.SetActive(true);
                                displayInfo.AddText(target.name + "my armour has blocked attack");
                            }
                            else
                            {
                                GameObject damageWeapon = Instantiate(Resources.Load("Effects/DamageWeapon"), target.transform.position, target.transform.rotation) as GameObject;
                                damageWeapon.name = "DamageWeapon";
                                damageWeapon.transform.parent = target.transform;
                                damageWeapon.transform.localPosition = new Vector3(0, 1, 0);
                            }

                        }
                        //

                        if (enemyStats.fireDamage > 0)
                        {
                            int fireDamFinal = Mathf.RoundToInt(enemyStats.fireDamage * ((100 - ps.totFireRes) / 100));
                            //    ps.AddjustCurrentHealth(-fireDamFinal, gameObject);
                            totalDamage = totalDamage + fireDamFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + fireDamFinal + "fire damage to " + target.name);
                            Debug.Log(fireDamFinal);
                        }
                        if (enemyStats.iceDamage > 0)
                        {
                            int iceDamFinal = Mathf.RoundToInt(enemyStats.iceDamage * ((100 - ps.totIceRes) / 100));
                            //    ps.AddjustCurrentHealth(-iceDamFinal, gameObject);
                            totalDamage = totalDamage + iceDamFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + iceDamFinal + "ice damage to " + target.name);
                        }
                        if (enemyStats.acidDamage > 0)
                        {
                            int acidDamFinal = Mathf.RoundToInt(enemyStats.acidDamage * ((100 - ps.totAcidRes) / 100));
                            //    ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + acidDamFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + acidDamFinal + "acid damage to " + target.name);
                        }
                        if (enemyStats.elecDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.elecDamage * ((100 - ps.totElecRes) / 100));
                            //    ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + damageFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "electrical damage to " + target.name);
                        }
                        if (enemyStats.magicDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.magicDamage * ((100 - ps.totMagicRes) / 100));
                            //   ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + damageFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "magical damage to " + target.name);
                        }
                        if (enemyStats.mindDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.mindDamage * ((100 - ps.totMindRes) / 100));
                            //    ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + damageFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "mind damage to " + target.name);
                        }
                        if (enemyStats.necroDamage > 0)
                        {
                            float preDam = ((100 - (float)(ps.totNecroRes)) / 100);
                            float damNecro = enemyStats.necroDamage * preDam;

                            int damageFinal = Mathf.RoundToInt(damNecro);
                            //                  Debug.Log("res " + ps.totNecroRes + "/Pre " + preDam + "/after " + damNecro + "/" + target + "/" + damageFinal);
                            //    ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + damageFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "necro damage to " + target.name);
                            //                 Debug.Log("necro dama");
                        }
                        if (enemyStats.entropyDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.entropyDamage * ((100 - ps.totEntroRes) / 100));
                            //    ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            totalDamage = totalDamage + damageFinal;
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "entropy damage to " + target.name);
                        }

                        if (ps != null)
                        {
                            if (DialogueLua.GetActorField (target.name, "curHealth").AsInt <= totalDamage)
                            {
                                target = null;
                                enemyAI.ChangeToSearch();
                                
                            }
                            ps.AddjustCurrentHealth(-totalDamage, gameObject);

                            
                        }
                        else if (npcS != null)
                        {
                            npcS.AddjustCurrentHealth(-totalDamage, gameObject);
                            if (npcS.curHealth <= 0)
                            {
                                target = null;
                                enemyAI.ChangeToSearch();
                            }
                        }
                        
                    }
                }
                #endregion
            }

            /*
            else if (target.GetComponent<npcStats>() != null)
            {
                #region  
                npcStats ps = (npcStats)target.GetComponent("npcStats");

                if (enemyStats == null)
                {
                    enemyStats = GetComponent<EnemyStats>();
                }
                attack = enemyStats.melee;
                attackTimer = cooldown;
                //	int attackNo = Random.Range (1, 5);
                if (anim)
                {
                    int attackNo = Random.Range(1, 5);
              //      int attackNo = 1;
                    anim.SetTrigger("Attack" + attackNo.ToString());
                    anim.SetBool("Attacking", false);
                }
                targetTransform = target.transform;
                transform.LookAt(targetTransform);
                RandomNumber(0, 100);
                if (dice <= enemyStats.melee)
                {
                    //         PlayerStats ps = (PlayerStats)target.GetComponent("PlayerStats");
                    int def = ps.defenseSkill;
                    int skill = ps.totAttack;
                    string playerName = ps.name;
                    int modSkill = (int)(skill * 0.5f);

                    RandomNumber(0, 100);
                    if (def >= modSkill)
                    {
                        //              Debug.Log(def + "> modSkill");
                    }
                    else
                    {
                        def = modSkill;
                    }

                    if (dice > def)
                    {
                        RandomDamage(enemyStats.minDamage, enemyStats.maxDamage);
                        //Getting Resistences values of the player target; Res = 0 means 0% resistence, while 100 means 100% resistence
                        int realDamCrushing = Mathf.RoundToInt((enemyStats.crushingDamage * dice2) / 100 * ((100 - ps.totCruRes) / 100));
                        int realDamPiercing = Mathf.RoundToInt((enemyStats.piercingDamage * dice2) / 100 * ((100 - ps.totPierRes) / 100));
                        int realDamSlash = Mathf.RoundToInt((enemyStats.slashingDamage * dice2) / 100 * ((100 - ps.totSlaRes) / 100));
                        int highestDamage = Mathf.Max(realDamCrushing, realDamPiercing, realDamSlash);
                        damagePhysical = highestDamage;
                        //         Debug.Log(dice2 + "/" + (enemyStats.slashingDamage * dice2) / 100 + "/" + ((100 - ps.totCruRes) / 100));
                        dam = damagePhysical - ps.armour;
                        //          Debug.Log("dam: " + dam + "/DamagePhysical: " + damagePhysical + "/Armour: " + ps.armour + " /Target: " + target);

                        if (ps.armour < damagePhysical)
                        {
                            ps.AddjustCurrentHealth(-dam, gameObject);
                            //							Debug.Log (gameObject.name +  " Attack sucessful: " + dice + " physical damage: " + dam + " received by " + playerName + "defense " + dice2);
                            displayInfo.AddText(gameObject.name + " has caused " + dam + "damage to " + target.name);
                        }
                        //armour is superior to physical damage inflicted, therefore weapon has not effect. 
                        else
                        {
                            //             Debug.Log(" Weapon has not physical effect on " + ps.name + "/" + damagePhysical);
                        }
                        //

                        if (enemyStats.fireDamage > 0)
                        {
                            int fireDamFinal = Mathf.RoundToInt(enemyStats.fireDamage * ((100 - ps.totFireRes) / 100));
                            ps.AddjustCurrentHealth(-fireDamFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + fireDamFinal + "fire damage to " + target.name);

                        }
                        if (enemyStats.iceDamage > 0)
                        {
                            int iceDamFinal = Mathf.RoundToInt(enemyStats.iceDamage * ((100 - ps.totIceRes) / 100));
                            ps.AddjustCurrentHealth(-iceDamFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + iceDamFinal + "ice damage to " + target.name);
                        }
                        if (enemyStats.acidDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.acidDamage * ((100 - ps.totAcidRes) / 100));
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "acid damage to " + target.name);
                        }
                        if (enemyStats.elecDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.elecDamage * ((100 - ps.totElecRes) / 100));
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "electrical damage to " + target.name);
                        }
                        if (enemyStats.magicDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.magicDamage * ((100 - ps.totMagicRes) / 100));
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "magical damage to " + target.name);
                        }
                        if (enemyStats.mindDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.mindDamage * ((100 - ps.totMindRes) / 100));
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "mind damage to " + target.name);
                        }
                        if (enemyStats.necroDamage > 0)
                        {
                            float preDam = ((100 - (float)(ps.totNecroRes)) / 100);
                            float damNecro = enemyStats.necroDamage * preDam;

                            int damageFinal = Mathf.RoundToInt(damNecro);
                            //                  Debug.Log("res " + ps.totNecroRes + "/Pre " + preDam + "/after " + damNecro + "/" + target + "/" + damageFinal);
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "necro damage to " + target.name);
                            //                 Debug.Log("necro dama");
                        }
                        if (enemyStats.entropyDamage > 0)
                        {
                            int damageFinal = Mathf.RoundToInt(enemyStats.entropyDamage * ((100 - ps.totEntroRes) / 100));
                            ps.AddjustCurrentHealth(-damageFinal, gameObject);
                            displayInfo.AddText(gameObject.name + " has caused " + damageFinal + "entropy damage to " + target.name);
                        }

                    }
                }
                #endregion
            }
            */
        }
        
    }

    void RandomNumber (int min, int max)
	{
		dice = Random.Range (min, (max + 1));	
	}

    void RandomDamage (int min, int max)
    {
        dice2 = Random.Range(min, (max+1)) + enemyStats.addDamage;
    }


}