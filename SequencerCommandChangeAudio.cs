using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandChangeAudio : SequencerCommand
    {
        private GameObject gc = null;

        void Start()
        {
            string path0 = GetParameter(0);
            if (gc == null)
            {
                gc = GameObject.FindGameObjectWithTag("GameController");
            
            }
            GameController gameController = gc.GetComponent<GameController>();


            //  string path0 = GetParameter(0);
            gameController.GetComponent<AudioController>().ChangeToOther(path0, 0.8f);

            Stop();
        }
    }
}



