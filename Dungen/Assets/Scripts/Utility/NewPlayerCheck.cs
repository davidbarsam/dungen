using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayerCheck : MonoBehaviour
{
    public GameObject NewPlayerGameObject;

	void Start () 
	{
	    if (SceneManager.GetActiveScene().buildIndex == 0)
	    {
	        if (PlayerPrefsManager.GetNewPlayer() && NewPlayerGameObject != null)
	        {
                NewPlayerGameObject.SetActive(true);
	        }
	    }
	}
	
	void Update () 
	{
	    if (Input.GetKeyDown(KeyCode.R))
	    {
            PlayerPrefsManager.SetNewPlayer(true);
	    }
	}
}
