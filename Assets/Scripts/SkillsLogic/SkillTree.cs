using SkillsLogic;
using DataLogic;
using System.Collections.Generic;
using System.Linq;

public class SkillTree
{
    public int GetHiarchyLevelOfSkill(List<Skill> skills, string skillName)
    {
        int level = 0;
        int skillIndex = skills.FindIndex(x => x.Name == skillName);

        Skill sk = skills[skillIndex];
        bool hasReq = sk.RequieredSkills.Any();

        if (hasReq)
        {
            int highestLevel = 1;
            for (int i = 0; i < sk.RequieredSkills.Length; i++)
            {
                
                string reqSkill = skills[skillIndex].RequieredSkills[i];

                highestLevel =  DataChecks.GetMax(GetHiarchyLevelOfSkill(skills,reqSkill) +1, highestLevel);
            }
        }
        return level;        
    }



    

}
