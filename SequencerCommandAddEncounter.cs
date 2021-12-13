using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddEncounter: SequencerCommand
    {

        void Start()
        {


            // 0 = Units // 
            string path0 = GetParameter(0);

            string currentEncounters = DialogueLua.GetVariable("encounters").AsString;
            string newList = currentEncounters + "*" + path0;
            DialogueLua.SetVariable("encounters", newList);
            Debug.Log(path0 + "/" + currentEncounters + "/" + newList);
            Stop();
        }
    }
}



