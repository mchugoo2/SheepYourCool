using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mMesh;

    Vector3[] mVertices;
    int[] mTriangles;
    // Start is called before the first frame update
    void Start()
    {
        mMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mMesh;


        CreateShape();
        UpdateMesh();

    }



    private void CreateShape()
    {
        mVertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),

        };

        mTriangles = new int[]
        {
            0,1,2,
            1,3,2
        };
    }

    private void UpdateMesh()
    {
        mMesh.Clear();

        mMesh.vertices = mVertices;
        mMesh.triangles = mTriangles;

        mMesh.RecalculateNormals();
    }
}
