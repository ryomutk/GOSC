
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

public class MeasurementResult
{
    Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>,double> rateDict = new Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double>();
    string measurementResult = "";
    string infoMsg = "";
    MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector;
    DenseMatrix measureBase;

    public MeasurementResult(Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>,double> rates,MathNet.Numerics.LinearAlgebra.Vector<Complex> result,string ket,DenseMatrix baseMatrix)
    {
        this.rateDict = rates;
        this.measurementResult = ket;
        this.stateVector = result;
        this.measureBase = baseMatrix;

        infoMsg = "Mesurement Result Was:" + measurementResult + " = " + this.stateVector+"<br>"+ 
        "with base"+ baseMatrix;
    }
}