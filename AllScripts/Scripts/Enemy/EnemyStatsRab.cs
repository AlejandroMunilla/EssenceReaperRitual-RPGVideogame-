using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


public class EnemyStatsRab : MonoBehaviour 
{
	//BATTLE STATS
	public string name;
	public string profesion;
	public string race = "none";
	public int constitution;
    public int inteligence;
	public int intuition;
	public int health;
	public int melee;
	public int ranged;
	public int defence;         // it works for both parry and defence. Simulating sorting for the highest of either. 
	public int damage;
	public int armour;
	public int mana;
	public int curHealth;
    public bool dead = false;
    public int minDamage = 1;
    public int maxDamage = 1;
    public int addDamage = 0;
    public int crushingDamage;
	public int piercingDamage;
	public int slashingDamage;
	public int fireDamage;
	public int iceDamage;
	public int acidDamage;
	public int elecDamage;
	public int magicDamage;
	public int necroDamage;
	public int mindDamage;
	public int entropyDamage;
	public float speed;
	public float attackRange = 2;

	//Skills
	public int hearingDistance;
	public int sightDistance;
	public int perception;
	public int hide;
	public int disarmTraps;


	// RESISTENCES in  0 - 1, NOT % !!!!  1 is = 100% damage received, the opposite to player resistence. 
	public float resCrushing;
	public float resSlashing;
	public float resPiercing;
	public float resFire;
	public float resAcid;
	public float resIce;
	public float resElectricity;
	public float resMagic;
	public float resNecro;
	public float resMind;
	public float resEntropy;


	//LEVEL AND EXPERIENCE
	public int level = 1;
	public int experienceReward;

	//varible to get enemy position so when it dies we know the position to instantiate deadbody and looting
	private Vector3 enemyDeadPos;
    public bool adjustDeath = false;
    public string deathAnimation = "death";
    public Vector3 adjustTranslate =  new Vector3(0, 0.386f, 0);
    public Vector3 adjustRotate = new Vector3(0, 0, 180);
    private Animator anim;
    private Animation anima;
	private int dying = Animator.StringToHash("Dying");

    //ITEMS AND QUESTS
    public bool autosetup = true;
	public string loot ;
    public int coins = 0;
	public string changeQuest ;
	public string stateQuest ;
    private bool generalAI = false;
    private bool loaded = false;
    private bool loaded2 = false;
	private GameObject bag;
	private GameObject gc;
	private GameObject label;
	private DisplayEnemyInfoHead displayLabel;
    private EnemyAIGeneral enemyAIGeneral;

    //AI specifics
    public string smartTarget = "" ;          //Enemies with this ability attack weak party members
    public string hardSmartTarget = "";
    public bool removeCorpse = true;
    private string specialAbility = "";


    //Cursor
    private Texture2D cursorAttack;
    private Texture2D cursorNormal;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    private GameController gameController;
    private bool cursonOn = false;

    void Awake()
	{
        
        if (transform.Find("Label").gameObject)
        {
            label = transform.Find("Label").gameObject;
            displayLabel = label.GetComponent<DisplayEnemyInfoHead>();
        }
        GameObject go = GameObject.FindGameObjectWithTag("GameController");
        gameController = go.GetComponent<GameController>();
        loaded = true;
    }

	void Start ()
	{
        if (loaded == false)
        {
            if (transform.Find("Label").gameObject)
            {
                label = transform.Find("Label").gameObject;
                displayLabel = label.GetComponent<DisplayEnemyInfoHead>();
            }
            GameObject go = GameObject.FindGameObjectWithTag("GameController");
            gameController = go.GetComponent<GameController>();
            loaded = true;
        }

        if (loaded2 == false)
        {
            if (GetComponent<Animator>())
            {
                anim = GetComponent<Animator>();
            }
            else
            {
                anima = GetComponent<Animation>();
            }

            curHealth = health;
            gc = GameObject.FindGameObjectWithTag("GameController");
            //	gameObject.transform.parent = gc.transform;


            if (autosetup == true)
            {
                Invoke("SetUpMonster", 2);
            }

            GameObject parentRoot = transform.root.gameObject;
            //       Debug.Log(parentRoot);

            if (parentRoot.GetComponent<EnemyAIGeneral>())
            {

                enemyAIGeneral = parentRoot.GetComponent<EnemyAIGeneral>();
                enemyAIGeneral.enemiesAI.Add(gameObject);
                generalAI = true;
            }
            cursorAttack = (Texture2D)(Resources.Load("Icons/Cursor/Attack"));
            cursorNormal = (Texture2D)(Resources.Load("Icons/Cursor/Normal"));
            loaded2 = true;
        }
    }
     

