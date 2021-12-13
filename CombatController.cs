using UnityEngine;
using PixelCrushers.DialogueSystem;


public class CombatController : MonoBehaviour 
{
	public bool inCombat = false;
    private bool changedToBattle = false;
    private bool changedToPeace = false;
	private GameController gc;
	private GameObject gameController;
    private EnemyController enemyController;
	private string pathAnim;
	private RuntimeAnimatorController animController;
    private AudioController audioController;
    private PlayerControls playerControls;


	void Start () 
	{
		inCombat = false;
		pathAnim = "Animator/ThirdPersonAnimator";
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		gc = GetComponent<GameController>();
        enemyController = GetComponent<EnemyController>();
		animController = (RuntimeAnimatorController)(Resources.Load (pathAnim, typeof(RuntimeAnimatorController)));
        audioController = GetComponent<AudioController>();
        playerControls = GetComponent<PlayerControls>();
    }

	void OnEnable()
	{
        inCombat = false;
		pathAnim = "Animator/ThirdPersonAnimator";
		animController = (RuntimeAnimatorController)(Resources.Load (pathAnim, typeof(RuntimeAnimatorController)));
	}

	private void CheckInvisibles ()
	{
		Debug.Log ("Check Invisible Enemies");
	}

	public void ChangeToPeace ()
	{
     //   Debug.Log(changedToPeace);
        if (changedToPeace == false)
        {
            audioController.ChangeToPeace();
            if (audioController.playSerie == true)
            {
                audioController.PlaySerie();
            }
            inCombat = false;
            DialogueLua.SetVariable("inCombat", "No");
            ChangeAIToPeace(gc.player);

            if (gc.PC2Obj != null)
            {
                ChangeAIToPeace(gc.PC2Obj);
            }
            if (gc.PC3Obj != null)
            {
                ChangeAIToPeace(gc.PC3Obj);
            }
            if (gc.PC4Obj != null)
            {
                ChangeAIToPeace(gc.PC4Obj);
            }
            if (gc.PC5Obj != null)
            {
                ChangeAIToPeace(gc.PC5Obj);
            }
            if (gc.npc.Count > 0)
            {
                foreach (GameObject go in gc.npc)
                {
                    if (go.GetComponent<npcAI>())
                    {
                        go.GetComponent<npcAI>().ChangeToPeace();
                    }
                }
            }
            changedToBattle = false;
            changedToPeace = true;

        }

    }

