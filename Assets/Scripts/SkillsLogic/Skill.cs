using System;
using System.Collections;
using System.Collections.Generic;
using StatsLogic;


namespace SkillsLogic
{


    public class Skill
    {
        public Stat training { get; internal set; }

        public string[] RequiredSkills { get; internal set; }

        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public Dictionary<string, float> Effectors;

        public Skill(string skillName, string description = null)
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

            training = StatsHandler.CreateSingleStat(skillName + "_training");
            //RequieredSkills = new List<string>();

            Effectors = new Dictionary<string, float>();

            Effectors.Add("Test effector 1", .1f);
            Effectors.Add("Test effector 2", .2f);
        }

        public void SetRequieredSkills(string[] skill)
        {
            RequiredSkills = skill;
        }
    }
}
