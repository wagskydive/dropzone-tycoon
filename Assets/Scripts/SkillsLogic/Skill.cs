using System.Collections;
using System.Collections.Generic;
using StatsLogic;


namespace SkillsLogic
{


    public class Skill
    {
        public Stat training { get; internal set; }

        public string[] RequieredSkills { get; internal set; }

        public string Name { get; internal set; }

        Dictionary<string, float> Effectors;

        public Skill(string skillName)
        {
            training = StatsHandler.CreateSingleStat(skillName+"_training");
        }


    }
}
