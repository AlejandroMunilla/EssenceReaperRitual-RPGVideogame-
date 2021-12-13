using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonWolf : MonoBehaviour {

	Animation anim;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent <Animation>();
	}
	
	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.W))
		{
			Debug.Log ("Walk");
			anim.AddClip (anim.clip, "Walk");
		}

	}
}
