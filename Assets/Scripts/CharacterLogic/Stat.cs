using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLogic
{
    public class Stat
    {
        public event Action<float> OnValueChanged;
        public event Action<string> OnThreshholdReached;

        public string Name { get; private set; }

        public float Value { get; private set; }

        internal float Threshhold { get; private set; }

        internal float TimeStamp { get; private set; }

        internal float ValueChangePerSecond { get; private set; }

        internal bool HasThreshhold { get; private set; }

        bool biggerThan;

        internal Stat(string name)
        {
            Name = name;
            Value = 0;
            HasThreshhold = false;
        }

        internal void SetStatValue(float newValue, float timeStamp = 0)
        {
            Value = newValue;
            if(timeStamp != 0)
            {
                TimeStamp = timeStamp;
            }
            OnValueChanged?.Invoke(Value);
        }

        internal void SetCurrentMultiplier(float valueChangePerSecond, float currentTime)
        {
            ValueChangePerSecond = valueChangePerSecond;
            Value = GetCurrentValue(currentTime);
            TimeStamp = currentTime;
        }

        internal void SetTreshhold(float treshhold)
        {
            Threshhold = treshhold;
            EvaluateBiggerThan();
            HasThreshhold = true;
        }

        internal void RemoveTreshhold()
        {
            HasThreshhold = false;
        }


        internal float GetCurrentValue(float currentTime)
        {
            Value += (currentTime - TimeStamp) * ValueChangePerSecond;

            return Value;
        }

        internal void Tick(float currentTime)
        {
            GetCurrentValue(currentTime);
            if (HasThreshhold)
            {
                if (biggerThan && Value <= Threshhold)
                {
                    OnThreshholdReached?.Invoke(Name);
                }

                if (!biggerThan && Value >= Threshhold)
                {
                    OnThreshholdReached?.Invoke(Name);
                }

                EvaluateBiggerThan();
            }
            OnValueChanged?.Invoke(Value);
        }

        private void EvaluateBiggerThan()
        {
            if (Value > Threshhold)
            {
                biggerThan = true;
            }
            else
            {
                biggerThan = false;
            }
        }
    }
}
