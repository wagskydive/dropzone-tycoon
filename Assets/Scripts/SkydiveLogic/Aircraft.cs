using System;
using System.Linq;
using CharacterLogic;

namespace SkydiveLogic
{
    [Serializable]
    public class Aircraft
    {
        public string typeName;
        public event Action<Load> OnAircraftLoaded;
        Character currentPilot;
        public int maxSlots;
        public Load CurrentLoad;

        public void SubscribeToCurrentLoad()
        {
            CurrentLoad.OnJumpersLoaded += SendLoadedEvent;
        }
        void SendLoadedEvent()
        {
            OnAircraftLoaded?.Invoke(CurrentLoad);
        }
    }
}
