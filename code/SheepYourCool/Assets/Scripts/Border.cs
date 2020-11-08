using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private MeshRenderer mMeshRenderer;
    public void GetSensed()
    {
        mMeshRenderer.enabled = true;
    }

    public void NoMoreSensed()
    {
        mMeshRenderer.enabled = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        mMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