	public void AddjustCurrentHealth(int dam, GameObject go)
	{
		curHealth += dam;

		if (curHealth <0) 
			{curHealth = 0;}

		if (curHealth > health) 
			{ curHealth = health;}

		if (curHealth == 0)
		{
            if (dead == false)
            {
                dead = true;
                Death();
                if (cursonOn == true)
                {
                    label.GetComponent<DisplayEnemyInfoHead>().enabled = false;
                    Cursor.SetCursor(cursorNormal, Vector2.zero, cursorMode);
                }
            }			
		}
	//	Debug.Log (go.name + " caused damage to " + gameObject + "/curHealth " + curHealth);
	}

	public void Death()
	{
        gc.GetComponent<EnemyController>().enemies.Remove(gameObject);
        gameObject.tag = "EnemyDead";
        if (gc.GetComponent<EnemyController>().enemies.Contains (gameObject))
        {
            gc.GetComponent<EnemyController>().enemies.Remove(gameObject);
        }        

        if (transform.Find("TorusRed") != null)
        {
            GameObject go = transform.Find("TorusRed").gameObject;
            go.GetComponent<EnemyCircle>().enabled = false;
        }

        if (GetComponent<EnemyAdd>())
        {
            GetComponent<EnemyAdd>().End();
        }

        if (generalAI == true)
        {
            enemyAIGeneral.enemiesAI.Remove(gameObject);
        }

        if (GetComponent<EnemyAI>())
        {
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            enemyAI.StopCoroutine("FSM");
            enemyAI.enabled = false;
            if (anim.GetBool("Dead") == false)
            {
                anim.SetBool("Dead", true);
            }
        }
        else
        {
            anima.Play(deathAnimation);            
            //          InvokeRepeating("CheckDeathAnim", 0, 0.2f);
            anima.Stop();
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    //        GetComponent<NavMeshObstacle>().enabled = true;
            if (GetComponent<SphereCollider>())
            {
                GetComponent<SphereCollider>().enabled = false;
            }          
            
      //      transform.Translate(0, 0.386f, 0);
     //       transform.Rotate(0, 0, 180);
            
            EnemyAIAnim enemyAI = GetComponent<EnemyAIAnim>();
            enemyAI.StopCoroutine("FSM");
            enemyAI.enabled = false;
        }

        //to adjust different death animations that may end not well in position and rotation
        if (adjustDeath == true)
        {
            transform.Translate(adjustTranslate);
            transform.Rotate(adjustRotate);
        }
        
        transform.gameObject.tag = "EnemyDead";
		curHealth = 0;	

        if (coins > 0 )
        {
            InstantiateLoot();
        }
		else if (loot != null && loot != "")
		{
			InstantiateLoot();
		}
        
		if (changeQuest != null && changeQuest != "No")
		{
			DialogueLua.SetQuestField (changeQuest, "State", stateQuest);
            if (changeQuest == "done")
            {
                DialogueLua.SetQuestField(changeQuest, "Track", false);
                DialogueLua.SetQuestField(changeQuest, "Trackable", false);
            }
		}
		gc.GetComponent<ExpController>().AdjustExp (experienceReward, level);

        
        int enemiesKilled = DialogueLua.GetActorField("Enemies", race).AsInt;
   //     Debug.Log(enemiesKilled);
        if (enemiesKilled == 0)
        {
            enemiesKilled = 1;
        }
        else
        {
            enemiesKilled = enemiesKilled + 1;
        }
        DialogueLua.SetActorField("Enemies", race, enemiesKilled);

        if (removeCorpse == true)
        {
            Invoke("RemoveDeadEnemy", 6);
        }
        
		gc.GetComponent <GameController>().enemies.Remove (gameObject);
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        
        //     Messenger.Broadcast("EnemyDead");

    }

	void ToggleInfoOn()
	{
        if (displayLabel != null)
        {
            displayLabel.enabled = true;
        }
		

	}
	void ToggleInfoOff ()
	{
        if (displayLabel != null)
        {
            displayLabel.enabled = false; 
        }

        
	} 

	void InstantiateLoot ()
	{
        
		bag = Instantiate (Resources.Load ("Objects/LootBag") as GameObject);
		bag.transform.position = transform.position;
        if (loot != null && loot != "")
        {
            bag.GetComponent<Loot>().loot =  loot;
      //      Debug.Log(loot);
        }
        if (coins > 0 )
        {
            bag.GetComponent<Loot>().coins = coins;
        }
            
		bag.name = "Loot Bag";
    }

	void RemoveDeadEnemy ()
	{
        //     gameObject.SetActive(false);
        
        gameObject.SetActive(false);
	}


    //sometimes Animation.Play does not work so we keep calling it for it until animation is played
    void CheckDeathAnim ()          
    {
        Debug.Log("CheckDeathAnim");
        Debug.Log(anima.isPlaying);
        if (anima.isPlaying == true)
        {
            anima.Stop();
        }
        else
        {
            CancelInvoke("CheckDeathAnim");
        }
    }

}