using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script grabs whatever text object is assigned, whether attached or linked,
/// and gives it the recorded high score.
/// Requireds a text component.
/// </summary>
[RequireComponent(typeof(Text))]
public class GetHighScore : MonoBehaviour
{

    public Text m_HighScoreDisplay;

	void Start ()
    {
        if (GetComponent<Text>())
        {
            m_HighScoreDisplay = GetComponent<Text>();
        }

        if (m_HighScoreDisplay)
        {
            m_HighScoreDisplay.text = PlayerPrefsManager.GetPersonalHighScore().ToString();
        }
        else
        {
            Debug.LogError(name + " is missing a text reference!");
        }
	}
	
	void Update ()
    {
		
	}
}
