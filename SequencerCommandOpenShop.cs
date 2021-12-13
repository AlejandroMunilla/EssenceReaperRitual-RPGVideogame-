using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandOpenShop : SequencerCommand
    {

        GameObject gc = null;
        // Use this for initialization
        void Start()
        {
            string path = GetParameter(0);
            gc = GameObject.FindGameObjectWithTag("GameController");
            gc.GetComponent<Shopping>().merchantName = path;
            gc.GetComponent<Shopping>().InvokeRepeating ("InvokeEnable", 0.1f, 0.1f);
            Stop();
        }

        
    }
}
