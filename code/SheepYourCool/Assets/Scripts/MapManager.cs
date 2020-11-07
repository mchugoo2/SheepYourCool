using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MapManager : MonoBehaviour
{
    public int mBorderPointsEachSide = 20;
    public GameObject mBorderPartPrefab;
    public GameObject mBorderColumnPrefab;

    public float mBorderPointSharpness = 0.3f; //the greater this is, the deeper border points go in to the middle

    private MeshGenerator mMeshGenerator;
    private List<Vector2> mBorderPointList;
    private List<GameObject> mBorderParts;
    private List<GameObject> mBorderColumns;

    private Vector2[] mBorderCorners = new Vector2[4];

    // Start is called before the first frame update
    void Start()
    {
        mMeshGenerator = GetComponent<MeshGenerator>();
        mMeshGenerator.Initialize();

        gameObject.transform.position = new Vector3(-mMeshGenerator.mXSize / 2f, gameObject.transform.position.y, -mMeshGenerator.mZSize / 2f);

        Vector3 pos = gameObject.transform.position;

        mBorderPointList = new List<Vector2>();
        mBorderParts = new List<GameObject>();
        mBorderColumns = new List<GameObject>();

        mBorderCorners[0] = new Vector2(pos.x, pos.z);
        mBorderCorners[1] = new Vector2(pos.x + mMeshGenerator.mXSize, pos.z);
        mBorderCorners[2] = new Vector2(pos.x + mMeshGenerator.mXSize, pos.z + mMeshGenerator.mZSize);
        mBorderCorners[3] = new Vector2(pos.x, pos.z + mMeshGenerator.mZSize);

        CreateBorder();
    }

    private void CreateBorder()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 firstCorner = mBorderCorners[i];
            Vector2 secondCorner = i == 3 ? mBorderCorners[0] : mBorderCorners[i + 1];



            mBorderPointList.Add(firstCorner);

            Vector2 distStep = (secondCorner - firstCorner) / (mBorderPointsEachSide-1);
            bool changeOnX = distStep.x != 0; //if not, there is change on z

            Vector2 lastVertexNormal = firstCorner;
            Vector2 lastVertexRandomized = firstCorner;
            for (int step = 1; step < mBorderPointsEachSide; step++)
            {
                
                Vector2 currentVertex = step == mBorderPointsEachSide - 1 ? new Vector2(secondCorner.x, secondCorner.y) : lastVertexNormal + distStep; //distinction just to make sure that secondCorner is really reached

                Vector2 oldCurrentVertex = new Vector2(currentVertex.x, currentVertex.y);

                if (currentVertex != secondCorner) //is never first corner
                {
                    currentVertex = CalculateRandomVertexPos(currentVertex, lastVertexNormal, changeOnX);
                }

                lastVertexNormal = oldCurrentVertex;

                Debug.Log("currentVertex now: " + currentVertex);
                Debug.Log("===================");

                mBorderPointList.Add(currentVertex);

                CreateBorderPartAndColumn(currentVertex, lastVertexRandomized);

                
                lastVertexRandomized = currentVertex;
            }

        }
    }

    private void CreateBorderPartAndColumn(Vector2 currentVertex, Vector2 lastVertexRandomized)
    {
        GameObject borderPart = Instantiate(mBorderPartPrefab);
        Vector2 between2d = currentVertex - lastVertexRandomized;
        Vector3 between = new Vector3(between2d.x, 0, between2d.y);
        float distBetween = between.magnitude;

        borderPart.transform.localScale = new Vector3(1, 20, distBetween);
        borderPart.transform.position = new Vector3(lastVertexRandomized.x, 0, lastVertexRandomized.y) + (between / 2.0f);
        borderPart.transform.LookAt(currentVertex);
        borderPart.transform.rotation = Quaternion.LookRotation(between);
        
        borderPart.transform.parent = gameObject.transform;

        mBorderParts.Add(borderPart);

        GameObject borderColumn = Instantiate(mBorderColumnPrefab);
        mBorderColumnPrefab.transform.position = new Vector3(currentVertex.x, 0, currentVertex.y);
        mBorderColumnPrefab.transform.localScale = new Vector3(5, 20, 5);

        mBorderColumns.Add(borderColumn);
    }

    private Vector2 CalculateRandomVertexPos(Vector2 currentVertex, Vector2 lastVertex, bool changeOnX)
    {
        Debug.Log("last vertex: " + lastVertex);
        Debug.Log("current vertex: " + currentVertex);
        
        Vector2 minPosOnLine = currentVertex - (currentVertex - lastVertex) / 2.0f;
        Vector2 maxPosOnLine = currentVertex + (currentVertex - lastVertex) / 2.0f;

        Vector2 posOnLine = minPosOnLine + UnityEngine.Random.value * (maxPosOnLine - minPosOnLine);

        Vector2 minPosToMiddle = currentVertex;
        Vector2 addSize = changeOnX ? new Vector2(0, -mBorderPointSharpness*currentVertex.y) : new Vector2(-mBorderPointSharpness*currentVertex.x, 0);
        //Vector2 addSize = changeOnX ? new Vector2(0, (float)mMeshGenerator.mZSize/mBorderPointsEachSide) : new Vector2((float)mMeshGenerator.mXSize / mBorderPointsEachSide, 0);
        //if (changeOnX && currentVertex.y > 0 || !changeOnX && currentVertex.x > 0)
        //    addSize = -addSize;
        Vector2 maxPosToMiddle = currentVertex + addSize;
        Vector2 posToMiddle = minPosToMiddle + UnityEngine.Random.value * (maxPosToMiddle - minPosToMiddle);
        return (changeOnX ? new Vector2(posOnLine.x, posToMiddle.y) : new Vector2(posToMiddle.x, posOnLine.y));
        //return (changeOnX? new Vector2(currentVertex.x, posToMiddle.y) : new Vector2(posToMiddle.x, currentVertex.y));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
