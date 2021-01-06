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


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Character character = DummyObjects.ProvideDummyCharacter("Dummy 1");
        SetCharacter(character);
        characterObject = GetComponent<CharacterObject>();
        characterObject.character = character;
        currentState = new STATE_Idle(character);

    }

    public void TakeSeat(SelectableObject seatObject, int seatIndex = 0)
    {
        isSeated = true;
        characterObject.SetAllSelectable();
        characterObject.isCurrentlySelectable = false;
        if (characterObject.isSelected)
        {
            characterObject.DeselectObject();
            seatObject.SelectObject();
        }
        OperatableObject operatableObject = (OperatableObject)seatObject;
        if (operatableObject != null&& seatIndex == 0)
        {
            operatableObject = (OperatableObject)seatObject;
            operatableObject.SetOperator(this);
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
        if(currentState != null)
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
            if(currentState != null)
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

