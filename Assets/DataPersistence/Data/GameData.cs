using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int startVertexID;
	public List<Vector3> vertexPositions;
	public List<int> vertexIDs;
	public List<Vector3> edges;
	public bool isDirected;
	public int graphId;

	// Constructor contains all default settings
	public GameData()
	{
		graphId = 3;
		startVertexID = 0;
		vertexPositions = new List<Vector3>{new(0, 1, 0), new(-3, 0, 0), new(3, 0 ,0)};
		vertexIDs = new List<int>{0, 1, 2};
		isDirected = false;
		edges = new();
	}
}
