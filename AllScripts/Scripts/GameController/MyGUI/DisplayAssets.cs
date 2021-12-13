using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class DisplayAssets : MonoBehaviour {

    public List<string> units = new List<string>();
    public List<string> heroes = new List<string>();
    public List<string> buildings = new List<string>();
    public List<string> allies = new List<string>();
    public List<string> technology = new List<string>();
    private List<string> resources = new List<string>();
    private List<string> items = new List<string>();
    private bool displayInfo = false;
    private int buttonHeight;
    private int buttonWidth;
    private int xName;
    private int xValue;
    private int xInfo;
    private int infoButtonWidth;
    private int yUnits;
    private int yPaper;
    private int yUnitsLabel;
    private int yLabels;
    private int yHeroesLabel;
    private int yHeroes;
    private int yBuildingLabel;
    private int yBuilding;
    private int levelHeroes;
    private int valueHeroes;
    private int totalWarPoints = 0;
    private int totalLines = 0;
    private int yNameInfo;
    private int yArmourInfo;
    private int yPicInfo;
    private int ydescription;
    private int yWarAssetsValue;
    private int widthPic;
    private int descriptionWidth;
    private int descriptionHeight;
    private Texture2D picInfo;
    private string selectedAsset;
    private string typeSelected;
    private string heroesLevelString;
    private string warPointsString;
    private string description;
    private GUIStyle guiStyle;
    GUIContent content = new GUIContent();
    private Texture2D buttonTexture;

    //** RectSlide
    private int internalHeight;
    private Vector2 rectSlideGeneral;
    private Rect internalRectGeneral;
    private Rect rectGeneral;

    void Start ()
    {
	    
	}

    public void GetData ()
    {
        
        buttonHeight = (int)(Screen.height * 0.05f);
        buttonWidth = (int)(Screen.width * 0.40f);
        xName = (int)(Screen.width * 0.04f);
        xValue = (int)(Screen.width * 0.40f);
        xInfo = (int)(Screen.width * 0.65f);
        infoButtonWidth = (int)(Screen.width * 0.09f);

        widthPic = (int)(Screen.height * 0.35f);
        yPicInfo = (int)(Screen.height * 0.12f);
        yNameInfo = yPicInfo + widthPic + buttonHeight;
        yArmourInfo = yNameInfo + buttonHeight;
        ydescription = yArmourInfo + buttonHeight;
        descriptionWidth = (int) (Screen.width  * 0.92f);
        descriptionHeight = (int)(Screen.height * 0.80f);
        

        yPaper = (int)(Screen.height * 0.11f);
        yLabels = yPaper + buttonHeight;
        yUnitsLabel = yLabels + buttonHeight;
        yUnits = yUnitsLabel + buttonHeight;       
        

//        DialogueLua.SetVariable("units", "14000*14010*14010");
//        DialogueLua.SetVariable("buildings", "None");
//        DialogueLua.SetVariable("units", "None");
        DialogueLua.SetActorField("14000", "name", "Angus_and_Blackguards");
        DialogueLua.SetActorField("14000", "level", 3);
        DialogueLua.SetActorField("14000", "currentValue", 650);
        DialogueLua.SetActorField("14000", "size", 20);
        DialogueLua.SetActorField("14000", "maxSize", 20);
        DialogueLua.SetActorField("14000", "armour", 5);

        DialogueLua.SetActorField("14012", "name", "Mercenary_Orcs_Grumbark");
        DialogueLua.SetActorField("14012", "level", 3);
        DialogueLua.SetActorField("14012", "currentValue", 500);
        DialogueLua.SetActorField("14012", "size", 20);
        DialogueLua.SetActorField("14012", "maxSize", 20);
        DialogueLua.SetActorField("14012", "armour", 3);

        DialogueLua.SetActorField("14010", "name", "Archers Militia");
        DialogueLua.SetActorField("14010", "level", 1);
        DialogueLua.SetActorField("14010", "currentValue", 100);
        DialogueLua.SetActorField("14010", "size", 25);
        DialogueLua.SetActorField("14010", "maxSize", 25);

        DialogueLua.SetActorField("14202", "name", "Nemesian Priests");
        DialogueLua.SetActorField("14202", "level", 1);
        DialogueLua.SetActorField("14202", "currentValue", 150);
        DialogueLua.SetActorField("14202", "size", 20);
        DialogueLua.SetActorField("14202", "maxSize", 20);
        DialogueLua.SetActorField("14202", "armour", 0);

        DialogueLua.SetActorField("14203", "name", "Gremlims");
        DialogueLua.SetActorField("14203", "level", 1);
        DialogueLua.SetActorField("14203", "currentValue", 200);
        DialogueLua.SetActorField("14203", "size", 20);
        DialogueLua.SetActorField("14203", "maxSize", 20);
        DialogueLua.SetActorField("14203", "armour", 1);

        DialogueLua.SetActorField("None", "name", "There are not available assets yet");
        DialogueLua.SetActorField("None", "level", "");
        DialogueLua.SetActorField("None", "value", "");
        DialogueLua.SetActorField("None", "size", "");
        DialogueLua.SetActorField("None", "maxSize", "");
        DialogueLua.SetActorField("None", "armour", "");

        DialogueLua.SetActorField("16000", "name", "Heartbreak_Castle");
        DialogueLua.SetActorField("16000", "level", 10);
        DialogueLua.SetActorField("16000", "currentValue", 10000);
        DialogueLua.SetActorField("16000", "size", 10000);
        DialogueLua.SetActorField("16000", "maxSize", 10000);
        DialogueLua.SetActorField("16000", "type", "Stronghold");
        //      guiSkin = GetComponent<GeneralWindow>().mySkin;
        buttonTexture = (Texture2D)(Resources.Load("GUI/background2"));
        Invoke("PopulateLists", 0);
    }

    public void PopulateLists ()
    {
        totalWarPoints = 0;
        totalLines = 6;
        units.Clear();
        string unitsString =  DialogueLua.GetVariable("units").AsString;
    //    Debug.Log (unitsString);
        string [] arrayUnits = unitsString.Split(new string[] { "*" }, System.StringSplitOptions.None);
        foreach (string st in arrayUnits)
        {
     //       Debug.Log(st);
            units.Add(st);
            totalWarPoints = DialogueLua.GetActorField(st, "currentValue").AsInt + totalWarPoints;
            totalLines = totalLines++;
        }

        heroes.Clear();

        yHeroesLabel = ( (units.Count + 6) * buttonHeight);
        yHeroes = yHeroesLabel + buttonHeight;
    //    Debug.Log(units.Count + "/" + yHeroesLabel);
        string heroesString = DialogueLua.GetVariable("PCList").AsString;
        string[] arrayHeroes = heroesString.Split(new string[] { "*" }, System.StringSplitOptions.None);

        string listMembers = DialogueLua.GetVariable("listMembers").AsString;
    //    Debug.Log(listMembers);
        string[] arrayMembers = listMembers.Split(new string[] { "*" }, System.StringSplitOptions.None);

        levelHeroes = DialogueLua.GetActorField("Player", "level").AsInt;
        heroesLevelString = "HEROES (Level " + levelHeroes.ToString() + ")";
        valueHeroes = levelHeroes * 2;
        totalLines = totalLines + 4;
        foreach (string st in arrayHeroes)
        {
            if (st == "null")
            {

            }
            else if (st != "Player")
            {
        //        Debug.Log(st);
                heroes.Add(st);
                totalWarPoints = valueHeroes + totalWarPoints;
                totalLines = totalLines++;
            }
            else
            {
         //       Debug.Log(st);
                totalWarPoints = (valueHeroes * 2) + totalWarPoints;
                totalLines = totalLines++;
            }
        }

        foreach (string st in arrayMembers)
        {
            if (st == "null")
            {

            }
            else if (st != "Player")
            {
                //        Debug.Log(st);
                heroes.Add(st);
                totalWarPoints = valueHeroes + totalWarPoints;
                totalLines = totalLines++;
            }
        }

        buildings.Clear();
        yBuildingLabel = ((heroes.Count + units.Count + 8) * buttonHeight);
        yBuilding = yBuildingLabel + buttonHeight;
        string buildingString = DialogueLua.GetVariable("buildings").AsString;
        string[] arrayBuilding = buildingString.Split(new string[] { "*" }, System.StringSplitOptions.None);
        totalLines = totalLines + 4;
        foreach (string st in arrayBuilding)
        {
      //      Debug.Log(st);
            buildings.Add(st);
            totalWarPoints = DialogueLua.GetActorField(st, "currentValue").AsInt + totalWarPoints;
            totalLines = totalLines++;
        }

        warPointsString = "TOTAL : " + totalWarPoints.ToString();

        rectGeneral = new Rect(0, Screen.height * 0.11f, (Screen.width - ((int)(Screen.width * 0.02f))), Screen.height * 0.70f);
        internalHeight = yBuilding + ((buildings.Count + 8) * buttonHeight);
        internalRectGeneral = new Rect(0, Screen.height * 0.1f, Screen.width * 0.45f, internalHeight);
        yWarAssetsValue = internalHeight - buttonHeight;

    }

    public void DisplayData ()
    {

        //    GUI.DrawTexture(new Rect(0, yPaper, Screen.width, Screen.height), buttonTexture);
        rectSlideGeneral = GUI.BeginScrollView(rectGeneral, rectSlideGeneral, internalRectGeneral);

        if (displayInfo == false)
        {
            GUI.Label(new Rect(xValue, yLabels, buttonWidth, buttonHeight), "WAR POINTS " + warPointsString);
            GUI.Label(new Rect(xName, yLabels, buttonWidth, buttonHeight), "ASSETS");
            DisplayUnits();
            DisplayHeroes();
            DisplayBuildings();
            DisplayAllies();

            GUI.Label(new Rect(xValue, (internalHeight +  buttonHeight), buttonWidth, buttonHeight), "WAR POINTS " + warPointsString);
        }
        else
        {
            DisplayInfo();

        }

        GUI.EndScrollView();
    }

    void DisplayUnits ()
    {
    //    Debug.Log(yUnits + Screen.height);
        GUI.Label(new Rect(xName, yUnitsLabel, buttonWidth, buttonHeight), "UNITS");
        for (int cnt = 0; cnt < units.Count; cnt++)
        {
            
            string textBasic = DialogueLua.GetActorField(units[cnt], "name").AsString + " (level " + DialogueLua.GetActorField(units[cnt], "level").AsString + ")";
            string textMore = " (" + DialogueLua.GetActorField(units[cnt], "size").AsString + "/" + DialogueLua.GetActorField(units[cnt], "maxSize").AsString + ")";
            string textToDisplay = textBasic + textMore;
            GUI.Label(new Rect(xName, yUnits + (cnt * buttonHeight), buttonWidth, buttonHeight), textToDisplay);
            GUI.Label(new Rect(xValue, yUnits + (cnt * buttonHeight), buttonWidth, buttonHeight), DialogueLua.GetActorField(units[cnt], "currentValue").AsString);
            if (GUI.Button(new Rect(xInfo, yUnits + (cnt * buttonHeight), infoButtonWidth, buttonHeight), "INFO"))
            {
                if (units[cnt] != "None")
                {
                    selectedAsset = units[cnt];
                    picInfo = (Texture2D)(Resources.Load("Textures/Units/" + units[cnt]));
                    TextAsset descriptionAsset = (TextAsset)(Resources.Load("Text/Units/" + units[cnt]));
                    if (descriptionAsset != null)
                    {
                        description = descriptionAsset.text;
                    }
                    else
                    {
                        description = "";
                    }
                    Debug.Log(description);
                    typeSelected = "Unit";
                    displayInfo = true;
                }
            }
        }
    }

    void DisplayHeroes ()
    {
        GUI.Label(new Rect(xName, yHeroesLabel, buttonWidth, buttonHeight), heroesLevelString);
        GUI.Label(new Rect(xName, yHeroes, buttonWidth, buttonHeight), "Herald");
        GUI.Label(new Rect(xValue, yHeroes, buttonWidth, buttonHeight), (2 * valueHeroes).ToString());
        for (int cnt = 0; cnt < heroes.Count; cnt++)
        {
            string textToDisplay = heroes[cnt];
            GUI.Label(new Rect(xName, yHeroes + ((cnt + 1) * buttonHeight), buttonWidth, buttonHeight), textToDisplay);
            GUI.Label(new Rect(xValue, yHeroes + ((cnt + 1) * buttonHeight), buttonWidth, buttonHeight), valueHeroes.ToString());
        }
    //    Debug.Log(yHeroes);
    }

    void DisplayBuildings ()
    {
        GUI.Label(new Rect(xName, yBuildingLabel, buttonWidth, buttonHeight), "BUILDING");
        for (int cnt = 0; cnt < buildings.Count; cnt++)
        {

            string textBasic = DialogueLua.GetActorField(buildings[cnt], "name").AsString + " (level " + DialogueLua.GetActorField(buildings[cnt], "level").AsString + ")";
            string textMore = " (" + DialogueLua.GetActorField(buildings[cnt], "size").AsString + "/" + DialogueLua.GetActorField(buildings[cnt], "maxSize").AsString + ")";
            string textToDisplay = textBasic + textMore;
            GUI.Label(new Rect(xName, yBuilding + (cnt * buttonHeight), buttonWidth, buttonHeight), textToDisplay);
            GUI.Label(new Rect(xValue, yBuilding + (cnt * buttonHeight), buttonWidth, buttonHeight), DialogueLua.GetActorField(buildings[cnt], "currentValue").AsString);
            if (GUI.Button(new Rect(xInfo, yBuilding + (cnt * buttonHeight), infoButtonWidth, buttonHeight), "INFO"))
            {
                if (units[cnt] != "None")
                {
                    selectedAsset = buildings[cnt];
                    typeSelected = "Building";
                    displayInfo = true;
                }
            }
        }
    }

    void DisplayAllies ()
    {

    }

    private void DisplayInfo ()
    {
        if (typeSelected == "Unit")
        {
            GUI.DrawTexture(new Rect(xName, yPicInfo, widthPic, widthPic), picInfo);
            GUI.Label(new Rect(xName, yNameInfo, buttonWidth, buttonHeight), DialogueLua.GetActorField(selectedAsset, "name").AsString);
            GUI.Label(new Rect(xName, yArmourInfo, buttonWidth, buttonHeight), "Armour: " + DialogueLua.GetActorField(selectedAsset, "armour").AsString);
            GUI.Label(new Rect(xName, ydescription, descriptionWidth, descriptionHeight), description);



            if (GUI.Button(new Rect(xInfo, yUnits + buttonHeight, infoButtonWidth, buttonHeight), "EXIT"))
            {
                displayInfo = false;
            }
        }
        else if (typeSelected == "Building")
        {
            GUI.Label(new Rect(xName, yUnits + buttonWidth, buttonWidth, buttonHeight), DialogueLua.GetActorField(selectedAsset, "name").AsString);
            if (GUI.Button(new Rect(xInfo, yUnits + buttonHeight, infoButtonWidth, buttonHeight), "EXIT"))
            {
                displayInfo = false;
            }
        }
    }

    public void AddUnit (string unitToAdd)
    {
        string currentUnits = DialogueLua.GetVariable("units").AsString;
        Debug.Log(unitToAdd + "/" + currentUnits);
        if (currentUnits == "nill" || currentUnits == "None" || currentUnits == "" || currentUnits == "Nill")
        {
      //      Debug.Log(unitToAdd);
            DialogueLua.SetVariable("units", unitToAdd);
        }
        else
        {
      //      Debug.Log(unitToAdd);
            currentUnits = DialogueLua.GetVariable("units").AsString;
            string setUnits = currentUnits + "*" + unitToAdd;
            DialogueLua.SetVariable("units", setUnits);
        }
        Invoke("PopulateLists", 0.5f);
    //    Debug.Log(currentUnits);
    }

    public void AddBuilding (string buildingToAdd)
    {
        Debug.Log(buildingToAdd);
        string currentBuildings = DialogueLua.GetVariable("buildings").AsString;
        if (currentBuildings == "nill" || currentBuildings == "None" || currentBuildings == "" || currentBuildings == "Nill")
        {
            DialogueLua.SetVariable("buildings", buildingToAdd);
        }
        else
        {
            string setBuildings = currentBuildings + "*" + buildingToAdd;
            DialogueLua.SetVariable("buildings", setBuildings);
        }
    }

    public int CheckArmySize()
    {
        PopulateLists();
        int armySize = units.Count;
        return armySize;
    }
}
