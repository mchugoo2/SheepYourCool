using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] public GameObject mSheepPrefab;
    public GameObject mCatchedSheepPrefab;
    public static GameObject[] mAllSheep;

    [SerializeField] public int mSheepAmount = 10;
    [SerializeField] public static int mSheepRunSize = 20;

    public static Vector3 mGoalPos = new Vector3(0f, 0f, 0f);

    public List<GameObject> mAllSheepAsList;
    

    private bool mIsInitialized = false;

    public void Initialize()
    {
        mAllSheepAsList = new List<GameObject>();
        mAllSheep = new GameObject[mSheepAmount];
        for (int i = 0; i < mSheepAmount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-mSheepRunSize, mSheepRunSize),
                5,
                Random.Range(-mSheepRunSize, mSheepRunSize));
            GameObject sheep = Instantiate(mSheepPrefab, pos, Quaternion.identity);
            mAllSheep[i] = sheep;
            mAllSheepAsList.Add(sheep);
        }

        mIsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mIsInitialized)
            return;

        if (Random.Range(0, 1000) < 5)
        {
            mGoalPos = new Vector3(
                Random.Range(-mSheepRunSize, mSheepRunSize),
                0,
                Random.Range(-mSheepRunSize, mSheepRunSize));
        }
    }

    public void CatchSheep(int index)
    {
        GameObject sheep = mAllSheepAsList[index];
        if (sheep.GetComponent<Flock>().mIsCatched)
        {
            return;
        }

        GameObject newSheep = Instantiate(mCatchedSheepPrefab, sheep.transform.position, Quaternion.identity);
        Destroy(sheep);

        mAllSheepAsList[index] = newSheep;
        mAllSheep[index] = newSheep;
    }
}
