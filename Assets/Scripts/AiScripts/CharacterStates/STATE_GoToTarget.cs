using System;
using CharacterLogic;
using StateMachineLogic;
using UnityEngine;


public class STATE_GoToTarget : AIState
{

    Vector3 targetPosition;
    float startDistance;
    float reachedDistance;

    CharacterBrain characterBrain;

    public STATE_GoToTarget(CharacterBrain brain, Vector3 _position, float _reachedDistance) : base(brain.character)
    {
        targetPosition = _position;
        characterBrain = brain;

        reachedDistance = _reachedDistance;
        startDistance = Vector3.Distance(characterBrain.transform.position, targetPosition);
    }

    bool TargetReached(float distanceLeft)
    {
        return distanceLeft < reachedDistance;
    }

    public override float GetCompletionFactor()
    {
        float baseFactor = base.GetCompletionFactor();
        float distance = Vector3.Distance(characterBrain.transform.position, targetPosition);
        if (TargetReached(distance))
        {
            LeaveState();
        }
        float factor = (distance - reachedDistance)/ (startDistance - reachedDistance);
        //Debug.Log("goto completion factor: " + factor);

        if (baseFactor == 0)
        {

            return factor;
        }
        else
        {
            return baseFactor;
        }
    }



    public override void EnterState()
    {
        characterBrain.SetTarget(targetPosition);
        base.EnterState();
        
    }

    //public override void Tick(float tickTime)
    //{
    //    if (TargetReached())
    //    {
    //        OnStateFinished?.Invoke();
    //    }
    //}
}

