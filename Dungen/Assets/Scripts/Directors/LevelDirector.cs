using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    // DEFINITIONS
    // These are terms I use throughout this script:
    // Blocks: Prefabs of premade level segments
    // Level Blocks: Same as blocks

    public static LevelDirector _this;

    // Container for LevelBlocks (prefabs), initialized into Dictionary at Start
    [Serializable]
    public struct LevelBlocks
    {
        public int id;
        public GameObject obj;
    }

    [Header("Add the level block prefabs here.")]
    public LevelBlocks[] blocks;

    private Dictionary<int, GameObject> DLevelBlocks = new Dictionary<int, GameObject>();

    [Header("Additional Generator Variables:")]
    [Tooltip("What is the distance the LevelDirector must go to reach the end of the current block")]
    public Vector3 m_BlockGenOffset;

    // How far the LevelDirector moves before spawning a new block

    [Tooltip("The starting point for generation, usually (0,0,0)")]
    public Transform m_LevelOrigin;

    // The starting point for generation, usually (0,0,0)

    [Tooltip("How many instances of a blank hallway prefab to spawn before spawning obstacles, enemies")]
    public int m_HallwayStartLength;

    // How many instances of a blank hallway prefab to spawn before spawning obstacles, enemies

    [Tooltip("How many breaks between obstacles (a blank hallway) should be inserted between obstacales?")]
    public int m_GapsInObstacles;

    // How many breaks between obstacles (a blank hallway) should be inserted between obstacales?

    [Tooltip("How many blocks should be active at once?")]
    public int m_BlocksActive;

    // How many blocks should be active at once?

    [Tooltip("Where should blocks start being unloaded?")]
    public GameObject m_Killzone;

    // Where blocks will start being unloaded

    // Variables
    [HideInInspector] public int blocksLoaded = 0;

    private GameObject previousBlockLoaded;

    // Use this for initialization
    private void Start()
    {
        _this = this;

        PopulateDictionary();

        if (m_LevelOrigin == null)
        {
            Exception exception = new Exception("There is no level origin! Give LevelDirector a level origin!");
            throw exception;
        }

        if (m_HallwayStartLength > 0)
        {
            GenerateHallwayStart(m_HallwayStartLength);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (blocksLoaded < m_BlocksActive)
        {
            GenerateLevel();
        }
    }

    private void PopulateDictionary()
    {
        foreach (LevelBlocks levelBlocks in blocks)
        {
            DLevelBlocks.Add(levelBlocks.id, levelBlocks.obj);
        }

        Debug.Log("DEBUG: " + DLevelBlocks.Count + " blocks loaded.");
    }

    private void GenerateHallwayStart(int length)
    {
        transform.position = m_LevelOrigin.position;

        for (int i = 0; i < length; i++)
        {
            PlaceBlock(0);
            GoToNewOrigin(m_BlockGenOffset);
            blocksLoaded++;
        }
    }

    /// <summary>
    /// Generates the level based off of a random int generator, which feeds an
    /// ID into PlaceBlock, which generates a block.
    /// </summary>
    private void GenerateLevel()
    {
        // Only obstacles can be picked in this range, hence the hard coded 1
        int randomID = UnityEngine.Random.Range(1, DLevelBlocks.Count);
        PlaceBlock(randomID);
        GoToNewOrigin(m_BlockGenOffset);
        blocksLoaded++;
        if(m_GapsInObstacles > 0)
        {
            for(int i = 0; i < m_GapsInObstacles; i++)
            {
                PlaceBlock(0);
                GoToNewOrigin(m_BlockGenOffset);
                blocksLoaded++;
            }
        }
    }

    /// <summary>
    /// Places a level block in the next available slot. Takes in an ID from the block dictionary.
    /// </summary>
    /// <param name="blockID">Block identifier.</param>
    private void PlaceBlock(int blockID)
    {
        Instantiate(DLevelBlocks[blockID], transform.position, Quaternion.identity);
        return;
    }

    private void GoToNewOrigin(GameObject block)
    {
        transform.position = block.transform.position + m_BlockGenOffset;
        return;
    }

    private void GoToNewOrigin(Vector3 offset)
    {
        transform.position = transform.position + offset;
        return;
    }
}