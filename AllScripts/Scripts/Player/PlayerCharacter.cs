using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour 
{
	public Transform RHandWeapon;
	public GameObject player;


	private static List<Item2> _inventory = new List<Item2>();
	public static List <Item2> Inventory 
	{
		get { return _inventory;}
	}
	/*
	private static Item2 _equipedWeapon;
	public static List <Item2> EquipedWeapon
	{
		get {return _equipedWeapon;}
		set {_equipedWeapon = value;}
	}
	*/

	// Use this for initialization
	void Start () 
	{	
		player = gameObject;
//		RHandWeapon = player.transform.Find("Bip01/Pelvis/Spine/Spine1/RClavicle/RUpperArm/RForearm/RHand/RHandWeapon");

	}
	

}
