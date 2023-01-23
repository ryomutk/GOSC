using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Factorization;
public class QuantumMath : Singleton<QuantumMath>
{

    DenseMatrix pauliZ;
    DenseMatrix pauliX;
    DenseMatrix pauliY;

    private void Start()
    {
        pauliZ = DenseMatrix.OfArray(new Complex[2, 2] { { 1, 0 }, { 0, -1 } });
        pauliX = DenseMatrix.OfArray(new Complex[2, 2] { { 0, 1 }, { 1, 0 } });
        pauliY = DenseMatrix.OfArray(new Complex[2, 2] { { 0, -Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } });
    }

    public Matrix<Complex> GetAugVector(Complex[,] baseArray)
    {
        var baseMatrix = DenseMatrix.OfArray(baseArray);
        var result = baseMatrix.Evd().EigenVectors;
        return result;
    }
    public Matrix<Complex> GetAugVector(DenseMatrix baseMatrix)
    {
        var result = baseMatrix.Evd().EigenVectors;
        return result;
    }

    [Sirenix.OdinInspector.Button]
    public Matrix<Complex> GetAugVector(Pauli pauli)
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
}
