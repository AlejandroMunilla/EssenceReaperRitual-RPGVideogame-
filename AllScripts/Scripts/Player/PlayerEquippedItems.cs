/// This class handles instantiante/Destroy of Weapons of equipped items. 
/// Data base of items is handled by Lua (Dialogue Manager), stored in variables
/// of each character; each character is an actor on Lua with one variable named as
/// (RHand, LHand, Head, etc). If RHand variable = to RHand, it means there is not 
/// item equipped. If there is a RHand = Sword, it means there is a sword and this 
/// class with instantiate the sword on the right bone of the character. 
/// AMG, March 04th, 2016. 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;


public class PlayerEquippedItems : MonoBehaviour
{
    private GameObject gamecontroller;
    private CombatController combatController;
    private string mainPlayerName;
    private string saveDirectory;
    private bool male = true;
    private bool alive = true;
    private bool loaded = false;
//    public string itemRHand;
    public string itemLHand;
    public string pathRHand = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 back_right_Wmount";
    public string pathRHandCombat = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 Rhand_Weapon";
    public string pathLHand;
    public string pathLHandCombat;
    public string headCamera;
    public Transform taHeadCamera;

    void Awake()
    {
        
        gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        if (gamecontroller.GetComponent<CombatController>() != null)
        {
            combatController = gamecontroller.GetComponent<CombatController>();
        }
        
    }

    void OnEnable ()
    {
        if (loaded == true)
        {
            return;
        }

        if (gamecontroller.GetComponent<GameController>() != null)
        {
            string directoryFile = gamecontroller.GetComponent<GameController>().saveDirectory;
            mainPlayerName = gamecontroller.GetComponent<GameController>().mainPlayerName;
        }
        else
        {
            mainPlayerName = DialogueLua.GetActorField("Player", "name").AsString;
        }

        Corrections();
        CreateArmourObject();
        Invoke ("SetUpEquipped", 0);
        loaded = true;
    }

    void Start ()
    {
        CreateArmourObject();
        if (loaded == true)
        {
            return;
        }
        else
        {
            string directoryFile = gamecontroller.GetComponent<GameController>().saveDirectory;
            mainPlayerName = gamecontroller.GetComponent<GameController>().mainPlayerName;
            SetUpEquipped();
        }
    }

