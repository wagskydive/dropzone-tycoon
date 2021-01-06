using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider FrontLeft;

    [SerializeField]
    private WheelCollider FrontRight;

    [SerializeField]
    private WheelCollider RearLeft;

    [SerializeField]
    private WheelCollider RearRight;


    [SerializeField]
    private Transform FrontLeftT;
    [SerializeField]
    private Transform FrontRightT;
    [SerializeField]
    private Transform RearLeftT;
    [SerializeField]
    private Transform RearRightT;


    [SerializeField]
    float maxSteeringAngle = 30;

    [SerializeField]
    float motorPower = 500;




    
    public IGiveInput InputGiver;

    Vector4 currentInput;


    float steeringAngle;

    public void Brake()
    {
        if(currentInput.w > 0)
        {
            FrontLeft.brakeTorque = currentInput.w*30;
            FrontRight.brakeTorque = currentInput.w * 30;
            RearLeft.brakeTorque = currentInput.w * 30;
            RearRight.brakeTorque = currentInput.w * 30;
        }
        else
        {
            FrontLeft.brakeTorque = 0;
            FrontRight.brakeTorque = 0;
            RearLeft.brakeTorque = 0;
            RearRight.brakeTorque = 0;
        }

    }


    public  void GetInput()
    {
        if(InputGiver == null)
        {
            InputGiver = GetComponent<IGiveInput>();
        }
        currentInput = InputGiver.GetInput();

    }


    private void Steer()
    {
        steeringAngle = maxSteeringAngle * currentInput.x;
        FrontLeft.steerAngle = steeringAngle;
        FrontRight.steerAngle = steeringAngle;
        


    }

    private void Accelerate()
    {
        FrontLeft.motorTorque = currentInput.y * motorPower;
        FrontRight.motorTorque = currentInput.y * motorPower;
    }

    private void UpdateWheelPoses()
    {

        UpdateWheelPose(FrontLeft, FrontLeftT);
        UpdateWheelPose(FrontRight, FrontRightT);
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
        Accelerate();
        Brake();
        UpdateWheelPoses();
    }
}
