using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra;
using System.Numerics;
using System;

public static class QuantumMath
{
    static DenseMatrix _pauliZ = DenseMatrix.OfArray(new Complex[2, 2] { { 1, 0 }, { 0, -1 } });
    static DenseMatrix _pauliX = DenseMatrix.OfArray(new Complex[2, 2] { { 0, 1 }, { 1, 0 } });
    static DenseMatrix _pauliY = DenseMatrix.OfArray(new Complex[2, 2] { { 0, -Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } });

    public static DenseMatrix pauliZ { get { return _pauliZ; } }
    public static DenseMatrix pauliX { get { return _pauliX; } }
    public static DenseMatrix pauliY { get { return _pauliY; } }

    public static Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>> StateInBase(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector, DenseMatrix baseMatrix)
    {
        var result = new Dictionary<Complex, MathNet.Numerics.LinearAlgebra.Vector<Complex>>();
        var eigenvecs = GetAugVector(baseMatrix);
        var veca = eigenvecs.Row(0);
        var vecb = eigenvecs.Row(1);
        var alpha = GetProjectScolar(veca,stateVector);
        var remain = stateVector - alpha * veca;
        var beta = GetProjectScolar(vecb,stateVector);
        //var newVector = alpha*veca + beta*vecb;
        
        result[alpha] = veca;
        result[beta] = vecb;

        return result;
    }

    static Complex GetProjectScolar(MathNet.Numerics.LinearAlgebra.Vector<Complex> from,MathNet.Numerics.LinearAlgebra.Vector<Complex> to)
    {
        return from.DotProduct(to) / (from.SumMagnitudes() * from.SumMagnitudes());
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

    public static string GetStateKet(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector,string def)
    {
        if (GetAugVector(Pauli.X).Row(0).Equals(stateVector))
        {
            return "|1〉";
        }
        else if (GetAugVector(Pauli.X).Row(1).Equals(stateVector))
        {
            return "|0〉";
        }
        if (GetAugVector(Pauli.Z).Row(0).Equals(stateVector))
        {
            return "|+〉";
        }
        else if (GetAugVector(Pauli.Z).Row(1).Equals(stateVector))
        {
            return "|-〉";
        }

        return def;
    }
}
