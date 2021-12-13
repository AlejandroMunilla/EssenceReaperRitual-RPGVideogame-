using UnityEngine;

public class DisplayQuestsGUI : MonoBehaviour
{
    private int buttonWidth;
    private int buttonHeight;
    private int buttonX;
    private int buttonY;

    void Awake ()
    {
        buttonWidth = (int)(Screen.width * 0.2f);
        buttonHeight = (int)(Screen.height * 0.1f);
        buttonX = Screen.width - buttonWidth;
        buttonY = Screen.height - buttonHeight;
    }

    void OnDisable ()
    {
        GetComponent<PlayerControls>().enabled = true;
        if (GetComponent<GeneralWindow>().guiRootGameObject != null)
        {
            GetComponent<GeneralWindow>().guiRootGameObject.SetActive(false);
        }        
    }

    void OnGUI ()
    {
        GUI.skin = GetComponent<GeneralWindow>().mySkin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), GetComponent<GeneralWindow>(). background);
        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "BACK"))
        {
            GetComponent<GeneralWindow>().enabled = true;
            GetComponent<DisplayToolBar>().enabled = true;
            this.enabled = false;
        }
    }

}
