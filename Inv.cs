using UnityEngine;
using System.Collections;

public class Inv : MonoBehaviour 
{
	public string name;
	public string value;
	private Texture2D _icon;
	

	public Inv (string newName, string newValue)
	{
		name = newName;
		newValue = newValue;
	}
	
	private Texture2D Icon 
	{
		get {return _icon;}
		set {_icon = value;}
	}
	
}
