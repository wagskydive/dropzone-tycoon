using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(VehicleObject))]
public class CarAiController : MonoBehaviour, IGiveInput
{
    Rigidbody rigidbody;

    NavMeshAgent navMeshAgent;
    NavMeshPath currentPath;

    VehicleObject vehicle;
    Transform currentTarget;

    [SerializeField]
    float maxSpeed;

    float currentMaxSpeed;

    Vector4 lastInputs = Vector4.zero;
    int pathPassed;

    bool hasDriver;
    bool enginIsOn;

    int vehicleNavMeshAgentTypeID;
    int characterNavMeshAgentTypeID;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        vehicleNavMeshAgentTypeID = navMeshAgent.agentTypeID;
        vehicle = GetComponent<VehicleObject>();
    }

    public void SetupNavigation(CharacterBrain _characterBrain, Transform _target)
    {
        //navMeshAgent = _characterBrain.gameObject.GetComponent<NavMeshAgent>();
        //characterNavMeshAgentTypeID = navMeshAgent.agentTypeID;
        navMeshAgent.enabled = true;
        //navMeshAgent.agentTypeID = vehicleNavMeshAgentTypeID;
        //characterNavMeshAgentTypeID = navMeshAgent.agentTypeID;
        //navMeshAgent.agentTypeID = 1;
        hasDriver = vehicle.hasOperator;
        currentTarget = _target;
        navMeshAgent.areaMask = 73;


        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        RecalculatePath();


        if (maxSpeed <= .01f)
        {
            maxSpeed = 20;
        }
        currentMaxSpeed = maxSpeed;
    }

    private void RecalculatePath()
    {
        navMeshAgent.enabled = true;


        if (currentPath == null)
        {
            currentPath = new NavMeshPath();
            navMeshAgent.CalculatePath(currentTarget.position, currentPath);

            if (currentPath.corners.Length > 0)
            {

                pathPassed = 0;
                currentPathTarget = currentPath.corners[0];
                Debug.Log("New Target Set: " + currentPathTarget);
            }

        }
        else
        {
            NavMeshPath newPath = new NavMeshPath();
            navMeshAgent.CalculatePath(currentTarget.position, newPath);

            //navMeshAgent.path = currentPath;
            if (newPath.corners.Length > 0 && newPath.corners[0] != currentPath.corners[0])
            {
                currentPath = newPath;
                pathPassed = 0;
                currentPathTarget = currentPath.corners[0];
                Debug.Log("New Target Set: " + currentPathTarget);
            }
        }


        navMeshAgent.enabled = false;
    }

    public void StartEngine()
    {
        enginIsOn = true;
    }

    public bool GetSensorInput()
    {
        currentMaxSpeed = maxSpeed / 2;

        float frontRaysLength = rigidbody.velocity.magnitude *3;

        float turnRate = rigidbody.angularVelocity.y * 4;


        float rightRayAngle = Mathf.Clamp(turnRate, .35f, .95f);
        Ray rRay = new Ray(transform.position + transform.forward + transform.right + Vector3.up, transform.TransformDirection((Vector3.forward + Vector3.right * rightRayAngle).normalized * frontRaysLength));
        Debug.DrawRay(rRay.origin, rRay.direction * frontRaysLength, Color.red);

        float leftRayAngle = Mathf.Clamp(turnRate, -.95f, -.35f);
        Ray lRay = new Ray(transform.position + transform.forward + transform.right*-1+ Vector3.up, transform.TransformDirection((Vector3.forward + Vector3.right * leftRayAngle).normalized * frontRaysLength));
        Debug.DrawRay(lRay.origin, lRay.direction * frontRaysLength, Color.red);


        Ray mRay = new Ray(transform.position + transform.forward + Vector3.up, transform.TransformDirection(Vector3.forward * frontRaysLength));
        Debug.DrawRay(mRay.origin, mRay.direction * frontRaysLength, Color.red);

        //Ray mCloseRay = new Ray(transform.position + transform.forward + Vector3.up, transform.TransformDirection(Vector3.forward * 5));
        //Debug.DrawRay(mCloseRay.origin, mRay.direction * 5, Color.red);
        //
        //Ray mrRay = new Ray(transform.position + transform.forward + Vector3.up, transform.TransformDirection((Vector3.forward + Vector3.right * .15f) * frontRaysLength));
        //Debug.DrawRay(mRay.origin, mrRay.direction * frontRaysLength, Color.red);
        //
        //Ray mlRay = new Ray(transform.position + transform.forward + Vector3.up, transform.TransformDirection((Vector3.forward + Vector3.left * .15f) * frontRaysLength));
        //Debug.DrawRay(mRay.origin, mlRay.direction * frontRaysLength, Color.red);
        //
        //Ray rRoadRay = new Ray(transform.position + Vector3.up, transform.TransformDirection(Vector3.right * 20) + Vector3.down * 6f);
        //Debug.DrawRay(rRoadRay.origin, rRoadRay.direction * 20, Color.blue);
        //
        //Ray lRoadRay = new Ray(transform.position + Vector3.up, transform.TransformDirection(Vector3.left * 20) + Vector3.down * 6f);
        //Debug.DrawRay(lRoadRay.origin, lRoadRay.direction * 20, Color.blue);


        RaycastHit rHit;
        RaycastHit lHit;
        RaycastHit mHit;
        //RaycastHit mCloseHit;
        //RaycastHit mrHit;
        //RaycastHit mlHit;

        Physics.Raycast(rRay, out rHit, frontRaysLength);
        Physics.Raycast(lRay, out lHit, frontRaysLength);
        Physics.Raycast(mRay, out mHit, frontRaysLength);
        //Physics.Raycast(mCloseRay, out mCloseHit, 5);
        //Physics.Raycast(mrRay, out mrHit, 14);
        //Physics.Raycast(mlRay, out mlHit, 14);

        bool didSense = false;

        //if (mrHit.collider != null && mlHit.collider == null)
        //{
        //    didSense = true;
        //    lastInputs.x -= .15f;
        //}
        //if (mlHit.collider != null && mrHit.collider == null)
        //{
        //    didSense = true;
        //    lastInputs.x += .15f;
        //}
        if (mHit.collider != null)
        {
            didSense = true;
            lastInputs.y -= .06f;
        }

        if (rHit.collider != null)
        {
            didSense = true;
            //Debug.Log("Hit with: " + rHit.collider.gameObject.name);
            if (rigidbody.velocity.z < 0.2f)
            {
                lastInputs.y -= .06f;
            }
            lastInputs.x -= .6f;
        }

        if (lHit.collider != null)
        {
            didSense = true;
            if (rigidbody.velocity.z < 0.2f)
            {
                lastInputs.y -= .06f;
            }
            lastInputs.x += .6f;
        }

        //if (mHit.collider != null || mrHit.collider != null && mlHit.collider != null)
        //{
        //    didSense = true;
        //    lastInputs.x += .14f;
        //
        //
        //}
        //
        //
        //if (mCloseHit.collider != null)
        //{
        //    didSense = true;
        //    if (rigidbody.velocity.z > 0)
        //    {
        //        lastInputs.w += .05f * rigidbody.velocity.z;
        //
        //    }
        //}

        return didSense;
    }

    [Range(0, .1f)]
    float notOnRightSideOfRoad = 0;

    void SenseRoad()
    {
        Ray downRay = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Debug.DrawRay(downRay.origin, downRay.direction, Color.white);

        RaycastHit downRayHit;

        Physics.Raycast(downRay, out downRayHit, 3);
        if (downRayHit.collider != null && downRayHit.collider.gameObject.GetComponent<DrivableRoad>() != null)
        {
            Ray rRoadRay = new Ray(transform.position, transform.TransformDirection(Vector3.down * .13f + Vector3.right + Vector3.forward));
            Debug.DrawRay(rRoadRay.origin, rRoadRay.direction * 5, Color.yellow);
            RaycastHit rRoadRayHit;

            Physics.Raycast(rRoadRay, out rRoadRayHit, 5);
            if (rRoadRayHit.collider != null && rRoadRayHit.collider.gameObject.GetComponent<DrivableRoad>() != null)
            {
                notOnRightSideOfRoad += .0005f;

                lastInputs.x += notOnRightSideOfRoad;
            }
            else
            {
                notOnRightSideOfRoad = 0;
            }
        }
        else
        {
            Ray rRoadRay = new Ray(transform.position, transform.TransformDirection(Vector3.down * .23f + Vector3.right + Vector3.forward));
            Debug.DrawRay(rRoadRay.origin, rRoadRay.direction * 5, Color.yellow);
            RaycastHit rRoadRayHit;

            Physics.Raycast(rRoadRay, out rRoadRayHit, 5);

            if (rRoadRayHit.collider != null && rRoadRayHit.collider.gameObject.GetComponent<DrivableRoad>() != null)
            {
                lastInputs.x += .01f;
            }

            Ray lRoadRay = new Ray(transform.position, transform.TransformDirection(Vector3.down * .23f + Vector3.left + Vector3.forward));
            Debug.DrawRay(lRoadRay.origin, lRoadRay.direction * 5, Color.yellow);
            RaycastHit lRoadRayHit;

            Physics.Raycast(lRoadRay, out lRoadRayHit, 5);

            if (lRoadRayHit.collider != null && lRoadRayHit.collider.gameObject.GetComponent<DrivableRoad>() != null)
            {
                lastInputs.x -= .01f;
            }

        }
    }

    Vector3 currentPathTarget = Vector3.zero;

    float timer = 0;
    float pathCalculateTime = 1;

    public Vector4 GetInput()
    {


        if (hasDriver)
        {
            if (enginIsOn)
            {
                if (timer < pathCalculateTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    RecalculatePath();
                }

                //NavigateTowardsTarget();
                //SenseRoad();
                if (!GetSensorInput())
                {
                    NavigateTowardsTarget();
                    //SenseRoad();
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
            lastInputs = Vector4.zero;
            if (rigidbody.velocity.magnitude > 0)
            {
                lastInputs.w = 1;
            }
        }

        return lastInputs;
    }

    private void NavigateTowardsTarget()
    {
        if (currentPathTarget != Vector3.zero)
        {
            float dot = Vector3.Dot(transform.forward, (currentPathTarget - transform.position).normalized);
            float angle = Vector3.SignedAngle(transform.forward, (currentPathTarget - transform.position).normalized, transform.up);

            //navMeshAgent.enabled = false;

            float distance = Vector3.Distance(transform.position, currentPathTarget);
            if (dot < 0)
            {
                // turn around
                if (rigidbody.velocity.magnitude < 1f)
                {
                    lastInputs.y -= .05f;
                }

                lastInputs.x = 1;
            }
            else
            {
                if (distance < 10 && rigidbody.velocity.magnitude > 4)
                {
                    lastInputs.w += rigidbody.velocity.magnitude + .01f;

                }
                else
                {
                    lastInputs.w = 0;
                }

                lastInputs.x = angle / 90;
                if (rigidbody.velocity.magnitude < currentMaxSpeed)
                {
                    lastInputs.y += .05f;
                }
                else if (rigidbody.velocity.magnitude > currentMaxSpeed)
                {
                    if (lastInputs.y > 0)
                    {
                        lastInputs.y = 0;
                    }
                    lastInputs.y -= .05f;
                }
            }




            if (distance < 5)
            {
                if (pathPassed < currentPath.corners.Length)
                {
                    pathPassed++;
                    currentPathTarget = currentPath.corners[pathPassed];

                }
            }

            //Debug.Log("Dot is: " + dot + "Distance is: " + distance + "Angle: " + angle + "PathLength Left: " + pathPassed);

        }
    }
}
