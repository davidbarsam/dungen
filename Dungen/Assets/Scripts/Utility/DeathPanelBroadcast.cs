using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanelBroadcast : MonoBehaviour {

    public GameDirector m_GameDirector;

	void Start ()
    {
        if(m_GameDirector == null)
        {
            Debug.LogError(name + " is missing a GameDirector reference!");
        }
	}
	
	void Update ()
    {

	}

    public void BroadcastToDirector()
    {
        m_GameDirector.deathPanelEnabled = true;
    }
}
