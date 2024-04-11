using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTool : ITool
{
	private readonly Graph graph;
	private LayerMask selectLayer;
	private readonly InputController inputController;

	public MoveTool(Graph graph, LayerMask selectLayer, InputController inputController)
	{
		this.graph = graph;
		this.selectLayer = selectLayer;
		this.inputController = inputController;
	}
	
	public void DoUpdate()
	{
		// Not used
	}

	public void LeftClicked(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void LeftReleased(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void Reset()
	{
		// Nothing to reset
	}

	public void RightClicked(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void RightReleased(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void ShiftClicked(InputAction.CallbackContext context)
	{
		// Not used
	}

    public void ShiftReleased(InputAction.CallbackContext context)
    {
        // Not used
    }
}
