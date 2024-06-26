using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EditTool : ITool
{
	private readonly Graph graph;
	private LayerMask selectLayer;
	private GameObject selectedForMove;
	private Vertex leftClickCache = null;
	private Vertex rightClickCache = null;
	private Color selectColor = new(0.4f, 1f, 0.5f);
	private Color selectColor2 = new(0.5f, 1f, 1f);
	private Color oldColor;
	private Vector3 offset;
	private bool snapping;
	private readonly InputController inputController;
	private readonly Dictionary<Vertex, Color> oldColors = new();

	public EditTool(Graph graph, LayerMask selectLayer, InputController inputController)
	{
		this.graph = graph;
		this.selectLayer = selectLayer;
		this.inputController = inputController;
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

	public void LeftClicked(InputAction.CallbackContext context)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100, selectLayer))
		{
			if (hit.collider.gameObject.TryGetComponent<Vertex>(out leftClickCache))
			{
				selectedForMove = hit.collider.gameObject;
				offset = selectedForMove.transform.localPosition - new Vector3(hit.point.x, hit.point.y, 0);
				oldColors.Add(leftClickCache, leftClickCache.gameObject.GetComponent<Renderer>().material.color); // Color can be wrong, might fix
				selectedForMove.GetComponent<Renderer>().material.color = selectColor;

				foreach (Vertex vertex in graph.NeighboursOf(leftClickCache).Item1)
				{
					oldColors.Add(vertex, vertex.gameObject.GetComponent<Renderer>().material.color);
					vertex.gameObject.GetComponent<Renderer>().material.color = selectColor2;
				}
			}
			if (hit.collider.gameObject.TryGetComponent<Edge>(out var edge))
			{
				int value;
				try
				{
					value = int.Parse(inputController.inputField.text);
				}
				catch (FormatException)
				{
					Debug.LogError("Invalid input sequence");
					return;
				}
				edge.Weight = value;
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
		if (selectedForMove == null) { return; }
		if (selectedForMove.Equals(graph.gameObject))
		{
			selectedForMove = null;
			return;
		}
		selectedForMove.GetComponent<Renderer>().material.color = oldColors[leftClickCache];
		foreach (Vertex vertex in graph.NeighboursOf(leftClickCache).Item1)
		{
			vertex.gameObject.GetComponent<Renderer>().material.color = oldColors[vertex];
		}
		selectedForMove = null;
		leftClickCache = null;
		oldColors.Clear();
	}

	public void RightClicked(InputAction.CallbackContext context)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100, selectLayer))
		{
			if (rightClickCache == null && hit.collider.gameObject.TryGetComponent<Vertex>(out rightClickCache))
			{
				Material material = rightClickCache.gameObject.GetComponent<Renderer>().material;
				oldColor = oldColors.ContainsKey(rightClickCache) ? oldColors[rightClickCache] : material.color;
				material.color = selectColor;
				return;
			}
			if (!hit.collider.gameObject.TryGetComponent<Vertex>(out var v2))
			{
				return;
			}
			if (v2.Equals(rightClickCache))
			{
				rightClickCache.gameObject.GetComponent<Renderer>().material.color = oldColor;
				rightClickCache = null;
				return;
			}
			graph.ConnectVertices(rightClickCache, v2);
			rightClickCache.gameObject.GetComponent<Renderer>().material.color = oldColor;
			rightClickCache = null;
		}
	}

	public void RightReleased(InputAction.CallbackContext context)
	{

	}

	public void ShiftClicked(InputAction.CallbackContext context)
	{
		snapping = true;
	}

	public void ShiftReleased(InputAction.CallbackContext context)
	{
		snapping = false;
	}

	public void Reset()
	{
		if (rightClickCache != null)
		{
			rightClickCache.gameObject.GetComponent<Renderer>().material.color = oldColor;
			rightClickCache = null;
		}
	}
}
