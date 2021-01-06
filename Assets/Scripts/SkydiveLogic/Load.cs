using System.Collections.Generic;
using System.Linq;
using System;

namespace SkydiveLogic
{

    [Serializable]
    public class Load
    {
        public event Action OnJumpersLoaded;
        public int maxSlots;

        public List<JumpGroup> jumpers = new List<JumpGroup>();

        public void JumpersLoaded()
        {
            SetLoadOrder();
            OnJumpersLoaded?.Invoke();
        }

        int JumpersOnLoad()
        {
            int output = 0;
            for (int i = 0; i < jumpers.Count; i++)
            {
                output += jumpers[i].members.Count;
            }
            return output;
        }

        bool GroupFits(JumpGroup group)
        {
            return group.members.Count <= maxSlots - JumpersOnLoad();
        }

        public void AddGroupToLoad(JumpGroup group)
        {
            if (GroupFits(group))
            {
                jumpers.Add(group);
            }
        }


        void SetLoadOrder()
        {
            jumpers.OrderBy(x => x.ExitAltitude);
            jumpers.OrderBy(x => x.jumpType);

        }
    }
}
