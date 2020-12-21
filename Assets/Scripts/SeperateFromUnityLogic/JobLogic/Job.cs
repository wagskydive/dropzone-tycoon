using System;



namespace JobLogic
{
    public class Job
    {
        public event Action<Job> OnJobStarted;

        public event Action<Job> OnJobCompleted;


        public string JobName { get; private set; }

        public float CompletedFactor { get; internal set; }

        public float CompletionTime { get; private set; }

        public float CurrentSpeedFactor { get; private set; }

        public bool SelfComplete { get; private set; }

        float lastWorkTime = 0;
        bool hasStarted;


        public Job(string jobName, float completionTime, float speedFactor = 1)
        {
            JobName = jobName;
            CompletionTime = completionTime;
            CurrentSpeedFactor = speedFactor;
            CompletionTime = 0;
        }

        public Job(string jobName)
        {
            JobName = jobName;
            SelfComplete = true;
        }

        void StartJob(float startTime)
        {
            lastWorkTime = startTime;
            hasStarted = true;
            OnJobStarted?.Invoke(this);
        }

        void Complete()
        {
            OnJobCompleted?.Invoke(this);
        }

        public void ManualComplete()
        {
            Complete();
        }

        public void WorkStep(float currentTime)
        {
            if (SelfComplete)
            { return; }
            if (!hasStarted)
            {
                StartJob(currentTime);
            }

            float timeStep = currentTime - lastWorkTime;

            CompletedFactor += CompletionTime / timeStep;

            if(CompletedFactor >= 1)
            {
                Complete();
            }

            lastWorkTime = currentTime;

        }
    }
}


