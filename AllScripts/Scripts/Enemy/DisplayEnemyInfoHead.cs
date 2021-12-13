using UnityEngine;
using System.Collections;

public class DisplayEnemyInfoHead : MonoBehaviour {

	private float heightAddjust;
	private GameObject parent;
	private EnemyStats enemyStats;
	public GUIStyle myGUI;

	// Use this for initialization
	void Start () 
	{
		heightAddjust = Screen.height;
		parent = transform.parent.gameObject;
	//	Debug.Log (parent);
		enemyStats = parent.GetComponent <EnemyStats>();
        this.enabled = false;
	}

    void Enable ()
    {
        heightAddjust = Screen.height;
        parent = transform.parent.gameObject;
        //	Debug.Log (parent);
        enemyStats = parent.GetComponent<EnemyStats>();
        this.enabled = false;
    }

	void OnGUI ()
	{

	//	GUI.color = Color.black;
		Vector2 targetPos = Camera.main.WorldToScreenPoint (transform.position);
		GUI.Button (new Rect (targetPos.x, (Screen.height - targetPos.y), 50, 25), parent.name + " " + enemyStats.curHealth + "/" + enemyStats.health, myGUI);
	}
}
