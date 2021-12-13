/// <summary>
/// Dice roll.
/// AMG, Sept 1st, 2015
/// This Class mimics dice rolling.
/// Example; 1D10 would means make minValue = 1 and maxValue = 11. MaxValue always desired value +1.

using UnityEngine;
using System.Collections;

public class DiceRoll : MonoBehaviour 

{
	public int minValue = 1;
	public int maxValue = 1;
	public int diceRoll;
	public App app;

	void Start()
	{
		app = GetComponent <App>();
	}

	public void DiceRolling (int minValue, int maxValue)
	{
		diceRoll = Random.Range (minValue, maxValue);
	//	Debug.Log ("Min_; " + minValue + " /Max_; " + maxValue + " /DiceRoll :" + diceRoll);


	}

}
