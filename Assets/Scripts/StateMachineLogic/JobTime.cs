using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLogic
{
    public class JobTime
    {
        public event Action OnJobComplete;

        float defaultCompletionTime;

        float defaultTimeLeft;

        public float CompletionFactor()
        {
            return (float)Math.Round(defaultTimeLeft/ defaultCompletionTime, 2);
        }

        public JobTime(float time)
        {
            defaultCompletionTime = time;
            defaultTimeLeft = time;
        }

        public void DoWork(float tickTime)
        {
            defaultTimeLeft -= tickTime;
            if(defaultTimeLeft <= 0)
            {
                OnJobComplete?.Invoke();
            }
        }

    }
}
