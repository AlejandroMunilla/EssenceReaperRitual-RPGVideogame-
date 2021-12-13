using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandInn : SequencerCommand
    {
        private GameObject gc;
        private void Start ()
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
            Time.timeScale = 1;
            gc.GetComponent<InnRest>().enabled = true;
            Stop();
        }
    }
}
