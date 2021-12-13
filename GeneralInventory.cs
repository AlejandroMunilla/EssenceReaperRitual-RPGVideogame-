using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralInventory : MonoBehaviour 
{

	private static List<Item2> genInventory = new List<Item2>();
	public static List <Item2> GenInventory 
	{
		get { return genInventory;}
	}

}
