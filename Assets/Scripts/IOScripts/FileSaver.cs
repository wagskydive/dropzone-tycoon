using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SkillsLogic;
using DataLogic;
using System.Text;

public class FileSaver
{
    public static string SkillTreeToJson(string path, Skill[] tree)
    {
        JSONObject treeObject = new JSONObject();

        

        for (int i = 0; i < tree.Length; i++)
        {
            JSONObject skillObject = new JSONObject();
            skillObject.Add("Description", tree[i].Description);


            if (tree[i].RequiredSkills != null)
            {
                JSONArray requirementsArray = new JSONArray();
            
                for (int j = 0; j < tree[i].RequiredSkills.Length; j++)
                {                   
                    requirementsArray.Add("Req " + j.ToString(), new JSONString(tree[i].RequiredSkills[j]));                    
                }
                Debug.Log(requirementsArray.ToString());

                skillObject.Add("Requirements", requirementsArray);
            }


            if (tree[i].Effectors != null)
            {
                JSONObject effectors = new JSONObject();

                foreach (KeyValuePair<string, float> effector in tree[i].Effectors)
                {
                    effectors.Add(effector.Key, System.Math.Round(effector.Value,3));


                }
                skillObject.Add("Effectors", effectors);
            }

            treeObject.Add(tree[i].Name, skillObject);
        }

        
        Debug.Log(treeObject.ToString());
        File.WriteAllText(path, treeObject.ToString());
        return treeObject.ToString();
        
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
