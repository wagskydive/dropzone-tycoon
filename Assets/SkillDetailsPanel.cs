using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDetailsPanel : MonoBehaviour
{
    public SkillNode SkillN;

    public Editable Name;

    public Editable Description;

    public Editable Requirements;

    public Editable Effectors;

    public void AssignSkillNode(SkillNode skill)
    {
        SkillN = skill;
        Name.SetDisplaytext(skill.NameText.text);
        Description.SetDisplaytext(skill.DescriptionText.text);
        Requirements.SetDisplaytext(skill.RequirementsText.text);
        Requirements.AssignNode(skill.NameText.text, skill);
        //Effectors.SetDisplaytext(skill.Effe)
    }

}
