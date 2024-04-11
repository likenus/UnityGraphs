using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeleteTool : ITool
{
	private readonly Graph graph;
	private LayerMask selectLayer;
	private GameObject selectedForDelete = null;
	private Color selectColor = new(1f, .2f, .2f);
	private Color oldColor;

	public DeleteTool(Graph graph, LayerMask selectLayer)
	{
		this.graph = graph;
		this.selectLayer = selectLayer;
	}

	public void LeftClicked(InputAction.CallbackContext context)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (!Physics.Raycast(ray, out RaycastHit hit, 100, selectLayer))
		{
			return;
		}
		if (selectedForDelete == null)
		{
			selectedForDelete = hit.collider.gameObject;
			oldColor = selectedForDelete.GetComponent<Renderer>().material.color;
			selectedForDelete.GetComponent<Renderer>().material.color = selectColor;
		}
		else
		{
			if (!hit.collider.gameObject.Equals(selectedForDelete))
			{
				selectedForDelete.GetComponent<Renderer>().material.color = oldColor;
				selectedForDelete = hit.collider.gameObject;
				oldColor = selectedForDelete.GetComponent<Renderer>().material.color;
				selectedForDelete.GetComponent<Renderer>().material.color = selectColor;
				return;
			}
			if (selectedForDelete.TryGetComponent<Edge>(out var edge))
			{
				graph.RemoveEdge(edge); // Edge is destroyed by graph
			}
			if (selectedForDelete.TryGetComponent<Vertex>(out var vertex))
			{
				graph.RemoveVertex(vertex); // Vertex is destroyed by graph
			}
		}
	}

	public void LeftReleased(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void RightClicked(InputAction.CallbackContext context)
	{
		Reset();
	}

	public void RightReleased(InputAction.CallbackContext context)
	{
		// Not used
	}

	public void DoUpdate()
	{
		// Not used
	}

	public void Reset()
	{
		if (selectedForDelete != null)
		{
			selectedForDelete.GetComponent<Renderer>().material.color = oldColor;
		}
		selectedForDelete = null;
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