	private void ChangeAIToPeace(GameObject character)
	{
        GameObject activePlayer = gc.activePC;
        

		if (character != null && character.name != "DummyPlayer")
		{
			PlayerStats playerstats = character.GetComponent<PlayerStats>();
			Rigidbody ridBody = character.GetComponent<Rigidbody>();
			if (character.tag == "PlayerDead")
			{
				character.tag = "Player";
			}

			if (character.GetComponent <UnityEngine.AI.NavMeshAgent>().enabled == false)
			{
				character.GetComponent <UnityEngine.AI.NavMeshAgent>().enabled = true;
			}
			character.GetComponent<UnityEngine.AI.NavMeshAgent>().Resume();

            if (character.name == "Lycaon")
            {
                if (character != activePlayer)
                {
                    character.GetComponent<Animation>().Play("Idle");
              //      character.GetComponent<PlayerAICombatBW>().enabled = false;
                    character.GetComponent<PlayerAIBlackWolf>().player = activePlayer;
                    character.GetComponent<PlayerAIBlackWolf>().SetCombatOrPeace();
                }
                else
                {

                }
            }
            else if (character.name == "Rose")
            {
                character.GetComponent<PlayerAIRose>().ChangeToPeace();
            }
            else
            {
                if (character.GetComponent<Animator>() != null)
                {
                    if (character.GetComponent<Animator>().GetBool("Dead") == true)
                    {
                        character.GetComponent<Animator>().SetBool("Dead", false);
                    }                        
                }
                else
                {
                    Debug.Log("Set Up for animations like Rose");
                }
                if (character.GetComponent<PlayerAICombat>() != null)
                {
                    character.GetComponent<PlayerAICombat>().StopCoroutine("FSM");
                    character.GetComponent<PlayerAICombat>().enabled = false;
                }
                else
                {
                    Debug.Log("Set Up for animations like Rose");
                }

                if (character.GetComponent<PlayerAI>() != null)
                {
                    character.GetComponent<PlayerAI>().player = activePlayer;
                    character.GetComponent<PlayerAI>().enabled = true;
                }
                else
                {
                    Debug.Log("Set Up for animations like Rose");
                }

                if (character.GetComponent<PlayerMoveAI>() != null)
                {
                    character.GetComponent<PlayerMoveAI>().enabled = false;
                    character.GetComponent<PlayerMoveAI>().enabled = true;
                }
                else
                {
                    Debug.Log("Set Up for animations like Rose");
                }

                //isKinematic must be true otherwise PCs dont move properly
                character.GetComponent<Rigidbody>().isKinematic = true;
                character.GetComponent<Rigidbody>().isKinematic = true;

                if (character.name != "Weirum")
                {
                    character.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Animator/ThirdPersonAnimator")) as RuntimeAnimatorController;

                }
            }

            string realism = GetComponent<DisplayOptionsScript>().realism;
            int playerCurHealth = playerstats.curHealth;
            int totHealth = playerstats.totHealth;
            //Rounds up to the nearest integer. curHealth is always an integer. 
            int minimumHealth = Mathf.CeilToInt(totHealth * 0.20f);
            if (character.GetComponent<PlayerStats>().curHealth < Mathf.CeilToInt(character.GetComponent<PlayerStats>().totHealth * 0.2f))
            {
                character.GetComponent<PlayerStats>().curHealth = Mathf.CeilToInt(character.GetComponent<PlayerStats>().totHealth * 0.2f);
            }

            if (playerstats.deadstate == true)
            {
                playerstats.deadstate = false;
            }


            character.GetComponent<PlayerAttack>().inCombat = false;
            if (character.name != "Lycaon")
            {
                character.GetComponent<PlayerEquippedItems>().SetUpEquipped();
            }

            if (gc.npc.Count > 0)
            {
                foreach (GameObject go in gc.npc)
                {
                    if (go.GetComponent<npcAI>())
                    {
                        go.GetComponent<npcAI>().ChangeToPeace();
                    }
                }
            }

        }
	}

	public void ChangeToBattle()
	{
        if (changedToBattle == false && GetComponent<GameController>().inDialogue == false && inCombat == false)
        {
      //      Debug.Log("Change");
            InvokeRepeating("CheckEndCombat", 5, 1);
            inCombat = true;
            playerControls.Pause();
            DialogueLua.SetVariable("inCombat", "Yes");
            audioController.ChangeToBattle();
            if (audioController.playSerie == true)
            {
                audioController.CancelInvoke("SerieAudio");
            }

            GameObject PC2Obj = gc.PC2Obj;
            GameObject PC3Obj = gc.PC3Obj;
            GameObject PC4Obj = gc.PC4Obj;
            GameObject PC5Obj = gc.PC5Obj;

            ChangeAIToBattle(gc.player);
            if (PC2Obj != null)
            {
                ChangeAIToBattle(PC2Obj);
            }
            if (PC3Obj != null)
            {
                ChangeAIToBattle(PC3Obj);
            }
            if (PC4Obj != null)
            {
                ChangeAIToBattle(PC4Obj);
            }
            if (PC5Obj != null)
            {
                ChangeAIToBattle(PC5Obj);
            }

            if (gc.npc.Count > 0)
            {
                foreach (GameObject go in gc.npc)
                {
                    if (go.GetComponent<npcAI>())
                    {
                        go.GetComponent<npcAI>().ChangeToCombat();
                    }
                }
            }

            if (enemyController.enemies != null)
            {
                foreach (GameObject go in enemyController.enemies)
                {

                    EnemyAI ea = go.GetComponent<EnemyAI>();
                    if (ea != null)
                    {
                        if (ea.enabled == false)
                        {
                            ea.enabled = true;
                        }
                    }

                    EnemyAIAnim eanim = go.GetComponent<EnemyAIAnim>();
                    if (eanim != null)
                    {
                        if (eanim.enabled == false)
                        {
                            eanim.enabled = true;
                        }
                    }

                }
            }

            changedToPeace = false;
            changedToBattle = true;

        }
 
    }

