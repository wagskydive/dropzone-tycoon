using CharacterLogic;
using StateMachineLogic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class STATE_TakeSeat : AIState
{

    CharacterBrain brain;

    SelectableObject seatObject;
    Transform seatTransform;
    int seatIndex = 0;

    public STATE_TakeSeat(CharacterBrain _brain, SelectableObject _seatObject, int _seatIndex = 0) : base(_brain.character)
    {
        brain = _brain;
        seatObject = _seatObject;

        if (seatObject.seats.Any())
        {
            if (_seatIndex == 0)
            {
                seatTransform = seatObject.seats[0];
            }
            else if (seatObject.seats[_seatIndex] == null)
            {
                seatTransform = _seatObject.transform;
            }
            else
            {
                seatTransform = seatObject.seats[_seatIndex];
            }
        }
        else
        {
            seatTransform = _seatObject.transform;
        }

        seatIndex = _seatIndex;


        if (Vector3.Distance(seatTransform.position, brain.transform.position) > 2)
        {
            STATE_GoToTarget goToTarget = new STATE_GoToTarget(brain, seatTransform.position, 2);
            goToTarget.OnStateFinished += SitDown;
            SetPreReq(goToTarget);
        }
        
        SetJobTime(new JobTime(.2f));
        
    }



    void SitDown()
    {
        brain.gameObject.GetComponent<Collider>().enabled = false;
        brain.navMeshAgent.enabled = false;
        brain.transform.SetParent(seatObject.transform);
        brain.transform.position = seatTransform.position;
        
        brain.TakeSeat(seatObject, seatIndex);

        LeaveState();
    }


}
