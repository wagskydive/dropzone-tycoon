using SkillsLogic;
using DataLogic;
using System.Collections.Generic;
using System.Linq;

public static class SkillTreeDataHandler
{
    public static int GetHiarchyLevelOfSkill(List<Skill> skills, string skillName)
    {
        int level = 0;
        int skillIndex = skills.FindIndex(x => x.Name == skillName);

        Skill skillToCheck = skills[skillIndex];
        
        if (skillToCheck.RequiredSkills.Any())
        {
            int highestLevel = 1;
            for (int i = 0; i < skillToCheck.RequiredSkills.Length; i++)
            {
                
                int reqSkill = skillToCheck.RequiredSkills[i];

                int highestLevelOfRequirement =  DataChecks.GetMax(GetHiarchyLevelOfSkill(skills,skills[reqSkill].Name) +1, highestLevel);
                highestLevel = DataChecks.GetMax(highestLevel, highestLevelOfRequirement);

            }
        }
        return level;        
    }


    static bool StringArrayHasString(string[] array, string subject)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if(array[i] == subject)
            {
                return true;
            }

        }
        return false;
    }
    

    static bool IntArrayHasInt(int[] array, int subject)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == subject)
            {
                return true;
            }

        }
        return false;
    }

    public static bool ValidateRequirement(Skill[] allSkills, string requiredSkillToCheck, string subjectSkill)
    {
        int[] invalidReqs = InValidRequirememts(allSkills, subjectSkill);
        if(invalidReqs == null || invalidReqs.Length == 0)
        {
            return true;
        }


        if (invalidReqs.Contains(FindIndexOfSkillByNameInSkillArray(allSkills, requiredSkillToCheck)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }





    public static int[] GetAllSkillsThatHaveRequirement(Skill[] allSkills, int requirement)
    {
        List<int> output = new List<int>();
        for (int i = 0; i > allSkills.Length; i++)
        {
            if (allSkills[i].RequiredSkills != null)
            {
                for (int j = 0; j > allSkills[i].RequiredSkills.Length; i++)
                {
                    if (allSkills[i].RequiredSkills[j] == requirement)
                    {
                        output.Add(i);
                        break;
                    }
                }
            }
        }
        if(output.Count == 0)
        {
            return null;
        }
        return output.ToArray();
    }

    public static int[] InValidRequirememts(Skill[] allSkills, string subject)
    {
        int[] downstream = GetAllDownstreamSkills(allSkills, FindIndexOfSkillByNameInSkillArray(allSkills, subject));
        int[] upstream = GetAllUpstreamSkills(allSkills, FindIndexOfSkillByNameInSkillArray(allSkills, subject));

        return ConcatIntArrays(downstream, upstream);
    }


    public static int[] ValidRequirememts(Skill[] allSkills,string subject)
    {
        List<int> validReqs = new List<int>();

        int[] invalidRequirements = InValidRequirememts(allSkills, subject);

        if(invalidRequirements == null)
        {
            for (int i = 0; i < allSkills.Length; i++)
            {
                validReqs.Add(i);
            }
            return validReqs.ToArray();
        }
        else
        {
            for (int i = 0; i < allSkills.Length; i++)
            {
                if (!invalidRequirements.Contains(i))
                {
                    validReqs.Add(i);
                }
            }

            return validReqs.ToArray();
        }

        


    }


    public static int[] ConcatIntArrays(int[] first, int[] second)
    {
        if (first == null)
        {
            return second;
        }
        if (second == null)
        {
            return first;
        }

        List<int> list = new List<int>(first);
        list.AddRange(second);
        return list.ToArray();
    }

    public static int FindIndexOfSkillByNameInSkillArray(Skill[] skills, string skillName)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].Name == skillName)
            {
                return i;
            }
        }
        return -1;
    }


    public static int[] GetAllDownstreamSkills(Skill[] allSkills, int subject)
    {
        int[] skillsWithMeAsReq = GetAllSkillsThatHaveRequirement(allSkills, subject);
        List<int> downstreamList = new List<int>();
        if(skillsWithMeAsReq != null)
        {
            downstreamList.AddRange(skillsWithMeAsReq);
            for (int i = 0; i < skillsWithMeAsReq.Length; i++)
            {
                int[] downStream = GetAllDownstreamSkills(allSkills, skillsWithMeAsReq[i]);
                if(downStream != null && downStream.Length > 0)
                {
                    downstreamList.AddRange(downStream);
                }
            }
        }
        if(downstreamList.Count == 0)
        {
            return null;
        }
        return downstreamList.ToArray();

    }

    //Skills that are requirements of subject recursive
    public static int[] GetAllUpstreamSkills(Skill[] allSkills, int subject)
    {
        int[] subjectRequirements = allSkills[subject].RequiredSkills;
        List<int> upstreamList = new List<int>();

        if (subjectRequirements != null && subjectRequirements.Length > 0)
        {

            upstreamList.AddRange(subjectRequirements);
            for (int i = 0; i < subjectRequirements.Length; i++)
            {
                int[] upstream = GetAllUpstreamSkills(allSkills, subjectRequirements[i]);
                if (upstream != null && upstream.Length > 0)
                {
                    upstreamList.AddRange(upstream);
                }
            }
        }

        if (upstreamList.Count == 0)
        {
            return null;
        }

        return upstreamList.ToArray();
    }
}
