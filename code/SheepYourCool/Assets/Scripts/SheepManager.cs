using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    public GameManager mGameManager;

    [SerializeField] public GameObject mSheepPrefab;
    [SerializeField] public GameObject mWuffles;
    public GameObject mCatchedSheepPrefab;
    public GameObject mBorderedSheepPrefab;
    public static GameObject[] mAllSheep;

    [SerializeField] public int mSheepAmount = 10;
    [SerializeField] public static int mSheepRunSize = 20;

    public static Vector3 mGoalPos = new Vector3(0f, 0f, 0f);

    private bool mIsInitialized = false;

    public static int mNormalSheepAmount = 0;
    public static int mCaughtSheepAmount = 0;
    public static int mBorderedSheepAmount = 0;


    public void Initialize()
    {
        mNormalSheepAmount = mSheepAmount;
        mAllSheep = new GameObject[mSheepAmount];
        for (int i = 0; i < mSheepAmount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-mSheepRunSize, mSheepRunSize),
                5,
                Random.Range(-mSheepRunSize, mSheepRunSize));
            GameObject sheep = Instantiate(mSheepPrefab, pos, Quaternion.identity);
            sheep.GetComponent<Flock>().MeetMisterWuffles(mWuffles);
            mAllSheep[i] = sheep;
            sheep.GetComponent<Flock>().Initialize(this, i);
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
        GameObject sheep = mAllSheep[index];
        if (sheep.GetComponent<Flock>().mCurrentStatus != Flock.Status.NORMAL)
        {
            return;
        }

        GameObject newSheep = Instantiate(mCatchedSheepPrefab, sheep.transform.position, Quaternion.identity);
        Destroy(sheep);

        mAllSheep[index] = newSheep;
        mNormalSheepAmount--;
        mCaughtSheepAmount++;

        if(mNormalSheepAmount <= 0)
        {
            mGameManager.GameOver(true);
        }
    }

    public void SheepBordered(int index)
    {
        GameObject sheep = mAllSheep[index];
        if (sheep.GetComponent<Flock>().mCurrentStatus != Flock.Status.NORMAL)
        {
            return;
        }

        GameObject newSheep = Instantiate(mBorderedSheepPrefab, sheep.transform.position, Quaternion.identity);
        Destroy(sheep);

        mAllSheep[index] = newSheep;
        mNormalSheepAmount--;
        mBorderedSheepAmount++;

        mGameManager.UpdateCanvas();

        if (mNormalSheepAmount <= 0)
        {
            mGameManager.GameOver(true);
        }
    }

    public static void Reset()
    {
        mNormalSheepAmount = 0;
        mCaughtSheepAmount = 0;
        mBorderedSheepAmount = 0;
    }
}
