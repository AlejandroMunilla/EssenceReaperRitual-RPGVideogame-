using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddUnit : SequencerCommand
    {
        private int armySize = 0;
        private GameObject gc = null;
        private DisplayAssets assets;
        void Start()
        {
            if (gc == null)
            {
                gc = GameObject.FindGameObjectWithTag("GameController");
                assets = gc.GetComponent<DisplayAssets>();
            }

            // 0 = Units // 
            string path0 = GetParameter(0);

            assets.AddUnit(path0);
      //      Debug.Log(path0);
            Stop();
            armySize = assets.CheckArmySize();
       //     Debug.Log(armySize);
            CheckQuests();

        }

        private void CheckQuests()
        {
            string theCross = DialogueLua.GetActorField("03TheCross", "recovered").AsString;
            if (armySize == 3 && theCross != "Yes")
            {
                if (DialogueLua.GetActorField ("TheCrossBattle", "visible").AsString != "Yes")
                {
                    DialogueLua.SetActorField("TheCrossBattle", "visible", "Yes");
                    DialogueLua.SetActorField("TheCrossBattle", "open", "Yes");
                    gc.GetComponent<WorldMap>().Invoke("CheckAllFlags", 0.1f);
                }
            }

            if (armySize == 4)
            {
                DialogueLua.SetActorField("Quivira", "visible", "Yes");
                DialogueLua.SetActorField("Quivira", "open", "Yes");

                gc.GetComponent<WorldMap>().Invoke("CheckAllFlags", 0.1f);

                DialogueLua.SetQuestField("Undead_Lord", "Description", "You found out that a powerful necromancer, Samael, is back. He was close to conquer the world one thousand years ago. You need to go to the ruins of Quivira to try to kill Samael with the help of Dalila, somehow");
                DialogueLua.SetQuestField("Army", "Description", "done");


            }
        }


    }
}



