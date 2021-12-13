using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DisplayAI : MonoBehaviour 
{
	private int startXPos;
	private int startYPos;
	private int buttonWidth;
	private int buttonHeight;
	private int button1;
	private int button2;
	private string behaviour = "Agressive";
	private string [] display;
	private string passive;
	private string defensive;
	private string aggressive;
	private string ranged;
	private string [] AIOptions = new string[]
	{
		"Passive",
		"Defensive",
		"Aggressive",
		"Ranged"
	};
    private GeneralWindow generalWindow;


	public void OnEnable () 
	{
		startXPos = (int) (Screen.width * 0.02f);
		startYPos = (int) (Screen.height * 0.22f);
		buttonWidth = 100;
		buttonHeight = 30;
		button1 = startYPos + buttonHeight + 10;
		button2 = button1 + (5 * buttonHeight);
        generalWindow = GetComponent<GeneralWindow>();

		passive = "Character will not attack even in the event" +
			" he/she received damage, unless an order to attack is" +
				" issued. ";

		defensive = "Character will attack only if he/she received" +
			" damage, otherwise will not move or initiate any attack";

		aggressive = "Character will initiate combat if there is an enemy" +
			" within sight. If enemy is defeated, character will look for" +
			" the nearest enemy and will attack from maximum range of the " +
			" weapon in use";

		ranged = "Character will initiate combat if there is an enemy " +
			" within sight. If an enemy approched closer, character would " +
			" flee to a safe distant and then would resume attacking. Note that a" +
			" character with this behaviour and melee weapon will never seek out " +
			" to use his weapon, however he/she might still use any automatic" +
			" ranged skill";
	}

	public void DisplayAIOptions ()
	{
		DisplayAIState();
	}

	void DisplayAIState ()
	{
		GUI.Label (new Rect (startXPos, startYPos, buttonWidth, buttonHeight), "AI BEHAVIOUR");

        if (GUI.Button(new Rect(startXPos, button1, buttonWidth, buttonHeight), new GUIContent("Passive", passive)))
        {
            DialogueLua.SetActorField(generalWindow.activePCObj.name, "behaviour", "passive");
        }
        GUI.Label(new Rect(200, 200, 300, 300), GUI.tooltip);

        if (GUI.Button(new Rect(startXPos, button1 + buttonHeight, buttonWidth, buttonHeight), new GUIContent("Defensive", defensive)))
        {
            DialogueLua.SetActorField(generalWindow.activePCObj.name, "behaviour", "defensive");
        }
        GUI.Label(new Rect(200, 200, 300, 300), GUI.tooltip);

        if (GUI.Button(new Rect(startXPos, button1 + (buttonHeight * 2), buttonWidth, buttonHeight), new GUIContent("Agressive", aggressive)))
        {
            DialogueLua.SetActorField(generalWindow.activePCObj.name, "behaviour", "aggressive");
        }
        GUI.Label(new Rect(200, 200, 300, 300), GUI.tooltip);

        if (GUI.Button(new Rect(startXPos, button1 + (buttonHeight *3), buttonWidth, buttonHeight), new GUIContent("Ranged", ranged)))
        {
            DialogueLua.SetActorField(generalWindow.activePCObj.name, "behaviour", "ranged");
        }
        GUI.Label(new Rect(200, 200, 300, 300), GUI.tooltip);
        Debug.Log ( generalWindow.activePCObj.name);
        GUI.Label (new Rect (startXPos, button2, 400, buttonHeight), "Current Behaviour: " + DialogueLua.GetActorField(generalWindow.activePCObj.name, "behaviour").AsString);
	}
}
