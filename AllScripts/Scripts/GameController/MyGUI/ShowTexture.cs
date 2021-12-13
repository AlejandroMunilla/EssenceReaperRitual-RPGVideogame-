using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTexture : MonoBehaviour {

    public Texture2D textureToShow;



    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureToShow);
    }
}
