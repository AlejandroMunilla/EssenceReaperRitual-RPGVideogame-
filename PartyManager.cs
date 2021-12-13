using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour 
{
    private int portraitWidth;
    private int screenWidth;
    private int screenHeight;
    private int buttonHeight;
    private Vector3 pos01;
    private Vector3 pos02;
    private Vector3 pos03;
    private Vector3 pos04;
    private Vector3 pos05;
    private GameController gameController;
    private List<string> inParty = new List<string>();
    private List<string> members = new List<string>();
    private Texture2D inParty1;
    private Texture background;
    private GUISkin mySkin;
    private Camera mainCamera;

    
    void OnEnable()
    {
        mainCamera = Camera.main;
        mainCamera.GetComponent<MouseOrbitImp>().enabled = false;
        mainCamera.GetComponent<RtsCameraImp>().enabled = false;
        mainCamera.GetComponent<RtsCameraMouse>().enabled = false;
        gameController = GetComponent<GameController>();
        gameController.activePC.GetComponent<TargetActivePC>().enabled = false;
        GetLists();
        //      Debug.Log("On");
        portraitWidth = (int)(Screen.width * 0.11f);
        screenHeight = (int)(Screen.height);
        screenWidth = (int)(Screen.width);
        buttonHeight = (int)(Screen.height * 0.06f);
        background = GetComponent<GeneralWindow>().background;
        mySkin = GetComponent<GeneralWindow>().mySkin;
        gameController.inDialogue = true;
        GetComponent<CameraMap>().TurnOffAllQuests();
        gameController.playerConversation.GetComponent<PlayerConversation>().OnConversationStart();
        GetComponent<DisplayPortraits>().enabled = false;
        gameController.inDialogue = true;
    }


    void OnGUI ()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture (new Rect(0, 0, screenWidth, screenHeight), background);

        GUI.Label(new Rect(Screen.width * 0.15f, Screen.height *0.10f, Screen.width * 0.75f, Screen.height *0.08f), "CHARACTERS IN GAME PARTY");
        if (GUI.Button(new Rect(Screen.width * 0.15f, Screen.height * 0.20f, portraitWidth, portraitWidth), inParty1))
        {
            DialogueManager.ShowAlert("Main character may not leave the party");
        }

        int lineNo = 1;
        for (int cnt = 1; cnt < inParty.Count; cnt++)
        {
            if (GUI.Button(new Rect(Screen.width * 0.15f + (portraitWidth * lineNo), Screen.height * 0.20f, portraitWidth, portraitWidth), (Texture2D)(Resources.Load("Portraits/" + inParty[cnt]))))
            {
                string toRemove = inParty[cnt];
                members.Add(toRemove);
                inParty.RemoveAt (cnt);                
            }
            lineNo++;
        }

        GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.45f, Screen.width * 0.75f, Screen.height * 0.08f), "ALLIED READY TO JOIN YOUR PARTY");

        int lineNo2 = 0;
        if (members.Count > 0)
        {
            for (int cnt = 0; cnt < members.Count; cnt++)
            {
                if (GUI.Button(new Rect(Screen.width * 0.15f + (portraitWidth * lineNo2), Screen.height * 0.50f, portraitWidth, portraitWidth), (Texture2D)(Resources.Load("Portraits/" + members[cnt]))))
                {
                    if (inParty.Count < 5)
                    {
                        string toRemove = members[cnt];
                        inParty.Add(toRemove);
                        members.RemoveAt(cnt);
                    }
                    else
                    {
                        DialogueManager.ShowAlert("Maximum 5 party members");
                    }

                }
                lineNo2++;
            }
        }


        if (GUI.Button(new Rect(Screen.width * 0.15f, Screen.height * 0.85f, portraitWidth, buttonHeight), "DONE"))
        {
            DoneAndExit();
        }
    }

    private void GetLists ()
    {
        inParty.Clear();
        members.Clear();
        string listMembers = DialogueLua.GetVariable("listMembers").AsString;
   //     string listMembers = "Oleg";
        string listInParty = DialogueLua.GetVariable("PCList").AsString;
        Debug.Log(listInParty);
        string [] arrayMembers = listMembers.Split(new string[] { "*" }, System.StringSplitOptions.None);
        string [] arrayInParty = listInParty.Split(new string[] { "*" }, System.StringSplitOptions.None);

        inParty.Add("Player");
        inParty1 = (Texture2D)(Resources.Load("Portraits/Player/Human/Female"));

        foreach (string st in arrayInParty)
        {
            if (st != "null" && st != "")
            {
                if (st != "Player")
                {
                    inParty.Add(st);
                }
            }
    //      Debug.Log(st);

                
        }

        if (arrayMembers.Length > 0)
        {
            if (arrayMembers.Length == 1 && arrayMembers[0] == "null")
            {
                members.Clear();
            }
            else
            {
                for (int cnt = 0; cnt < arrayMembers.Length; cnt++)
                {
                    if (arrayMembers[cnt] != "null")
                    {
                        if (arrayMembers[cnt] == "Player" || arrayMembers[cnt] == "Dylan")
                        {

                        }
                        else
                        {
                            members.Add(arrayMembers[cnt]);
                        }
                    }
                    else
                    {
                        members.Clear();
                    }
                }
            }
        }
        else
        {
            members.Clear();
            Debug.Log(members.Count);
        }


    }

    public void AddNewMember(string newMember)
    {
        string listMembers = DialogueLua.GetVariable("listMembers").AsString;


        if (listMembers == "" || listMembers == null || listMembers == "null")
        {
            listMembers = newMember;
     //       Debug.Log(listMembers);

        }
        else
        {
            listMembers = listMembers + "*" + newMember;
        }

        DialogueLua.SetVariable("listMembers", listMembers);
        DialogueLua.SetActorField(newMember, "vanguard", "Yes");
        Debug.Log(listMembers);
    }

    private void DoneAndExit ()
    {
        Debug.Log("DoneAndExit");
        Time.timeScale = 1;
        SavePCList();
        SaveMembers();        
   //     Invoke("AutoSave", 0.05f);
        Invoke("InstantiateCharacters", 0.1f);
    }

    private void SavePCList ()
    {
        string pcList = "Player";

        foreach (string st in inParty)
        {
            if (st != "Player")
            {
                pcList = pcList + "*" + st;
            }
            DialogueLua.SetActorField(st, "inParty", "No");
            
        }
        Debug.Log(pcList);
        DialogueLua.SetVariable("PCList", pcList);
        pos01 = gameController.player.transform.position;
        if (gameController.PC2 != null && gameController.PC2 != "DummyPlayer")
        {
            pos02 = gameController.PC2Obj.transform.position;
        }
        if (gameController.PC3 != null && gameController.PC3 != "DummyPlayer")
        {
            pos03 = gameController.PC3Obj.transform.position;
        }
        if (gameController.PC4 != null && gameController.PC4 != "DummyPlayer")
        {
            pos04 = gameController.PC4Obj.transform.position;
        }
        if (gameController.PC5 != null && gameController.PC5 != "DummyPlayer")
        {
            pos05 = gameController.PC5Obj.transform.position;
        }
    }

    private void SaveMembers ()
    {
        string memberList = null;
        if (members.Count > 0)
        {
            foreach (string st in members)
            {

                if (memberList == null)
                {
                    memberList = st;
                }
                else if (st != "nil" && st != "null")
                {
                    memberList = memberList + "*" + st;
                }
                DialogueLua.SetActorField(st, "inParty", "No");
            }
            Debug.Log(memberList);
            DialogueLua.SetVariable("listMembers", memberList);
        }
        else
        {
            DialogueLua.SetVariable("listMembers", "null");
        }

    }

    private void InstantiateCharacters()
    {
        Debug.Log("InstantiateCharacter");
        foreach (GameObject go in gameController.players)
        {
            go.name = go.name + "Obsolete";
            go.SetActive(false);
        }
        GetComponent<LoadGame>().pos01 = pos01;
        GetComponent<LoadGame>().pos02 = pos02;
        GetComponent<LoadGame>().pos03 = pos03;
        GetComponent<LoadGame>().pos04 = pos04;
        GetComponent<LoadGame>().pos05 = pos05;
        GetComponent<LoadGame>().LoadPCList();
        EndExit();
    }


    private void EndExit()
    {
        gameController.inDialogue = false;
        GetComponent<CameraMap>().CheckActiveQuests();
        gameController.playerConversation.GetComponent<PlayerConversation>().OnConversationEnd();
        GetComponent<GeneralWindow>().LoadPortraits();

    //    gameController.activePC.GetComponent<TargetActivePC>().enabled = false;
        Time.timeScale = 1;
        //      GetComponent<GeneralWindow>().LoadPortraits();
        this.enabled = false;
        /*
        foreach (GameObject go in gameController.players)
        {
            if (go.name != "Player")
            {
            go.GetComponent<TargetActivePC>().target.SetActive(false);
            }
            
        }*/

    }

    private void AddjustPositions ()
    {
        gameController.player.transform.position = pos01;
        if (gameController.PC2 != null && gameController.PC2 != "DummyPlayer")
        {
            gameController.PC2Obj.transform.position = pos02;
        }
        if (gameController.PC3 != null && gameController.PC3 != "DummyPlayer")
        {
            gameController.PC3Obj.transform.position = pos03;
        }
        if (gameController.PC4 != null && gameController.PC4 != "DummyPlayer")
        {
            gameController.PC3Obj.transform.position = pos03;
        }
        if (gameController.PC5 != null && gameController.PC5 != "DummyPlayer")
        {
            gameController.PC3Obj.transform.position = pos03;
        }
    }

    public void CheckAndRemove (string member)
    {
        bool inParty = false;
        if (gameController == null)
        {
            OnEnable();
        }
        foreach (GameObject go in gameController.players)
        {
            if (go.name == "member")
            {
                inParty = true;
            }

        }

        RemoveMember(member, inParty);
    }

    public void RemoveMember (string member, bool intantiateChanges)
    {        
        string tempInParty = null;
        string tempMembers = null;
        string listMembers = DialogueLua.GetVariable("listMembers").AsString;
        string listInParty = DialogueLua.GetVariable("PCList").AsString;
        string[] arrayMembers = listMembers.Split(new string[] { "*" }, System.StringSplitOptions.None);
        string[] arrayInParty = listInParty.Split(new string[] { "*" }, System.StringSplitOptions.None);
        Debug.Log(member + "/" + listMembers + listInParty);
        foreach (string st in arrayInParty)
        {            
            if (st != member)
            {
                if (tempInParty == null)
                {
                    tempInParty = st;
                }
                else
                {
                    tempInParty = tempInParty + "*" + st;
                }
            }
        }

        DialogueLua.SetVariable("PCList", tempInParty);
        
        if (arrayMembers.Length == 1 && arrayMembers[0] == "null")
        {

        }
        else
        {
            foreach (string st in arrayMembers)
            {
                Debug.Log(st);
                if (st != member)
                {
                    if (tempMembers == null)
                    {
                        tempMembers = st;
                    }
                    else
                    {
                        tempMembers = tempMembers + "*" + st;
                    }

                }
                else
                {
                    if (arrayMembers.Length == 1)
                    {
                        tempMembers = "null";
                    }
                }

                DialogueLua.SetVariable("listMembers", tempMembers);

            }

            Invoke("InvokeList", 0.1f);
        }       

        if (intantiateChanges == true)
        {
            DoneAndExit();
        }
        
    }

    private void InvokeList ()
    {
        Debug.Log(DialogueLua.GetVariable("listMembers").AsString);
        Debug.Log(DialogueLua.GetVariable("PCList").AsString);
    }

    public void InvokeActiveSelf ()
    {
        InvokeRepeating("ActiveSelf", 1, 0.5f);
    }

    private void ActiveSelf ()
    {
        if (GetComponent<DisplayToolBar>().enabled == true)
        {
            Debug.Log("Active");
            this.enabled = true;
            CancelInvoke("ActiveSelf");
        }
    }
}
