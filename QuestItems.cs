using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestItems : MonoBehaviour {

	private static List<Item2> questItems = new List<Item2>();
	public static List <Item2> QuestItem
	{
		get { return questItems;}
	}
}