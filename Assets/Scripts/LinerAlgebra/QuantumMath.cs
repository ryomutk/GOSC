using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

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

    public static Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>> StateInBase(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector, DenseMatrix baseMatrix)
    {
        var result = new Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>>();
        var eigenvecs = GetAugVector(baseMatrix);
        var veca = eigenvecs.Row(0);
        var vecb = eigenvecs.Row(1);
        var alpha = GetProjectScolar(veca, stateVector);
        var remain = stateVector - alpha * veca;
        var beta = GetProjectScolar(vecb, stateVector);
        //var newVector = alpha*veca + beta*vecb;

        result[alpha] = veca;
        result[beta] = vecb;

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
            return GetAugVector(pauliZ);
        }
        if (pauli == Pauli.Y)
        {
            return GetAugVector(pauliY);
        }

        return null;
    }

    public static MeasurementResult Measure(QuantumPatch quantumPatch, DenseMatrix baseMatrix)
    {
        Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>> state = quantumPatch.StateInBase(baseMatrix);
        Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double> rateDict = new Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, double>();
        foreach (var headAndVector in state)
        {
            var p = headAndVector.Key.Magnitude * headAndVector.Key.Magnitude;
            rateDict[headAndVector.Value] = p;
        }

        var rand = (double)UnityEngine.Random.value;

        double nowrate = 0;
        foreach (var rate in rateDict)
        {
            nowrate += rate.Value;

            if (rand < nowrate)
            {
                var ket = GetStateKet(rate.Key, "|a>");
                return new MeasurementResult(rateDict, rate.Key, ket, baseMatrix);
            }
        }

        throw new Exception("Deporlalized Error");
    }

    public static string GetStateKet(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector, string def)
    {
        if (GetAugVector(Pauli.X).Row(0).Equals(stateVector))
        {
            return "|1>";
        }
        else if (GetAugVector(Pauli.X).Row(1).Equals(stateVector))
        {
            return "|0>";
        }
        if (GetAugVector(Pauli.Z).Row(0).Equals(stateVector))
        {
            return "|+>";
        }
        else if (GetAugVector(Pauli.Z).Row(1).Equals(stateVector))
        {
            return "|->";
        }

        return def;
    }
}
