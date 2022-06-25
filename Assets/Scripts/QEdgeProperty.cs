using System;
using System.Collections.ObjectModel;
using UnityEngine;

public class QuantumEdgeProperty:EdgeProperty
{
	public string operatorName;
	public Edge[] members;
	public int length { get { return members.Length; } }

	public QuantumEdgeProperty()
	{
		
	}

	public QuantumEdgeProperty(string gate,bool oqqupied)
	{
		this.operatorName = gate;
		this.oqqupied = oqqupied;
	}


}