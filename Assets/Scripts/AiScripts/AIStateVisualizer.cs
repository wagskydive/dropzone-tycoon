using UnityEngine;
using StateMachineLogic;

public class AIStateVisualizer : MonoBehaviour
{
    private CharacterBrain[] characterBrains;

    [SerializeField]
    private Color color = Color.green;

    [SerializeField]
    private GUIStyle style;

    private void Start()
    {
        style = new GUIStyle();
    }

    private void OnDrawGizmos()
    {
        

        if(characterBrains == null)
        {
            characterBrains = FindObjectsOfType<CharacterBrain>();
        }
        if(characterBrains != null)
        {
            style.normal.textColor = color;
            foreach (var characterBrain in characterBrains)
            {
                if(characterBrain != null)
                {
                    if (characterBrain.currentState != null)
                    {
                        UnityEditor.Handles.Label(characterBrain.transform.position + (Vector3.up * 1.5f), characterBrain.currentState.ToString(), style);
                    }
                    else
                    {
                        UnityEditor.Handles.Label(characterBrain.transform.position + (Vector3.up * 1.5f), "No Current State", style);

                    }
                    float percLeft = characterBrain.GetCurrentJobLeft();
                    if (percLeft != 0)
                    {
                        Vector3 basePos = characterBrain.transform.position + Vector3.up;
                        UnityEditor.Handles.Label(basePos, (percLeft * 100).ToString() + "'%' Done", style);

                    }
                }

            }
        }



    }

}

