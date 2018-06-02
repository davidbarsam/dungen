using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject PausePanelGameObject;
    [Header("Affected Elements")]
    public AudioSource MusicAudioSource;
    public AudioSource UiAudioSource;
    public AudioClip PauseAudioClip;

    private bool isPaused;


	void Start ()
	{
	    isPaused = false;
	}
	
	void Update () 
	{
	    if (!GameDirector._this.GetPlayerStatus())
	    {
	        if(Input.GetButtonDown("Submit") && !isPaused) { PauseGame(); }
	        else if (Input.GetButtonDown("Submit") && isPaused) { UnPauseGame(); }

	        if (isPaused)
	        {
	            Time.timeScale = 0;
	        }
	        else if (!isPaused)
	        {
	            Time.timeScale = 1;
	        }
	    }

	}

    public void PauseGame()
    {
        isPaused = true;
        PausePanelGameObject.SetActive(true);
        MusicAudioSource.Pause();
        UiAudioSource.PlayOneShot(PauseAudioClip);
    }

    public void UnPauseGame()
    {
        isPaused = false;
        PausePanelGameObject.SetActive(false);
        MusicAudioSource.UnPause();
    }

    public void ExitScene()
    {
        isPaused = false;
    }
}
