using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour 
{
	private float speed = 6f;
	private float turn = 5f;
	private Rigidbody rigidBody;
	private Vector3 movement;
	private float SpeedDamptime = 0.1f;

	private Animator anim;


	//Horizontal ROtate
	//vertical moveforward

	void Start () 
	{
		anim = GetComponent <Animator>();
		rigidBody = GetComponent <Rigidbody>();
	}

	void Update()
	{
	//	float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		float h = turn * Input.GetAxis("Mouse X");

	//	Move (h, v);

		Turning ();

		Animating (h,v);

		if (Input.GetKeyDown(KeyCode.E))
		{
			Death();
		}

	}
	/*
	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);

		movement = movement * speed * Time.deltaTime;

		rigidBody.MovePosition (transform.position + movement);
	}	*/

	void Turning ()
	{

	}

	void Animating (float h, float v)
	{
		anim.SetFloat ("Forward", v, SpeedDamptime, Time.deltaTime);
		anim.SetFloat ("Turn", h);
	}

	void Death()
	{		
		Debug.Log ("E pressed");
		anim.SetTrigger ("Dead");
	}
}
