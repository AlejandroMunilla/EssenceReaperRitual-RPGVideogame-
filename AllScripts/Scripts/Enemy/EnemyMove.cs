using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour 
{
	public Vector3 playerPos;
	public GameObject player;	
	public float speed = 1f;
	public float angle;	

	private float turn = 5f;
	private Rigidbody rigidBody;
	private Vector3 movement;
	private Animator anim;
	private Transform myTransform;
	private Transform target;
	private UnityEngine.AI.NavMeshAgent nav;
	private float xSpeed = 120.0f;
	private float ySpeed = 120.0f;
	private float x = 0.0f;
	private float y = 0.0f;
	private EnemyAI enemyAI;
	public bool playerInRange;
    public bool ignoreAttackDistance = false;   // This is to move enemies when they dont aim to attack a player
    public float distance = 6; 
    public float deadZone = 5f;             // The number of degrees for which the rotation isn't controlled by Mecanim.
	private HashIDs hash;                   // Reference to the HashIDs script.
	private AnimatorSetup animSetup;        // An instance of the AnimatorSetup helper class.
	private EnemyStats enemyStats;
	public float speedDampTime = 0.5f;              // Damping time for the Speed parameter.
	public float angularSpeedDampTime = 0.7f;       // Damping time for the AngularSpeed parameter
	public float angleResponseTime = 0.6f;          // Response time for turning an angle into angularSpeed
	
	void Start () 
	{
		anim = GetComponent <Animator>();
		rigidBody = GetComponent <Rigidbody>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		animSetup = GetComponent <AnimatorSetup>();
		enemyStats = GetComponent<EnemyStats>();
		hash = GetComponent<HashIDs>();
		myTransform = transform;
		enemyAI = GetComponent<EnemyAI>();
	}

    void OnEnable ()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animSetup = GetComponent<AnimatorSetup>();
        enemyStats = GetComponent<EnemyStats>();
        hash = GetComponent<HashIDs>();
        myTransform = transform;
        enemyAI = GetComponent<EnemyAI>();
        speed = 3;
    }
	
	void OnAnimatorMove ()
	{	
        if (Time.deltaTime == 0)
        {
            return;
        }
		// Set the NavMeshAgent's velocity to the change in position since the last frame, by the time it took for the last frame.
		nav.velocity = anim.deltaPosition / Time.deltaTime * 0.8f;
		
		// The gameobject's rotation is driven by the animation's rotation.
		transform.rotation = anim.rootRotation;
	}	
	
	public void NavAnimSetup ()
	{
		if (player != null)
		{

			// Create the parameters to pass to the helper function.

			float speed;
			float angle;
      //      Debug.Log(gameObject.name);
			float distPlayer = Vector3.Distance (player.transform.position, myTransform.position);

            // If the enemy is within attack range...


            float distanceToUse = enemyStats.attackRange;

            if (ignoreAttackDistance == true)
            {
                distanceToUse = distance;
            }

			if (distPlayer < distanceToUse)
			{

				if (nav.enabled == true)
				{
					nav.Stop();
				}

				// ... the enemy should stop...
				speed = 0f;
				
				// ... and the angle to turn through is towards the player.
				angle = FindAngle(transform.forward, player.transform.position - transform.position, transform.up);
				
				// ...and attack!!
				playerInRange = true;
				
			}
			else
			{		
				playerInRange = false;			
			//	nav.enabled = true;
			//	nav.destination = player.transform.position;
				nav.speed = 1;
			//	nav.Resume();
				
				// Otherwise the speed is a projection of desired velocity on to the forward vector...
				speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
				
				// ... and the angle is the angle between forward and the desired velocity.
				angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
				
				// If the angle is within the deadZone...
				if(Mathf.Abs(angle) < deadZone)
				{
					// ... set the direction to be along the desired direction and set the angle to be zero.
					transform.LookAt(transform.position + nav.desiredVelocity);
					angle = 0f;
				}
			}
			
			// Call the Setup function of the helper class with the given parameters.
			Setup(speed, angle);
		}
	}	
	
	float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		// If the vector the angle is being calculated to is 0...
		if(toVector == Vector3.zero)
			// ... the angle between them is 0.
			return 0f;
		
		// Create a float to store the angle between the facing of the enemy and the direction it's travelling.
		float angle = Vector3.Angle(fromVector, toVector);
		
		// Find the cross product of the two vectors (this will point up if the velocity is to the right of forward).
		Vector3 normal = Vector3.Cross(fromVector, toVector);
		
		// The dot product of the normal with the upVector will be positive if they point in the same direction.
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		
		// We need to convert the angle we've found from degrees to radians.
		angle *= Mathf.Deg2Rad;
		
		return angle;
	}
	
	public void Setup(float speed, float angle)
	{
		
		
		// Angular speed is the number of degrees per second.
		float angularSpeed = angle / angleResponseTime;
		
		// Set the mecanim parameters and apply the appropriate damping to them.
		anim.SetFloat("Forward", speed, speedDampTime, Time.deltaTime);
		anim.SetFloat("Turn", angularSpeed);
	}   
}
