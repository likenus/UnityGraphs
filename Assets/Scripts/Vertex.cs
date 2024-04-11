using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public int id;
	public List<Vertex> neighbours;
	public List<Edge> edges;
	public bool Colored { get; set; } = false;
	public TMP_Text label;

	private bool _showWeight = false;
	private Color markColor = new Color(0.5f, 1f, 1f);
	public Color DefaultColor { get; set; }
	public string Text
	{
		get
		{
			return label.text;
		}
		set
		{
			label.text = value;
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
			label.enabled = value;
		}
	}

	private void Start()
	{
		DefaultColor = GetComponent<Renderer>().material.color;
		label.enabled = ShowWeight;
	}

	public void Paint()
	{
		GetComponent<Renderer>().material.color = markColor;
		Colored = true;
	}

	public void Reset()
	{
		GetComponent<Renderer>().material.color = DefaultColor;
		Colored = false;
		Text = "";
	}

	public override bool Equals(object other)
	{
		if (other == null || GetType() != other.GetType())
		{
			return false;
		}

		Vertex tmp = (Vertex)other;

		return tmp.id == this.id;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(id);
	}
}
