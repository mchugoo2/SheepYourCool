using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int mXSize = 50;
    public int mZSize = 50;

    private Mesh mMesh;

    private Vector3[] mVertices;
    private int[] mTriangles;

    // Start is called before the first frame update
    void Start()
    {
        mMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mMesh;


        CreateShape();
        UpdateMesh();
        MeshCollider coll;
        if (TryGetComponent<MeshCollider>(out coll))
        {
            coll.sharedMesh = null;
            coll.sharedMesh = mMesh;
        }
        

    }



    private void CreateShape()
    {
        //create vertices

        mVertices = new Vector3[(mXSize + 1) * (mZSize + 1)];

        for (int i = 0, z = 0; z <= mZSize; z++)
        {
            for (int x = 0; x <= mXSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                mVertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        //create triangles
        mTriangles = new int[mXSize * mZSize * 6];
        int vert = 0;
        int tris = 0;


        for (int z = 0; z < mZSize; z++)
        {
            for (int x = 0; x < mXSize; x++)
            {

                mTriangles[tris + 0] = vert + 0;
                mTriangles[tris + 1] = vert + mXSize + 1;
                mTriangles[tris + 2] = vert + 1;
                mTriangles[tris + 3] = vert + 1;
                mTriangles[tris + 4] = vert + mXSize + 1;
                mTriangles[tris + 5] = vert + mXSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }


    }

    private void UpdateMesh()
    {
        mMesh.Clear();

        mMesh.vertices = mVertices;
        mMesh.triangles = mTriangles;

        mMesh.RecalculateNormals();
        mMesh.RecalculateBounds();
    }

    private void OnDrawGizmos()
    {
        if (mVertices == null)
        {
            return;
        }

        for (int i = 0; i < mVertices.Length; i++)
        {
            Gizmos.DrawSphere(mVertices[i], 0.1f);
        }
    }
}
