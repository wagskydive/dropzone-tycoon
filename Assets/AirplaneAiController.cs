using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkydiveLogic;

public class AirplaneAiController : MonoBehaviour, IGiveInput, IAuxControl
{
    Vector4 lastInputs = Vector4.zero;
    Vector4 lastAuxInputs = Vector4.zero;
    Rigidbody rb;
    float takeOffHeading;
    [SerializeField]
    Transform boardingArea;

    [SerializeField]
    AircraftObject aircraftObject;

    [SerializeField]
    float desiredHeading = 0;
    float currentHeading;
    [SerializeField]
    float desiredAltitude = 4600;
    float currentAltitude;
    float desiredPitch = 0;
    float currentPitch;
    float currentRoll;

    bool hasPilot;

    bool engineIsOn = true;

    public RunwayObject currentRunway;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        aircraftObject.OnAircraftLoaded += HandleAircraftLoaded;
    }

    void HandleAircraftLoaded(Load load)
    {
        loaded = true;
        desiredAltitude = load.jumpers[0].ExitAltitude;
    }

    void GetCurrentFlightState()
    {
        Vector3 forwardDir = transform.forward.normalized;
        forwardDir.y = 0;
        currentHeading = Vector3.SignedAngle(Vector3.forward, forwardDir, Vector3.up);

        currentAltitude = transform.position.y;

        //Debug.Log("Current Heading: " + currentHeading);


        currentPitch = Vector3.SignedAngle(forwardDir, transform.forward.normalized, transform.right);
        currentRoll = Vector3.SignedAngle(Vector3.up, transform.up.normalized, forwardDir);
        //Debug.Log("Current Roll: " + currentRoll);
        float offset = desiredHeading - currentHeading;
        if (Mathf.Abs(offset) > 180)
        {
            if (desiredHeading < 0)
            {
                desiredHeading += 360;
            }
            else
            {
                desiredHeading -= 360;
            }

        }
    }

    bool hasTakenOff;

    void TakeOff()
    {
        lastAuxInputs.w = 0;
        lastInputs.y = 1;
        //desiredHeading = Vector3.SignedAngle(Vector3.forward, currentRunway.EndPoint.position - currentRunway.StartPoint.position, Vector3.up);
        lastInputs.w = 1;
        lastAuxInputs.x = .3f;
        //if (lastAuxInputs.x > .7f)
        //{
        //    lastAuxInputs.x = .7f;
        //}

        if (rb.velocity.magnitude > 20)
        {
            desiredPitch -= .05f;
            desiredPitch = Mathf.Clamp(desiredPitch, 0, -15);


        }
        if (transform.position.y > 3)
        {
            hasTakenOff = true;
            lastAuxInputs.x = 0;
        }
    }

    bool startPointReached;
    bool readyForTakeOff;

    bool loaded;


    public Vector4 GetInput()
    {
        if (aircraftObject.hasOperator)
        {

            if (engineIsOn)
            {

                GetCurrentFlightState();
                if (!loaded)
                {
                    TaxiToPoint(boardingArea.position);

                }
                else
                {


                    if (!hasTakenOff)
                    {
                        if (!readyForTakeOff)
                        {
                            if (!startPointReached)
                            {
                                lastInputs.w = .1f;
                                startPointReached = TaxiToPoint(currentRunway.StartPoint.position);
                            }
                            else
                            {
                                lastInputs.w = 0;
                                TurnForTakeOff();
                            }
                        }
                        else
                        {
                            TakeOff();

                            MaintainHeading();
                        }

                    }
                    else
                    {
                        //lastInputs.w = .9f;
                        lastInputs.w = 1;
                        MaintainAltitude();
                        MaintainHeading();
                        MaintainRoll();
                        MaintainPitch();
                    }

                }
                lastInputs = lastInputs.normalized;

            }
            else
            {
                lastInputs = Vector4.zero;
            }
        }
        else
        {

        }

        return lastInputs;
    }

    private void ClimbHeadingAdjustment()
    {
        if (currentAltitude < desiredAltitude)
        {
            desiredHeading += .015f;
            if (desiredHeading > 180)
            {
                desiredHeading -= 360;
            }
        }
    }

    float desiredRoll;

    private void MaintainRoll()
    {
        if (currentRoll != desiredRoll)
        {
            float rollcorrection = PIDRoll(desiredRoll / 45, currentRoll / 45, Time.fixedDeltaTime);

            lastInputs.x -= rollcorrection;

        }
    }

    private void MaintainPitch()
    {
        desiredPitch -= Mathf.Abs(currentRoll / 180);
        lastInputs.y += PIDPitch(desiredPitch, currentPitch, Time.fixedDeltaTime);
    }
    private void MaintainHeading()
    {
        if (currentHeading != desiredHeading)
        {
            desiredRoll = desiredHeading - currentHeading;
            //desiredRoll = PIDHeading(desiredHeading, currentHeading, Time.fixedDeltaTime);
            desiredRoll = -Mathf.Clamp(desiredRoll, -30, 30);
            Debug.Log("Desired Roll: " + desiredRoll + "\n");
        }
    }

    private bool TaxiToPoint(Vector3 pos)
    {
        lastAuxInputs.w = -.35f;

        desiredHeading = Vector3.SignedAngle(Vector3.forward, pos - transform.position, Vector3.up);
        Debug.DrawLine(transform.position, pos);
        Debug.Log("Desired Heading: " + desiredHeading + "\n");
        Debug.Log("Current Heading: " + currentHeading + "\n");
        lastInputs.z = desiredHeading / 180 - currentHeading / 180;
        if (rb.velocity.magnitude < 2 - (Mathf.Abs(lastInputs.z) * 2))
        {

            lastInputs.w += .01f;
            lastAuxInputs.y = 0;
        }
        else
        {
            lastInputs.w -= .01f;
            lastAuxInputs.y += .1f; ;
        }

        float distance = Vector3.Distance(transform.position, pos);

        if (distance < 4)
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    float headingTimer = 0;

    private void TurnForTakeOff()
    {


        lastAuxInputs.w = -.35f;
        desiredHeading = Vector3.SignedAngle(Vector3.forward, currentRunway.EndPoint.position - currentRunway.StartPoint.position, Vector3.up);

        lastInputs.z = desiredHeading / 180 - currentHeading / 180;
        //lastInputs.z = PIDYaw(desiredHeading * .1f, currentHeading * .1f, Time.fixedDeltaTime);
        if (rb.velocity.magnitude < 1 - (Mathf.Abs(lastInputs.z) * 1))
        {

            lastInputs.w += .01f;
            lastAuxInputs.y = 0;
        }
        else
        {
            lastInputs.w -= .01f;
            lastAuxInputs.y += .01f; ;
        }
        if (Mathf.Abs(desiredHeading - currentHeading) < .3f)
        {
            headingTimer += Time.fixedDeltaTime;
            if (headingTimer >= .9f)
            {
                lastAuxInputs.y = 0;
                lastInputs.w = 0;
                readyForTakeOff = true;
                rb.velocity = Vector3.zero;
            }

        }
        else
        {
            headingTimer = 0;
        }

    }





    private void MaintainAltitude()
    {

        if (currentAltitude != desiredAltitude)
        {

            float difference = desiredAltitude - currentAltitude;
            if (difference > 0)
            {
                desiredPitch = -8;
                //lastInputs.w += .15f;
            }

            else
            {
                desiredPitch = 8;
                //lastInputs.w -= .15f;
                //if( lastInputs.w < 0)
                //{
                //    lastInputs.w = 0;
                //}
            }

        }
    }

    float headingReset;

    [SerializeField]
    public float pFactorHeading, iFactorHeading, dFactorHeading;


    [SerializeField]
    public float pFactorPitch, iFactorPitch, dFactorPitch;


    [SerializeField]
    public float pFactorRoll, iFactorRoll, dFactorRoll;

    [SerializeField]
    public float pFactorYaw, iFactorYaw, dFactorYaw;

    float rollIntegral;
    float rollLastError;

    float headingIntegral;
    float headingLastError;

    float yawIntegral;
    float yawLastError;

    float pitchIntegral;
    float pitchLastError;

    public float PIDPitch(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        pitchIntegral += present * timeFrame;
        float deriv = (present - pitchLastError) / timeFrame;
        rollLastError = present;
        return present * pFactorPitch + pitchIntegral * iFactorPitch + deriv * dFactorPitch;
    }

    public float PIDRoll(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        rollIntegral += present * timeFrame;
        float deriv = (present - rollLastError) / timeFrame;
        rollLastError = present;
        return present * pFactorRoll + rollIntegral * iFactorRoll + deriv * dFactorRoll;
    }

    public float PIDHeading(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        headingIntegral += present * timeFrame;
        float deriv = (present - headingLastError) / timeFrame;
        headingLastError = present;
        return present * pFactorHeading + headingIntegral * iFactorHeading + deriv * dFactorHeading;
    }

    public float PIDYaw(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        yawIntegral += present * timeFrame;
        float deriv = (present - yawLastError) / timeFrame;
        yawLastError = present;
        return present * pFactorYaw + yawIntegral * iFactorYaw + deriv * dFactorYaw;
    }


    public Vector4 GetAuxInputs()
    {
        //lastAuxInputs = lastAuxInputs.normalized;
        return lastAuxInputs;
    }
}
