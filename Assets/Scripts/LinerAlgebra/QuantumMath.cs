using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;
using System.Linq;

public static class QuantumMath
{
    static DenseMatrix _pauliZ = DenseMatrix.OfArray(new Complex[2, 2] { { 1, 0 }, { 0, -1 } });
    static DenseMatrix _pauliX = DenseMatrix.OfArray(new Complex[2, 2] { { 0, 1 }, { 1, 0 } });
    static DenseMatrix _pauliY = DenseMatrix.OfArray(new Complex[2, 2] { { 0, -Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } });
    public static DenseMatrix pauliZ { get { return _pauliZ; } }
    public static DenseMatrix pauliX { get { return _pauliX; } }
    public static DenseMatrix pauliY { get { return _pauliY; } }

    public static DenseMatrix GetPauliGate(Pauli pauli)
    {
        switch (pauli)
        {
            case Pauli.X:
                return _pauliX;
            case Pauli.Y:
                return _pauliY;
            case Pauli.Z:
                return _pauliZ;
        }

        return null;
    }

    public static Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> StateInBase(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector, DenseMatrix baseMatrix)
    {
        var result = new Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex>();
        var eigenvecs = GetAugVector(baseMatrix);
        var veca = eigenvecs.Row(0);
        var vecb = eigenvecs.Row(1);
        var alpha = GetProjectScolar(veca, stateVector);
        var remain = stateVector - alpha * veca;
        var beta = GetProjectScolar(vecb, stateVector);
        //var newVector = alpha*veca + beta*vecb;

        result[veca] = alpha;
        result[vecb] = beta;

        return result;
    }

    static Complex GetProjectScolar(MathNet.Numerics.LinearAlgebra.Vector<Complex> from, MathNet.Numerics.LinearAlgebra.Vector<Complex> to)
    {
        var dp = from.DotProduct(to);
        //"ベクトルの長さは常に1内積=射影の長さ"
        return dp;
    }

    public static Matrix<Complex> GetAugVector(Complex[,] baseArray)
    {
        var baseMatrix = DenseMatrix.OfArray(baseArray);
        var result = baseMatrix.Evd().EigenVectors;
        return result;
    }
    public static Matrix<Complex> GetAugVector(DenseMatrix baseMatrix)
    {
        var result = baseMatrix.Evd().EigenVectors;

        return result;
    }

    [Sirenix.OdinInspector.Button]
    public static Matrix<Complex> GetAugVector(Pauli pauli)
    {
        if (pauli == Pauli.X)
        {
            return GetAugVector(pauliX);
        }
        if (pauli == Pauli.Z)
        {
            return GetAugVector(baseMatrix: pauliZ);
        }
        if (pauli == Pauli.Y)
        {
            return GetAugVector(pauliY);
        }

        return null;
    }
    public static EntangleResult Measure(QuantumPatch quantumPatchA, QuantumPatch quantumPatchB, List<Edge> baseA, List<Edge> baseB)
    {

        var ba = GetPauliGate((baseA[0].property as QuantumEdgeProperty).operatorType);
        for (int i = 1; i < baseA.Count; i++)
        {
            if (i == 0)
            {
                ba.Multiply(GetPauliGate((baseA[i].property as QuantumEdgeProperty).operatorType));
            }
        }
        var bb = GetPauliGate((baseB[0].property as QuantumEdgeProperty).operatorType);
        for (int i = 1; i < baseB.Count; i++)
        {
            if (i == 0)
            {
                bb.Multiply(GetPauliGate((baseB[i].property as QuantumEdgeProperty).operatorType));
            }
        }
        return Measure(quantumPatchA, quantumPatchB, ba, bb);


    }


    public static EntangleResult Measure(QuantumPatch quantumPatchA, QuantumPatch quantumPatchB, DenseMatrix baseA, DenseMatrix baseB)
    {
        Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> stateA = quantumPatchA.StateInBase(baseA);
        Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> stateB = quantumPatchB.StateInBase(baseB);
        Dictionary<string, Complex> tensor = TensorInStr(stateA, stateB, out Dictionary<string, MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex>> ketValueDict);
        string choise = measureWithState<string>(tensor);
        var parity = 0;

        if (tensor.ToArray()[0].Key == choise || tensor.ToArray()[3].Key == choise)
        {
            parity = 1;
        }
        else
        {
            parity = -1;
        }
        Dictionary<string, Complex> entangled = makeEntangle(tensor, parity);

        return new EntangleResult
        {
            parity = parity,
            tensor = tensor,
            entangledStateStr = entangled,
            entangledState = null, //どっかに配列状態のStateを持っておくべき?
            ketValueDict = ketValueDict
        };
    }

    //不明なやつ
    static Dictionary<T, Complex> makeEntangle<T>(Dictionary<T, Complex> tensor, int parity)
    {
        int[] pair = new int[2] { 0, 0 };
        var tensorArray = tensor.ToArray();
        var result = new Dictionary<T, Complex>();
        if (parity == 1)
        {
            pair[0] = 0;
            pair[1] = 3;
        }
        else
        {
            pair[0] = 1;
            pair[1] = 2;
        }
        var rate = Math.Sqrt(1 / (tensorArray[pair[0]].Value.MagnitudeSquared() + tensorArray[pair[1]].Value.MagnitudeSquared()));

        return new Dictionary<T, Complex>() { { tensorArray[pair[0]].Key, tensorArray[pair[0]].Value * rate }, { tensorArray[pair[1]].Key, tensorArray[pair[1]].Value * rate } };
    }

