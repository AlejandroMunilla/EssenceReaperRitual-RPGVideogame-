using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAddPartyMember : SequencerCommand
    {
        private GameObject gc = null;
        private GameObject goToRemove;
        

        void Start()
        {
            if (gc == null)
            {
                gc = GameObject.FindGameObjectWithTag("GameController");
                
            }
            

            // 0 = Units // 
            string path0 = GetParameter(0);
            gc.GetComponent<PartyManager>().AddNewMember(path0);
            DialogueLua.SetActorField(path0, "vanguard", "Yes");
             InvokeRepeating ("CheckEnd", 0, 0);
            
        }

        private void CheckEnd()
        {

            string path0 = GetParameter(0);
            Debug.Log(path0);
            GameController gameController = gc.GetComponent<GameController>();
            if (gameController.removePC.Count > 0)
            {
                DialogueLua.SetActorField(path0, "vanguard", "Yes");
                foreach (GameObject go in gameController.removePC)
                {
                    if (go.name == path0)
                    {
                        if (go.activeSelf)
                        {
                            go.SetActive(false);
                            goToRemove = go;
                            go.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
                            gc.GetComponent<PartyManager>().InvokeActiveSelf();

                        }
                        else
                        {
                            
                            CancelInvoke("CheckEnd");
                            DialogueLua.SetActorField(path0, "vanguard", "Yes");
                            Stop();
                        }
                    }
                }
            }
        }

    }


}



