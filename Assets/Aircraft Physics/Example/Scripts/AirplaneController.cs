using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    List<WheelCollider> wheels = null;
    [SerializeField]
    List<WheelCollider> steeringWheels = null;
    [SerializeField]
    float steerAngle =0;
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;
    [SerializeField]
    Text displayText = null;
    [Range(0,1)]
    float thrustPercent;
    float brakesTorque;

    AircraftPhysics aircraftPhysics;
    Rigidbody rb;

    IGiveInput giveInput;
    IAuxControl giveAuxInput;



    private void Start()
    {
        giveInput = GetComponent<IGiveInput>();
        giveAuxInput = GetComponent<IAuxControl>();
        if (giveInput == null)
        {
            giveInput = gameObject.AddComponent<PlayerInput>();
        }
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
    }

   void SetFlightPhysics(bool active)
    {
        aircraftPhysics.enabled = active;
    }

    private void Update()
    {
        Vector4 inputs = giveInput.GetInput();
        Pitch =inputs.y;
        Roll = inputs.x;
        Yaw =inputs.z;
        thrustPercent += inputs.w*.1f;
        
        Vector4 auxInputs = giveAuxInput.GetAuxInputs();
        Flap = auxInputs.x;
        brakesTorque = auxInputs.y;
        

        if(auxInputs.w < -.3f)
        {
            SteerWheels(Yaw * 40);
            //if (aircraftPhysics.enabled)
            //{
            //    SetFlightPhysics(false);
            //}


        }

        else if(auxInputs.w > -.3f)
        {
            SteerWheels(0);
            if (!aircraftPhysics.enabled)
            {
                SetFlightPhysics(true);
            }
            
        }

        displayText.text = "Yaw: " + Yaw.ToString()+ "\n";
        displayText.text += "Pitch: " + Pitch.ToString() + "\n";
        displayText.text += "Roll: " + Roll.ToString() + "\n";
       
        displayText.text += "V: " + ((int)(rb.velocity.magnitude* 3.6f)).ToString("D3") + " km/u\n";
        displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";

        displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
        displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
    }

    void SteerWheels(float steerAngle)
    {
        if (steeringWheels.Any())
        {
            foreach (var wheel in steeringWheels)
            {
                wheel.steerAngle = steerAngle;

            }
        }

    }



    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        //Mathf.Clamp(thrustPercent, 0, 1);
        if(thrustPercent >= 1)
        {
            thrustPercent = 1;
        }
        if (thrustPercent <= 0)
        {
            thrustPercent = 0;
        }
        aircraftPhysics.SetThrustPercent(thrustPercent);
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = brakesTorque;
            // small torque to wake up wheel collider
            wheel.motorTorque = 0.01f;
        }
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }
}
