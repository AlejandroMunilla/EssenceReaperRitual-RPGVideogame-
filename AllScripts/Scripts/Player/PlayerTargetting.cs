using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTargetting : MonoBehaviour 
{
	public GameObject target;
	public bool targetEnemy;

	private Camera mainCamera;
	private Transform myTransform;


	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main;
		myTransform = transform;
		targetEnemy = false;
		Screen.lockCursor = true;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		RayCastTargetting();
		Screen.lockCursor = true;
		Cursor.visible = true;
	}

	void RayCastTargetting ()
	{
		if (Input.GetMouseButtonDown(0))
		{

			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, 100))
			{
				float distance = Vector3.Distance (hit.transform.position, myTransform.position);
				
				string hitTag = hit.transform.tag;

				
				if (hitTag == Tags.enemy)
				{
					target = hit.transform.gameObject;
					targetEnemy = true;
				}	
			}
		}
	}
}


