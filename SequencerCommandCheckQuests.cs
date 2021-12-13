using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandCheckQuests : SequencerCommand
    {

        // Use this for initialization
        void Start()
        {
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            gc.GetComponent<CameraMap>().CheckActiveQuests();
            DialogueManager.SendUpdateTracker();
            Stop();
        }
    }
}
