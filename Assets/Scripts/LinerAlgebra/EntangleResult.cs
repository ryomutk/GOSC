using System;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Collections.Generic;
using System.Numerics;

public struct EntangleResult
{
    public int parity;
    public Dictionary<string, Complex>  tensor;
    public Dictionary<string, Complex> entangledStateStr;
    public Dictionary<Vector<Complex>, Complex> entangledState;
    public Dictionary<string, MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex>> ketValueDict;
    public string info{get{return String.Format("MeasureResult:<br> Parity:{0} <br> state:<br>  {1}",parity,QuantumMath.DataToStr<string>(entangledStateStr));}}
}