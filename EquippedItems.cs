using UnityEngine;
using System.Collections;
using System;

public class EquippedItems : MonoBehaviour
{
	public string part;
	public int position;
	public string original;

	public EquippedItems (string newPart, int newPosition, string newOriginal)
	{
		part = newPart;
		position = newPosition;
		original = newOriginal;
	}
}