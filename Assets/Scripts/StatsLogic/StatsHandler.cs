using System;

namespace StatsLogic
{
    public static class StatsHandler
    {
        public static event Action<string, float> OnValueChanged;
        public static event Action<string> OnThreshholdReached;

        public static Stat[] CreateStats(string[] statNames)
        {
            Stat[] stats = new Stat[statNames.Length];
            for (int i = 0; i < statNames.Length; i++)
            {
                Stat stat = new Stat(statNames[i]);
                stats[i] = stat;
            }
            return stats;
        }



        public static void SetStatValue(Stat stat, float newValue, float timeStamp = 0)
        {
            stat.Value = newValue;
            if(timeStamp != 0)
            {
                stat.TimeStamp = timeStamp;
            }
            stat.InvokeOnValueChangedEvent();
            OnValueChanged?.Invoke(stat.Name, stat.Value);
        }

        public static void SetCurrentMultiplier(Stat stat, float valueChangePerSecond, float currentTime)
        {
            stat.ValueChangePerSecond = valueChangePerSecond;
            Tick(stat,currentTime);
            
        }

        public static void SetTreshhold(Stat stat, float treshhold)
        {
            stat.Threshhold = treshhold;
            EvaluateBiggerThan(stat);
            stat.ThreshholdReached = false;
            stat.HasThreshhold = true;
            stat.InvokeTreshHoldSet();
        }

        public static void RemoveTreshhold(Stat stat)
        {
            stat.HasThreshhold = false;
        }



        public static float Tick(Stat stat, float currentTime)
        {
            float oldValue = stat.Value;

            stat.Value += (currentTime - stat.TimeStamp) * stat.ValueChangePerSecond;
            stat.TimeStamp = currentTime;
            if (stat.Value != oldValue)
            {
                stat.InvokeOnValueChangedEvent();
                OnValueChanged?.Invoke(stat.Name, stat.Value);
            }

            if (stat.HasThreshhold)
            {
                if (stat.biggerThan && stat.Value <= stat.Threshhold)
                {
                    stat.ThreshholdReached = false;
                    stat.InvokeTreshHoldReached(false);
                    OnThreshholdReached?.Invoke(stat.Name);
                }

                if (!stat.biggerThan && stat.Value >= stat.Threshhold)
                {
                    stat.ThreshholdReached = true;
                    stat.InvokeTreshHoldReached(true);
                    OnThreshholdReached?.Invoke(stat.Name);
                }

                EvaluateBiggerThan(stat);
            }

            return stat.Value;
        }

        public static void EvaluateBiggerThan(Stat stat)
        {
            if (stat.Value > stat.Threshhold)
            {
                stat.biggerThan = true;
            }
            else
            {
                stat.biggerThan = false;
            }
        }
    }
}
