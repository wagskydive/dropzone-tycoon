using System;
using CharacterLogic;
using StateMachineLogic;
using UnityEngine;


public class GoToTarget : AIState
{
    public override event Action OnStateFinished;


    Transform target;
    float startDistance;


    public GoToTarget(CharacterBrain brain, Transform _target) : base(brain.character)
    {
        target = _target;
        brain.SetTarget(_target.position);
        startDistance = Vector3.Distance(transform.position, target.position);
    }

    bool TargetReached()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < .5f;
    }

    public override float GetCompletionFactor()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance / startDistance;
    }


    public override void Tick(float tickTime)
    {
        if (TargetReached())
        {
            OnStateFinished?.Invoke();
        }
    }
}

