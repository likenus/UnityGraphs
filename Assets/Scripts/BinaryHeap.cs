using System;

public class BinaryHeap
{
	private readonly int capacity;
	private readonly Vertex[] content;
	private readonly long[] priorities;
	private readonly int[] indices;
	public int Size { get; set; } = 0;
	
	public BinaryHeap(int capacity)
	{
		this.capacity = capacity;
		content = new Vertex[capacity];
		priorities = new long[capacity];
		indices = new int[capacity];
	}
	
	public void Push(Vertex v, long priority)
	{
		content[Size] = v;
		priorities[v.id] = priority;
		indices[v.id] = Size;
		Size++;
		BubbleUp(content[Size - 1]);
	}
	
	public Vertex PopMin()
	{
		Vertex v = content[0];
		Swap(v, content[Size - 1]);
		content[Size - 1] = null;
		SetPrio(v, -1);
		indices[v.id] = -1;
		Size--;
		if (Size > 0)
			SinkDown(content[0]);
		
		return v;
	}
	
	private Vertex LeftChild(Vertex v)
	{
		if (2 * IndexOf(v) + 1 < Size)
			return content[2 * IndexOf(v) + 1];
		return null;
	}
	
	private Vertex RightChild(Vertex v)
	{
		if (2 * IndexOf(v) + 2 < Size)
			return content[2 * IndexOf(v) + 2];
		return null;
	}
	
	private Vertex Parent(Vertex v)
	{
		if (IndexOf(v) > 0)
			return content[(IndexOf(v) - 1) / 2];
		return null;
	}
	
	private void Swap(Vertex v, Vertex w)
	{
		int i = IndexOf(v);
		int j = IndexOf(w);

		(content[j], content[i]) = (content[i], content[j]);
		indices[v.id] = j;
		indices[w.id] = i;
	}
	
	private void BubbleUp(Vertex v)
	{
		if (IndexOf(v) > 0 && GetPrio(v) < GetPrio(Parent(v)))
		{
			Swap(v, Parent(v));
			BubbleUp(v);
		}
	}
	
	private void SinkDown(Vertex v)
	{
		Vertex uL = LeftChild(v);
		Vertex uR = RightChild(v);
		Vertex u = v;
		
		if (uL != null && IndexOf(uL) < Size && GetPrio(uL) < GetPrio(u))
		{
			u = uL;
		}
		if (uR != null && IndexOf(uR) < Size && GetPrio(uR) < GetPrio(u))
		{
			u = uR;
		}
		
		if (!u.Equals(v))
		{
			Swap(u, v);
			SinkDown(u);
		}
	}
	
	private int IndexOf(Vertex v)
	{
		if (v == null) { return -1; }
		return indices[v.id];
	}
	
	private long GetPrio(Vertex v)
	{
		return priorities[v.id];
	}
	
	private void SetPrio(Vertex v, long prio)
	{
		priorities[v.id] = prio;
	}
	
	public void DecPrio(Vertex v, long prio)
	{
		long oldPrio = GetPrio(v);
		SetPrio(v, prio);
		
		if (prio < oldPrio)
		{
			BubbleUp(v);
		}
		else
		{
			SinkDown(v);
		}
	}
}