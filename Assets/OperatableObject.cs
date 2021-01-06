public class OperatableObject : SelectableObject
{
    public bool hasOperator;
    public CharacterBrain operatorBrain;


    public void SetOperator(CharacterBrain brain)
    {
        hasOperator = true;
        operatorBrain = brain;
    }

    public void RemoveOperator()
    {
        hasOperator = false;
        operatorBrain = null;
    }
}
