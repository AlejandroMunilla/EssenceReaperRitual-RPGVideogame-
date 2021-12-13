using UnityEngine;
using System.Collections;

public class MouseOrbitImp : MonoBehaviour 
{
//	[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = 1.0f;
	public float distanceMax = 15f;
    	
	private Rigidbody rigidbody;
	
	float x = 0.0f;
	float y = 0.0f;

    private string path;
	private GameObject gameCont;
    private GameController gameController;
	public GameObject activePlayer;
    private RtsCameraImp rtsCam;
    private RtsCameraMouse rtsCamMouse;

    void Awake()
    {
        rtsCam = GetComponent<RtsCameraImp>();
        rtsCamMouse = GetComponent<RtsCameraMouse>();
        gameCont = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameCont.GetComponent<GameController>();
    }

	// Use this for initialization
    /*
	void Start () 
	{
        Invoke("FindActivePlayer", 0.2f);
	}*/

    void OnEnable ()
    {
        rtsCam = GetComponent<RtsCameraImp>();
        rtsCamMouse = GetComponent<RtsCameraMouse>();
        gameCont = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameCont.GetComponent<GameController>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
        InvokeRepeating("FindActivePlayer", 0, 0);

    }

    void OnDisable ()
    {
        Messenger.Broadcast("ToggleCircleOn");
        Messenger.Broadcast("CameraRTSOn");
        CancelInvoke("FindActivePlayer");
    }

    public void FindActivePlayer ()
	{
		if (gameController.player != null)
        {
       //     Debug.Log(activePlayer);
            if (gameCont != null)
            {

                activePlayer = gameController.activePC;
          //      Debug.Log(activePlayer);
                if (activePlayer != null)
                {
                    
                    path = activePlayer.GetComponent<PlayerEquippedItems>().headCamera;
                    if (path != "No")
                    {
                        target = activePlayer.transform.Find(path);
                    }
                    else
                    {
                        target = activePlayer.GetComponent<PlayerEquippedItems>().taHeadCamera;
                    }

                    
                    Messenger.Broadcast("ToggleCircleOff");
                    Messenger.Broadcast("CameraRTSOff");
                    CancelInvoke("FindActivePlayer");
                }

            }
        }

	}
	
	void LateUpdate () 
	{
		if (target) 
		{
			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
			
			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) 
			{
                if (hit.transform.gameObject.tag == "Roof")
                {
     //                 ChangeController();
                }
                else
                {
                    distance -= hit.distance;
                }				
			}
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;			
			transform.rotation = rotation;
			transform.position = position;

            if (distance >= 4.0f)
                //activate RTS camera and disable this camera script
            {
                ChangeController();
            }
		}
    }

    public void ChangeController ()
    {
        if (activePlayer == null)
        {
            FindActivePlayer();
        }
        target = activePlayer.transform;
        Messenger.Broadcast("ToggleCircleOn");
        Messenger.Broadcast("CameraRTSOn");
        rtsCam.Distance = 5;
        rtsCam.Follow(target, true);
        rtsCamMouse.enabled = true;
        rtsCam.enabled = true;

        if (gameController.activePC.name == "Kira")
        {

        }

        else if (gameController.activePC.name != "Rose" && gameController.activePC.name != "Lycaon")
        {
            gameController.activePC.GetComponent<PlayerMoveAI>().target = gameController.activePC.GetComponent<TargetActivePC>().target;
            gameController.activePC.GetComponent<PlayerMoveAI>().enabled = true;
        }
        else if (gameController.activePC.name == "Rose")
        {
            gameController.activePC.GetComponent<PlayerMoveAIRose>().target = gameController.activePC.GetComponent<TargetActivePC>().target;
            gameController.activePC.GetComponent<PlayerMoveAIRose>().enabled = true;
        }
        else if (gameController.activePC.name == "Lycaon")
        {
            gameController.activePC.GetComponent<PlayerMoveAIBlackWolf>().target = gameController.activePC.GetComponent<TargetActivePC>().target;
            gameController.activePC.GetComponent<PlayerMoveAIBlackWolf>().enabled = true;
        }

        if (gameController.inDialogue == false)
        {
            if (gameCont.GetComponent<CombatController>().inCombat == false && gameCont.GetComponent<GameController>().inDialogue == false)
            {
                if (gameController.activePC.GetComponent<PlayerAI>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAI>().enabled = true;
                }
                else if (gameController.activePC.GetComponent<PlayerAIRose>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAIRose>().enabled = true;
                }
                else if (gameController.activePC.GetComponent<PlayerAIBlackWolf>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAIBlackWolf>().enabled = true;
                }

            }
            else
            {
                if (gameController.activePC.GetComponent<PlayerAICombat>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAICombat>().enabled = true;
                }
                else if (gameController.activePC.GetComponent<PlayerAIRose>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAIRose>().enabled = true;
                }
                else if (gameController.activePC.GetComponent<PlayerAIBlackWolf>() != null)
                {
                    gameController.activePC.GetComponent<PlayerAIBlackWolf>().enabled = true;
                }
            }
        }



        //    gameController.activePC.GetComponent<Rigidbody>().isKinematic = true;
        gameController.ChangeActivePlayer(gameController.activePC);

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 6, target.transform.position.z);
        this.enabled = false;
    
    }
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	public void ChangeActivePlayer(GameObject player)
	{
        if (player.name == "Lycaon")
        {
            path = "armature/root";
        }
        else
        {
            path = player.GetComponent<PlayerEquippedItems>().headCamera;
        }


        if (path != "No")
        {
            target = player.transform.Find(path);
        }
        else
        {
            target = player.GetComponent<PlayerEquippedItems>().taHeadCamera;
        }
    //    Debug.Log(player.name + "/" + path);
		
	}
}