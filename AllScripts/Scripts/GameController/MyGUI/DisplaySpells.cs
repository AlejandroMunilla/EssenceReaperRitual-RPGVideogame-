using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DisplaySpells : MonoBehaviour {

    public GUISkin skin;
    private int numberButtons = 16;
    private int buttonWide;
    private int posX;
    private int posY;
    private int buttonHeight;
    private GameObject gc = null;
    private GameController gameController;
    private string spells = "17000";


    void OnEnable ()
    {
        if (gc == null)
        {
            buttonWide = (int)(Screen.width / 16);
            posX = 0;
            posY = (int)(Screen.height * 0.91f);
            buttonHeight = (int)(Screen.height * 0.10f);
            gc = GameObject.FindGameObjectWithTag("GameController");
            gameController = gc.GetComponent<GameController>();
            skin = (GUISkin)(Resources.Load("GUI/Skin", typeof(GUISkin)));
            
        }    
    }
	
	// Update is called once per frame
	void OnGUI ()
    {
        GUI.skin = skin;

        GameObject activePC = gameController.activePC;
        if (activePC.GetComponent<PlayerAbilities>().spells != null)
        {
            for (int cnt = 0; cnt < activePC.GetComponent<PlayerAbilities>().spells.Count; cnt++)
            {
                string nameSpell = DialogueLua.GetActorField(activePC.GetComponent<PlayerAbilities>().spells[cnt], "name").AsString;
                if (GUI.Button(new Rect(posX + (buttonWide * cnt), posY, buttonWide, buttonHeight), new GUIContent((Texture)(Resources.Load("Icons/Spells/" + activePC.GetComponent<PlayerAbilities>().spells[cnt])), nameSpell)))
                {
                    if (activePC.GetComponent<PlayerStats>().castingSpell == false)
                    {
                        string spellToCast = activePC.GetComponent<PlayerAbilities>().spells[cnt];
                        if (activePC.transform.Find("Spells/" + spellToCast))
                        {
                       //     Debug.Log("no instantiated" + spellToCast + "/" + activePC.name);
                            if (activePC.transform.Find("Spells/" + spellToCast).gameObject.activeSelf == false)
                            {
                                activePC.transform.Find("Spells/" + spellToCast).gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            GameObject newSpell = Instantiate(Resources.Load("Spell/" + spellToCast)) as GameObject;
                            newSpell.transform.position = activePC.transform.position;
                            Transform spellsTransform = activePC.transform.Find("Spells");
                            newSpell.transform.parent = spellsTransform;
                            newSpell.SetActive(true);
                        }

                        GetComponent<DisplayToolBar>().enabled = true;
                        GetComponent<DisplaySpells>().enabled = false;
                    }
                }
            }
            if (GUI.Button(new Rect(posX + (buttonWide * (activePC.GetComponent<PlayerAbilities>().spells.Count)), posY, buttonWide, buttonHeight), new GUIContent("X", "Close spells")))
            {
                this.enabled = false;
                GetComponent<DisplayToolBar>().enabled = true;
            }


        }
        else
        {
            Debug.Log("no spells");
            this.enabled = false;
            GetComponent<DisplayToolBar>().enabled = true;
            
        }
        


   //     GUI.Button(new Rect(posX + (16 * buttonWide), posY, buttonWide, buttonHeight), "");

        GUI.Label(new Rect(posX, posY - buttonHeight, 500, buttonHeight), GUI.tooltip);
    }


}
