using UnityEngine;
using System.Collections;

public class StopEffect : MonoBehaviour
{
    public int timeToDisable = 5;
    // Use this for initialization
    void Start ()
    {
        Invoke("EndEffect", timeToDisable);
	}
	
    void EndEffect ()
    {
        Destroy(gameObject);
    }
}
