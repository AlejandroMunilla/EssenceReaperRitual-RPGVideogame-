using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandChangeScene : SequencerCommand
    {
        private GameObject gc = null;
        private DisplayAssets assets;
        void Start()
        {

            // 0 = Units // 
            string path0 = GetParameter(0);
            Debug.Log(path0);

            UnityEngine.SceneManagement.SceneManager.LoadScene(path0);
            Stop();
        }
    }
}



