using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Collections;


//    [RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControlWolf : MonoBehaviour
{
    private ThirdPersonCharacterWolf m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
	private Animator anim;
	private GameObject gc;
	private GameController gameController;
	private bool active = true;

	Vector3 m_GroundNormal;

	private enum State
	{
		IdleInCombat,
		MoveInCombat,
		Guard,
		Attack,
		Flee
	}
	
	private State _state;
        
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
	
        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacterWolf>();
		anim = GetComponent <Animator>();

		gc = GameObject.FindGameObjectWithTag ("GameController");
		if (gc != null)
		{
			gameController = gc.GetComponent <GameController>();
		}
    }

	void OnEnable ()
	{
		InvokeRepeating ("CheckMainPlayer", 0.1f, 2);
		StartCoroutine ("FSM");
		_state = ThirdPersonUserControlWolf.State.IdleInCombat;
	}

	void OnDisable ()
	{
		CancelInvoke();
		StopCoroutine ("FSM");
	}


	private IEnumerator FSM ()
	{
		while (active)
		{
			switch (_state)
			{				
			case State.IdleInCombat:
				Controls();
				break;
				
			case State.MoveInCombat:
				break;		

			}
			yield return null;
		}
	}

	void Controls ()
	{

		if (Input.GetKeyUp(KeyCode.I))
		{
			Messenger.Broadcast ("ToggleInventory");
		}

		if (Input.GetKeyUp(KeyCode.K))
		{
			Messenger.Broadcast ("ToggleCharacterWindow");
		}
			
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Messenger.Broadcast ("InteractableOn");	
		}
		else if (Input.GetKeyUp(KeyCode.Tab))
		{
			Messenger.Broadcast ("InteractableOff");	
		}

        /*
		if (Input.GetKeyDown(KeyCode.F))
		{

			anim.SetTrigger("Attack1");
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			
			anim.SetTrigger("Attack2");
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			
			anim.SetBool("Dead", true);
		} */
	}

	// Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
    //    bool crouch = Input.GetKey(KeyCode.C);
		bool crouch = false;

        // calculate move direction to pass to character
        if (m_Cam != null)
       	{
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v*Vector3.forward + h*Vector3.right;
        }
#if !MOBILE_INPUT
	// walk speed multiplier
	    if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script
    	m_Character.Move(m_Move, false, false);

        m_Jump = false;
    }




	void CheckMainPlayer()
	{
		if (gc != null)
		{

			if (gameController.activePC != gameObject)
			{
				CancelInvoke("CheckMainPlayer");
				this.enabled = false;
				Debug.Log ("CheckMainPlayer condition met");
			}
		}
	}
}

