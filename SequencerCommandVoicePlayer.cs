using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandVoicePlayer : SequencerCommand
    {

        // Use this for initialization
        void Start()
        {
            /*
            
            string path = GetParameter(0);
      //      Debug.Log(path);
            string gender = DialogueLua.GetActorField("Player", "gender").AsString;
    //        Debug.Log(gender);
            AudioClip audio = (AudioClip)(Resources.Load("Audio/Player/"+ gender + "/" + path));
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = audio;            
            audioSource.Play();
            Stop();
            */
        }
    }
}
