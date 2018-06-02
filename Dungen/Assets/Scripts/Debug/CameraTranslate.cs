using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTranslate : MonoBehaviour
{

    public float Velocity = 10f;

    public bool IsForTutorial;

	void Start ()
    {
        if (!IsForTutorial)
        {
            Debug.LogWarning(this + " has a DEBUG script on it. Remove before shipping.");
        }
        
	}
	
	void Update ()
    {
        Debug.Log(Time.timeScale);
        if (Time.timeScale > 0)
        {
            transform.Translate(new Vector3(0, 0, Velocity * Time.deltaTime));
        }
        
	}
}
