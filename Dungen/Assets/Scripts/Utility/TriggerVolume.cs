using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;



public class TriggerVolume : MonoBehaviour 
{

    [Header("Choose One Action")]
    public bool FreezeTime;
    public bool EnableObject;
    public GameObject ObjectToEnable;
    public bool PlaySoundEffect;
    public AudioClip SfxClip;
    public AudioSource SfxSource;
    public bool LoadNewLevel;
    public int LevelIndex;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("MainCamera"))
        {
            if (FreezeTime)
            {
                Time.timeScale = 0;
            }

            if (EnableObject)
            {
                if (ObjectToEnable)
                {
                    ObjectToEnable.SetActive(true);
                }
            }

            if (PlaySoundEffect)
            {
                SfxSource.PlayOneShot(SfxClip);
            }

            if (LoadNewLevel)
            {
                if (LevelIndex != 0)
                {
                    SceneManager.LoadSceneAsync(LevelIndex);
                }
            }
        }

    }

    /// <summary>
    /// Public funtion that sets Time.timeScale to 1. To be used with UI.
    /// </summary>
    public void ResumeTime()
    {
        Time.timeScale = 1;
    }


}
