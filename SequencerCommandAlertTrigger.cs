using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAlertTrigger : SequencerCommand
    {
        private AlertTrigger alertTrigger = null;
        private DisplayAssets assets;
        void Start()
        {
            string alertTxt = GetParameter(0);

            if (alertTrigger == null)
            {
                alertTrigger = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<AlertTrigger>();
                Debug.Log(alertTrigger);
                
            }
            alertTrigger.message = alertTxt;
            alertTrigger.OnUse();
        }
    }
}



