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
            skillObject.Add("Name", tree[i].Name);
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
                    effectors.Add(effector.Key, System.Math.Round(effector.Value, 3));


                }
                skillObject.Add("Effectors", effectors);
            }

            treeObject.Add(tree[i].Name, skillObject);
        }


        Debug.Log(treeObject.ToString());
        File.WriteAllText(path, treeObject.ToString());
        return treeObject.ToString();

    }


    public static Skill[] JsonToSkillTree(string path)
    {
        JSONObject treeRead = JSONNode.Parse(File.ReadAllText(path)).AsObject;
        


        for (int i = 0; i < treeRead.Count; i++)
        {
           
            JSONObject skillObject = treeRead[i].AsObject;

            Skill skill = new Skill(skillObject.GetValueOrDefault("Name", skillObject));
            skill.SetDescription(skillObject.GetValueOrDefault("Description", skillObject));


            JSONArray req = skillObject.GetValueOrDefault("Requirements", skillObject).AsArray;
            if (req != null)
            {
                string[] reqString = new string[req.Count];
                for (int j = 0; j < req.Count; j++)
                {
                    reqString[j] = req[j].Value;
                }
                skill.SetRequieredSkills(reqString);
            }

            JSONObject effectors = skillObject.GetValueOrDefault("Effectors", skillObject).AsObject;
            if(effectors != null)
            {
                Dictionary<string, float> Eff = new Dictionary<string, float>();

                foreach (KeyValuePair<string, JSONNode> eff in effectors)
                {
                    Eff.Add(eff.Key, eff.Value);
                }
                skill.SetEffectors(Eff);
            }





            Debug.Log("Found object: " + treeRead.Keys.Current+ " "+ skillObject.ToString());
        }

        return new Skill[0];

    }
}
    
