using UnityEngine;
using PixelCrushers.DialogueSystem;


public class InstantiateWeapons : MonoBehaviour {
    public bool rightHand = true;
    private GameObject player;
    private GameObject gameController;


    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    void Start ()
    {
        InvokeRepeating("DelayUntilLua", 0.1f, 0.1f);
	}

    private void DelayUntilLua ()
    {
        //    Debug.Log(DialogueLua.GetActorField("Player", "level").AsInt);
        bool finishedLoadLua = false;
        if (gameController.GetComponent<GCRTS>() != null)
        {
            finishedLoadLua = gameController.GetComponent<GCRTS>().finishedLoadLua;
   //         Debug.Log(finishedLoadLua);
        }
        else if (gameController.GetComponent<LoadGame>() != null)
        {
            finishedLoadLua = gameController.GetComponent<LoadGame>().finishedLoadLua;
     //       Debug.Log(finishedLoadLua);
        }

        if (finishedLoadLua == true)
        {
            Instantiate();
            CancelInvoke("DelayUntilLua");
        }

        /*
        if (DialogueLua.GetActorField("Player", "level").AsInt != 0)
        {

        }*/
    }

    void Instantiate ()
    {
        GameObject goToName = (Instantiate(Resources.Load("Weapons/Weapons"), transform.position, transform.rotation) as GameObject);
        goToName.name = "WeaponRight";
        goToName.transform.parent = transform;
        player = transform.root.gameObject;
        Invoke("EnableScript", 0.1f);

    }

    void EnableScript ()
    {
        PlayerEquippedItems pe = player.GetComponent<PlayerEquippedItems>();
        if (player.GetComponent<PlayerEquippedItems>())
        {
            if (player.GetComponent<PlayerEquippedItems>().enabled == false)
            {
                pe.enabled = true;
            }
            else
            {
                CancelInvoke("EnableScript");
                this.enabled = false;
            }
        }
    }
}
