using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddInventoryItem : SequencerCommand
    {
        void Start()
        {
            string path = GetParameter(0);

            string type = DialogueLua.GetActorField(path, "type").AsString;
            if (type == "misc")
            {
                Debug.Log(type);
                string miscInventory = DialogueLua.GetVariable("miscInventory").AsString;
                if (miscInventory.Length == 0 )
                {

                    string itemsAdd = path;
                    DialogueLua.SetVariable("miscInventory", itemsAdd);
                    Debug.Log(itemsAdd);
                }
                else
                {
                    string itemsAdd = miscInventory + "*" + path;
                    DialogueLua.SetVariable("miscInventory", itemsAdd);
                    Debug.Log(itemsAdd);
                }
            }
            else if (type == "potion")
            {
                Debug.Log("Add potion script!!!!!!!!!!!!!!!!!!!!!!!");
            }
            else
            {
                string genInventory = DialogueLua.GetVariable("GeneralInventory").AsString;
                if (genInventory.Length == 0)
                {

                    string itemsAdd = path;
                    DialogueLua.SetVariable("GeneralInventory", itemsAdd);
                }
                else
                {
                    string itemsAdd = genInventory + "*" + path;
                    DialogueLua.SetVariable("GeneralInventory", itemsAdd);
                    //           Debug.Log(itemsAdd);
                }
            }


            DialogueManager.ShowAlert(DialogueLua.GetActorField (path, "name").AsString + " added to Inventory");
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            gc.GetComponent<DisplayItemScript>().UpDateInventory();
      //      Debug.Log("Dialogue");
            Stop();
        }
    }

}
