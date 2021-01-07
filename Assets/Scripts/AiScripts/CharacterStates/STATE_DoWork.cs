using CharacterLogic;
using StateMachineLogic;
using UnityEngine;


public class STATE_DoWork : AIState
{


    public STATE_DoWork(CharacterBrain brain, float defaultCompletionTime, Transform target = null) : base(brain.character)
    {
        SetJobTime(new JobTime(defaultCompletionTime));
        if(Vector3.Distance(target.position, brain.transform.position) > .5f)
        {
            SetPreReq(new STATE_GoToTarget(brain, target, .5f));
        }
    }



}

