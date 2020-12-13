using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic
{
    public class JobsManager
    {
        public event Action<Job> OnJobCompleted;

        List<Job> AvailableJobs;
        List<Job> WorkingJobs;

        public JobsManager()
        {
            AvailableJobs = new List<Job>();
            WorkingJobs = new List<Job>();
        }

        public bool WorkAllJobs(float currentTime)
        {
            if (WorkingJobs.Any())
            {
                for (int i = 0; i < WorkingJobs.Count; i++)
                {
                    WorkingJobs[i].WorkStep(currentTime);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteJobFromAvailable(Job job)
        {
            if (AvailableJobs.Remove(job))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool RemoveJobFromWorking(Job job)
        {
            if (WorkingJobs.Remove(job))
            {
                return true;
            }
            else
            {
                return false;
            }
        }




        public void AddJobToAvailable(Job job)
        {
            AvailableJobs.Add(job);
        }

        public void AddJobToWorking(Job job)
        {
            WorkingJobs.Add(job);
        }


    }
}
