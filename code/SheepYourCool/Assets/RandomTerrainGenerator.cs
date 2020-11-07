using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Terrain))]
public class RandomTerrainGenerator : MonoBehaviour
{
    private Terrain mTerrain;
    // Start is called before the first frame update
    void Start()
    {
        mTerrain = GetComponent<Terrain>();
        //mTerrain.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
