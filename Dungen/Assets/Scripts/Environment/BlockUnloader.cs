using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUnloader : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        LevelDirector._this.blocksLoaded--;
        Destroy(other.gameObject);
    }

}
