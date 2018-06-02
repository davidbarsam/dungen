using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles all functions of the player, including input, effects, and play state management.
/// The PlayerDirector will communicate with GameDirector to control the flow of the game.
/// </summary>
public class PlayerDirector : MonoBehaviour
{

    public static PlayerDirector _this;

    [Header("Required")]
    public float MBaseVelocity = 5f;        // How fast the player moves forward
    public Camera MCamera;                  // The camera
    public int MPotionBoost = 2;           // How much distance the player will gain with a potion
    [Space(10f)]
    [Header("Tags")]
    [SerializeField] private string MWallTag = "Wall";      // The tag a wall obstacle will have
    [SerializeField] private string MSpikeTag = "Spike";    // The tag a spike obstacle will have
    [SerializeField] private string MMonsterTag = "Monster";// The tag a monster obstacle will have
    [SerializeField] private string MPotionTag = "Potion";  // The tag a potion will have
    [Space(10f)]
    [Header("Attacking")]
    public Button MAttackButton;            // UI Attack button
    [Range(1, 50), SerializeField] private int MAttacksMinimum = 1;  // Minimum amount of attacks to kill a monster
    [Range(1, 100), SerializeField] private int MAttacksMaximum = 20;  // Minimum amount of attacks to kill a monster
    // Amount of attacks to kill a monster depends on difficulty level, based on distance traveled.
    [Space(10f)]
    [Header("Other Properties")]
    public Button MLeftButton;              // UI Left Button
    public Button MRightButton;             // UI Right Button
    public float MLeftLaneOffset;           // Left lane's Z offset
    public float MRightLaneOffset;          // Right lane's Z offset

    // Enumeration of possible lanes
    private enum ELanes
    {
        Left,
        Right
    }                 

    // Enumeration of possible states
    private enum EStates
    {
        Moving,
        Stopped,
        Dead,
        Attack
    }
    private bool isPlayerDead = false;

    private int mCurrentLane;               // The current lane, stored as an integer. 0: Left Lane, 1: Right Lane
    private float mInternalTimer;           // Internal timer, used for animation timing.
    private int mCurrentState;              // The current state, stored as an integer. 0: Moving, 1: Stopped, 2: Dead, 3: Attack
    private float mCurrentDistanceToCamera; // Current distance to MCamera
    private int mAttackCount;           // How many attacks are left to kill the monster
    private int mAttacksRequired = 1;       // How many attacks are needed to kill the monster

    private GameObject mPlayer;             // The player gameObject
    private ParticleSystem mParticleSystem; // The player's particle system

    // Unity messages
    #region Unity Functions

    // Assign defaults
    private void Awake()
    {
        _this = this;   // Static reference to PlayerDirector
        mPlayer = gameObject;   // Reference to player's object
        mParticleSystem = GetComponent<ParticleSystem>();   // Reference to player's attached particle system
        mCurrentLane = 0;   // Initialize current lane to left lane
        mCurrentState = 0;  // Initialize current state to moving
        Vector3 newPos = new Vector3(mPlayer.transform.position.x, mPlayer.transform.position.y, MLeftLaneOffset);  // Put player in left lane
        mPlayer.transform.position = newPos;    // Put player in left lane
        mInternalTimer = 0; // Initialize internal timer to 0 seconds
    }

    // Enable particle system
    private void Start()
    {
        mParticleSystem.Play();
    }

    private void Update()
    {

        switch (mCurrentState)
        {
            case 0:
                Debug.Log(EStates.Moving);
                break;
            case 1:
                Debug.Log(EStates.Stopped);
                break;
            case 2:
                Debug.Log(EStates.Dead);
                break;
            case 3:
                Debug.Log(EStates.Attack);
                break;
        }

        // Start internal timer
        mInternalTimer += Time.deltaTime;

        if (!isPlayerDead)
        {
            // Take other input support besides on-screen buttons
            TakeInputs();
            // Move the player based on the current state
            MovePlayer(mCurrentState);
        }
        // Update the particle system based on the current state
        UpdateParticleSystem(mCurrentState);
        // Check if the player is dead (too close to camera)
        CheckAndHandlePlayerDeath();
    }



    private void OnEnable()
    {
        // Set PlayerAudio active
        GetComponent<PlayerAudio>().enabled = true;
    }

