using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;

public class PlayerConversation : MonoBehaviour {

	private GameController gameController;
	private GameObject player;
	private GameObject gc;
	private Animator anim;
	public bool conversationOn = false;
	private bool alive = true;
    private bool thirdPerson = false;
    private string textToAdd;
    private RtsCameraImp rtsCamImp;	
	private enum State
	{
		Sequence1,
		Sequence2,
		Sequence3,
		Sequence4
	}	
	private State _state;
    private enum State2
    {
        Idle,
    }
    private State2 _state2;

    void Awake () 
	{
        Camera mainCamera = Camera.main;
        rtsCamImp = mainCamera.GetComponent<RtsCameraImp>();
        DialogueLua.SetVariable("showAlert", "");
  //      DialogueManager.StartConversation("06VivaceForest04");
        anim = GetComponent <Animator>();
		gc = GameObject.FindGameObjectWithTag ("GameController");

        if (gc.GetComponent<GameController>())
        {
            gameController = gc.GetComponent<GameController>();
            gameController.playerConversation = gameObject;
            StartCoroutine("SQE");
            _state = PlayerConversation.State.Sequence1;
        }
	}

	private IEnumerator SQE ()
	{
		while (alive)
		{
			switch (_state)
			{
			case State.Sequence1:
				Sequence1();
                    CheckPortraits();
				break;
			}
			yield return null;
		}
	}

	void Sequence1 ()
	{
		if (gc.GetComponent<GameController>().activePC != null)
		{
	//		Debug.Log (gc.GetComponent<GameController>().activePC);
			StopAllCoroutines ();
			player = gc.GetComponent<GameController>().activePC;
		}
	}

	public void OnConversationStart ()// (Transform actor)
	{
    //    Transform conversationModel = gameObject.transform;
   //     Texture2D img1 = gc.GetComponent<GeneralWindow>().img1;
   //     DialogueManager.ConversationModel.ActorInfo.portrait = img1;
        conversationOn = true;
        gameController.dialogue = true;
        gc.GetComponent<GameController>().inDialogue = true;
        if (gameController.player != null)
		{
  //         Debug.Log("x1");
            player.GetComponent<TargetActivePC>().enabled = false;
            player = gc.GetComponent <GameController>().activePC;
			gc.GetComponent <GameController>().inDialogue = true;
			if (gameController.sequenceOn == false)
			{
				gc.GetComponent<GeneralWindow>().conversationON = true;

				gc.GetComponent<DisplayToolBar>().enabled = false;
                gc.GetComponent<DisplayPortraits>().enabled = false;
                if (player.name == "Rose")
                {
                    player.GetComponent<ThirdPersonCharacterRose>().enabled = false;
                    player.GetComponent<ThirdPersonUserRose>().enabled = false;
                    GameObject targetPlayerRose = player.GetComponent<PlayerAIRose>().target;
                    if (targetPlayerRose != null)
                    {
                        targetPlayerRose.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);

                    }
                    player.GetComponent<Animation>().Play("stand blink");
                }
                else if (player.name == "Weirum")
                {
                    player.GetComponent<ThirdPersonCharacterWolf>().enabled = false;
                    player.GetComponent<ThirdPersonUserControlWolf>().enabled = false;
                    GameObject targetPlayer = player.GetComponent<PlayerAI>().target;
                    if (targetPlayer != null)
                    {
                        targetPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);

                    }

                    player.GetComponent<Animator>().SetFloat("Forward", 0);
                    player.GetComponent<Animator>().SetFloat("Turn", 0);
                }
                else if (player.name == "Lycaon")
                {
                    player.GetComponent<ThirdPersonCharacterBlackWolf>().enabled = false;
                    player.GetComponent<ThirdPersonUserBlackWolf>().enabled = false;
                    GameObject targetPlayer = player.GetComponent<PlayerAIBlackWolf>().target;
                    if (targetPlayer != null)
                    {
                        targetPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);

                    }

                    player.GetComponent<Animator>().SetFloat("Forward", 0);
                    player.GetComponent<Animator>().SetFloat("Turn", 0);
                }
                else
                {
                    player.GetComponent<ThirdPersonCharacter>().enabled = false;
                    player.GetComponent<ThirdPersonUserControl>().enabled = false;
                    GameObject targetPlayer = player.GetComponent<PlayerAI>().target;
                    if (targetPlayer != null)
                    {
                        targetPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);

                    }
                    
