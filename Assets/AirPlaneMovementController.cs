using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AirPlaneMovementController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider Front;


    [SerializeField]
    private WheelCollider RearLeft;

    [SerializeField]
    private WheelCollider RearRight;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Transform FrontT;
    [SerializeField]
    private Transform RearLeftT;
    [SerializeField]
    private Transform RearRightT;


    [SerializeField]
    float maxSteeringAngle = 30;

    [SerializeField]
    float motorPower = 500;

    [SerializeField]
    float liftRatio = 1.5f;


    Vector4 currentInput;

    float steeringAngle;

    float currentThrottle = 0;

    [SerializeField]
    public IGiveInput InputGiver;

    public void GetInput()
    {
        currentInput = InputGiver.GetInput();

    }


    private void Steer()
    {
        steeringAngle = maxSteeringAngle * currentInput.x;
        Front.steerAngle = steeringAngle;
        rigidbody.AddRelativeTorque(Vector3.right * currentInput .y* 150);
        rigidbody.AddRelativeTorque(Vector3.back* currentInput.x* 150);
    }


    private void SetThrottle()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(currentThrottle < 3)
            {
                currentThrottle += Time.deltaTime;
            }
            
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (currentThrottle > 0)
            {
                currentThrottle -= Time.deltaTime;
            }
        }

    }


    private void SetEngineForce()
    {
        rigidbody.AddRelativeForce(Vector3.forward * currentThrottle * motorPower);

    }

    private void UpdateWheelPoses()
    {

        UpdateWheelPose(Front, FrontT);
        UpdateWheelPose(RearRight, RearRightT);
        UpdateWheelPose(RearLeft, RearLeftT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos;// = _transform.position;
        Quaternion _quat;// = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);
        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        SetThrottle();
        SetEngineForce();
        SetLiftForce();
        UpdateWheelPoses();
    }

    private void SetLiftForce()
    {
        rigidbody.AddRelativeForce(Vector3.up * rigidbody.velocity.z * liftRatio);

    }
}
