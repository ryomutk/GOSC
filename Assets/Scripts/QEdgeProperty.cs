using System;
using System.Collections.ObjectModel;
using UnityEngine;


public class QEdgeProperty:EdgeProperty
{
	public string operatorName;
	public Edge[] members;
	public bool oqqupied;
	public int length { get { return members.Length; } }

	public QEdgeProperty()
	{
		
	}

	public QEdgeProperty(string gate,bool oqqupied)
	{
		this.operatorName = gate;
		this.oqqupied = oqqupied;
	}


}