using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAreaTrapRat : SequencerCommand
    {
        private GameObject gc = null;

        void Start()
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
            GameObject areaTrap = gc.GetComponent<SetUp03TheCross>().areaTrapRat;

            string questState = DialogueLua.GetQuestField("Rats", "State").AsString;

            if (questState == "active")
            {
                areaTrap.SetActive(true);
                areaTrap.GetComponent<BoxCollider>().enabled = true;
                areaTrap.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                areaTrap.SetActive(false);
            }


            Stop();
        }
    }
}



