using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandCameraControl: SequencerCommand
    {

        void Start()
        {


            // 0 = Units // 
            string path0 = GetParameter(0);
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            GameObject camera = null;
            GameObject target = null;
            Camera mainCamera = Camera.main;
            foreach (GameObject go in gameController.cameraDialogue)
            {
                if (go.name == path0)
                {

                    camera = go;
                    target = camera.transform.root.gameObject;
                    //                 Debug.Log(go.name + "/" + path0 + "/" + target.name);
                }
            }
            gameController.SetUpCameraToDialogue();

            //         Debug.Log(path0);
            if (camera != null)
            {
                mainCamera.transform.position = camera.transform.position;
                mainCamera.transform.rotation = camera.transform.rotation;
            }



            Stop();
        }
    }
}



