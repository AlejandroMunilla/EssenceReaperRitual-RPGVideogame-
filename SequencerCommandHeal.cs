using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandHeal : SequencerCommand
    {
        void Start()
        {
            string path1 = GetParameter(0);
            int path2 = int.Parse (GetParameter(1));            
            int path3 = int.Parse ( GetParameter(2));
    //       Debug.Log(path1 + "/" + path2 + "/" + path3);
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            GameController gameController = gc.GetComponent<GameController>();
            gameController.Heal(path1, path2, path3);
            Stop();
        }
    }

}
