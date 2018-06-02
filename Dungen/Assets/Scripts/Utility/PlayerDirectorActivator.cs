using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectorActivator : MonoBehaviour
{
    public PlayerDirector MPlayerDirector;
    public PlayerAudio MPlayerAudio;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected.");
        MPlayerDirector.enabled = true;
        MPlayerAudio.enabled = true;
        GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }

    
}
