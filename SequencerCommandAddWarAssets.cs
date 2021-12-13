using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddWarAssets : SequencerCommand
    {
        private GameObject gc = null;
        private DisplayAssets assets;
        void Start()
        {
            if (gc == null)
            {
                gc = GameObject.FindGameObjectWithTag("GameController");
                assets = gc.GetComponent<DisplayAssets>();
            }
            
            // 0 = Units // 1 = Heroes // 2 = Buildings // 3 = Allies
            string path0 = GetParameter(0);
            string path1 = GetParameter(1);
            string path2 = GetParameter(2);
            string path3 = GetParameter(3);

            if (path0 != "No")
            {
                
                assets.units.Add(path0);
                Debug.Log(path0);
            }

            if (path2 != "No")
            {
                assets.buildings.Add(path2);
                Debug.Log(path2);
            }

            if (path3 != "No")
            {
                assets.allies.Add(path3);
                Debug.Log(path3);
            }

            Stop();
        }
    }
}



