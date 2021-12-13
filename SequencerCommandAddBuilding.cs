using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddBuilding : SequencerCommand
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

            // 0 = Units // 
            string path0 = GetParameter(0);

            assets.AddBuilding(path0);
            Debug.Log(path0);
            Stop();
        }
    }
}



