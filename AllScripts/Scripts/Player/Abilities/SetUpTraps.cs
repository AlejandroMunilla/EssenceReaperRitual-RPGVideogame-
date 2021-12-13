using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class SetUpTraps : MonoBehaviour {

    private int trapsNo = 0;
    private int maxTrapNo = 1;
    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04
    }
    private State state;
    private GameObject player;
    private TargetActivePC targetActivePC;
    private GameController gameController;
    private GeneralWindow generalWindow;
    private DisplayPortraits displayPortraits;
    private DisplayToolBar displayToolBar;
    private GameObject gc;
    private LayerMask layerMask = 1 << 8;
    private Camera mainCamera;
    private Vector3 pointPosition;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;


    private void Awake ()
    {
        layerMask = 1 << 8;
    }

    void OnEnable ()
    {
        int level = DialogueLua.GetActorField("Player", "level").AsInt;
        maxTrapNo = (int)(level / 3) + 1;
        Debug.Log(maxTrapNo);

        if (trapsNo >= maxTrapNo)
        {
     //       Debug.Log("Max No of Traps deployed");
            PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("All traps deployed");
            gameObject.SetActive(false);
        }
        else
        {
            player = transform.root.gameObject;
            targetActivePC = player.GetComponent<TargetActivePC>();
            gc = GameObject.FindGameObjectWithTag("GameController");
            if (gameController == null)
            {
                gameController = gc.GetComponent<GameController>();
            }
            Cursor.SetCursor(gameController.GetComponent<GameController>().cursorTarget, hotSpot, cursorMode);
            gameController.currentCursor = gameController.cursorTarget;
            generalWindow = gc.GetComponent<GeneralWindow>();
            displayToolBar = gc.GetComponent<DisplayToolBar>();
            displayPortraits = gc.GetComponent<DisplayPortraits>();
            mainCamera = Camera.main;
     //       Debug.Log(player);
            targetActivePC.abilityInUse = "Set_Up_Traps";
            targetActivePC.enabled = false;
            gameController.abilityInUse = true;
            state = SetUpTraps.State.Seq01;
            StartCoroutine("FSM");
        }
	}

    IEnumerator FSM()
    {
        while (true)
        {
            switch (state)
            {
                case State.Seq01:
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq02:
                    Seq02();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq03:
                    Seq03();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq04:
                    Seq04();
                    yield return new WaitForSeconds(0);
                    break;
            }
            yield return null;
        }
    }

    private void Seq01()
    {

//        Debug.Log("Seq01");
        if (gameController.inDialogue == false)
        {
            if (generalWindow.enabled == false)
            {
                if (displayPortraits.portraitWindowRect.Contains(Input.mousePosition) == true || displayToolBar.toolBarRect.Contains(Input.mousePosition) == true)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("Mouse 0");
                    }

                }
                else
                {
                    Debug.Log("Ray");
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, layerMask))
                    {
                        if (hit.transform.gameObject.tag == "Terrain")
                        {
                            
                            if (Input.GetMouseButtonUp(0))
                            {
                  //              Debug.Log("0");
                                pointPosition = hit.point;
                                state = SetUpTraps.State.Seq02;

                            }

                            if (gameController.currentCursor != gameController.cursorTarget)
                            {
                                gameController.currentCursor = gameController.cursorTarget;
                                Cursor.SetCursor(gameController.GetComponent<GameController>().cursorTarget, hotSpot, cursorMode);
                            }
                        }
                        else
                        {
                            gameController.currentCursor = gameController.cursorNormal;
                            Cursor.SetCursor(gameController.GetComponent<GameController>().cursorNormal, hotSpot, cursorMode);

                        }
                    }
                }
            }
        }
    }

    private void Seq02 ()
    {
        float distancePlayer = Vector3.Distance(player.transform.position, pointPosition);
        if (distancePlayer < 2.6f)
        {
            InstantiateTrap(pointPosition);
   //         Debug.Log(pointPosition);
            EndCourotine();
        }
        else
        {            
            state = SetUpTraps.State.Seq03;
        }
    }

    private void Seq03 ()
    {
   //     Debug.Log("Seq03");
        if (player.GetComponent<PlayerAI>().enabled == true)
        {
            if (player == gameController.activePC)
            {
                player.GetComponent<PlayerAI>().target.transform.position = pointPosition;
                state = SetUpTraps.State.Seq04;
            }
        }
        else if (player.GetComponent<PlayerAICombat>().enabled == true)
        {
            if (player == gameController.activePC)
            {
                player.GetComponent<PlayerAICombat>().target.transform.position = pointPosition;
      //          state = SetUpTraps.State.Seq04;
            }
        }
        else
        {

        }
    }

    private void Seq04 ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("0 click");
        }
        else
        {
            float distancePlayer = Vector3.Distance(player.transform.position, pointPosition);
   //         Debug.Log(distancePlayer);
            if (distancePlayer < 2.6f)
            {
                InstantiateTrap(pointPosition);
    //            Debug.Log(pointPosition);
                EndCourotine();
            }
        }       
    }

    private void InstantiateTrap (Vector3 pos)
    {
        GameObject trapInstantiated = Instantiate (Resources.Load("Models/Trap/Trap1"), pos, Quaternion.identity) as GameObject;
        trapInstantiated.name = "Trap";
        trapInstantiated.GetComponent<TrapPreFab>().player = player;
        trapsNo++;
        int trapLevel = DialogueLua.GetActorField("Player", "level").AsInt;
        trapInstantiated.GetComponent<TrapPreFab>().trapLevel = trapLevel;
        trapInstantiated.GetComponent<TrapPreFab>().player = player;
        trapInstantiated.SetActive(true);
        Debug.Log(trapLevel);
  //      Instantiate(Resources.Load("Models/" + PCchoice + "/" + PC2), pos02, rot) as GameObject;
    }

    private void EndCourotine ()
    {
        StopAllCoroutines();
        targetActivePC.enabled = true;
        targetActivePC.abilityInUse = "";
        gameController.abilityInUse = false;
        Cursor.SetCursor(gameController.GetComponent<GameController>().cursorNormal, hotSpot, cursorMode);
        gameController.currentCursor = gameController.cursorNormal;
        gameObject.SetActive(false);
    }


 
}
