using System;
using System.Collections;
using System.Collections.Generic;
using StatsLogic;


namespace SkillsLogic
{


    public class Skill
    {
        public Stat Training { get; internal set; }

        public int[] RequiredSkills { get; internal set; }

        public int[] IsRequirementOf { get; internal set; }

        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public Dictionary<string, float> Effectors { get; internal set; }

        public bool IsRoot()
        {
            bool isRoot = true;
            if(RequiredSkills != null && RequiredSkills.Length > 0)
            {
                isRoot = false;
            }
            return isRoot;
        }

        public bool IsLeaf()
        {
            bool isLeaf = true;
            if (IsRequirementOf != null && IsRequirementOf.Length > 0)
            {
                isLeaf = false;
            }
            return isLeaf;
        }



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

        public void SetName(string newName)
        {
            Name = newName;        
        }



        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetRequieredSkills(int[] skillIndexes)
        {
            RequiredSkills = skillIndexes;
        }


        public void SetEffectors(Dictionary<string, float> effectors)
        {
            Effectors = effectors;
        }
    }
}
