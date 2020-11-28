using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SkillsLogic;


public class FileSaver
{
    public static void SaveSkillTreeToJson(string path, Skill[] skillTree)
    {
        string content = "";
        SkillSave[] tree = MakeSkillSaves(skillTree);
        for (int i = 0; i < tree.Length; i++)
        {
            content += JsonUtility.ToJson(tree[i], true) + "\n";
        }
        File.WriteAllText(path, content);
    }

    private static SkillSave[] MakeSkillSaves(Skill[] tree)
    {
        SkillSave[] skillSaves = new SkillSave[tree.Length];

        for (int i = 0; i < tree.Length; i++)
        {
            Skill skill = tree[i];
            skillSaves[i] = new SkillSave(skill);
        }

        return skillSaves;
    }


}
