using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MapManager : MonoBehaviour
{
    private MeshGenerator mMeshGenerator;


    // Start is called before the first frame update
    void Start()
    {
        mMeshGenerator = GetComponent<MeshGenerator>();
        mMeshGenerator.Initialize();
        gameObject.transform.position = new Vector3(-mMeshGenerator.mXSize / 2f, gameObject.transform.position.y, -mMeshGenerator.mZSize / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
