using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandPlayAudio : SequencerCommand
    {

        // Use this for initialization
        void Start()
        {
            string path = GetParameter(0);
        //    GameObject gc = GameObject.FindGameObjectWithTag("GameController");

            AudioClip audio = (AudioClip)(Resources.Load("Audio/" + path));
      //      Debug.Log(path);
            AudioSource audioSource = GetComponent<AudioSource>();
        //    AudioSource audioSource = gc.transform.Find ("Barks").GetComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.clip = audio;            
            audioSource.Play();

            Stop();

        }
    }
}
