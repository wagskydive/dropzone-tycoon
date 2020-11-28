using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataLogic;


namespace SkillsLogic
{
    public static class SaveAndLoad
    {
        public static string SaveSkillTree(Skill[] skills, string path)
        {
            
            JSONObject skillTreeJson = new JSONObject();

            

            for (int i = 0; i < skills.Length; i++)
            {
                
                skillTreeJson.Add("Name",skills[i].Name);
                
                if (skills[i].RequiredSkills != null && skills[i].RequiredSkills.Length > 0)
                {
                    JSONArray RequiredSkills = new JSONArray();
                    //string[] req = new string[skills[i].RequieredSkills.Length];
                    for (int j = 0; j < skills[i].RequiredSkills.Length; j++)
                    {

                        RequiredSkills.Add(skills[i].RequiredSkills[j]);
                        
                    }
                    //skillTreeJson.Add
                    //skillTreeJson.Add(skills[i].Name, RequiredSkills);
                }
                else
                {
                    skillTreeJson.Add(skills[i].Name, "No Required Skills");

                }



            }

            FileStream fileStream = new FileStream(path, FileMode.Create );
            using(StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(skillTreeJson.ToString());
            }

            //File.WriteAllText(path, skillTreeJson.ToString());


            return skillTreeJson.ToString();
        }
    }
}
