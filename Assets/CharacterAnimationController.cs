using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    Vector3 lastPosition;

    CharacterBrain characterBrain;

    bool isWalking;

    int average = 5;
    Vector3[] PrevPos;
    Vector3 NewPos;

    Quaternion PrevRot;
    Quaternion NewRot;

    //public Vector3 ObjVelocity;
    public Vector3 ObjRotation;

    private void Start()
    {
        characterBrain = GetComponent<CharacterBrain>();
        lastPosition = transform.position;
        PrevPos = new Vector3[average];
        for (int i = 0; i < average; i++)
        {
            PrevPos[i] = transform.position;
        }
        PrevRot = transform.rotation;
        characterBrain.OnSitDown += SetSitting;
    }

    int CurrentAveragInt = 0;


    void SetSitting()
    {
        //transform.rotation = 
        animator.applyRootMotion = true;
        animator.SetTrigger("SitDown");
        
    }

    Vector3 AverageVelocity()
    {
        Vector3 integral = Vector3.zero;
        for (int i = 0; i < average; i++)
        {
            integral += PrevPos[i];
        }
        return integral / average;
    }

    private void FixedUpdate()
    {
        //GetVelocity();
        SetWalking(GetVelocity());


    }

    private Vector3 GetVelocity()
    {
        Vector3 ObjVelocity = transform.InverseTransformDirection((transform.position - AverageVelocity()) / Time.fixedDeltaTime);


        Vector3 angularVelocity = (transform.rotation.eulerAngles - PrevRot.eulerAngles) / Time.fixedDeltaTime;
        //Debug.Log("Vlinear" + ObjVelocity+ "VRot" + angularVelocity);

        PrevPos[CurrentAveragInt] = transform.position;

        CurrentAveragInt++;
        if (CurrentAveragInt > average - 1)
        {
            CurrentAveragInt = 0;
        }
        PrevRot = transform.rotation;
        return ObjVelocity;
    }

    void SetWalking(Vector3 zVelocity)
    {
        if (zVelocity.z >0.3f)
        {

            Debug.Log(zVelocity);
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isMoving", true);


            }
            
        }
        else
        {


            if (isWalking)
            {
                isWalking = false;
                animator.SetBool("isMoving", false);


            }
            //isWalking = false;

        }
        lastPosition = transform.position;
    }
}
