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

    public STATE_TakeSeat(CharacterBrain _brain, SelectableObject _seatObject, int _seatIndex = 0, bool needToOperate = false) : base(_brain.character)
    {
        brain = _brain;
        seatObject = _seatObject;
        seatIndex = _seatIndex;
        if (!needToOperate && seatObject.GetType() == typeof(OperatableObject))
        {
            seatIndex = 1;
        }
        if (seatObject.seats.Any())
        {
            if (seatIndex == 0)
            {
                if (seatObject.IsSeatFree(seatIndex))
                {
                    seatTransform = seatObject.seats[0];
                }
                else if (!needToOperate)
                {
                    seatIndex = seatObject.FindFreeSeat();
                    if (seatIndex != -1)
                    {
                        seatTransform = seatObject.seats[seatIndex];
                    }
                }
                else
                {
                    seatTransform = _seatObject.transform;
                }

            }

            else
            {
                if (seatObject.IsSeatFree(seatIndex))
                {
                    seatTransform = seatObject.seats[seatIndex];
                }
                else
                {
                    seatIndex = seatObject.FindFreeSeat(true);
                    if (seatIndex != -1)
                    {
                        seatTransform = seatObject.seats[seatIndex];
                    }
                    else
                    {
                        seatTransform = _seatObject.transform;
                    }
                }

            }
        }
        else
        {
            seatTransform = _seatObject.transform;
        }




        if (Vector3.Distance(seatTransform.position, brain.transform.position) > 2)
        {
            STATE_GoToTarget goToTarget = new STATE_GoToTarget(brain, seatTransform, 2);
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
        seatObject.SeatOccupationBinaryNumber += (int)Mathf.Pow(seatIndex + 1, seatObject.seats.Length);
        LeaveState();
    }


}
