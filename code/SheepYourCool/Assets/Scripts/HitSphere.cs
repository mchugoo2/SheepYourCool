using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSphere : MonoBehaviour
{
    private ThirdPersonMovement mThirdPersonMovement;
    private bool mIsInitialized = false;
    public void Initialize(ThirdPersonMovement thirdPersonMovement)
    {
        mThirdPersonMovement = thirdPersonMovement;
        mIsInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mIsInitialized)
            return;
        mThirdPersonMovement.CollisionHit(other);
    }
}