    static Dictionary<string, Complex> TensorInStr(Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> stateA, Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> stateB, out Dictionary<string, MathNet.Numerics.LinearAlgebra.Vector<Complex>> ketValueDict)
    {
        var alphabet = "abcdefghijklmnopqrstuvwxyz";
        var count = 0;
        Func<char> gnc = () =>
        {
            var a = alphabet[count];
            count++;
            return a;
        };
        var result = new Dictionary<string, Complex>();
        Dictionary<string, MathNet.Numerics.LinearAlgebra.Vector<Complex>> ket_value_dict = new Dictionary<string, MathNet.Numerics.LinearAlgebra.Vector<Complex>>();
        foreach (var a in stateA)
        {
            foreach (var b in stateB)
            {
                var labelA = "|" + gnc + ">";
                var labelB = "|" + gnc + ">";
                ket_value_dict[labelA] = a.Key;
                ket_value_dict[labelB] = b.Key;
                result[GetStateKet(stateVector: a.Key, labelA) + GetStateKet(b.Key, labelB)] = a.Value * b.Value;

            }
        }
        ketValueDict = ket_value_dict;
        return result;
    }

    static Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>> TensorInVal()
    {
        throw new NotImplementedException();
    }

    public static MeasurementResult Measure(QuantumPatch quantumPatch, DenseMatrix baseMatrix)
    {
        Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> state = quantumPatch.StateInBase(baseMatrix);
        Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double> rateDict = new Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double>();
        foreach (var headAndVector in state)
        {
            var p = headAndVector.Value.MagnitudeSquared();
            rateDict[headAndVector.Key] = p;
        }

        var key = measureWithRate<MathNet.Numerics.LinearAlgebra.Vector<Complex>>(rateDict);
        var ket = GetStateKet(key, "|a>");
        return new MeasurementResult(rateDict, key, ket, baseMatrix);


        throw new Exception("Deporlalized Error");
    }

    static T measureWithRate<T>(Dictionary<T, double> rateDict)
    {
        var rand = (double)UnityEngine.Random.value;

        double nowrate = 0;
        foreach (var rate in rateDict)
        {
            nowrate += rate.Value;

            if (rand < nowrate)
            {
                return rate.Key;
            }
        }

        throw new Exception("something went wrong");
    }

    static T measureWithState<T>(Dictionary<T, Complex> rateDict)
    {
        var rand = (double)UnityEngine.Random.value;

        double nowrate = 0;
        foreach (var rate in rateDict)
        {
            nowrate += rate.Value.Magnitude * rate.Value.Magnitude;

            if (rand < nowrate)
            {
                return rate.Key;
            }
        }

        throw new Exception("something went wrong");
    }

    public static string GetStateKet(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector, string def)
    {
        if (GetAugVector(Pauli.Z).Row(1).Equals(stateVector))
        {
            return "|1>";
        }
        else if (GetAugVector(Pauli.Z).Row(0).Equals(stateVector))
        {
            return "|0>";
        }
        if (GetAugVector(Pauli.X).Row(0).Equals(stateVector))
        {
            return "|+>";
        }
        else if (GetAugVector(Pauli.X).Row(1).Equals(stateVector))
        {
            return "|->";
        }

        return def;
    }

    public static string ComplexToStr(Complex complex)
    {
        return complex.Real + complex.Imaginary == 0 ? (complex.Imaginary > 0 ? " + " : " ") + complex.Imaginary + "i" : "";
    }

    public static string DataToStr<T>(Dictionary<T, Complex> data)
    {
        var msg = "";
        var count = 0;
        foreach (var cv in data)
        {
            if (count == 0)
            {
                msg += (cv.Value.Real != 0 ? replaceF(cv.Value.Real) : "" + (cv.Value.Imaginary > 0 ? " + " : " ")) + (cv.Value.Imaginary != 0 ? replaceF(cv.Value.Imaginary) : "") + (cv.Value.Magnitude > 0 ? (cv.Key is MathNet.Numerics.LinearAlgebra.Vector<Complex> cvkey ? QuantumMath.GetStateKet(stateVector: cvkey, "|a>") : cv.Key) : "");
            }
            else
            {
                msg += (cv.Value.Real != 0 ? (cv.Value.Real > 0 ? " + " : " ") + replaceF(cv.Value.Real) : " ") + (cv.Value.Imaginary != 0 ? (cv.Value.Imaginary < 0 ? " + " : " ") + replaceF(cv.Value.Imaginary) : "") + (cv.Value.Magnitude != 0 ? (cv.Key is MathNet.Numerics.LinearAlgebra.Vector<Complex> cvkey ? QuantumMath.GetStateKet(stateVector: cvkey, "|a>") : cv.Key) : "");
            }

            count++;
        }
        return msg;
    }

    static float root2bunnoichi = 0.707106781186547f;
    public static string replaceF(double num)
    {
        var result = num.ToString();
        if (Math.Abs((float)num) == root2bunnoichi)
        {
            result = "1/√2";
            if (num < 0)
            {
                result = "-" + result;
            }
        }

        return result;
    }
}