    void ChangeAIToBattle (GameObject character)
	{
        GameObject activePlayer = gc.activePC;
        if (character == activePlayer)
        {
            if (character.name == "Lycaon")
            {
                character.GetComponent<TargetActivePC>().target.SetActive(false);
            }
            else if (character.name == "Rose")
            {
                character.GetComponent<TargetActivePC>().target.SetActive(false);
            }
            else if (character.name == "Kira")
            {
                character.GetComponent<TargetActivePC>().target.SetActive(false);
            }
            else if (character.name == "Weirum")
            {
                character.GetComponent<TargetActivePC>().target.SetActive(false);
            }
            else
            {
                character.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Animator/Sword")) as RuntimeAnimatorController;
                if (character.GetComponent<PlayerAI>().enabled == true)
                {
                    character.GetComponent<PlayerAI>().enabled = false;
                    character.GetComponent<TargetActivePC>().target.SetActive(false);
                    character.GetComponent<PlayerAICombat>().enabled = true;
                }
            }
        }
        else
        {
            if (character.name == "DummyPlayer")
            {
                return;
            }
  //          Debug.Log(character);

            if (character.name == "Lycaon")
            {
                character.GetComponent<PlayerAIBlackWolf>().SetCombatOrPeace();
                character.GetComponent<PlayerMoveAIBlackWolf>().enabled = false;
                character.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
            }
            else if (character.name == "Rose")
            {
                character.GetComponent<PlayerAIRose>().ChangeToCombat();
            }
            else if (character.name == "Kira")
            {
                character.GetComponent<PlayerAIKira>().ChangeToCombat();
            }

            else
            {
                //	Debug.Log ("ChangeAIToBattle/" + character);
                if (character.name != "Weirum" && character.name != "Rose")
                {
                    if (character.tag =="Player")
                    {
                        string animatorPath = "Animator/" + character.GetComponent<PlayerAttack>().animController;
           //             Debug.Log(character + "/" + animatorPath);

                        character.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(animatorPath)) as RuntimeAnimatorController;

                    }
                    //          Debug.Log(character);
                }
                if (character.name != "Rose" && character.name != "Kira")
                {
                    Debug.Log(character.name);
                    character.GetComponent<PlayerAI>().StopCoroutine("FSM");
                    character.GetComponent<PlayerAI>().enabled = false;
                    character.GetComponent<PlayerAICombat>().enabled = true;
                    character.GetComponent<PlayerMoveAI>().enabled = false;
                    character.GetComponent<PlayerMoveAI>().enabled = true;
                }
            }
        }

        if (character.name != "Lycaon" || character.name != "Weirum" )
        {
            character.GetComponent<PlayerEquippedItems>().SetUpEquipped();
        }
	}	
	
	public void CheckEndCombat ()
	{
   //     Debug.Log(gc.enemies.Count);
        if (enemyController.enemies != null)
        {
            if (enemyController.enemies.Count == 0)
            {
                foreach (GameObject go in gc.enemies)
                {
                    Debug.Log(go);
                }
                CancelInvoke("CheckEndCombat");
                Invoke("ChangeToPeace", 1);
            }
        }

        else
        {
    //        TimerAbilities(false);
        }
	}

}
