
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex32;

public class MeasurementResult
{
    Dictionary<Vector<Complex32>,double> rateDict = new Dictionary<Vector<Complex32>, double>();
    string measurementResult = "";
    string infoMsg = "";
    Vector<Complex32> stateVector;
    DenseMatrix measureBase;

    public MeasurementResult(Dictionary<Vector<Complex32>,double> rates,Vector<Complex32> result,string ket,DenseMatrix baseMatrix)
    {
        this.rateDict = rates;
        this.measurementResult = ket;
        this.stateVector = result;
        this.measureBase = baseMatrix;

        infoMsg = "Mesurement Result Was:" + measurementResult + " = " + this.stateVector+"<br>"+ 
        "with base"+ baseMatrix;
    }
}