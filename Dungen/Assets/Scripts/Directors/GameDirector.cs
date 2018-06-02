using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("Required")]
    public LevelDirector m_LevelDirector;
    public PlayerDirector m_PlayerDirector;
    public Text m_LootDisplay;
    public Text m_KillDisplay;
    public Text m_DistanceDisplay;

    [Header("Point Values")]
    [SerializeField] private float m_LootValue = 10f;
    [SerializeField] private float m_KillValue = 5f;

    [Header("Death Panel")]
    public GameObject m_DeathPanel;
    public Text m_DeathScoreDisplay;
    [HideInInspector] public bool deathPanelEnabled = false;

    [Header("Audio")]
    public AudioSource m_MusicSource;
    public AudioSource m_SoundSource;

    // Values
    private bool isPlayerDead;
    private float m_Score = 0f; // Score = DistanceCount + (LootCount * LootValue) + (KillCount * KillValue)
    private float m_DistanceCount = 0f;
    private float m_LootCount = 0f;
    private float m_KillCount = 0f;

    public static GameDirector _this;

	private void Start ()
    {
        _this = this;
        Time.timeScale = 1;
    }

    private void Update ()
    {
        isPlayerDead = CameraDirector._this.IsPlayerDead();

        // UI updates
        m_LootDisplay.text = "Loot: " + m_LootCount.ToString();
        m_KillDisplay.text = "Kill: " + m_KillCount.ToString();
        m_DistanceDisplay.text = "Distance: " + m_DistanceCount.ToString();

        CheckIfDead();

    }

    private void LateUpdate()
    {
        m_DistanceCount = Mathf.Round(Mathf.Abs(m_PlayerDirector.transform.position.x / 5));
    }

    /// <summary>
    /// When the player dies, calculate the final score, check and/or submit the high score, do social integration stuff,
    /// and enable post-death UI. Also, for optimization, delete the hallway instances.
    /// </summary>
    private void CheckIfDead()
    {
        if (isPlayerDead)
        {
            
            CalculateScore();

            CheckAndSubmitHighScore(m_Score);
            // TODO add Google Play integration
            // TODO add Apple GameCenter integration (?)

            // Enable the post-death UI
            if (m_DeathPanel.activeInHierarchy == false)
            {
                m_DeathPanel.SetActive(true);
            }

            if (deathPanelEnabled)      // This variable is set from DeathPanelBroadcast as a trigger
            {
                // Get every object in the scene, find the "hallway" ones, and delete them.
                // If the player is dead, there's no reason to render the hallway blocks.
                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                {
                    // If the object's name contains "Hallway" (likely instantiated by LevelDirector)
                    if (obj.name.Contains("Hallway"))
                    {
                        // Destroy it
                        Destroy(obj);
                    }
                }
            }

            // Pass score onto death panel's display
            m_DeathScoreDisplay.text = m_Score.ToString();

            if (m_MusicSource.isPlaying)
            {
                m_MusicSource.Stop();
            }
            if (!m_SoundSource.gameObject.activeInHierarchy)
            {
                m_SoundSource.gameObject.SetActive(true);
            }

            
        }
    }

    private void CalculateScore()
    {
        m_Score = m_DistanceCount + (m_LootCount * m_LootValue) + (m_KillCount * m_KillValue);
    }

    void CheckAndSubmitHighScore(float score)
    {
        if(score > PlayerPrefsManager.GetPersonalHighScore())
        {
            PlayerPrefsManager.SetPersonalHighScore(score);
        }
    }

    #region Public Accessors
    // Adds +1 to loot count
    public void AddLootScore()
    {
        m_LootCount++;
    }
    // Adds amt to loot score
    public void AddLootScore(int amt)
    {
        m_LootCount += amt;
    }
    // Adds +1 to kill score
    public void AddKillScore()
    {
        m_KillCount++;
    }
    // Adds amt to kill score
    public void AddKillScore(int amt)
    {
        m_KillCount += amt;
    }
    // Returns death state
    public bool GetPlayerStatus()
    {
        return isPlayerDead;
    }
    #endregion
}
