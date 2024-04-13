using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour, IDataPersistence
{
	public GameObject edgePrefab;
	public GameObject vertexPrefab;
	public Toggle directedToggle;

	public SortedDictionary<int, Vertex> Vertices { get; private set; } = new();
	public List<Edge> Edges { get; private set; } = new();
	private readonly System.Random random = new();
	private bool _isDirected = false;
	private bool _showWeight = false;
	private Vertex _startVertex;
	private int id = 0;
	private Color startColor = new(1f, 0.812f, 0.6194f);
	public bool IsDirected
	{
		get
		{
			return _isDirected;
		}
		set
		{
			_isDirected = value;
			foreach (Edge edge in Edges)
			{
				edge.IsDirected = value;
			}
		}
	}
	public bool ShowWeight
	{
		get
		{
			return _showWeight;
		}
		set
		{
			_showWeight = value;
			foreach (Edge edge in Edges)
			{
				edge.ShowWeight = value;
			}
			foreach (Vertex vertex in Vertices.Values)
			{
				vertex.ShowWeight = value;
			}
		}
	}
	public Vertex StartVertex
	{
		get { return _startVertex; }
		set
		{
			_startVertex = value;
			_startVertex.gameObject.GetComponent<Renderer>().material.color = startColor;
			_startVertex.DefaultColor = startColor;
		}
	}

	public void LoadData(GameData data)
	{
		Clear();
		id = data.graphId;
		directedToggle.isOn = data.isDirected;
		for (int i = 0; i < data.vertexIDs.Count; i++)
		{
			Vertex v = SpawnVertex(data.vertexPositions[i], data.vertexIDs[i]);
			if (v.id == data.startVertexID)
			{
				StartVertex = v;
			}
		}
		for (int i = 0; i < data.edges.Count; i++)
		{
			Vector3 edgeData = data.edges[i];
			ConnectVertices(Vertices[(int)edgeData.x], Vertices[(int)edgeData.y], (int)edgeData.z);
		}
	}

	public void SaveData(ref GameData data)
	{
		data.graphId = id;
		data.isDirected = IsDirected;
		data.startVertexID = StartVertex.id;
		data.vertexPositions = Vertices.Values.Select(v => v.transform.localPosition).ToList();
		data.vertexIDs = Vertices.Values.Select(v => v.id).ToList();
		data.edges = Edges.Select(e => new Vector3(e.vertices.Item1.id, e.vertices.Item2.id, e.Weight)).ToList();
	}
	public void SpawnVertex()
	{
		GameObject newVertex = Instantiate(vertexPrefab);
		newVertex.transform.SetParent(transform);
		newVertex.transform.position = new Vector3((float)random.NextDouble() * 5, (float)random.NextDouble() * 5, 0);
		Vertex v = newVertex.GetComponent<Vertex>();
		v.ShowWeight = ShowWeight;
		v.id = id++;
		v.name = string.Format("Vertex ({0})", v.id);
		if (Vertices.Count == 0)
		{
			StartVertex = v;
		}
		Vertices.Add(v.id, v);
	}

	// Only used for loading from File
	private Vertex SpawnVertex(Vector3 position, int vid)
	{
		GameObject newVertex = Instantiate(vertexPrefab);
		newVertex.transform.SetParent(transform);
		newVertex.transform.localPosition = position;
		Vertex v = newVertex.GetComponent<Vertex>();
		v.ShowWeight = ShowWeight;
		v.id = vid;
		v.name = string.Format("Vertex ({0})", vid);
		Vertices.Add(v.id, v);
		return v;
	}

	public void Reset()
	{
		foreach (Vertex vertex in Vertices.Values)
		{
			vertex.Reset();
		}
		foreach (Edge edge in Edges)
		{
			edge.Reset();
		}
	}

	public void ConnectVertices(Vertex vertex1, Vertex vertex2)
	{
		if (vertex1.neighbours.Contains(vertex2))
		{
			return; // Multiple edges between two vertices not allowed
		}

		GameObject newEdge = Instantiate(edgePrefab);
		newEdge.transform.SetParent(transform);
		Edge edge = newEdge.GetComponentInChildren<Edge>();
		edge.IsDirected = IsDirected;
		edge.ShowWeight = ShowWeight;
		edge.vertices = (vertex1, vertex2);

		Edges.Add(edge);
		vertex1.neighbours.Add(vertex2);
		vertex1.edges.Add(edge);
		vertex2.neighbours.Add(vertex1);
		vertex2.edges.Add(edge);
	}

	// Only used for Loading from File
	private void ConnectVertices(Vertex vertex1, Vertex vertex2, int weight)
	{
		GameObject newEdge = Instantiate(edgePrefab);
		newEdge.transform.SetParent(transform);
		Edge edge = newEdge.GetComponentInChildren<Edge>();
		edge.IsDirected = this.IsDirected;
		edge.ShowWeight = ShowWeight;
		edge.vertices = (vertex1, vertex2);
		edge.Weight = weight;

		Edges.Add(edge);
		vertex1.neighbours.Add(vertex2);
		vertex1.edges.Add(edge);
		vertex2.neighbours.Add(vertex1);
		vertex2.edges.Add(edge);
	}

	public void RemoveEdge(Edge edge)
	{
		Vertex v1 = edge.vertices.Item1;
		Vertex v2 = edge.vertices.Item2;

		Edges.Remove(edge);
		v1.edges.Remove(edge);
		v2.edges.Remove(edge);
		v1.neighbours.Remove(v2);
		v2.neighbours.Remove(v1);

		Destroy(edge.transform.parent.gameObject);
	}

	public (List<Vertex>, List<Edge>) NeighboursOf(Vertex vertex)
	{
		List<Edge> incidentEdges = new();
		if (IsDirected)
		{
			List<Vertex> neighbours = new();
			foreach (Edge edge in vertex.edges)
			{
				Vertex w = edge.vertices.Item2;
				if (w != vertex)
				{
					incidentEdges.Add(edge);
					neighbours.Add(w);
				}
			}
			return (neighbours, incidentEdges);
		}
		return (vertex.neighbours, vertex.edges);
	}

	public void RemoveVertex(Vertex vertex)
	{
		if (vertex.Equals(StartVertex) && Vertices.Count > 1)
		{
			StartVertex = Vertices[1];
		}
		List<Edge> tmp = new(vertex.edges);
		foreach (Edge edge in tmp)
		{
			RemoveEdge(edge);
		}
		int newFreeId = vertex.id;
		Vertices.Remove(vertex.id);
		Destroy(vertex.gameObject);

		// Rearrange dictionary
		if (Vertices.Count > 0)
		{
			(int, Vertex) ceil = (Vertices.Last().Key, Vertices.Last().Value);
			if (newFreeId < ceil.Item1)
			{
				ceil.Item2.id = newFreeId;
				ceil.Item2.name = string.Format("Vertex ({0})", ceil.Item2.id);
				Vertices.Remove(ceil.Item1);
				Vertices.Add(newFreeId, ceil.Item2);
			}
		}
		id--;
	}

	public void Clear()
	{
		foreach (Vertex v in new List<Vertex>(Vertices.Values))
		{
			RemoveVertex(v);
		}
	}

	public Edge EdgeBetween(Vertex v, Vertex w)
	{
		foreach (Edge e in NeighboursOf(v).Item2)
		{
			if (e.vertices.Item1.Equals(w) || e.vertices.Item2.Equals(w))
			{
				return e;
			}
		}
		return null;
	}
}
