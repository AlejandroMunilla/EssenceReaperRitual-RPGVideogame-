using UnityEngine;

public class PlayerCircle : MonoBehaviour {

    private MeshRenderer rend;
    private Camera mainCam;
    private MouseOrbitImp mouseOrbit;
    private RtsCameraImp rtsCam;
	// Use this for initialization

    void Awake ()
    {
        mainCam = Camera.main;
        mouseOrbit = mainCam.GetComponent<MouseOrbitImp>();
        rtsCam = mainCam.GetComponent<RtsCameraImp>();

    }

	void Start ()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
        Messenger.AddListener("InteractableOn", ToggleCircleOn);
        Messenger.AddListener("InteractableOff", ToggleCircleOff);

        Messenger.AddListener("ToggleCircleOn", ToggleCircleOn);
        Messenger.AddListener("ToggleCircleOff", ToggleCircleOff);

        Messenger.AddListener("EnemyDead", EnemyDead);
        Messenger.AddListener("EnemyRise", ToggleCircleOff);

       

    }

    void OnEnable()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
        Messenger.AddListener("InteractableOn", ToggleCircleOn);
        Messenger.AddListener("InteractableOff", ToggleCircleOff);

        Messenger.AddListener("ToggleCircleOn", ToggleCircleOn);
        Messenger.AddListener("ToggleCircleOff", ToggleCircleOff);

        Messenger.AddListener("EnemyDead", EnemyDead);
        Messenger.AddListener("EnemyRise", ToggleCircleOff);


    }


    /*
    void OnDestroy ()
    {
        Messenger.RemoveListener("InteractableOn", ToggleCircleOn);
        Messenger.RemoveListener("InteractableOff", ToggleCircleOff);

        Messenger.RemoveListener("ToggleCircleOn", ToggleCircleOn);
        Messenger.RemoveListener("ToggleCircleOff", ToggleCircleOff);
    }
    
    void OnDisable()
    {
        Messenger.RemoveListener("InteractableOn", ToggleCircleOn);
        Messenger.RemoveListener("InteractableOff", ToggleCircleOff);

        Messenger.RemoveListener("ToggleCircleOn", ToggleCircleOn);
        Messenger.RemoveListener("ToggleCircleOff", ToggleCircleOff);

   //     Messenger.RemoveListener("EnemyDead", EnemyDead);
   }*/

    void ToggleCircleOn ()
    {
        if (rend != null)
        {
            rend.enabled = true;
        }        

        
    }

    void ToggleCircleOff ()
    {
            rend.enabled = false;

    }

    void EnemyDead ()
    {
        
        if (transform.root.gameObject.tag == "EnemyDead")
        {
            ToggleCircleOff();
            Messenger.RemoveListener("InteractableOn", ToggleCircleOn);
            Messenger.RemoveListener("InteractableOff", ToggleCircleOff);

            Messenger.RemoveListener("ToggleCircleOn", ToggleCircleOn);
            Messenger.RemoveListener("ToggleCircleOff", ToggleCircleOff);
        }
    }
}
