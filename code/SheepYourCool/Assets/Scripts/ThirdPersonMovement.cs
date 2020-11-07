using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController mController;

    public float mSpeed = 6f;
    public float mTurnSmoothTime = 0.1f;
    public float mTurnSmoothVelocity;

    public float mWeight = 1f; //if bigger, Wuffles falls faster

    public Transform mCameraTransform;

    private float fallSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mController.isGrounded);

        //movement in x and z dir
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref mTurnSmoothVelocity, mTurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            mController.Move(moveDir.normalized * mSpeed * Time.deltaTime);
        }

        //falling if Wuffles loses ground contact

        if (mController.isGrounded)
            fallSpeed = 0f;

        else
            fallSpeed += Physics.gravity.y * Time.deltaTime;

        mController.Move(new Vector3(0f, fallSpeed * mWeight, 0f));
    }
}
