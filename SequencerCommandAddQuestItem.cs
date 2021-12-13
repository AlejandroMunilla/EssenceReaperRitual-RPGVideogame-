using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddQuestItem : SequencerCommand
    {
        void Start()
        {
            string path = GetParameter(0);
            string questItems = DialogueLua.GetVariable("QuestItems").AsString;
            if (questItems.Length == 0)
            {

                string itemsAdd = questItems + "*" + path;
                DialogueLua.SetVariable("QuestItems", itemsAdd);
            }
            else
            {
                string itemsAdd = questItems + "*" + path;
                DialogueLua.SetVariable("QuestItems", itemsAdd);
          //      Debug.Log(itemsAdd);
            }
            DialogueManager.ShowAlert(DialogueLua.GetActorField (path, "name").AsString + " added to Quest Objects");
            Stop();
        }
    }

}
