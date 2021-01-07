using System;
using UnityEngine;
using UnityEngine.AI;
using StateMachineLogic;
using CharacterLogic;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBrain : MonoBehaviour
{
    [SerializeField]
    Queue<AIState> stateQueue = new Queue<AIState>();
    [SerializeField]
    Queue<AIState> failedStateQueue = new Queue<AIState>();

    [SerializeField]
    public Character character;
    CharacterObject characterObject;

    public NavMeshAgent navMeshAgent;

    public event Action<AIState> OnStateSet;
    [SerializeField]
    public AIState currentState;

    bool isActive = true;

    bool retryFailedStates = false;

    bool isSeated;

    bool isOperatingItem;
    SelectableObject seatObject;

    Transform fakeTarget;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Character character = DummyObjects.ProvideDummyCharacter("Dummy 1");
        SetCharacter(character);
        characterObject = GetComponent<CharacterObject>();
        characterObject.character = character;
        currentState = new STATE_Idle(character);

    }

    public void TakeSeat(SelectableObject _seatObject, int seatIndex = 0)
    {
        isSeated = true;
        seatObject = _seatObject;
        //characterObject.SetAllSelectable();
        //characterObject.isCurrentlySelectable = false;

        OperatableObject operatableObject = (OperatableObject)seatObject;
        if (operatableObject != null && seatIndex == 0)
        {
            isOperatingItem = true;
            operatableObject = (OperatableObject)seatObject;
            operatableObject.SetOperator(characterObject);
        }

    }
    public void GoTo(Vector3 position)
    {
        if(fakeTarget == null)
        {
            fakeTarget = new GameObject(character.CharacterName + "target").transform;
        }
        fakeTarget.position = position;
        GoTo(fakeTarget);
    }

    public void GoTo(Transform target)
    {
        if (isOperatingItem && seatObject != null)
        {
            if (seatObject.GetType() == typeof(VehicleObject))
            {
                STATE_DriveGoToTarget driveGoToTarget = new STATE_DriveGoToTarget(this, (VehicleObject)seatObject, target, 10);
                EnqueueState(driveGoToTarget);
            }

        }
        else
        {
            STATE_GoToTarget goToTarget = new STATE_GoToTarget(this, target, 2);
            EnqueueState(goToTarget);
        }
    }

    public void OperateItem(SelectableObject operatingItem)
    {
        isOperatingItem = true;
    }


    public void SetCharacter(Character _character)
    {
        character = _character;
    }


    public void SetActive(bool active)
    {
        isActive = active;
    }

    internal void SetTarget(Vector3 position)
    {
        navMeshAgent.SetDestination(position);

    }


    private void SetState(AIState state)
    {
        if (currentState != null)
        {
            currentState.LeaveState();
        }
        currentState = state;
        currentState.EnterState();
        currentState.OnStateFinished += StateFinished;
        currentState.OnStateFailed += StateFailed;

        OnStateSet?.Invoke(state);
    }

    private void StateFinished()
    {
        currentState.OnStateFinished -= StateFinished;
        currentState = new STATE_Idle(character);

    }

    private void StateFailed(AIState state)
    {
        EnqueueFailedState(state);
        currentState = new STATE_Idle(character);
    }

    public override string ToString()
    {
        return currentState != null ? currentState.ToString() : "None";
    }

    public float GetCurrentStateFactor()
    {

        if (currentState != null)
        {

            return currentState.GetCompletionFactor();

        }
        else
        {
            return 0;
        }
    }



    private void Update()
    {
        if (isActive)
        {
            if (currentState != null)
            {
                if (currentState.GetType() == typeof(STATE_Idle))
                {

                    if (retryFailedStates && failedStateQueue.Any())
                    {
                        SetState(failedStateQueue.Dequeue());
                    }

                    else if (stateQueue.Any())
                    {

                        SetState(stateQueue.Dequeue());
                    }
                }
                currentState.GetCompletionFactor();
            }



        }
    }

    public void EnqueueState(AIState state)
    {
        stateQueue.Enqueue(state);
    }
    public void EnqueueFailedState(AIState state)
    {
        failedStateQueue.Enqueue(state);

    }

}

