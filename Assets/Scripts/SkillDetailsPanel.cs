using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDetailsPanel : MonoBehaviour
{
    public SkillNodeRuntimeOld SkillN;

    public Editable Name;

    public Editable Description;

    public Editable Requirements;

    public Editable Effectors;


    public void AssignSkillNode(SkillNodeRuntimeOld skill)
    {


        SkillN = skill;

        Name.SetDisplaytext(skill.NameText.text);
        Name.OnEdited += HandleNameEdit;

        Description.SetDisplaytext(skill.DescriptionText.text);
        Description.OnEdited += HandleDescriptionEdit;


        Requirements.SetDisplaytext(skill.RequirementsText.text);
        Requirements.AssignSkillNode(skill.NameText.text, skill);

        Requirements.OnEdited += HandleRequirementEdit;



        //Effectors.SetDisplaytext(skill.Effe)


    }

    void HandleNameEdit(string edit)
    {
        SkillN.UpdateSkillName(edit);
        AssignSkillNode(SkillN);
    }

    void HandleDescriptionEdit(string edit)
    {
        SkillN.UpdateSkillDescription(edit);
        AssignSkillNode(SkillN);
    }

    void HandleRequirementEdit(string edit)
    {
        if (Requirements.addMode)
        {
            SkillN.AddRequirement(edit);
        }
        else
        {
            SkillN.RemoveRequirement(edit);
        }
        AssignSkillNode(SkillN);
        
    }


}