    //This gets info from Lua inventory of the character and instantiate gameobjects of 
    //weapons, items, etc. It simply calls each individual location with a different void function.
    public void SetUpEquipped()
    {
        SetUpBreast();
        SetUpRightHand();
        SetUpLeftHand();
        SetUpHead();
        SetUpNeck();

        string _namePlayer = gameObject.name;
  //      Debug.Log(_namePlayer);
   //     string mainPlayerNameGC = gamecontroller.GetComponent<GameController>().mainPlayerName;
        string mainPlayerNameGC = DialogueLua.GetActorField("Player", "name").AsString;
        if (_namePlayer == mainPlayerNameGC)
        {
            _namePlayer = "Player";
        }



        if (DialogueLua.GetActorField(_namePlayer, "Head").AsString != "6000")
        {
            if (DialogueLua.GetActorField(_namePlayer, "Head").AsString == "Head")
            {
                DialogueLua.SetActorField(_namePlayer, "Head", "6000");
            }
        }        

        if (DialogueLua.GetActorField(_namePlayer, "Arms").AsString != "6007")
        {
            if (DialogueLua.GetActorField(_namePlayer, "Arms").AsString == "Arms")
            {
                DialogueLua.SetActorField(_namePlayer, "Arms", "6007");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "Waist").AsString != "6003")
        {
            if (DialogueLua.GetActorField(_namePlayer, "Waist").AsString == "Waist")
            {
                DialogueLua.SetActorField(_namePlayer, "Waist", "6003");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "Legs").AsString != "6006")
        {
            if (DialogueLua.GetActorField(_namePlayer, "Legs").AsString == "Legs")
            {
                DialogueLua.SetActorField(_namePlayer, "Legs", "6006");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "Neck").AsString != "6009")
        {
            if (DialogueLua.GetActorField(_namePlayer, "Neck").AsString == "Neck")
            {
                DialogueLua.SetActorField(_namePlayer, "Neck", "6009");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "FingerR").AsString != "6004")
        {
            if (DialogueLua.GetActorField(_namePlayer, "FingerR").AsString == "FingerR")
            {
                DialogueLua.SetActorField(_namePlayer, "FingerR", "6004");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "FingerL").AsString != "6005")
        {
            if (DialogueLua.GetActorField(_namePlayer, "FingerL").AsString == "FingerL")
            {
                DialogueLua.SetActorField(_namePlayer, "FingerL", "6005");
            }
        }

        

        if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString != "6008")
        {
            if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "LHand")
            {
                DialogueLua.SetActorField(_namePlayer, "LHand", "6008");
            }
        }

        if (DialogueLua.GetActorField(_namePlayer, "Arrows").AsString != "Arrows")
        {

        }

        if (DialogueLua.GetActorField(_namePlayer, "Bullets").AsString != "Bullets")
        {

        }

        if (DialogueLua.GetActorField(_namePlayer, "Quick1").AsString != "Quick1")
        {

        }

        if (DialogueLua.GetActorField(_namePlayer, "Quick2").AsString != "Quick2")
        {

        }

        if (DialogueLua.GetActorField(_namePlayer, "Quick3").AsString != "Quick3")
        {

        }

        if (DialogueLua.GetActorField(_namePlayer, "Quick4").AsString != "Quick4")
        {

        }
    }
    public void SetUpBreast()
    {
        string namePlayer = gameObject.name;
        string equippedID = DialogueLua.GetActorField(namePlayer, "Breast").AsString;
        if (equippedID == "Breast" || equippedID == "RHand")
        {
            DialogueLua.SetActorField(namePlayer, "Breast", "6001");
            equippedID = "6001";
        }
  //      Debug.Log(DialogueLua.GetActorField(equippedID, "name").AsString);

        if (equippedID != "6001")
        {
            if (namePlayer == "Player")
            {
        //        Debug.Log(transform.Find("Armour/" + equippedID));
                Transform armourTA = transform.Find("Armour");
                if (transform.Find ("Armour/" + equippedID) != null)
                {
                    transform.Find("Armour/" + equippedID).gameObject.SetActive(true);
                }
                else
                {
                    GameObject armourGO = Instantiate(Resources.Load("Armour/" + equippedID), transform.position, Quaternion.identity) as GameObject;
                    armourGO.transform.parent = armourTA;
                    armourGO.name = equippedID;
                    armourGO.SetActive(true);
                }

            }
        }
      //  InstantiateBreast();
    }
    public void SetUpRightHand()
    {
        
        string namePlayer = gameObject.name;
        string equippedWeapon = DialogueLua.GetActorField(namePlayer, "RHand").AsString;
    //    Debug.Log(equippedWeapon);
        if (equippedWeapon == "RHand" || equippedWeapon == "")
        {
            DialogueLua.SetActorField(namePlayer, "RHand", "6002");
            equippedWeapon = "6002";
        }
        if (gameObject.activeSelf)
        {
            StartCoroutine("InstantiateRHand");
        }
       
    }

    public void SetUpLeftHand()
    {
        //    Debug.Log("RHand");
        string namePlayer = gameObject.name;
        string equippedWeapon = DialogueLua.GetActorField(namePlayer, "LHand").AsString;
   //     Debug.Log(equippedWeapon + "/" + gameObject.name);
        if (equippedWeapon == "LHand" || equippedWeapon == "")
        {
            DialogueLua.SetActorField(namePlayer, "LHand", "6008");
            equippedWeapon = "6008";
        }
        if (gameObject.activeSelf)
        {
            StartCoroutine("InstantiateLHand");
        }
        
    }


    public void SetUpHead()
    {

    }

    public void SetUpNeck()
    {

        string namePlayer = gameObject.name;
        string equippedWeapon = DialogueLua.GetActorField(namePlayer, "Neck").AsString;
        if (equippedWeapon == "Neck" || equippedWeapon == "")
        {
            DialogueLua.SetActorField(namePlayer, "Neck", "6009");
            equippedWeapon = "6009";
        }
        if (gameObject.activeSelf)
        {
     //       Debug.Log(equippedWeapon);
            StartCoroutine("InstantiateNeck");
        }
    }


    public void SetUpFingerR ()
    {

        string namePlayer = gameObject.name;
        string equippedWeapon = DialogueLua.GetActorField(namePlayer, "FingerR").AsString;
        if (equippedWeapon == "FingerR" || equippedWeapon == "")
        {
            DialogueLua.SetActorField(namePlayer, "FingerR", "6004");
            equippedWeapon = "6004";
        }
        if (gameObject.activeSelf)
        {
            //       Debug.Log(equippedWeapon);
            StartCoroutine("InstantiateFingerR");
        }
    }

    public void SetUpFingerL ()
    {
        string namePlayer = gameObject.name;
        string equippedWeapon = DialogueLua.GetActorField(namePlayer, "FingerL").AsString;
        if (equippedWeapon == "FingerL" || equippedWeapon == "")
        {
            DialogueLua.SetActorField(namePlayer, "FingerL", "6005");
            equippedWeapon = "6005";
        }
        if (gameObject.activeSelf)
        {
            //       Debug.Log(equippedWeapon);
            StartCoroutine("InstantiateFingerL");
        }
    }

    IEnumerator InstantiateRHand()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (gameObject.name == "Weirum")
            {
                StopCoroutine("InstantiateRHand");
                yield break;
            }
            string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;
            string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
            bool weaponInstantiateRight = false;
            bool weaponRealNoActive = true;
            bool weaponOtherNoActive = true;
            string pathReal;
            string pathOther;

            if (combatController != null)
            {
                if (combatController.inCombat == false)
                {
                    pathReal = pathRHand + "/WeaponRight";
                    pathOther = pathRHandCombat + "/WeaponRight";
                }
                else
                {
                    pathReal = pathRHandCombat + "/WeaponRight";
                    pathOther = pathRHand + "/WeaponRight";
                }
            }
            else
            {
                pathReal = pathRHandCombat + "/WeaponRight";
                pathOther = pathRHand + "/WeaponRight";
            }


            if (equippedWeapon == "8000")
            {
                string dalila = DialogueLua.GetActorField("Player", "dalila").AsString;
                if (dalila == "" || dalila == null)
                {
                    dalila = "Sceptre";
                }
                equippedWeapon = "Dalila" + dalila;
      //          Debug.Log("Dalila");
            }


            Transform otherTransform = transform.Find(pathOther);
           
            if (gameObject.name != "Lycaon")
            {
                //        Debug.Log(otherTransform);

        //        Debug.Log(gameObject.name + "/" + pathOther);
                if (otherTransform != null)
                {
                    foreach (Transform t in otherTransform)
                    {

                        if (t.gameObject.activeSelf)
                        {
                            t.gameObject.SetActive(false);
                            weaponOtherNoActive = false;
                        }
                    }
                }
                else
                {
                    weaponOtherNoActive = false;
                }

            }


            if (weaponOtherNoActive == true)
            {
                Transform realTransform = transform.Find(pathReal);
               
                if (gameObject.name != "Lycaon")
                {
                    foreach (Transform t in realTransform)
                    {
                        if (t.gameObject.name != equippedWeapon)
                        {
                            if (t.gameObject.activeSelf)
                            {
                                t.gameObject.SetActive(false);
                                weaponRealNoActive = false;
                            }
                        }
                    }
                }
            }

            if (weaponInstantiateRight == false)
            {
                if (transform.Find(pathReal + "/" + equippedWeapon))
                {
   //                 Debug.Log("false");
                    GameObject go = transform.Find(pathReal + "/" + equippedWeapon).gameObject;
                    go.SetActive(true);
                    if (go.activeSelf)
                    {
                        weaponInstantiateRight = true;
                    }
                }
                else if (weaponName == "RHand" || equippedWeapon == "6002")
                {
                    weaponInstantiateRight = true;
                }
            }

            if (weaponInstantiateRight == true && weaponRealNoActive == true && weaponOtherNoActive == true)
            {
                StopCoroutine("InstantiateRHand");
            }
     //       Debug.Log(weaponInstantiateRight + "/" + weaponRealNoActive + "/" + weaponOtherNoActive);
     //       Debug.Log(equippedWeapon);
            yield return new WaitForSeconds(0.05f);
        }


    }

    IEnumerator InstantiateLHand ()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (gameObject.name == "Weirum")
            {
                StopCoroutine("InstantiateLHand");
                yield break;
            }
            string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "LHand").AsString;
            string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
   //         Debug.Log(equippedWeapon);
   //         Debug.Log(weaponName);
            
            bool weaponInstantiateRight = false;
            bool weaponRealNoActive = true;
            bool weaponOtherNoActive = true;
            string pathReal;
            string pathOther;

            if (equippedWeapon == "4006")
            {
                pathReal = pathLHandCombat + "/WeaponRight";
                pathOther = pathLHandCombat + "/WeaponRight";
            }
            else if (combatController != null)
            {
                if (combatController.inCombat == false)
                {
                    pathReal = pathLHand + "/WeaponRight";
                    pathOther = pathLHandCombat + "/WeaponRight";
                }
                else
                {
                    pathReal = pathLHandCombat + "/WeaponRight";
                    pathOther = pathLHand + "/WeaponRight";
                }
            }
            else
            {
                pathReal = pathLHandCombat + "/WeaponRight";
                pathOther = pathLHand + "/WeaponRight";
            }

            if (equippedWeapon == "8000")
            {
                string dalila = DialogueLua.GetActorField("Player", "dalila").AsString;
                if (dalila == "" || dalila == null)
                {
                    dalila = "Sceptre";
                }
                equippedWeapon = "Dalila" + dalila;
                Debug.Log(equippedWeapon);
            }


            Transform otherTransform = transform.Find(pathOther);

            if (gameObject.name != "Lycaon")
            {
                //         Debug.Log( gameObject.name + "/" + pathOther);
                if (otherTransform != null)
                {
                    foreach (Transform t in otherTransform)
                    {
                        if (t.gameObject.activeSelf)
                        {
                            t.gameObject.SetActive(false);
                            weaponOtherNoActive = false;
                        }
                    }
                }
                else
                {
                    weaponOtherNoActive = false;
                }



            }



            if (weaponOtherNoActive == true)
            {
                if (gameObject.name != "Lycaon")
                {
                    Transform realTransform = transform.Find(pathReal);
                    //           Debug.Log(pathReal);
                    foreach (Transform t in realTransform)
                    {
                        if (t.gameObject.name != equippedWeapon)
                        {
                            if (t.gameObject.activeSelf)
                            {
                                t.gameObject.SetActive(false);
                                weaponRealNoActive = false;
                            }
                        }
                    }
                }


            }

            if (weaponInstantiateRight == false)
            {
                if (transform.Find(pathReal + "/" + equippedWeapon))
                {
                    //                 Debug.Log("false");
                    GameObject go = transform.Find(pathReal + "/" + equippedWeapon).gameObject;
                    go.SetActive(true);
                    if (go.activeSelf)
                    {
                        weaponInstantiateRight = true;
                    }
                }
                else if (weaponName == "LHand" || equippedWeapon == "6008")
                {
                    weaponInstantiateRight = true;
                }
            }

            if (weaponInstantiateRight == true && weaponRealNoActive == true && weaponOtherNoActive == true)
            {
                StopCoroutine("InstantiateLHand");
            }
            //       Debug.Log(weaponInstantiateRight + "/" + weaponRealNoActive + "/" + weaponOtherNoActive);
            //       Debug.Log(equippedWeapon);
            yield return new WaitForSeconds(0.05f);
        }


    }

    IEnumerator InstantiateNeck ()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "Neck").AsString;
            string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
            string path = "Neck/" + equippedWeapon;
            bool inactiveUnused = true;
            bool instantiatedItem = true;
            GameObject neck;
     //       Debug.Log(equippedWeapon);

            if (transform.Find ("Neck") == null)
            {
            //    Debug.Log(gameObject.name);
                neck = Instantiate(Resources.Load("Weapons/Empty"), transform.position, transform.rotation) as GameObject;
                neck.transform.parent = gameObject.transform;
                neck.name = "Neck";
                GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                weaponNeck.transform.parent = neck.transform;
                weaponNeck.name = equippedWeapon;
                instantiatedItem = false;
            }
            else
            {
                neck = transform.Find("Neck").gameObject;
                foreach (Transform ta in transform.Find ("Neck"))
                {
                    if (ta.name != equippedWeapon)
                    {
                        if (ta.gameObject.activeSelf)
                        {
                            inactiveUnused = false;
                            ta.gameObject.SetActive(false);
                            Debug.Log(ta.name);
                        }

                    }
                }

                if (transform.Find(path) == null)
                {
                    GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                    weaponNeck.transform.parent = neck.transform;
                    weaponNeck.name = equippedWeapon;
                    Debug.Log(path);
                    instantiatedItem = false;
                }
                else
                {
                    transform.Find(path).gameObject.SetActive(true);
                }
                    
            }
      //      Debug.Log(instantiatedItem + "/" + inactiveUnused);
            if (instantiatedItem == true && inactiveUnused == true)
            {
                StopCoroutine("InstantiateNeck");
            }


            yield return new WaitForSeconds(0.05f);
        }


    }


    private void InstantiateBreast ()
    {        
        string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "Breast").AsString;
        string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;        
        bool itemInstantiated = false;
        bool oldItemdestroyed = true;
        GameObject breast = null;
        //Instantiate weapon
        #region
        bool breastCreated = false;
        foreach (Transform t in transform)
        {
            if (t.name == "Breast")
            {
                breast = t.gameObject;
                breastCreated = true;
            }
        }


        if (breastCreated == true)
        {           
            if (weaponName == "Breast")
            {
                itemInstantiated = true;
            }
            else
            {                 
                if (transform.Find ("Breast/" + weaponName))
                {
                    itemInstantiated = true;
                }
                else
                {
                    GameObject goToName = (Instantiate(Resources.Load("Armour/" + weaponName), transform.position, transform.rotation) as GameObject);
                    goToName.transform.parent = transform.Find("Breast");
                    goToName.name = weaponName;
                }                
            }

            foreach (Transform t in transform.Find("Breast"))
            {
                if (t.name != weaponName)
                {
                    oldItemdestroyed = false;
                    Destroy(t.gameObject);
                }
            }
        }
        else
        {
     //       Debug.Log(equippedWeapon + "/" + weaponName);
            GameObject goToName = (Instantiate(Resources.Load("Armour/Breast"), transform.position, transform.rotation) as GameObject);
            goToName.transform.parent = transform;
            goToName.name = "Breast";
            oldItemdestroyed = false;
        }
        #endregion
 //       Debug.Log(equippedWeapon + "/" + weaponName);
        if (itemInstantiated == true && oldItemdestroyed == true)
        {
             StopCoroutine("InstantiateBreast");
        }
        else
        {
            Invoke("SetUpBreast", 0.05f);
        }
    }

    IEnumerator InstantiateFingerR()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "FingerR").AsString;
            string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
            string path = "FingerR/" + equippedWeapon;
            bool inactiveUnused = true;
            bool instantiatedItem = true;
            GameObject fingerR;
            //       Debug.Log(equippedWeapon);

            if (transform.Find("FingerR") == null)
            {
                fingerR = Instantiate(Resources.Load("Weapons/Empty"), transform.position, transform.rotation) as GameObject;
                fingerR.transform.parent = gameObject.transform;
                fingerR.name = "FingerR";
                GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                weaponNeck.transform.parent = fingerR.transform;
                weaponNeck.name = equippedWeapon;
                instantiatedItem = false;
            }
            else
            {
                fingerR = transform.Find("FingerR").gameObject;
                foreach (Transform ta in transform.Find("FingerR"))
                {
                    if (ta.name != equippedWeapon)
                    {
                        if (ta.gameObject.activeSelf)
                        {
                            inactiveUnused = false;
                            ta.gameObject.SetActive(false);
                            Debug.Log(ta.name);
                        }

                    }
                }

                if (transform.Find(path) == null)
                {
                    GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                    weaponNeck.transform.parent = fingerR.transform;
                    weaponNeck.name = equippedWeapon;
                    Debug.Log(path);
                    instantiatedItem = false;
                }
                else
                {
                    transform.Find(path).gameObject.SetActive(true);
                }

            }
            //      Debug.Log(instantiatedItem + "/" + inactiveUnused);
            if (instantiatedItem == true && inactiveUnused == true)
            {
                StopCoroutine("InstantiateFingerR");
            }


            yield return new WaitForSeconds(0.05f);
        }


    }

    IEnumerator InstantiateFingerL()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "FingerL").AsString;
            string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
            string path = "FingerL/" + equippedWeapon;
            bool inactiveUnused = true;
            bool instantiatedItem = true;
            GameObject fingerR;
            //       Debug.Log(equippedWeapon);

            if (transform.Find("FingerL") == null)
            {
                fingerR = Instantiate(Resources.Load("Weapons/Empty"), transform.position, transform.rotation) as GameObject;
                fingerR.transform.parent = gameObject.transform;
                fingerR.name = "FingerL";
                GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                weaponNeck.transform.parent = fingerR.transform;
                weaponNeck.name = equippedWeapon;
                instantiatedItem = false;
            }
            else
            {
                fingerR = transform.Find("FingerL").gameObject;
                foreach (Transform ta in transform.Find("FingerL"))
                {
                    if (ta.name != equippedWeapon)
                    {
                        if (ta.gameObject.activeSelf)
                        {
                            inactiveUnused = false;
                            ta.gameObject.SetActive(false);
                            Debug.Log(ta.name);
                        }

                    }
                }

                if (transform.Find(path) == null)
                {
                    GameObject weaponNeck = Instantiate(Resources.Load("Weapons/" + equippedWeapon), transform.position, transform.rotation) as GameObject;
                    weaponNeck.transform.parent = fingerR.transform;
                    weaponNeck.name = equippedWeapon;
                    Debug.Log(path);
                    instantiatedItem = false;
                }
                else
                {
                    transform.Find(path).gameObject.SetActive(true);
                }

            }
            //      Debug.Log(instantiatedItem + "/" + inactiveUnused);
            if (instantiatedItem == true && inactiveUnused == true)
            {
                StopCoroutine("InstantiateFingerL");
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator InstantiateHead ()
    {
        string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "Breast").AsString;
        string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
        bool itemInstantiated = false;
        bool oldItemdestroyed = true;
        Debug.Log(weaponName);
        //Instantiate weapon
        #region
        if (transform.Find("Head"))
        {
            if (weaponName == "Head")
            {
                itemInstantiated = true;
            }
            else
            {
                if (transform.Find("Head/" + weaponName))
                {
                    itemInstantiated = true;
                }
                else
                {
                    GameObject goToName = (Instantiate(Resources.Load("Armour/" + weaponName), transform.position, transform.rotation) as GameObject);
                    goToName.transform.parent = transform.Find("Head");
                    goToName.name = weaponName;
                }
            }

            foreach (Transform t in transform.Find("Head"))
            {
                Debug.Log(t.name + "/" + weaponName);
                if (t.name != weaponName)
                {

                    Destroy(t.gameObject);
                    oldItemdestroyed = false;
                }
            }
        }
        else
        {
            GameObject goToName = (Instantiate(Resources.Load("Armour/Breast"), transform.position, transform.rotation) as GameObject);
            goToName.transform.parent = transform;
            goToName.name = weaponName;
        }
        #endregion
        //Destroy weapon
        if (itemInstantiated == true && oldItemdestroyed == true)
        {
            StopCoroutine("InstantiateHead");
        }
        yield return new WaitForSeconds(0.05f);
    }

    private void Corrections ()
    {
        if (DialogueLua.GetActorField(gameObject.name, "Head").AsString != "6000")
        {
            DialogueLua.SetActorField(gameObject.name, "Head", "6000");
        }
    }

    private void CreateArmourObject ()
    {
        if (transform.Find ("Armour") == null)
        {
            GameObject armourGO = Instantiate(Resources.Load("Armour/Armour"), transform.position, Quaternion.identity) as GameObject;
            armourGO.transform.parent = transform;
            armourGO.name = "Armour";
        }
    }
}



