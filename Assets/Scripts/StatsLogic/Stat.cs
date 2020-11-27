using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;

namespace StatsLogic
{
    public class Stat
    {
        public event Action<float> OnValueChanged;

        public event Action<bool> OnThreshholdReached;

        public event Action<float> OnThreshholdSet;

        public string Name { get; internal set; }

        internal float Value;

        public float Threshhold { get; internal set; }

        internal float TimeStamp;

        public float ValueChangePerSecond { get; internal set; }

        public bool HasThreshhold { get; internal set; }

        public bool ThreshholdReached { get; internal set; }

        internal bool biggerThan;

        internal Stat(string name)
        {
            Name = name;
            Value = 0.5f;
            HasThreshhold = false;
        }

        internal void InvokeOnValueChangedEvent()
        {
            OnValueChanged?.Invoke(Value);
        }

        internal void InvokeTreshHoldReached(bool positive)
        {
            OnThreshholdReached?.Invoke(positive);
        }

        internal void InvokeTreshHoldSet()
        {
            OnValueChanged?.Invoke(Threshhold);
        }

    }
}
