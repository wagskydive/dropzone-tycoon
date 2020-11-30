using System;
using System.Collections;
using System.Collections.Generic;
using StatsLogic;


namespace SkillsLogic
{


    public class Skill
    {
        public Stat Training { get; internal set; }

        public string[] RequiredSkills { get; internal set; }

        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public Dictionary<string, float> Effectors { get; internal set; }

        public Skill(string skillName, string description = null, Dictionary<string, float> effectors= null)
        {
            Name = skillName;
            if(description == null)
            {
                Description = "No Description";
            }
            else
            {
                Description = description;
            }

            Training = StatsHandler.CreateSingleStat(skillName + "_training");


            Effectors =effectors;

        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetRequieredSkills(string[] skill)
        {
            RequiredSkills = skill;
        }

        public void SetEffectors(Dictionary<string, float> effectors)
        {
            Effectors = effectors;
        }
    }
}
