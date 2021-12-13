using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour 
{
	public bool _alive;
	public bool InCombat;
    public bool AI = true;
    public bool dontMove = false;
    public bool sequence = false;
    public bool automaticPlayer = false;
	public float distToStop;
	public GameObject enemy;
	public GameObject player;
    public GameObject target;
    public float yieldTime = 0.5f;
    float onMeshThreshold = 3;
    private Vector3 goToPosition;
	private GameObject gc;
	private GameController gameController;
    private Camera mainCamera;
	private PlayerAttack playerAttack;
	private UnityEngine.AI.NavMeshAgent nav;
    private UnityEngine.AI.NavMeshObstacle navObs;
	private PlayerMoveAI playerMoveAI;
	private PlayerStats playerStats;
	private Animator anim;
    private RtsCameraImp rtsCameraImp;
	private float delay = 0.1f;

	public enum State
	{
		Idle,
		Move,
		Guard,
		Dead,
		Flee,
        IdleActive,
        MoveActive,
        DontMove,
        GoTo

	}
	public State _state;

	void Awake ()
	{
		distToStop = 3.0f;
		gc = GameObject.FindGameObjectWithTag("GameController");
		gameController = gc.GetComponent <GameController>();
        //this is used for certain courotines and sequences
        if (automaticPlayer == false)
        {
            player = gameController.player;
        }
		
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = GetComponent <Animator>();
		playerMoveAI = GetComponent<PlayerMoveAI>();
		playerAttack = GetComponent<PlayerAttack>();
		playerStats = GetComponent <PlayerStats>();
        mainCamera = Camera.main;
        rtsCameraImp = mainCamera.GetComponent<RtsCameraImp>();
        
	}


    void OnEnable ()
	{
        if (GetComponent<TargetActivePC>())
        {
            target = GetComponent<TargetActivePC>().target;
        }
        distToStop = 3.0f;
        nav.stoppingDistance = distToStop;
        gc = GameObject.FindGameObjectWithTag("GameController");
        GameController gameController = gc.GetComponent<GameController>();
        if (gameController.player != gameObject)
        {
            //this is used for certain courotines and sequences
            if (automaticPlayer == false)
            {
                player = gameController.player;
            }            
        }
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerMoveAI = GetComponent<PlayerMoveAI>();
        playerAttack = GetComponent<PlayerAttack>();
        playerStats = GetComponent<PlayerStats>();
        //	myTransform = transform;
        _alive = true;
        if (nav.enabled == false)
        {
            nav.enabled = true;
        }
        nav.Resume();
        if (target != null)
        {            
            target.transform.position = transform.position;
            if (mainCamera.GetComponent<MouseOrbitImp>().enabled)
            {
                target.SetActive(false);
            }
            else
            {
                target.SetActive(true);
            }
        }

  //      Debug.Log(playerStats.curHealth);
        if (playerStats.curHealth > 0)
        {
            if (sequence == true)
            {
                StartCoroutine("FSM");
                _state = PlayerAI.State.Idle;
                return;
            }

            //if the controlled player is this object (this script is attached to).
            if (gameController.activePC == gameObject)
            {
                if (mainCamera.GetComponent<RtsCameraImp>().enabled == true)
                {
                    StartCoroutine("FSM");
                    _state = PlayerAI.State.IdleActive;
                }
                else
                {
                    this.enabled = false;
                }
            }
            //if gameobject is not the PC in control 
            else
            {
                if (target != null)
                {
                    target.SetActive(false);
                }


                StartCoroutine("FSM");
                _state = PlayerAI.State.Idle;
            }
        }
        else
        {
            //         Debug.Log(playerStats.curHealth);

            this.enabled = false;
        }

        bool navMeshActive = IsAgentOnNavMesh(gameObject);
     //   Debug.Log(gameObject.name + " navMesh: " + navMeshActive);

        if (navMeshActive == false)
        {
            bool isNavEnable = false;
            if (nav.enabled == true)
            {
                nav.enabled = false;
                isNavEnable = true;
            }

            transform.position = gc.GetComponent<LoadGame>().posBug;

            if (isNavEnable == true)
            {
                nav.enabled = true;
            }
        }

    }

	void OnDisable ()
	{
	//	GameController.OnChangePlayer += ChangePlayer;
        StopAllCoroutines();
	}

    public void OnDestroy ()
    {
        GameController.OnChangePlayer -= ChangePlayer;
        StopAllCoroutines();
    }

    // Don't set this too high, or NavMesh.SamplePosition() may slow down
    

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }

    private IEnumerator FSM ()
	{
		while (_alive)
		{
			switch (_state)
			{
                case State.Idle:
                    Idle();
                    yield return new WaitForSeconds(yieldTime);
                    break;

                case State.Move:
                    Move();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Dead:

                    break;

                case State.IdleActive:
                    IdleActive();
                    break;

                case State.MoveActive:
                    MoveActive();
                    break;

                case State.DontMove:
                    DontMove();
                    yield return new WaitForSeconds(1.5f);
                    break;

                case State.GoTo:
                    GoTo();
                    yield return new WaitForSeconds(0);
                    break;


            }
			yield return null;
		}
	}	

	private void Idle ()
	{
    //    Debug.Log("Idle");
        anim.SetFloat ("Forward", 0);
		anim.SetFloat ("Turn", 0);		
		nav.Stop();


        if (gameController.globalAI == true)
        {
            if (player == null || AI == false)
            {
                return;
            }

            float distPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distPlayer > distToStop)
            {
                _state = PlayerAI.State.Move;
            }
            else
            {
                return;
            }
        }

	}

	private void Move ()
	{
    //    Debug.Log("Move");
        nav.enabled = true;
		nav.Resume();
        if (player != null)
		{
			playerMoveAI.target = player;
			playerMoveAI.distToStop = distToStop;
			float distPlayer = Vector3.Distance (player.transform.position, transform.position);
      //      Debug.Log("Move");
            if (distPlayer < (distToStop - 0.1))
			{
				_state = PlayerAI.State.Idle;
				playerMoveAI.NavAnimSetup();
			}
			else
			{
				playerMoveAI.target = player;
				playerMoveAI.distToStop = distToStop;
				playerMoveAI.NavAnimSetup();
			}
		}
		else
		{
			player = gameController.activePC;
		}
	}

    private void IdleActive ()
    {
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);
        nav.Stop();
   //     Debug.Log(GetComponent<TargetActivePC>().target);
        if (target != null)
        {
            float distTarget = Vector3.Distance(target.transform.position, transform.position);
   //         Debug.Log(distTarget + "/" + gameObject);
            if (distTarget > 0.3f)
            {
                target.SetActive(true);
                _state = PlayerAI.State.MoveActive;
            }
            else
            {
                //if this PC is not the active PC, then after arriving upon destination, 
                // change it to state idle.
                if (gameController.activePC != gameObject)
                {
                    //                Debug.Log("Idle" + gameObject +"/active" + gameController.activePC);
                    if (dontMove == false)
                    {
                        _state = PlayerAI.State.Idle;
                    }
                    else
                    {
                        _state = PlayerAI.State.DontMove;
                    }                    
                }                
            }
        }
        
    }

    private void MoveActive()
    {
        nav.enabled = true;
        nav.Resume();
    //    Debug.Log(target);
        if (target != null)
        {
            playerMoveAI.target = target;
            playerMoveAI.distToStop = 0.3f;
            float distTarget = Vector3.Distance(target.transform.position, transform.position);
       //     Debug.Log(distTarget);
            target.SetActive(true);
            if (distTarget < (0.3f))
            {
                _state = PlayerAI.State.IdleActive;
                target.SetActive(false);
                playerMoveAI.NavAnimSetup();
     //           Debug.Log("ToIdleActive");
            }
            else
            {
                playerMoveAI.target = target;
                playerMoveAI.distToStop = 0.3f;
                playerMoveAI.NavAnimSetup();
            }
        }
        else
        {
            target.SetActive(false);
            _state = PlayerAI.State.IdleActive;
            playerMoveAI.NavAnimSetup();
        }
    }

    private void DontMove ()
    {
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Turn", 0);
        nav.Stop();

        if (dontMove == false)
        {
            _state = PlayerAI.State.Idle;
        }
    }

    private void GoTo ()
    {
   //     Debug.Log("goto" + gameObject.name);
        nav.enabled = true;
        nav.Resume();

        if (target != null)
        {
            playerMoveAI.target = target;
            playerMoveAI.distToStop = 0.3f;
            float distTarget = Vector3.Distance(target.transform.position, transform.position);

            if (distTarget < (0.5f))
            {
                _state = PlayerAI.State.Idle;
                target.SetActive(false);
                playerMoveAI.NavAnimSetup();
            }
            else
            {
                playerMoveAI.target = target;
                playerMoveAI.distToStop = 0.3f;
                playerMoveAI.NavAnimSetup();
            }
        }
        else
        {
            target.SetActive(false);
            _state = PlayerAI.State.Idle;
            playerMoveAI.NavAnimSetup();
        }
    }

    private void ChangePlayer(GameObject playera)
	{
		player = playera;

        if (player == gameObject)
        {
            // it means we are in tactical view, AI should be activated
           if (mainCamera.GetComponent<RtsCameraImp>().enabled == true)
            {
                ChangeToActive();
            }
           else
            {
                
            }
        }
        else
        {
            ChangeToNoActive();
        }
	}

    public void ChangeToNoActive ()
    {
        StopAllCoroutines();

        if (gameObject.activeSelf)
        {
    //        Debug.Log(gameObject.name);
            StartCoroutine("FSM");
            float distTarget = Vector3.Distance(target.transform.position, transform.position);

            if (distTarget < (0.3f))
            {
                if (dontMove == false)
                {
                    _state = PlayerAI.State.Idle;
                }
                else
                {
                    _state = PlayerAI.State.DontMove;
                }
            }
            else
            {
                _state = PlayerAI.State.GoTo;
            }
        }
            
    }
    
    public void ChangeToActive ()
    {/*
        StopAllCoroutines();
        StartCoroutine("FSM");*/
        _state = PlayerAI.State.IdleActive;
        if (target != null)
        {
            target.transform.position = transform.position;
            target.SetActive(false);
        }

        if (rtsCameraImp.enabled == true)
        {
            if (gameController.activePC == gameObject)
            {
                if (gameObject.name == "Weirum")
                {
                    GetComponent<ThirdPersonUserControlWolf>().enabled = false;
           //         GetComponent<ThirdPersonCharacterWolf>().enabled = false;
                }
                else
                {
                    
                    GetComponent<ThirdPersonUserControl>().enabled = false;
                }
                
            }
        }

    }

    public void ChangeToGoTo (Vector3 pos)
    {
        target.transform.position = pos;
        _state = PlayerAI.State.GoTo;
    }    
}
