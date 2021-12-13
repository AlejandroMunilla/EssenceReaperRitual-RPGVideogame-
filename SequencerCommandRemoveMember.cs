using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandRemoveMember : SequencerCommand
    {
        private GameObject gc = null;
        void Start()
        {
            if (gc == null)
            {
                gc = GameObject.FindGameObjectWithTag("GameController");
            }

            // 0 = Units // 
            string path0 = GetParameter(0);
            Debug.Log(path0);
            if (DialogueLua.GetActorField (path0, "inParty").AsString == "Yes")
            {
                gc.GetComponent<PartyManager>().RemoveMember(path0, true);
            }
            else if (DialogueLua.GetActorField(path0, "vanguard").AsString == "Yes")
            {
                gc.GetComponent<PartyManager>().RemoveMember(path0, false);
            }
            
            Stop();
        }
    }
}



