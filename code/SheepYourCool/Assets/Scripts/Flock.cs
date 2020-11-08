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
    [SerializeField] private float mRotationSpeed = 0.5f;
    [SerializeField] private float mNeighbourDistance = 5.0f;
    [SerializeField] private float mCriticalNeighbourhood = 0.2f;
    private bool turn = false;

    // Start is called before the first frame update
    void Start()
    {
        mBaseSpeed = Random.Range(mMinSpeed, mMaxSpeed);
        mSpeed = mBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.mSheepRunSize)
        {
            turn = true;
        }
        else turn = false;

        if (turn)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                mRotationSpeed * Time.deltaTime);
        }
        else if (Random.Range(0, 5) < 1)
        {
            ApplyRules();
        }

        //falling if sheep loses ground contact
        else mFallSpeed += Physics.gravity.y * Time.deltaTime;

        mController.Move(new Vector3(0, mFallSpeed, Time.deltaTime * mSpeed));
    }

    void ApplyRules()
    {
        GameObject[] allSheep = GlobalFlock.mAllSheep;

        Vector3 groupCenter = Vector3.zero;
        Vector3 groupAvoid = Vector3.zero;
        float groupSpeed = 0.1f;
        Vector3 goalPos = GlobalFlock.mGoalPos;
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
}