/*
  public static class CoroutineUtilities 
 {
     public static IEnumerator WaitForRealTime(float delay){
         while(true){
             float pauseEndTime = Time.realtimeSinceStartup + delay;
             while (Time.realtimeSinceStartup < pauseEndTime){
                 yield return 0;
             }
             break;
         }
     }
 }
 */



        //     string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;
        //     string weaponName = DialogueLua.GetActorField(equippedWeapon, "name").AsString;
        /*
        if (equippedWeapon == "Dalila")
        {
            string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
            if (dalilaItem == null || dalilaItem == "null")
            {
                dalilaItem = "Sceptre";
            }
            equippedWeapon = "Dalila" + dalilaItem;
        }
        bool test = true;
        while (test)
        {
            switch (stateRHand)
            {
                case StateRHand.Create:

                    yield return new WaitForSeconds(0.1f);
                    CreateRHand ();
                    break;
/*
                case StateRHand.CreateInBattle:
                    Create(pathItemCombat, equippedWeapon);
                    yield return new WaitForSeconds(0.1f);
                    break;
            }
        }
        //     StopCoroutine("InstantiateWeapon");
        yield return null;*/



/*
string _namePlayer = gameObject.name;
string mainPlayerNameGC = gamecontroller.GetComponent<GameController>().mainPlayerName;
//If Right hand is equipped with weapon
if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString != "6002")
{
    if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "RHand")
    {
        DialogueLua.SetActorField(_namePlayer, "RHand", "6002");
        return;
    }
    Debug.Log(DialogueLua.GetActorField(_namePlayer, "RHand").AsString);
    string equippedWeapon = DialogueLua.GetActorField(_namePlayer, "RHand").AsString;

    if (equippedWeapon == "Dalila")
    {
        gamecontroller.GetComponent<DisplayItemScript>().dalilaEquipped = true;
        string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
        if (dalilaItem == null || dalilaItem == "null")
        {
            dalilaItem = "Sceptre";
        }

        equippedWeapon = "Dalila" + dalilaItem;
    }
    if (equippedWeapon == "Torch")
    {
        Transform ta = gameObject.transform.Find(pathRHandCombat);
        GameObject tago = ta.gameObject;
        GameObject goToName = (Instantiate(Resources.Load("Weapons/" + DialogueLua.GetActorField(_namePlayer, "RHand").AsString), ta.position, ta.rotation) as GameObject);
        goToName.transform.parent = tago.transform;
        goToName.name = DialogueLua.GetActorField(_namePlayer, "RHand").AsString;

        //But if there is a weapon on the back, destroy it
        if (gameObject.transform.Find(pathRHand + "/" + equippedWeapon))
        {
            Destroy(gameObject.transform.Find(pathRHand + "/" + equippedWeapon).gameObject);
        }
    }
    //NOT IN COMBAT
    #region
    else if (gamecontroller.GetComponent<CombatController>().inCombat == false)
    {
        Transform ta = gameObject.transform.Find(pathRHand);
        GameObject tago = ta.gameObject;
        //check if object exist
        if (gameObject.transform.Find(pathRHand + "/" + equippedWeapon) == null)
        {
            GameObject goToName = (Instantiate(Resources.Load("Weapons/" + equippedWeapon), ta.position, ta.rotation) as GameObject);
            goToName.transform.parent = tago.transform;
            goToName.name = equippedWeapon;
            if (DialogueLua.GetActorField(equippedWeapon, "rotate").AsString == "Yes")
            {
                Transform ta2 = gameObject.transform.Find(pathRHand + "/" + equippedWeapon + "/" + equippedWeapon);
                GameObject tago2 = ta2.gameObject;
                // tago2.transform.localEulerAngles = new Vector3(17.52f, 86.97f, 179.95f);
                tago2.transform.localEulerAngles = new Vector3(DialogueLua.GetActorField("LadyBiter", "xRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "yRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "zRot").AsFloat);
            }
            _state2 = PlayerEquippedItems.State2.Create;
            StartCoroutine("InstantiateWeapon");                    
        }
        //But if there is a weapon on the hand, destroy it

        if (gameObject.transform.Find(pathRHandCombat + "/" + equippedWeapon))
        {
            if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "RHand")
            {
                return;
            }
            GameObject go2 = gameObject.transform.Find(pathRHandCombat + "/" + equippedWeapon).gameObject;
            Destroy(go2);
        }
        //This is to ensure that the weapon on the hand is destroyed. I dont know why, but
        //if this is not done, the weapon is not destroyed. Problem with animator? 
        StartCoroutine("DestroyWeapon");
        _state = PlayerEquippedItems.State.Destroying;
    }
    #endregion

    //IN COMBAT
    #region
    else if (gamecontroller.GetComponent<CombatController>().inCombat == true)
    {
        Transform ta = gameObject.transform.Find(pathRHandCombat);
        GameObject tago = ta.gameObject;
        GameObject goToName = (Instantiate(Resources.Load("Weapons/" + equippedWeapon), ta.position, ta.rotation) as GameObject);
        goToName.transform.parent = tago.transform;
        goToName.name = equippedWeapon;
        _state2 = PlayerEquippedItems.State2.CreateInBattle;
        StartCoroutine("InstantiateWeapon");

        //But if there is a weapon on the back, destroy it
        if (gameObject.transform.Find(pathRHand + "/" + equippedWeapon))
        {    
            if (DialogueLua.GetActorField(_namePlayer, "RHand").AsString == "RHand")
            {
                return;
            }                
            Destroy(gameObject.transform.Find(pathRHand + "/" + equippedWeapon).gameObject);
        }
        StartCoroutine("DestroyWeaponBattle");
        stateCombat = PlayerEquippedItems.StateCombat.Destroy;
    }

    #endregion
}
//************if right hand is not equipped with weapon..........
#region
else
{
 //   InvokeRepeating("EmptyRHand", 0, 0.1f);
    //And left hand is not equipped either, hand to hand combat!
    /*
    if (DialogueLua.GetActorField(_namePlayer, "LHand").AsString == "LHand")
    {
        Transform ta = gameObject.transform.Find(pathRHandCombat);
        GameObject tago = ta.gameObject;
        GameObject goToName = (Instantiate(Resources.Load("Weapons/Hands"), ta.position, ta.rotation) as GameObject);
        goToName.transform.parent = tago.transform;
        goToName.name = "Hands";
    }
}
#endregion*/

