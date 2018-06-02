using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{

	void Start ()
    {

	}
	
	void Update ()
    {
		
	}

    public void LoadLevelAsync(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
