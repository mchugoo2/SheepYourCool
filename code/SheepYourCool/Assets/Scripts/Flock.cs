using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public CharacterController mController;
    private float mFallSpeed = 0f;

    public float mSpeed = 0.1f;
    private float mBaseSpeed;
    [SerializeField] private float mMinSpeed = 0.2f;
    [SerializeField] private float mMaxSpeed = 0.75f;
    [SerializeField] private float mRotationSpeed = 2f;
    [SerializeField] private float mNeighbourDistance = 20f;
    [SerializeField] private float mCriticalNeighbourhood = 0.2f;
    [SerializeField] private float mCriticalWuffles = 0.5f;
    private bool mTurn = false;
    private bool mFlee = false;

    public SheepSenseSphere mSheepSenseSphere;
    public SheepHitSphere mSheepHitSphere;
    private GameObject mMisterWuffles;

    private SheepManager mSheepManager;
    private int mOwnIndex = -1;

    public enum Status
    {
        NORMAL,
        IS_CATCHED,
        IS_BORDERED
    }

    public Status mCurrentStatus = Status.NORMAL;


    public void Initialize(SheepManager sheepManager, int ownIndex)
    {
        mSheepSenseSphere.Initialize(this);
        mSheepHitSphere.Initialize(this);
        mSheepManager = sheepManager;
        mOwnIndex = ownIndex;

        mBaseSpeed = UnityEngine.Random.Range(mMinSpeed, mMaxSpeed);
        mSpeed = mBaseSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mCurrentStatus != Status.NORMAL) 
            return;

        Debug.Log("Wuffles " + mMisterWuffles);
        if (Vector3.Distance(transform.position, mMisterWuffles.transform.position) <= mCriticalWuffles * mNeighbourDistance)
            mFlee = true;
        else mFlee = false;

        if (Vector3.Distance(transform.position, Vector3.zero) >= SheepManager.mSheepRunSize)
            mTurn = true;
        else mTurn = false;

        if (mFlee)
        {
            Vector3 direction =
                new Vector3(transform.position.x, 0, transform.position.z) -
                new Vector3(mMisterWuffles.transform.position.x, 0, mMisterWuffles.transform.position.z);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                mRotationSpeed * Time.deltaTime);
            mSpeed = 5f;
        }
        else if (mTurn)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                mRotationSpeed * Time.deltaTime);
        }
        else if (UnityEngine.Random.Range(0, 5) < 1)
        {
            ApplyRules();
        }

        //falling if sheep loses ground contact
        mFallSpeed += Physics.gravity.y * Time.deltaTime;

        float x = Time.deltaTime * mSpeed * (1 - Mathf.Abs(Mathf.Cos(transform.rotation.y)));
        float z = Time.deltaTime * mSpeed * Mathf.Abs(Mathf.Cos(transform.rotation.y));

        mController.Move(new Vector3(x, mFallSpeed, z));
    }

    void ApplyRules()
    {
        GameObject[] allSheep = SheepManager.mAllSheep;

        Vector3 groupCenter = Vector3.zero;
        Vector3 groupAvoid = Vector3.zero;
        float groupSpeed = 0.1f;
        Vector3 goalPos = SheepManager.mGoalPos;
        float distance;
        int groupSize = 0;

        foreach (GameObject sheep in allSheep)
        {
            if (sheep != this.gameObject)
            {
                distance = Vector3.Distance(
                    new Vector3(sheep.transform.position.x, 0, sheep.transform.position.z), 
                    new Vector3(this.transform.position.x, 0, this.transform.position.z));

                if (distance <= mNeighbourDistance)
                {
                    groupCenter += new Vector3(sheep.transform.position.x, 0, sheep.transform.position.z);
                    groupSize++;

                    if (distance < mCriticalNeighbourhood * mNeighbourDistance)
                    {
                        groupAvoid += new Vector3(
                            this.transform.position.x + sheep.transform.position.x,
                            0,
                            this.transform.position.z + sheep.transform.position.z);
                    }

                    Flock anotherFlock = sheep.GetComponent<Flock>();
                    groupSpeed += anotherFlock.mSpeed;
                }
            }

            if (groupSize > 0)
            {
                groupCenter = groupCenter / groupSize 
                    + (goalPos - new Vector3(this.transform.position.x, 0, this.transform.position.z));
                mSpeed = groupSpeed / groupSize;

                Vector3 direction = (groupCenter + groupAvoid) - new Vector3(transform.position.x, 0, transform.position.z);
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, 
                        Quaternion.LookRotation(direction), 
                        mRotationSpeed * Time.deltaTime);
                }
            }
            else mSpeed = Mathf.Max(mBaseSpeed, 0.999f * mSpeed);
        }
    }

    public void MeetMisterWuffles(GameObject wuffles)
    {
        mMisterWuffles = wuffles;
    }

    public void CollisionSensed(Collider collider)
    {
        collider.gameObject.SendMessage("GetSensed");
    }

    public void CollisionLeft(Collider collider)
    {
        collider.gameObject.SendMessage("NoMoreSensed");
    }

    public void CollisionHit(Collider collider)
    {
        foreach(Collider coll in mSheepSenseSphere.GetColls())
        {
            coll.gameObject.SendMessage("NoMoreSensed");
        }
        mSheepManager.SheepBordered(mOwnIndex);
    }
}
