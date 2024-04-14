
using System.Collections.Generic;

public class UnionFind
{
	private readonly List<UnionFindNode> nodes;
	public UnionFind(int size)
	{
		nodes = new();
		for (int i = 0; i < size; i++)
		{
			UnionFindNode node = new(i);
			node.Parent = node;
			nodes.Add(node);
		}
	}
	
	public int Find(int key)
	{
		UnionFindNode v = nodes[key];
		return Find(v).Key;
	}
	
	private UnionFindNode Find(UnionFindNode v)
	{
		if (v.Parent.Equals(v))
		{
			return v;
		}
		
		UnionFindNode p = Find(v.Parent);
		v.Parent = p;
		return p;
	}
	
	public void Union(int a, int b)
	{
		UnionFindNode v = nodes[a];
		UnionFindNode w = nodes[b];
		
		Union(v, w);
	}
	
	private void Union(UnionFindNode v, UnionFindNode w)
	{
		UnionFindNode r = Find(v);
		UnionFindNode s = Find(w);
		
		if (r.Rank < s.Rank)
		{
			r.Parent = s;
			s.Rank = r.Rank + 1;
		}
		else 
		{
			s.Parent = r;
			r.Rank = s.Rank + 1;
		}
	}
	
	internal class UnionFindNode
	{
		public int Key { get; set; }
		public int Rank { get; set; }
		public UnionFindNode Parent { get; set; }
		
		public UnionFindNode(int key)
		{
			Key = key;
		}
	}
}