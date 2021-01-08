using CharacterLogic;
using StateMachineLogic;


public class STATE_WaitSeconds : AIState
{
    public STATE_WaitSeconds(Character character, float seconds) : base(character)
    {
        SetJobTime(new JobTime(seconds));
    }
}

