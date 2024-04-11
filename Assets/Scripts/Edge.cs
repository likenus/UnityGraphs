using TMPro;
using UnityEngine;

public class Edge : MonoBehaviour
{
	public GameObject container;
	public GameObject tip;
	public (Vertex, Vertex) vertices;
	private int _weight = 1;
	public TMP_Text label;
	private Color markColor = new(0.5f, 1f, 1f);
	private Color defaultColor;
	private bool _showWeight = false;
	private bool _isDirected = false;
	public bool ShowWeight
	{
		get
		{
			return _showWeight;
		}
		set
		{
			_showWeight = value;
			label.enabled = value;
		}
	}
	public bool IsDirected
	{
		get
		{
			return _isDirected;
		}
		set
		{
			_isDirected = value;
			tip.SetActive(value);
		}
	}
	public int Weight
	{
		get
		{
			return _weight;
		}
		set
		{
			_weight = value;
			label.text = value.ToString();
		}
	}
	
	private void Start()
	{
		defaultColor = GetComponent<Renderer>().material.color;
		tip.SetActive(IsDirected);
		label.enabled = ShowWeight;
	}
	
	private void Update()
	{
		Vertex vertex1 = vertices.Item1;
		Vertex vertex2 = vertices.Item2;

		Vector3 v1 = vertex1.transform.position;
		Vector3 v2 = vertex2.transform.position;

		Vector3 v = v2 - v1;
		Quaternion angle = Quaternion.FromToRotation(new Vector3(0, 1, 0), v);

		container.transform.position = 0.5f * (v1 + v2);
		transform.rotation = angle;
		transform.localScale = new Vector3(0.2f, v.magnitude / 2f, 0.2f);
		
		tip.GetComponentInChildren<Renderer>().material.color = GetComponent<Renderer>().material.color;
	}
	
	public void Paint()
	{
		GetComponent<Renderer>().material.color = markColor;
		tip.GetComponentInChildren<Renderer>().material.color = markColor;
	}
	
	public void Reset()
	{
		GetComponent<Renderer>().material.color = defaultColor;
		tip.GetComponentInChildren<Renderer>().material.color = defaultColor;
	}
}
