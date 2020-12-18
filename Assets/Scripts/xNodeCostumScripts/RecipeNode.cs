using XNode;
using InventoryLogic;
using UnityEngine;



public class ItemAmountNode : Node
{
    public int Amount;

    [Input(connectionType = ConnectionType.Override)]
    public ItemNode ItemNode;

    [Output]
    public ItemAmountNode amountNode;

    protected override void Init()
    {
        base.Init();

    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        return null; // Replace this
    }


}
public class RecipeNode : Node
{

    [Output]
    public RecipeNode RecipeOutput;

    [Input(dynamicPortList =true)]
    public ItemAmountNode[] NeedsResourceOf;


    void AddDynamicPorts(Node node)
    {
        node.AddDynamicInput(typeof(ItemAmount), fieldName: "NeedsResourceOf");
    }

    void RemoveDynamicPorts(Node node)
    {
        node.RemoveDynamicPort("NeedsResourceOf");
    }

    // Use this for initialization
    protected override void Init()
    {
        base.Init();

    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {

        return null; // Replace this
    }
}