    // Handle obstacle and monster detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MWallTag) && !isPlayerDead)        // Ran into wall
        {
            mCurrentState = (int)EStates.Stopped;
        }
        else if (other.CompareTag(MMonsterTag) && !isPlayerDead)
        {
            mCurrentState = (int)EStates.Attack;
            mAttacksRequired = UnityEngine.Random.Range(MAttacksMinimum, MAttacksMaximum);
            mAttackCount = mAttacksRequired;
            MAttackButton.gameObject.SetActive(true);
        }
        else if(other.CompareTag(MSpikeTag) && !isPlayerDead)   // Ran into spikes
        {
            mCurrentState = (int)EStates.Dead;
        }
        else if(other.CompareTag(MPotionTag) && !isPlayerDead)
        {
            mCurrentState = (int)EStates.Moving;

            if(CameraDirector._this.GetDistanceToPlayer() < 6)
            {
                // "Consume" the potion by adding X to the player's distance and destroying the potion
                mPlayer.transform.position -= new Vector3(MPotionBoost, 0, 0);
                Destroy(other.gameObject);
                PlayerAudio._this.PlayPickupSound();
            }
            else
            {
                // Add it to the loot score because I don't wanna have to make loot especially for this so I'll be lazy instead
                GameDirector._this.AddLootScore();
                Destroy(other.gameObject);
                PlayerAudio._this.PlayPickupSound();
            }
            
        }
    }

    // Reset state to moving
    private void OnTriggerExit(Collider other)
    {
        if (!isPlayerDead)
        {
            mCurrentState = (int)EStates.Moving;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(MMonsterTag))
        {
            if(mAttackCount < MAttacksMinimum)
            {
                MAttackButton.gameObject.SetActive(false);
                Destroy(other.gameObject);
                if (!isPlayerDead)
                {
                    mCurrentState = (int)EStates.Moving;
                    GameDirector._this.AddKillScore();
                }
            }
        }
    }

    #endregion                         

    // Anything that updates in Update, LateUpdate, ManualUpdate
    #region Update Functions
    /// <summary>
    /// Updates attached particle system based on current state
    /// </summary>
    /// <param name="state">The current state of the player</param>
    private void UpdateParticleSystem(int state)
    {
        switch (state)
        {
            case 0:     // Moving
                if (mParticleSystem.isStopped)
                {
                    mParticleSystem.Play();
                }
                break;
            case 1:     // Stopped
                if (mParticleSystem.isPlaying)
                {
                    mParticleSystem.Stop();
                }
                break;
            case 2:     // Dead
                if (mParticleSystem.isPlaying)
                {
                    mParticleSystem.Stop();
                }
                break;
            case 3:     // Attack
                if (mParticleSystem.isPlaying)
                {
                    mParticleSystem.Stop();
                }
                break;
        }
    }
    #endregion

    // Anything managing player movement
    #region Movement Functions
    /// <summary>
    /// Translate the player's position based on the current state
    /// </summary>
    /// <param name="state">The current state of the player</param>
    private void MovePlayer(int state)
    {
        switch (state)
        {
            case 0:     // Moving
                transform.Translate
                (
                    Vector3.forward * MBaseVelocity * Time.deltaTime,
                    Space.Self
                );
                break;
            case 1:     // Stopped
                break;
            case 2:     // Dead
                break;
            case 3:     // Attack
                break;
            default:
                break;
        }
    }

    public void Attack()
    {
        if(mAttackCount <= mAttacksRequired)
        {
            --mAttackCount;
            PlayerAudio._this.PlayAttackSound();
        }
    }

    /// <summary>
    /// Move the player to the appropriate lane based on their current lane state
    /// </summary>
    /// <param name="state">Current lane state</param>
    private void MoveToLane(int state)
    {
        if(mCurrentState != (int)EStates.Attack)
        {
            switch (state)
            {
                case 0:
                    mPlayer.transform.localPosition = new Vector3(
                        mPlayer.transform.localPosition.x,
                        mPlayer.transform.localPosition.y,
                        -0.7f);
                    break;
                case 1:
                    mPlayer.transform.localPosition = new Vector3(
                        mPlayer.transform.localPosition.x,
                        mPlayer.transform.localPosition.y,
                        0.7f);
                    break;
            }
        }

    }

    private void TakeInputs()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            MoveLeft();
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            MoveRight();
        }

        if (Input.GetButtonDown("Fire1") && mCurrentState == (int)EStates.Attack)
        {
            Attack();
        }
    }

    /// <summary>
    /// Checks and handles if the player is dead based on the distance to the camera
    /// </summary>
    private void CheckAndHandlePlayerDeath()
    {
        isPlayerDead = CameraDirector._this.IsPlayerDead();
        if (isPlayerDead)
        {
            mCurrentState = (int)EStates.Dead;
            gameObject.SetActive(false);
            if (MAttackButton.gameObject.activeInHierarchy)
            {
                MAttackButton.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    // Anything interactable with UI buttons
    #region Button API
    /// <summary>
    /// Button API. Changes lane state to left lane
    /// </summary>
    public void MoveLeft()
    {
        if (mCurrentLane != (int)ELanes.Left)
        {
            mCurrentLane = (int)ELanes.Left;
            MoveToLane(mCurrentLane);
        }
    }

    /// <summary>
    /// Button API. Changes lane state to right lane
    /// </summary>
    public void MoveRight()
    {
        if (mCurrentLane != (int)ELanes.Right)
        {
            mCurrentLane = (int)ELanes.Right;
            MoveToLane(mCurrentLane);
        }
    }
    #endregion

    // Anything that returns a variable
    #region Return Functions
    /// <summary>
    /// Public function that returns the current state of PlayerDirector. Globally accessible.
    /// </summary>
    /// <returns></returns>
    public int ReturnCurrentState()
    {
        return mCurrentState;
    }

    /// <summary>
    /// Returns the appropriate lane position based on the current lane state
    /// </summary>
    /// <returns>Lane's Z offset</returns>
    private float ReturnCurrentLane()
    {
        switch (mCurrentLane)
        {
            case 0:     // Left Lane
                return MLeftLaneOffset;
            case 1:     // Right Lane
                return MRightLaneOffset;
        }

        return MLeftLaneOffset;
    }
    #endregion

}