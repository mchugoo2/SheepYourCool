using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseSphere : MonoBehaviour
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
        Debug.Log("ENTER");
        if (!mIsInitialized)
            return;
        mThirdPersonMovement.CollisionSensed(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!mIsInitialized)
            return;
        mThirdPersonMovement.CollisionLeft(other);
    }

}
