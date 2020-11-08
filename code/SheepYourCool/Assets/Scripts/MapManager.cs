using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MapManager : MonoBehaviour
{

    public GameManager mGameManager;

    //BORDER STUFF
    //========================================
    public int mBorderPointsEachSide = 20;
    public GameObject mBorderPartPrefab;
    public GameObject mBorderColumnPrefab;

    public float mBorderPointSharpness = 0.7f; //the greater this is, the deeper border points go in to the middle

    private MeshGenerator mMeshGenerator;
    private List<Vector2> mBorderPointList;
    private List<GameObject> mBorderParts;
    private List<GameObject> mBorderColumns;

    private Vector2[] mBorderCorners = new Vector2[4];

    //FENCE STUFF
    //========================================
    public GameObject mFencePartPrefab;
    public GameObject mFencePostPrefab;

    public float mTwoFencePointsAsOneThreshold = 2.0f;
    public float mMaxFenceDistanceThreshold = 5f;
    public float mFenceHeight = 5f;

    private GameObject mCurrentFenceParent;
    private List<Vector3> mCurrentFencePoints;
    private List<GameObject> mCurrentFenceParts;
    private List<GameObject> mCurrentFencePosts;

    private void Awake()
    {
        mMeshGenerator = GetComponent<MeshGenerator>();
    }

    public void Initialize()
    {

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

        ResetFenceBuilding();
    }

    private void CreateBorder()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 firstCorner = mBorderCorners[i];
            Vector2 secondCorner = i == 3 ? mBorderCorners[0] : mBorderCorners[i + 1];



            mBorderPointList.Add(firstCorner);

            Vector2 distStep = (secondCorner - firstCorner) / (mBorderPointsEachSide - 1);
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

        Vector2 minPosOnLine = currentVertex - (currentVertex - lastVertex) / 2.0f;
        Vector2 maxPosOnLine = currentVertex + (currentVertex - lastVertex) / 2.0f;

        Vector2 posOnLine = minPosOnLine + UnityEngine.Random.value * (maxPosOnLine - minPosOnLine);

        Vector2 minPosToMiddle = currentVertex;
        Vector2 addSize = changeOnX ? new Vector2(0, -mBorderPointSharpness * currentVertex.y) : new Vector2(-mBorderPointSharpness * currentVertex.x, 0);
        //Vector2 addSize = changeOnX ? new Vector2(0, (float)mMeshGenerator.mZSize/mBorderPointsEachSide) : new Vector2((float)mMeshGenerator.mXSize / mBorderPointsEachSide, 0);
        //if (changeOnX && currentVertex.y > 0 || !changeOnX && currentVertex.x > 0)
        //    addSize = -addSize;
        Vector2 maxPosToMiddle = currentVertex + addSize;
        Vector2 posToMiddle = minPosToMiddle + UnityEngine.Random.value * (maxPosToMiddle - minPosToMiddle);
        return (changeOnX ? new Vector2(posOnLine.x, posToMiddle.y) : new Vector2(posToMiddle.x, posOnLine.y));
    }



    public void PlaceFencePost(Vector3 pos)
    {


        bool first = mCurrentFencePoints.Count == 0;

        //first post -> just place it
        if (first)
        {
            CreatePost(pos);
            return;

        }

        //same pos as last post -> do nothing
        Vector3 lastFencePos = mCurrentFencePoints[mCurrentFencePoints.Count-1];
        if (Vector3.Distance(pos, lastFencePos) <= mTwoFencePointsAsOneThreshold || Vector3.Distance(pos, lastFencePos) > mMaxFenceDistanceThreshold)
        {
            return;

        }


        //same pos as very first post -> check if there is an enclosure
        Vector3 firstFencePos = mCurrentFencePoints[0];
        if (Vector3.Distance(pos, firstFencePos) <= mTwoFencePointsAsOneThreshold)
            if (mCurrentFencePoints.Count <= 1)
            {
                return;
            }

            else
            {
                CloseFence();
                return;
            }


        //if none of the above apply, place a fence between two fence posts
        CreatePost(pos);
        PlaceFencePart(false);

    }

    private void CreatePost(Vector3 pos)
    {
        mCurrentFencePoints.Add(pos);
        GameObject newFencePost = Instantiate(mFencePostPrefab, pos, Quaternion.identity, mCurrentFenceParent.transform);
        mCurrentFencePosts.Add(newFencePost);
    }

    private void CloseFence()
    {
        PlaceFencePart(true);
        mGameManager.FenceClosed(mCurrentFencePoints);
        ResetFenceBuilding();
    }

    private void PlaceFencePart(bool lastFencePart)
    {
        int length = mCurrentFencePoints.Count;

        //its already checked that there are two ore more fence posts
        Vector3 firstPost = lastFencePart? mCurrentFencePoints[0] : mCurrentFencePoints[length-2];
        Vector3 secondPost = mCurrentFencePoints[length-1];

        GameObject fencePart = Instantiate(mFencePartPrefab);
        Vector3 between = secondPost - firstPost;
        float distBetween = between.magnitude;

        fencePart.transform.localScale = new Vector3(1, mFenceHeight, distBetween);
        fencePart.transform.position = firstPost + (between / 2.0f);
        fencePart.transform.LookAt(secondPost);
        fencePart.transform.rotation = Quaternion.LookRotation(between);


        fencePart.transform.parent = mCurrentFenceParent.transform;


        mCurrentFenceParts.Add(fencePart);

    }

    public void RemoveCurrentFenceSystem()
    {

    }

    private void ResetFenceBuilding()
    {
        mCurrentFenceParent = new GameObject();
        mCurrentFenceParent.name = "FenceParent";
        mCurrentFenceParent.transform.parent = gameObject.transform;
        mCurrentFencePoints = new List<Vector3>();
        mCurrentFenceParts = new List<GameObject>();
        mCurrentFencePosts = new List<GameObject>();
    }

    public void SetSize(int size)
    {
        mMeshGenerator.mXSize = size;
        mMeshGenerator.mZSize = size;
    }
}
