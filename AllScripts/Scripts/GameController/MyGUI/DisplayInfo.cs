using UnityEngine;
using System.Collections;

public class DisplayInfo : MonoBehaviour {

    private int buttonWide;
    private int posX;
    private int posY;
    private int buttonHeight;
    private float lineHeight = 20;
    private int lineWidth;
    private int overlines = 0;
    public string text = null;
    public string[] description;
    public GUISkin skin;
    private Vector2 _ItemDesSlider = Vector2.zero;
    private Rect _ItemDesWindowRect = new Rect(1, 1, 30, 1000);
    private GUIStyle myStyle = new GUIStyle();
    private int heightScroll;

    // Use this for initialization
    public void Start ()
    {
        buttonWide = (int)(Screen.width * 0.25);
        posX = (int)(Screen.width * 0.75f);
        posY = (int)(Screen.height * 0.62f);
        buttonHeight = (int)(Screen.height * 0.28f);
        lineWidth = (int)(Screen.width * 0.23f);
        if (text == null)
        {
            text = "Game Loaded " + "\n";
        }
        
        skin = (GUISkin)(Resources.Load("GUI/Skin", typeof(GUISkin)));
        myStyle.fontSize = (int)(Screen.height * 0.020f);
        myStyle.normal.textColor = Color.white;
        myStyle.wordWrap = true;
   //     ChopText();
    }

    private void OnEnable()
    {
        UpdateLines();
    }

    void OnGUI ()
    {
        GUI.skin = skin;
        
        DisplayText();
   //     UpdateLines();
 //       lineHeight = lineHeight + 10;
    }

    void DisplayText ()
    {
        _ItemDesSlider = GUI.BeginScrollView(new Rect(posX, posY, buttonWide, buttonHeight), _ItemDesSlider,
                                      _ItemDesWindowRect);

        //     scrollPosition = Vector2.Lerp(scrollPosition, GUILayout.BeginScrollView(scrollPosition), .2);

        GUI.Label (new Rect(10, 20, lineWidth, lineHeight), text, myStyle);      

        GUI.EndScrollView();
    }

    public void AddText (string textToAdd)
    {
        text = text + textToAdd + "\n";
        UpdateLines();
    }

    void UpdateLines ()
    {
    //    overlines = 1 + (int)(text.Length / 40);
        //      lineHeight = (overlines * 30) + 10;
        lineHeight = myStyle.CalcHeight(new GUIContent(text), buttonWide);
        _ItemDesWindowRect = new Rect(0, 0, 1, ( lineHeight));
        _ItemDesSlider.y = lineHeight * 1.05f;
        
    }
}