                    player.GetComponent<Animator>().SetFloat("Forward", 0);
                    player.GetComponent<Animator>().SetFloat("Turn", 0);
                }
                player.GetComponent<TargetActivePC>().enabled = false;

            }
            StartCoroutine("FSM");
            _state2 = PlayerConversation.State2.Idle;
            gc.GetComponent<DisplayInfo>().enabled = false;
		}

        if (rtsCamImp.enabled == true)
        {
            if (gc.GetComponent<CombatController>().inCombat == false)
            {
                player.GetComponent<PlayerAI>().enabled = false;
            }
            else
            {
                player.GetComponent<PlayerAI>().enabled = true;
            }            
        }


            if (gameObject.tag == "NPC")
        {
            if (gameObject.GetComponent<npcAIPatrol>() )
            {
                gameObject.GetComponent<npcAIPatrol>().OnConversationStart();
            }
        }
        
	}

	public void OnConversationEnd ()
	{
    //    Debug.Log("End");
        if (gameObject.tag == "NPC")         
        {
            return;
        }
        conversationOn = false;
        player = gameController.activePC;


        if (gameController.sequenceOn == false)
		{
  
            gc.GetComponent<GameController>().inDialogue = false;
            gc.GetComponent<GeneralWindow>().conversationON = false;
            gc.GetComponent<GameController>().dmCameraControl = false;
			gc.GetComponent<DisplayToolBar>().enabled = true;
            gc.GetComponent<DisplayPortraits>().enabled = true;
            player.GetComponent<TargetActivePC>().enabled = true;
            
            if (rtsCamImp.enabled != true)
            {
                player.GetComponent<ThirdPersonCharacter>().enabled = true;
                player.GetComponent<ThirdPersonUserControl>().enabled = true;
            }
            else
            {
                player.GetComponent<ThirdPersonCharacter>().enabled = false;
                player.GetComponent<ThirdPersonUserControl>().enabled = false;
                if (gc.GetComponent<CombatController>().inCombat == false)
                {
                    player.GetComponent<PlayerAI>().enabled = true;
                }
                else
                {
                    player.GetComponent<PlayerAICombat>().enabled = true;
                }              

            }

            StopAllCoroutines();
            gc.GetComponent<DisplayInfo>().enabled = true;
            gameController.dialogue = false;
            gameController.ChangeActivePlayer(gameController.player);
            Invoke("ChangePlayer", 0.05f);
        }



    }

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (_state2)
            {
                case State2.Idle:
                    Idle();
                    yield return new WaitForSeconds(0.2f);
                    break;
            }
            yield return null;
        }
    }

    void Idle ()
    {


        if (conversationOn == false)
        {
            StopCoroutine("FSM");
        }
        else
        {
            if (gc.GetComponent<DisplayPortraits>().enabled == true)
            {
                OnConversationStart();
            }
        }

        if (DialogueLua.GetVariable ("showAlert").AsString != "" && DialogueLua.GetVariable("showAlert").AsString != null )
        {            
            DialogueManager.ShowAlert(DialogueLua.GetVariable("showAlert").AsString);
            DialogueLua.SetVariable("showAlert", "");           
        }
    }
    void OnConversationLine (Subtitle subtitle)
    {
        textToAdd = "";

        if (subtitle.speakerInfo.transform)
        {
            if (subtitle.speakerInfo.transform.name != null)
            {
                textToAdd = "* " + subtitle.speakerInfo.transform.name + " : ";
            }
        }

        if (subtitle.formattedText.text != null)
        {
            textToAdd = textToAdd + subtitle.formattedText.text + " \n ";
        }
        if (gc)
        {
            gc.GetComponent<DisplayInfo>().AddText(textToAdd);
        }



    }



    void ChangePlayer ()
    {
        gameController.ChangeActivePlayer(gameController.player);
    }


    void CheckPortraits ()
    {
 //       Debug.Log("Check");
        if (gc.GetComponent<DisplayPortraits>().enabled == true)
        {
            OnConversationStart();
        }
    }




}







