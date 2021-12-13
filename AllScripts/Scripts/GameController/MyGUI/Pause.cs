using UnityEngine;

public class Pause : MonoBehaviour {

    private int xPos;
    private int yPos;
    private int width;
    private int height;
	// Use this for initialization
	void Start ()
    {        
        xPos = (int)(Screen.width * 0.4f);
        yPos = (int)(Screen.height * 0.20f);
        width = (int)(Screen.width * 0.18f);
        height = (int)(Screen.height * 0.05f);
	}

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    void OnGUI ()
    {
        GUI.color = Color.red;
        GUI.Box (new Rect(xPos, yPos, width, height), "PAUSE");
    }


}
