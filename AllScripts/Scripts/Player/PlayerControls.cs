using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;

public class PlayerControls : MonoBehaviour {

    private bool active = true;
    private bool rtsCameraOn;
    private bool gameInPause = false;
    public bool toggleIsOn = false;
    private enum State
    {
        IdleInCombat,
        Tactical,
    }
    private State _state;
    private GameController gameController;
    private Pause pauseScript;
    private Camera mainCamera;
    //   private MyGUI myGUI;
    private GeneralWindow generalWindow;
    private CameraMap cameraMap;
    private DisplayItemScript displayItemScript;
    private MouseOrbitImp mouseOrbit;
    private RtsCameraImp rtsCamera;
    private RtsCameraMouse rtsMouse;

    /// <summary>
    /// 
    /// </summary>
    ///     private float fontSizeDialog = 0.02f;
    private GameObject dialogueManager;
    private ScaleFontSize scaleFont;
    private GUIRoot guiRoot;
    private float rateAdjust; 

    void Start ()
    {
        gameController = GetComponent<GameController>();
        generalWindow = GetComponent<GeneralWindow>();
        cameraMap = GetComponent<CameraMap>();
        displayItemScript = GetComponent<DisplayItemScript>();
        pauseScript = GetComponent<Pause>();
        mainCamera = Camera.main;
        rtsCamera = mainCamera.GetComponent<RtsCameraImp>();
        rtsMouse = mainCamera.GetComponent<RtsCameraMouse>();
        mouseOrbit = mainCamera.GetComponent<MouseOrbitImp>();
        if (transform.Find("Display") != null)
        {
            foreach (Transform child in transform.Find("Display"))
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Missing Display, child of GameController");
        }


    }

    void OnEnable()
    {
       	StartCoroutine ("FSM");
       	_state = PlayerControls.State.IdleInCombat;
    }

    void OnDisable()
    {
        CancelInvoke();
       	StopCoroutine ("FSM");
    }

    private IEnumerator FSM()
    {
        while (active)
        {
            switch (_state)
            {
                case State.IdleInCombat:
                    Controls();
                    break;

                case State.Tactical:
                    break;

            }
            yield return null;
        }
    }

    void Controls()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.R))
        {
            if (gameController.inDialogue == true)
            {
                return;
            }
            else
            {
                if (toggleIsOn == false)
                {
                    toggleIsOn = true;
                    ToggleOn();
                }
                else
                {
                    toggleIsOn = false;
                    ToggleOff();
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cameraMap.enabled != true)
            {
                Messenger.Broadcast("InteractableOn");
            }
            else
            {
                Messenger.Broadcast("InteractableOff");
            }           

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            Messenger.Broadcast("InteractableOff");
        }

        if (Input.GetKeyUp (KeyCode.Space))
        {
            Pause();         
        }


        if (Input.GetKeyDown (KeyCode.L))
        {
            IncreaseFont(false);
            Debug.Log("L");

        }
        if (Input.GetKeyDown (KeyCode.P))
        {

            Debug.Log("P");
            IncreaseFont(true);

        }
    }

    
    public void Pause ()
    {
  //      Debug.Log("Pause");
        if (gameController.inPause == true)
        {
            Time.timeScale = 1;
            gameController.inPause = false;
            pauseScript.enabled = false;
        }
        else
        {
            Time.timeScale = 0;
            gameController.inPause = true;
            pauseScript.enabled = true;
        }
    }

    public void CheckToggle ()
    {
        if (toggleIsOn == false)
        {
            toggleIsOn = true;
            ToggleOn();
        }
        else
        {
            toggleIsOn = false;
            ToggleOff();
        }
    }

    public void ToggleOn ()
    {
        generalWindow.enabled = true;
        if (gameController.inPause == false)
        {
            Time.timeScale = 0;
            gameInPause = false;
            gameController.inPause = true;
        }
        else
        {
            gameInPause = true;
            pauseScript.enabled = false;
        }

        if (mouseOrbit.enabled)
        {
            rtsCameraOn = false;
            mouseOrbit.enabled = false;
        }
        else
        {
            rtsCameraOn = true;
            rtsCamera.enabled = false;
            rtsMouse.enabled = false;
        }

    }

    public void ToggleOff ()
    {

        
        if (gameInPause == false)
        {
            Time.timeScale = 1;
            pauseScript.enabled = false;
            gameController.inPause = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseScript.enabled = true;
        }

        generalWindow.enabled = false;
        string displayPath = displayItemScript.displayPath;
        if (transform.Find ("Display") != null)
        {
            foreach (Transform child in transform.Find("Display"))
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Missing Display, child of GameController");
        }

        if (rtsCamera != null)
        {
            if (rtsCameraOn == true)
            {
                rtsCamera.enabled = true;
                rtsMouse.enabled = true;
            }
            else
            {
                mouseOrbit.enabled = true;
            }
        }
        if (GetComponent<DisplayPortraits>().enabled == false)
        {
            GetComponent<DisplayPortraits>().enabled = true;
        }
    }


    private void IncreaseFont (bool positive)
    {
        rateAdjust = DialogueLua.GetVariable("rateAdjust").AsFloat;
        Debug.Log(rateAdjust);
        if (positive == true)
        {
            rateAdjust = rateAdjust + 0.01f;
        }
        else
        {
            rateAdjust = rateAdjust - 0.01f;
        }
        
        DialogueLua.SetVariable("rateAdjust", rateAdjust);
        Debug.Log("final" + "/" + rateAdjust);
        /*
        foreach (var style in styles)
        {
            GUIStyle guiStyle = guiRoot.guiSkin.GetStyle(style.styleName);

            
            if (guiStyle != null)
            {
                guiStyle.fontSize = (int)(style.scaleFactor * rateAdjust * Screen.height);

                if (style.styleName == "Panel")
                {
                    Debug.Log("Panel");
                    guiStyle.normal.textColor = Color.black;

                }

            }

        }*/
        
    }
}
