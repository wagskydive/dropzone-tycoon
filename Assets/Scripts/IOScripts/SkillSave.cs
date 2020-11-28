using System.Collections.Generic;
using SkillsLogic;
using UnityEngine;


[SerializeField]
public class SkillSave
{
    public string Name;
    public string[] RequiredSkills;    
    //StatSave[] Effectors;

    public string Effectors;

    public SkillSave(Skill skill)
    { 
        Name = skill.Name;
        RequiredSkills = skill.RequiredSkills;
        Effectors = EffectorsFromSkill(skill);
    }

    string EffectorsFromSkill(Skill skill)
    {
        StatSave[] effectors = new StatSave[skill.Effectors.Count];

        string effectorsP = "";
        int i = 0;
        foreach (KeyValuePair<string, float> entry in skill.Effectors)
        {
            string[] keyVal = new string[2];
            keyVal[0] = entry.Key;
            keyVal[1] = entry.Value.ToString();

            effectorsP += JsonUtility.ToJson(keyVal);


            i++;
            // do something with entry.Value or entry.Key
        }

        //JsonUtility.ToJson(skill.Effectors);
        return JsonUtility.ToJson(skill.Effectors);
    }
}
