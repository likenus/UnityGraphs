using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public ITool tool;
	public Graph graph;
	public LayerMask selectLayer;
	public LayerMask colliderLayer;
	public TMP_Dropdown dropdown;
	public TMP_InputField inputField;
	private SelectActions inputActions;

	private void Awake()
	{
		inputActions = new SelectActions();
		tool = new EditTool(graph, selectLayer, this);
	}
	
	private void Update()
	{
		tool.DoUpdate();
	}

	public void OnDropdown()
	{
		switch (dropdown.value)
		{
			case 0:
				SetCreateTool();
				break;
			case 1:
				SetDeleteTool();
				break;
			case 2:
				SetShowTool();
				break;
			default:
				return;
		}
	}

	public void OnIsDirectedToggle()
	{
		graph.IsDirected = !graph.IsDirected;
	}
	
	public void OnShowWeightToggle() 
	{
		graph.ShowWeight = !graph.ShowWeight;
	}

	#region - Enable / Disable -
	private void OnEnable()
	{
		inputActions.Player.Select.started += tool.LeftClicked;
		inputActions.Player.Select.canceled += tool.LeftReleased;
		inputActions.Player.Connect.started += tool.RightClicked;
		inputActions.Player.Connect.canceled += tool.RightReleased;
		inputActions.Player.Snapping.started += tool.ShiftClicked;
		inputActions.Player.Snapping.canceled += tool.ShiftReleased;
		inputActions.Player.Enable();
	}

	private void OnDisable()
	{
		inputActions.Player.Select.started -= tool.LeftClicked;
		inputActions.Player.Select.canceled -= tool.LeftReleased;
		inputActions.Player.Connect.started -= tool.RightClicked;
		inputActions.Player.Connect.canceled -= tool.RightReleased;
		inputActions.Player.Snapping.started -= tool.ShiftClicked;
		inputActions.Player.Snapping.canceled -= tool.ShiftReleased;
		inputActions.Player.Disable();
		tool.Reset();
	}
	#endregion

	#region - Set Tools -
	public void SetCreateTool()
	{
		OnDisable();
		tool = new EditTool(graph, selectLayer, this);
		OnEnable();
	}

	public void SetDeleteTool()
	{
		OnDisable();
		tool = new DeleteTool(graph, selectLayer);
		OnEnable();
	}

	public void SetShowTool()
	{
		OnDisable();
		tool = new MoveTool(graph, selectLayer, this);
		OnEnable();
	}
	#endregion
}
