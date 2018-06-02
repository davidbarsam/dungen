using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWallXRay : MonoBehaviour
{

    private Transform parentTransform;

	void Start ()
	{
	    parentTransform = GetComponentInParent<Transform>();
	}

    void Update()
    {

    }

    // If you contact a box collider (placed where a wall would be), then change the alpha of the material based on how close the camera is to that collider
    private void OnTriggerStay(Collider other)
    {
        foreach (MeshRenderer mr in other.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material mat in mr.materials)
            {
                if (other.GetComponent<BoxCollider>())
                {
                    mat.color = new Color(1, 1, 1, (Mathf.Clamp01(parentTransform.position.x - other.GetComponent<BoxCollider>().transform.position.x)));
                }
                
            }
        }
    }
}
