using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] public GameObject mSheepPrefab;
    public static GameObject[] mAllSheep;

    [SerializeField] public int mSheepAmount = 10;
    [SerializeField] public static int mSheepRunSize = 20;

    public static Vector3 mGoalPos = new Vector3(0f, 0f, 0f);

    private bool mIsInitialized = false;

    public void Initialize()
    {
        mAllSheep = new GameObject[mSheepAmount];
        for (int i = 0; i < mSheepAmount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-mSheepRunSize, mSheepRunSize),
                5,
                Random.Range(-mSheepRunSize, mSheepRunSize));
            mAllSheep[i] = Instantiate(mSheepPrefab, pos, Quaternion.identity);
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
}