/*
//WHEN NOT IN COMBAT. This is a coroutine that works while there is a weapon on the hand, if not set up as coroutine
//weapon is not destroyed, I dont know where the problem is, maybe related it needs a frame to setup
IEnumerator DestroyWeapon()
{
    string pathItemCombat = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 Rhand_Weapon";
    string pathItem = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 back_right_Wmount";
    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;

    //    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;

    while (alive)
    {
        switch (_state)
        {
            case State.Destroying:
                yield return new WaitForSeconds(0.1f);
                Execute(pathItemCombat, equippedWeapon);
                yield return new WaitForSeconds(0.1f);
                break;
                /*
                case State.DestroyingBattle:
                    yield return new WaitForSeconds(0.1f);
                    ExecuteBattle (equippedWeapon);                    
                    break;
        }
    }
    //     StopCoroutine("DestroyWeapon");
    yield return null;
}


public IEnumerator DestroyWeaponRHand()
{
    //    Debug.Log(pathRHand + "/" + itemRHand);
    bool alive = true;
    while (alive)
    {
        switch (state3)
        {
            case State3.Destroy:
                DestroyFromInventory(pathRHand, itemRHand, "DestroyWeaponRHand");
                yield return new WaitForSeconds(0.1f);
                break;
        }
    }
    //     StopCoroutine("DestroyWeapon");
    yield return null;
}

void DestroyFromInventory(string path, string item, string courotineToStop)
{
    Debug.Log(path + "/" + item + "/" + courotineToStop);
    if (gamecontroller.GetComponent<CombatController>().inCombat == false)
    {
        if (transform.Find(path + "/" + item))
        {
            /*
            if (DialogueLua.GetActorField(gamecontroller.GetComponent<GameController>().mainPlayerName, "RHand").AsString == "RHand")
            {
                return;
            }
            GameObject go2 = gameObject.transform.Find(path + "/" + item).gameObject;
            Debug.Log(go2);
            Destroy(go2);
        }

        if (transform.Find(path + "/" + item) == null)
        {
            StopCoroutine(courotineToStop);
        }
    }
    //This part may not be useful, because when the IEnumerator DestroyWeapon is controlled
    //by the while condition, while equipped weapon-> destroy. 
    else
    {
        StopCoroutine("DestroyWeaponRHand");

    }
}

//This function is part of the above coroutine to destroy the equipped weapon

void Execute(string path, string weapon)
{
     string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;

    if (equippedWeapon == "RHand")
    {
        return;
    }

    if (equippedWeapon == "Dalila")
    {
        string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
        if (dalilaItem == null || dalilaItem == "null")
        {
            dalilaItem = "Sceptre";
        }
        equippedWeapon = "Dalila" + dalilaItem;
    }
    if (weapon == "Dalila")
    {
        string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
        if (dalilaItem == null || dalilaItem == "null")
        {
            dalilaItem = "Sceptre";
        }
        weapon = "Dalila" + dalilaItem;
    }

    if (gamecontroller.GetComponent<CombatController>().inCombat == false)
    {
        if (transform.Find(path + "/" + weapon) != null)
        {
            if (gamecontroller.GetComponent<GameController>().mainPlayerName == "RHand")
            {
                return;
            }
            if (gameObject.transform.Find(path + "/" + weapon).gameObject)
            {

                GameObject go2 = gameObject.transform.Find(path + "/" + weapon).gameObject;
                Destroy(go2);
            }

            if (gameObject.transform.Find(path + "/" + weapon) == null)
            {
                StopCoroutine("DestroyWeapon");
            }
        }
        else
        {
            StopCoroutine("DestroyWeapon");
        }
    }
}

void ExecuteBattle(string weapon)
{
    string pathItemCombat = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 Rhand_Weapon";
    Debug.Log(gameObject.transform.Find(pathItemCombat + "/" + weapon).gameObject);
    if (gamecontroller.GetComponent<CombatController>().inCombat == true)
    {
        if (gameObject.transform.Find(pathItemCombat + "/" + weapon).gameObject)
        {
            GameObject go2 = gameObject.transform.Find(pathItemCombat + "/" + weapon).gameObject;
            Destroy(go2);
        }

        if (gameObject.transform.Find(pathItemCombat + "/" + weapon) == null)
        {
            StopCoroutine("DestroyWeapon");
        }
    }
    //This part may not be useful, because when the IEnumerator DestroyWeapon is controlled
    //by the while condition, while equipped weapon-> destroy. 
    else
    {
        StopCoroutine("DestroyWeapon");

    }
}

//INSTANTIATE IN BOTH COMBAT AND NOT COMBAT
IEnumerator InstantiateWeapon()
{
    string pathItemCombat = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 Rhand_Weapon";
    string pathItem = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 back_right_Wmount";
    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;
    if (equippedWeapon == "Dalila")
    {
        string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
        if (dalilaItem == null || dalilaItem == "null")
        {
            dalilaItem = "Sceptre";
        }
        equippedWeapon = "Dalila" + dalilaItem;
    }
    bool test = true;
    while (test)
    {
        switch (_state2)
        {
            case State2.Create:

                yield return new WaitForSeconds(0.9f);
                Create(pathItem, equippedWeapon);
                break;

            case State2.CreateInBattle:
                Create(pathItemCombat, equippedWeapon);
                yield return new WaitForSeconds(0.1f);
                break;
        }
    }
    //     StopCoroutine("InstantiateWeapon");
    yield return null;
}

void Create(string path, string weapon)
{
    if (gameObject.transform.Find(path + "/" + weapon) == null)
    {
        Debug.Log("Create");
        //          Debug.Log(gameObject.transform.Find(path + "/" + weapon) + "/" + gameObject);
        string _namePlayer = gameObject.name;

        Transform ta = gameObject.transform.Find(path);
        GameObject tago = ta.gameObject;

        GameObject goToName = (Instantiate(Resources.Load("Weapons/" + weapon), ta.position, ta.rotation) as GameObject);
        goToName.transform.parent = tago.transform;
        goToName.name = weapon;

        if (DialogueLua.GetActorField(weapon, "rotate").AsString == "Yes")
        {
            Transform ta2 = gameObject.transform.Find(path + "/" + weapon + "/" + weapon);
            GameObject tago2 = ta2.gameObject;
            // tago2.transform.localEulerAngles = new Vector3(17.52f, 86.97f, 179.95f);
            tago2.transform.localEulerAngles = new Vector3(DialogueLua.GetActorField("LadyBiter", "xRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "yRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "zRot").AsFloat);
        }
    }
    else
    {
        StopCoroutine("InstantiateWeapon");
    }
}

//Destroy in battle
IEnumerator DestroyWeaponBattle()
{
    //     string pathItemCombat = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 Rhand_Weapon";
    string pathItem = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 back_right_Wmount";
    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;

    //    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;

    while (gameObject.transform.Find(pathItem + "/" + equippedWeapon))
    {
        switch (stateCombat)
        {
            case StateCombat.Destroy:
                ExecuteInBattle(pathItem, equippedWeapon);
                yield return new WaitForSeconds(0.1f);
                break;
        }
    }
     StopCoroutine("DestroyWeaponBattle");
    yield return null;
}

void ExecuteInBattle(string path, string weapon)
{
    string equippedWeapon = DialogueLua.GetActorField(gameObject.name, "RHand").AsString;
    if (equippedWeapon == "Dalila")
    {
        string dalilaItem = DialogueLua.GetActorField("Player", "dalila").AsString;
        if (dalilaItem == null || dalilaItem == "null")
        {
            dalilaItem = "Sceptre";
        }
        equippedWeapon = "Dalila" + dalilaItem;
        Debug.Log(equippedWeapon);
    }
    if (gamecontroller.GetComponent<CombatController>().inCombat == true)
    {
        if (transform.Find(path + "/" + equippedWeapon) != null)
        {
            if (gameObject.transform.Find(path + "/" + weapon).gameObject)
            {
                GameObject go2 = gameObject.transform.Find(path + "/" + weapon).gameObject;
                Destroy(go2);
            }
            if (gameObject.transform.Find(path + "/" + weapon) == null)
            {
                StopCoroutine("DestroyWeaponBattle");
            }
        }
        else
        {
            StopCoroutine("DestroyWeaponBattle");
        }
    }
    //This part may not be useful, because when the IEnumerator DestroyWeapon is controlled
    //by the while condition, while equipped weapon-> destroy. 
    else
    {
        StopCoroutine("DestroyWeaponBattle");

    }
}

void EmptyRHand ()
{
    if (transform.Find(pathRHandCombat) != null)
    {
        int itemsNumber = 0;
        Transform rootRHand = transform.Find(pathRHandCombat);
        foreach (Transform ta in rootRHand)
        {
            itemsNumber++;
            GameObject go2 = rootRHand.gameObject;
            Destroy(go2);
        }

        if (itemsNumber == 0)
        {
            StopCoroutine("EmptyRHand");
        }
    }
    else
    {
        StopCoroutine("EmptyRHand");
    }
}



#region
    if (gameObject.transform.Find(pathReal) == null)
    {
        if (weaponName == "RHand" || equippedWeapon == "6002")
        {
            weaponRightPlace = true;
        }
        else
        {
            GameObject tago = ta.gameObject;
            tago.SetActive(true);
            /*
            GameObject goToName = (Instantiate(Resources.Load("Weapons/" + weaponName), ta.position, ta.rotation) as GameObject);
            goToName.transform.parent = tago.transform;
            goToName.name = weaponName;
            if (DialogueLua.GetActorField(equippedWeapon, "rotate").AsString == "Yes")
            {
                Transform ta2 = gameObject.transform.Find(pathRHand + "/" + equippedWeapon + "/" + equippedWeapon);
                GameObject tago2 = ta2.gameObject;
                // tago2.transform.localEulerAngles = new Vector3(17.52f, 86.97f, 179.95f);
                tago2.transform.localEulerAngles = new Vector3(DialogueLua.GetActorField("LadyBiter", "xRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "yRot").AsFloat, DialogueLua.GetActorField("LadyBiter", "zRot").AsFloat);
            }
        }
    }
    else
    {
        weaponRightPlace = true;
    }
    #endregion
    //Check and destroy right weapon where it should not be
    #region
    if (gameObject.transform.Find(pathOther))
    {
        if (weaponName == "RHand" || equippedWeapon == "6002")
        {
            weaponRightPlace = true;
        }
        else
        {
    //        Debug.Log(pathOther + "/" + weaponName);
            ta.gameObject.SetActive (false);
        }
    }
    else
    {
        weaponDestroyed = true;
    }
    #endregion
    //Check and destroy other weapons that should not be

    bool weaponToDestroy = false;
    foreach (Transform t in ta)
    {
        if (t.gameObject.name != weaponName)
        {
    //        Debug.Log(weaponName + "/" + t.gameObject.name);
            weaponToDestroy = true;
            if (t.gameObject.activeSelf)
            {
                weaponToDestroy = true;
                t.gameObject.SetActive(false);
            }
        }

        if (weaponToDestroy == false)
        {
            weaponRightPeace = true;
        }
    }

    bool weaponToDestroyCombat = false;
Transform taBattle = gameObject.transform.Find(pathOther);
    foreach (Transform tBattle in taBattle)
    {
        if (tBattle.gameObject.name != weaponName)
        {
            weaponToDestroyCombat = true;
            Destroy(tBattle.gameObject);
        }

        if (weaponToDestroyCombat == false)
        {
            weaponRightBattle = true;
        }
    }

*/
