using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int mXSize = 50;
    public int mZSize = 50;

    public bool mDrawGizmos = true;

    //public float mPerlinNoiseMultiplier = 2f;
    //public float mPerlinNoiseXMultiplier = 0.3f;
    //public float mPerlinNoiseZMultiplier = 0.3f;


    public float mAmp1 = 18.76f;
    public float mAmp2 = 14.2f;
    public float mAmp3 = 1.0f;
    public float mScale1 = 3.26f;
    public float mScale2 = 11.04f;
    public float mScale3 = 10.35f;


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
                //float y = Mathf.PerlinNoise(x * mPerlinNoiseXMultiplier, z * mPerlinNoiseZMultiplier) * mPerlinNoiseMultiplier;
                float y = CalculateNoise(x, z);
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

    private float CalculateNoise(float x, float z)
    {

        float noise;
        noise = Mathf.PerlinNoise(x, z) * 5;
        noise += Mathf.PerlinNoise(x * mAmp1, z * mAmp1) * mScale1;
        noise -= Mathf.PerlinNoise(x * mAmp2, z * mAmp2) * mScale2;
        noise += Mathf.PerlinNoise(x * mAmp3, z * mAmp3) * mScale3 * 2;
        return noise;
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
        if (!mDrawGizmos || mVertices == null)
        {
            return;
        }

        for (int i = 0; i < mVertices.Length; i++)
        {
            Gizmos.DrawSphere(mVertices[i], 0.1f);
        }
    }
}
