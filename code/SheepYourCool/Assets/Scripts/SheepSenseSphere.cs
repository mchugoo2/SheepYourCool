using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSenseSphere : MonoBehaviour
{
    private Flock mFlock;
    private bool mIsInitialized = false;
    private List<Collider> mColls;
    public void Initialize(Flock flock)
    {
        mFlock = flock;
        mIsInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER");
        if (!mIsInitialized)
            return;
        mColls.Add(other);
        mFlock.CollisionSensed(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!mIsInitialized)
            return;
        mColls.Remove(other);
        mFlock.CollisionLeft(other);
    }

    public List<Collider> GetColls()
    {
        return mColls;
    }
}
