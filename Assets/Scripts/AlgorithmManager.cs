using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmManager : MonoBehaviour
{
	public Graph graph;
	public TMP_Dropdown algorithmId;
	public Slider slider;
	private float animationSpeed = 1f;

	private void Awake()
	{
		slider = (from gameObject in FindObjectsOfType<Slider>()
					where gameObject.name.Equals("SpeedSlider")
					select gameObject).First();
					
		algorithmId = (from gameObject in FindObjectsOfType<TMP_Dropdown>() 
					where gameObject.name.Equals("Algo Dropdown") 
					select gameObject).First();
	}
	public void RunAlgorithm()
	{
		Reset();
		switch (algorithmId.value)
		{
			case 0 :
				StartCoroutine(DoBFS());
				break;
			case 1 :
				StartCoroutine(DoDFS(graph.StartVertex, null));
				break;
			case 2 :
				StartCoroutine(DoDijkstra());
				break;
			case 3 :
				StartCoroutine(DoPrim());
				break;
			case 4 :
				StartCoroutine(DoKruskal());
				break;
			default :
				return;
		}
	}
	
	public void UpdateSlider()
	{
		animationSpeed = slider.value;
	}
	
	private IEnumerator DoBFS()
	{
		int[] distances = new int[graph.Vertices.Count];
		distances[graph.StartVertex.id] = 0;
		Queue<Vertex> q = new();
		graph.StartVertex.Paint();
		graph.StartVertex.Text = distances[graph.StartVertex.id].ToString();
		q.Enqueue(graph.StartVertex);

		if (!PlayBackSystem.Paused)
		{
			yield return new WaitForSecondsRealtime(1f / animationSpeed);
		}
		else 
		{
			yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
			PlayBackSystem.DoAlgorithmStep = false;
		}

		while (q.Count > 0)
		{
			Vertex v = q.Dequeue();
			(List<Vertex>, List<Edge>) pair = graph.NeighboursOf(v);

			foreach (Vertex w in pair.Item1)
			{
				if (w.Colored) { continue; }
				foreach (Edge e in pair.Item2)
				{
					if (e.vertices.Item1.Equals(w) || e.vertices.Item2.Equals(w))
					{
						e.Paint();
						break;
					}
				}
				w.Paint();
				distances[w.id] = distances[v.id] + 1;
				w.Text = distances[w.id].ToString();
				q.Enqueue(w);

				if (!PlayBackSystem.Paused)
				{
					yield return new WaitForSecondsRealtime(1f / animationSpeed);
				}
				if (PlayBackSystem.Paused)
				{
					yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
					PlayBackSystem.DoAlgorithmStep = false;
				}
			}
		}
	}
	
	private IEnumerator DoDijkstra()
	{
		long[] distances = new long[graph.Vertices.Count];
		Vertex[] parents = new Vertex[graph.Vertices.Count];
		for (int i = 0; i < distances.Length; i++)
		{
			distances[i] = int.MaxValue;
		}
		
		distances[graph.StartVertex.id] = 0;
		graph.StartVertex.Paint();
		BinaryHeap heap = new(graph.Vertices.Count);
		foreach (Vertex v in graph.Vertices.Values)
		{
			heap.Push(v, distances[v.id]);
		}
		
		while (heap.Count > 0)
		{
			Vertex u = heap.PopMin();
			u.Paint();
			u.Text = distances[u.id].ToString();
			if (parents[u.id] != null)
				graph.EdgeBetween(parents[u.id], u).Paint();
				
			if (!PlayBackSystem.Paused)
			{
				yield return new WaitForSecondsRealtime(1f / animationSpeed);
			}
			if (PlayBackSystem.Paused)
			{
				yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
				PlayBackSystem.DoAlgorithmStep = false;
			}
			
			foreach (Vertex v in graph.NeighboursOf(u).Item1)
			{
				Edge edge = graph.EdgeBetween(u, v);
				if (distances[v.id] > distances[u.id] + edge.Weight)
				{
					distances[v.id] = distances[u.id] + edge.Weight;
					if (!v.Colored)
					{
						parents[v.id] = u;
					}
					heap.DecPrio(v, distances[v.id]);
				}
			}
		}
	}
	
	private IEnumerator DoDFS(Vertex s, Vertex p)
	{
		s.Paint();
		if (p != null)
		{
			foreach (Edge e in graph.NeighboursOf(p).Item2)
			{
				if (e.vertices.Item1.Equals(s) || e.vertices.Item2.Equals(s))
				{
					e.Paint();
					break;
				}
			}
		}
		foreach (Vertex u in graph.NeighboursOf(s).Item1)
		{
			if (u == p || u.Colored) { continue; }
			if (!PlayBackSystem.Paused)
			{
				yield return new WaitForSecondsRealtime(1f / animationSpeed);
			}
			if (PlayBackSystem.Paused)
			{
				yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
				PlayBackSystem.DoAlgorithmStep = false;	
			}
			yield return StartCoroutine(DoDFS(u, s));
		}
	}
	
	private IEnumerator DoPrim()
	{
		if (graph.IsDirected)
		{
			Debug.LogError("Cant to Prim on directed graph.");
			yield break;
		}
		
		long[] priorities = new long[graph.Vertices.Count];
		Vertex[] parents = new Vertex[graph.Vertices.Count];
		for (int i = 0; i < priorities.Length; i++)
		{
			priorities[i] = int.MaxValue;
		}
		priorities[graph.StartVertex.id] = 0;
		
		BinaryHeap heap = new(graph.Vertices.Count);
		foreach (Vertex v in graph.Vertices.Values)
		{
			heap.Push(v, priorities[v.id]);
		}
		
		while (heap.Count > 0)
		{
			Vertex v = heap.PopMin();
			v.Paint();
			if (parents[v.id] != null)
				graph.EdgeBetween(parents[v.id], v).Paint();
			
			if (!PlayBackSystem.Paused)
				{
					yield return new WaitForSecondsRealtime(1f / animationSpeed);
				}
				if (PlayBackSystem.Paused)
				{
					yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
					PlayBackSystem.DoAlgorithmStep = false;	
				}
			
			foreach (Vertex u in graph.NeighboursOf(v).Item1)
			{
				Edge edge = graph.EdgeBetween(u, v);
				if (!u.Colored && priorities[u.id] > edge.Weight)
				{
					priorities[u.id] = edge.Weight;
					heap.DecPrio(u, edge.Weight);
					parents[u.id] = v;
				}
			}
		}
	}
	
	private IEnumerator DoKruskal()
	{
		if (graph.IsDirected)
		{
			Debug.LogError("Cant do Kruskal on directed graph.");
			yield break;
		}
		
		// Get all Edges and sort them
		List<Edge> edges = graph.Edges.OrderBy(e => e.Weight).ToList();
		UnionFind unionFind = new(edges.Count);
		
		foreach (Edge e in edges)
		{
			if (unionFind.Find(e.vertices.Item1.id) != unionFind.Find(e.vertices.Item2.id))
			{
				e.vertices.Item1.Paint();
				e.vertices.Item2.Paint();
				e.Paint();
				unionFind.Union(e.vertices.Item1.id, e.vertices.Item2.id);
				if (!PlayBackSystem.Paused)
				{
					yield return new WaitForSecondsRealtime(1f / animationSpeed);
				}
				if (PlayBackSystem.Paused)
				{
					yield return new WaitUntil(() => PlayBackSystem.DoAlgorithmStep);
					PlayBackSystem.DoAlgorithmStep = false;	
				}
			}
		}
	}
	
	public void Reset()
	{
		StopAllCoroutines();
		graph.Reset();
	}
}
