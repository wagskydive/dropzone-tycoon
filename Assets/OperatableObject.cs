public class OperatableObject : SelectableObject
{
    public bool hasOperator;
    public CharacterObject operatorCharacter;


    public void SetOperator(CharacterObject _character)
    {
        hasOperator = true;
        operatorCharacter = _character;
    }

    public void RemoveOperator()
    {
        hasOperator = false;
        operatorCharacter = null;
    }

    public override void SelectObject()
    {
        
        if (hasOperator)
        {
            operatorCharacter.isCurrentlySelectable = true;
            operatorCharacter.SelectObject();
        }
        else
        {
            base.SelectObject();
        }
    }
}
