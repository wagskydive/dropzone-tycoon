using CharacterLogic;
using StateMachineLogic;
using UnityEngine;


public class DoWork : AIState
{


    public DoWork(CharacterBrain brain, float defaultCompletionTime, Transform target = null) : base(brain.character)
    {
        SetJobTime(new JobTime(defaultCompletionTime));
        if(Vector3.Distance(target.position, brain.transform.position) > .5f)
        {
            SetPreReq(new GoToTarget(brain, target));
        }
    }



}

