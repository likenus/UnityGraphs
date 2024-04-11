using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
		switch (algorithmId.value)
		{
			case 0 :
				BFS();
				break;
			case 1 :
				DFS();
				break;
			case 2 :
				Dijkstra();
				break;
			default :
				return;
		}
	}
	
	public void UpdateSlider()
	{
		animationSpeed = slider.value;
	}
	
	private void BFS()
	{
		Reset();
		StartCoroutine(DoBFS());
	}
	
	private void DFS()
	{
		Reset();
		StartCoroutine(DoDFS(graph.StartVertex, null));
	}
	
	private void Dijkstra()
	{
		Reset();
		StartCoroutine(DoDijkstra());
	}
	
	private IEnumerator DoBFS()
	{
		int[] distances = new int[graph.Vertices.Count];
		distances[graph.StartVertex.id] = 0;
		Queue<Vertex> q = new();
		graph.StartVertex.Paint();
		graph.StartVertex.Text = distances[graph.StartVertex.id].ToString();
		q.Enqueue(graph.StartVertex);

		yield return new WaitForSeconds(1f / animationSpeed);

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
					yield return new WaitForSeconds(1f / animationSpeed);
				}
				else 
				{
					yield return new WaitForNextFrameUnit();
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
		
		while (heap.Size > 0)
		{
			Vertex u = heap.PopMin();
			u.Paint();
			u.Text = distances[u.id].ToString();
			if (parents[u.id] != null)
				graph.EdgeBetween(parents[u.id], u).Paint();
				
			yield return new WaitForSeconds(1f / animationSpeed);
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
			yield return new WaitForSeconds(1f / animationSpeed);
			yield return StartCoroutine(DoDFS(u, s));
		}
	}
	
	public void Reset()
	{
		StopAllCoroutines();
		graph.Reset();
	}
}
