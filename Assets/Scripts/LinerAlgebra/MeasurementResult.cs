
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

public class MeasurementResult
{
    public Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double> rateDict = new Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double>();
    public string measurementResult = "";
    public string infoMsg = "";
    public MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector;
    public DenseMatrix measureBase;

    public MeasurementResult(Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double> rates, MathNet.Numerics.LinearAlgebra.Vector<Complex> result, string ket, DenseMatrix baseMatrix)
    {
        this.rateDict = rates;
        this.measurementResult = ket;
        this.stateVector = result;
        this.measureBase = baseMatrix;

        infoMsg = "Mesurement Result:" + measurementResult + "<br>" +
        "with base<br>" + baseMatrix.ToMatrixString()+"<br>"+"rates:"+"<br>";
        foreach(var r in rates)
        {
            infoMsg += r.Key.ToVectorString() + ":" + r.Value + "<br>";
        }
    }
}