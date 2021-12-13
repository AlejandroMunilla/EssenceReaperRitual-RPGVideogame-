using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;



public class myDSGUI : MonoBehaviour
{
    void OnConversationLine(Subtitle subtitle)
    {
        Debug.Log(subtitle.formattedText.text);



    }
}
 

