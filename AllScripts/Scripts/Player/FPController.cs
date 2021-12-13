using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class FPController : MonoBehaviour 
{
	private Animator anim;
	private float x;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis ("Mouse X");

		anim.SetFloat("Turn", h, 0.1f, Time.deltaTime);

		if (Input.GetKey(KeyCode.W))
		{
			Debug.Log ("W");
		//	anim.SetLayerWeight (1,1);
			anim.SetFloat ("Forward", 3, 0.5f, Time.deltaTime);

			transform.Rotate(0, h * 6 * Time.deltaTime, 0);
		}
		else
		{
		//	anim.SetLayerWeight (1,0);
		}


	}
}
