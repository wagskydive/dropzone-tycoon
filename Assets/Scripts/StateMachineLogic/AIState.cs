using UnityEngine;
using CharacterLogic;
using System;

namespace StateMachineLogic
{
   

    public abstract class AIState
    {
        public virtual event Action OnStateFinished;
        public virtual event Action<AIState> OnStateFailed;

        public JobTime jobTime { get; private set; }
        public int HasFailedBefore { get; private set; }


        protected Transform transform;       
        protected Character character;

        AIState preReqState;

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

        public void SetPreReq(AIState state)
        {
            preReqState = state;
        }

        void PreReqMet()
        {
            preReqState.OnStateFinished -= PreReqMet;
            preReqState.OnStateFailed -= PreReqFailed;
            preReqState = null;
        }

        public virtual void Tick(float tickTime)
        {
            if(preReqState != null)
            {
                preReqState.Tick(tickTime);
                preReqState.OnStateFinished += PreReqMet;
                preReqState.OnStateFailed += PreReqFailed;
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

        public virtual float GetCompletionFactor()
        {
            if(jobTime != null)
            {
                return jobTime.CompletionFactor();
            }
            else
            {
                return 1;
            }
        }


        public void SetJobTime(JobTime jt)
        {
            jobTime = jt;
            jobTime.OnJobComplete += LeaveState;
        }

        public string GetCompletionInfo()
        {
            string info = (GetCompletionFactor()*100).ToString()+" '%' complete";
            if(preReqState != null)
            {
                string preReq = "Pre Req: " + preReqState.ToString() + " not met, " + preReqState.GetCompletionInfo();
                info = info + "\n"+preReq;
            }
            return info;
        }
    }
}
