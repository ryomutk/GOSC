using System;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Cell
{
	public ReadOnlyCollection<Edge> edges { get { return Array.AsReadOnly(_edges); } }
	Edge[] _edges = new Edge[4];
	public Cell()
	{
		for(int i = 0;i<4;i++)
        {
			_edges[i] = new Edge();
        }
	}
	
}
