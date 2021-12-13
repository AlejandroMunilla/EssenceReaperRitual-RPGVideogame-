using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandChangeLilith : SequencerCommand
    {
        private GameObject gc = null;

        void Start()
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
            gc.GetComponent<SetUp06Vivace>().ActivateLilith();
            Stop();
        }
    }
}



