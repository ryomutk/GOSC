﻿using System;
using System.Collections.ObjectModel;
using UnityEngine;

public class QuantumEdgeProperty:EdgeProperty
{
	public Pauli operatorType;
	public Edge[] members;
	public int length { get { return members.Length; } }

	public QuantumEdgeProperty()
	{
		
	}

	public QuantumEdgeProperty(string gate,bool oqqupied)
	{
		this.operatorType = gate=="X"?Pauli.X:Pauli.Z;
		this.oqqupied = oqqupied;
	}


}