using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DeleteTool : ITool
{
	private readonly Graph graph;
	private LayerMask selectLayer;
	private GameObject selectedForDelete = null;
	private GameObject selectedForMove;
	private Vector3 offset;
	private bool snapping = false;
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

		if (Physics.Raycast(ray, out RaycastHit hit, 100, selectLayer))
		{
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
		else if(!EventSystem.current.IsPointerOverGameObject())
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 0;
			selectedForMove = graph.gameObject;
			offset = selectedForMove.transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
		}
	}

	public void LeftReleased(InputAction.CallbackContext context)
	{
		selectedForMove = null;
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
		if (selectedForMove != null)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 0;

			Vector3 pointToMove = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
			if (snapping)
				selectedForMove.transform.localPosition =
					new Vector3((float)Math.Round(pointToMove.x), (float)Math.Round(pointToMove.y), 0);
			if (!snapping)
				selectedForMove.transform.localPosition = new Vector3(pointToMove.x, pointToMove.y, 0);
		}
	}

	public void Reset()
	{
		if (selectedForDelete != null)
		{
			selectedForDelete.GetComponent<Renderer>().material.color = oldColor;
		}
		selectedForDelete = null;
		selectedForMove = null;
	}

	public void ShiftClicked(InputAction.CallbackContext context)
	{
		snapping = true;
	}

	public void ShiftReleased(InputAction.CallbackContext context)
	{
		snapping = false;
	}
}
