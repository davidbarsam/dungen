using UnityEngine;
using System.Collections;

public class lightMapAssign : MonoBehaviour
{
    public int tunnelPieces = 5;
    public GameObject prefab;
    public Vector3 offset;
    // Use this for initialization
    void Start()
    {
        // Get the render component of our prefab 
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        // Store the lighmap index of our prefab 
        int lightmapIndex = r.lightmapIndex;
        Vector4 lightmapScaleOffset = r.lightmapScaleOffset;

        GameObject piece;
        for (int i = 1; i < tunnelPieces; i++)
        {
            // Instatiate our prefab at runtime 
            piece = (GameObject)Instantiate(prefab, new Vector3(i * offset.x, i * offset.y, i * offset.z), Quaternion.identity);
            // Assign our prefabs lightmap index to the instatiated ones 
            Renderer piecer = piece.GetComponent<Renderer>();
            piecer.lightmapIndex = lightmapIndex;
            piecer.lightmapScaleOffset = lightmapScaleOffset;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}