using XNode;
using InventoryLogic;
using UnityEngine;

public class ItemNode : Node {

    public string Catagory;

    [TextArea]
    public string Description;


    [Output]
    public ItemNode ResourceOutput;


    [Input]
    public int AmountNeeded;

    [Input]
    public RecipeNode recipe;




    // Use this for initialization
    protected override void Init() {
		base.Init();
        
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
    {
        return null;
    }

}