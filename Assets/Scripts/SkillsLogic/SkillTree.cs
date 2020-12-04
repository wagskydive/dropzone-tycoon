using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogic;

namespace SkillsLogic
{
    public class SkillTree
    {
        public event Action<SkillTree> OnSkillTreeModified;

        public string TreeName;

        public Skill[] tree;

        public void ResetTree(Skill[] tr)
        {
            tree = tr;
            OnSkillTreeModified?.Invoke(this);
        }


        public void AddSkill(Skill skill)
        {
            if(tree == null)
            {
                tree = new Skill[1];
                tree[0] = skill;
            }
            else
            {
                Skill[] newTree = new Skill[tree.Length + 1];
                for (int i = 0; i < newTree.Length; i++)
                {
                    if(i < tree.Length)
                    {
                        newTree[i] = tree[i];
                    }
                    else
                    {
                        newTree[i] = skill;
                    }
                }
                
                tree = newTree;
            }
            OnSkillTreeModified?.Invoke(this);
        }

        public void AddRequirementToSkill(string req, string skill)
        {
            int reqIndex = FindIndexOfSkillByNameInSkillArray(req);
            int skillIndex = FindIndexOfSkillByNameInSkillArray(skill);

            int[] currentReqs = tree[skillIndex].RequiredSkills;

            if (currentReqs == null)
            {
                currentReqs = new int[1];
                currentReqs[0] = reqIndex;
                tree[skillIndex].SetRequieredSkills(currentReqs);
            }
            else
            {
                int[] newReqs = new int[currentReqs.Length + 1];
                for (int i = 0; i < newReqs.Length; i++)
                {
                    
                    if (i < currentReqs.Length)
                    {
                        if(currentReqs[i] == reqIndex)
                        {
                            return;
                        }
                        newReqs[i] = currentReqs[i];
                    }
                    else
                    {
                        newReqs[i] = reqIndex;
                    }
                }

                tree[skillIndex].SetRequieredSkills(newReqs);
            }
            OnSkillTreeModified?.Invoke(this);
        }

        public int[] GetIndecesFromNames(string[] names)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < names.Length; i++)
            {
                output.Add(FindIndexOfSkillByNameInSkillArray(names[i]));
            }
            return output.ToArray();
        }

        public int GetHiarchyLevelOfSkill(string skillName)
        {
            int level = 0;
            int skillIndex = FindIndexOfSkillByNameInSkillArray(skillName);

            Skill skillToCheck = tree[skillIndex];

            if (skillToCheck.RequiredSkills != null && skillToCheck.RequiredSkills.Length > 0)
            {
                int highestLevel = 1;
                for (int i = 0; i < skillToCheck.RequiredSkills.Length; i++)
                {

                    int reqSkill = skillToCheck.RequiredSkills[i];

                    int highestLevelOfRequirement = DataChecks.GetMax(GetHiarchyLevelOfSkill(tree[reqSkill].Name) + 1, highestLevel);
                    highestLevel = DataChecks.GetMax(highestLevel, highestLevelOfRequirement);
                }
                level = DataChecks.GetMax(level, highestLevel);
            }
            return level;
        }

        public int GetMaxHiarchyLevelOfTree()
        {
            int max = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                int hiarchyLevel = GetHiarchyLevelOfSkill(tree[i].Name);
                if (hiarchyLevel > max)
                {
                    max = hiarchyLevel;
                }               
            }
            return max;
        }


         bool StringArrayHasString(string[] array, string subject)
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


         bool IntArrayHasInt(int[] array, int subject)
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

        public  bool ValidateRequirement(string requiredSkillToCheck, string subjectSkill)
        {
            int[] invalidReqs = InValidRequirememts(subjectSkill);
            if (invalidReqs == null || invalidReqs.Length == 0)
            {
                return true;
            }


            if (invalidReqs.Contains(FindIndexOfSkillByNameInSkillArray(requiredSkillToCheck)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }





        public  int[] GetAllSkillsThatHaveRequirement(int requirement)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i].RequiredSkills != null)
                {
                    for (int j = 0; j < tree[i].RequiredSkills.Length; j++)
                    {
                        if (tree[i].RequiredSkills[j] == requirement)
                        {
                            output.Add(i);
                            break;
                        }
                    }
                }
            }
            if (output.Count == 0)
            {
                return null;
            }
            return output.ToArray();
        }

        public  int[] InValidRequirememts(string subject)
        {
            int[] downstream = GetAllDownstreamSkills(FindIndexOfSkillByNameInSkillArray(subject));
            int[] upstream = GetAllUpstreamSkills(FindIndexOfSkillByNameInSkillArray(subject));

            return ConcatIntArrays(downstream, upstream);
        }


        public  int[] ValidRequirememts(string subject)
        {
            List<int> validReqs = new List<int>();

            int[] invalidRequirements = InValidRequirememts(subject);

            if (invalidRequirements == null)
            {
                for (int i = 0; i < tree.Length; i++)
                {
                    validReqs.Add(i);
                }
                return validReqs.ToArray();
            }
            else
            {
                for (int i = 0; i < tree.Length; i++)
                {
                    if (!invalidRequirements.Contains(i))
                    {
                        validReqs.Add(i);
                    }
                }

                return validReqs.ToArray();
            }




        }


        public  int[] ConcatIntArrays(int[] first, int[] second)
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

        public  int FindIndexOfSkillByNameInSkillArray(string skillName)
        {
            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i].Name == skillName)
                {
                    return i;
                }
            }
            return -1;
        }


        public int[] GetAllDownstreamSkills(int subject)
        {
            int[] skillsWithMeAsReq = GetAllSkillsThatHaveRequirement(subject);
            if(skillsWithMeAsReq == null)
            {
                return null;
            }


            List<int> downstreamList = new List<int>();

            downstreamList.AddRange(skillsWithMeAsReq);

            for (int i = 0; i < skillsWithMeAsReq.Length; i++)
            {
                int[] downStream = GetAllDownstreamSkills(skillsWithMeAsReq[i]);
                if (downStream != null && downStream.Length > 0)
                {
                    downstreamList.AddRange(downStream);
                }
            }
            return downstreamList.ToArray();

        }

        //Skills that are requirements of subject recursive
        public  int[] GetAllUpstreamSkills(int subject)
        {
            if (tree[subject].IsRoot())
            {
                return null;
            }
            int[] subjectRequirements = tree[subject].RequiredSkills;
            List<int> upstreamList = new List<int>();


            upstreamList.AddRange(subjectRequirements);
            if(ReqsOfReqs(tree[subject]) != null)
            {
                for (int i = 0; i < subjectRequirements.Length; i++)
                {
                    if (!tree[subjectRequirements[i]].IsRoot())
                    {
                        upstreamList.AddRange(GetAllUpstreamSkills(subjectRequirements[i]));
                    }
                }
            }

            return upstreamList.ToArray();
        }


        public int[] ReqsOfReqs(Skill skill)
        {
            List<int> reqList = new List<int>();
            int[] skillReqs = skill.RequiredSkills;
            if(skillReqs != null)
            {
                for (int i = 0; i < skillReqs.Length; i++)
                {
                    int[] reqReqs = tree[skillReqs[i]].RequiredSkills;
                    if(reqReqs != null)
                    {
                        for (int j = 0; j < reqReqs.Length; j++)
                        {
                            reqList.Add(reqReqs[j]);
                        }
                    }                    
                }
            }
            if(reqList.Count > 0)
            {
                return reqList.ToArray();
            }
            else
            {
                return null;
            }

        }

    }


}
