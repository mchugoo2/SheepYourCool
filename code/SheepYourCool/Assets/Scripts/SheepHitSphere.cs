using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepHitSphere : MonoBehaviour
{
    private Flock mFlock;
    private bool mIsInitialized = false;
    public void Initialize(Flock flock)
    {
        mFlock = flock;
        mIsInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mIsInitialized)
            return;
        mFlock.CollisionHit(other);
    }
}
