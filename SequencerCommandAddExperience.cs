using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddExperience : SequencerCommand
    {

        // Use this for initialization
        void Start()
        {
            int exp = GetParameterAsInt(0);
     //       Debug.Log(exp);
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            ExpController expController = gc.GetComponent<ExpController>();
            expController.FinalExp(exp);
            Stop();
        }
    }
}
