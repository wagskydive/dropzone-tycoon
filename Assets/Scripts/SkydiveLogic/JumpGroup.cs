using System.Collections.Generic;

namespace SkydiveLogic
{
    public enum JumpType
    {
        TR1,
        FS,
        FF,
        TR2,
        AFF,
        Tandem,
        WS,
    }

    [System.Serializable]
    public class JumpGroup
    {
        public int ExitAltitude;
        public JumpType jumpType;
        public List<Slot> members;
    }
}
