using System;
using CharacterLogic;
using StateMachineLogic;
using UnityEngine;


public class STATE_GoToTarget : AIState
{

    Transform target;
    float startDistance;
    float reachedDistance;

    CharacterBrain characterBrain;

    public STATE_GoToTarget(CharacterBrain brain, Transform _target, float _reachedDistance) : base(brain.character)
    {
        target = _target;
        characterBrain = brain;

        reachedDistance = _reachedDistance;
        startDistance = Vector3.Distance(characterBrain.transform.position, target.position);
    }

    bool TargetReached(float distanceLeft)
    {
        return distanceLeft < reachedDistance;
    }

    Vector3 lastTargetPosition;
    public override float GetCompletionFactor()
    {
        float baseFactor = base.GetCompletionFactor();
        if(lastTargetPosition != target.position)
        {
            characterBrain.SetTarget(target.position);
        }
        float distance = Vector3.Distance(characterBrain.transform.position, target.position);
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
        lastTargetPosition = target.position;
        characterBrain.SetTarget(target.position);
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

