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
    Queue<AIState> stateQueue = new Queue<AIState>();

    Queue<AIState> failedStateQueue = new Queue<AIState>();

    
    public Character character;

    NavMeshAgent navMeshAgent;

    public event Action<AIState> OnStateSet;

    public AIState currentState;

    bool isActive;

    bool retryFailedStates = false;






    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if(character == null)
        {
            SetCharacter(DummyObjects.ProvideDummyCharacter());
        }

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
        currentState.OnStateFinished += StateFinished;

        OnStateSet?.Invoke(state);
    }

    private void StateFinished()
    {
        currentState.OnStateFinished -= StateFinished;
        if (stateQueue.Any())
        {
            SetState(stateQueue.Dequeue());
        }
    }
    
    private void StateFailed(AIState state)
    {
        EnqueueFailedState(state);
        if (retryFailedStates && failedStateQueue.Any())
        {
            SetState(failedStateQueue.Dequeue());
        }
    }

    public override string ToString()
    {
        return currentState != null ? currentState.ToString() : "None";
    }

    public float GetCurrentJobLeft()
    {

        if (currentState != null &&currentState.jobTime != null)
        {
            return currentState.jobTime.CompletionFactor();
        }
        else
        {
            return 0;
        }       
    }

    public string GetCurrentCompletionString()
    {
        return currentState.GetCompletionInfo();
    }


    private void Update()
    {
        if (isActive)
        {
            if(currentState.GetType() == typeof(Idle))
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

