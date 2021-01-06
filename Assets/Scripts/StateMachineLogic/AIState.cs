using UnityEngine;
using CharacterLogic;
using System;

namespace StateMachineLogic
{
   
    [Serializable]
    public abstract class AIState
    {
        public virtual event Action OnStateFinished;
        public virtual event Action<AIState> OnStateFailed;

        public JobTime jobTime { get; private set; }
        public int HasFailedBefore { get; private set; }


        protected Transform transform;       
        protected Character character;

        public bool preReqAreMet = true;

        public AIState preReqState { get; private set; }

        private bool isPreReq;

        public AIState(Character _character)
        {
            character = _character;
            HasFailedBefore = 0;
        }

        public virtual void StateFalied()
        {
            OnStateFailed?.Invoke(this);
            HasFailedBefore++;
        }

        public void PreReqFailed(AIState state)
        {
            StateFalied();
        }

        public void SetPreReq(AIState _preReqState)
        {
            preReqState = _preReqState;
            preReqState.isPreReq = true;
            preReqState.OnStateFinished += PreReqMet;
            preReqState.OnStateFailed += PreReqFailed;
            preReqAreMet = false;
        }

        public string GetCurrentStateString()
        {
            if(preReqState != null)
            {
                return preReqState.ToString();
            }
            else
            {
                return this.ToString();
            }
        }

        void PreReqMet()
        {
            preReqState.OnStateFinished -= PreReqMet;
            preReqState.OnStateFailed -= PreReqFailed;
            preReqState = null;
            preReqAreMet = true;
        }

        public virtual void Tick(float tickTime)
        {
            if(!preReqAreMet)
            {
                preReqState.Tick(tickTime);

            }
            else if(jobTime != null)
            {
                jobTime.DoWork(tickTime);
            }
        }

        public virtual void LeaveState()
        {
            OnStateFinished?.Invoke();
        }
        
        public virtual void EnterState()
        {
            if (!preReqAreMet)
            {
                preReqState.EnterState();
            }
        }

        public virtual float GetCompletionFactor()
        {
            if(!preReqAreMet)
            {
                return preReqState.GetCompletionFactor();
            }
            else
            {
                if (jobTime != null)
                {
                    return jobTime.CompletionFactor();
                }
                else
                {
                    
                    return 0;
                }
            }

        }


        public void SetJobTime(JobTime jt)
        {
            jobTime = jt;
            jobTime.OnJobComplete += LeaveState;
        }
    }
}
