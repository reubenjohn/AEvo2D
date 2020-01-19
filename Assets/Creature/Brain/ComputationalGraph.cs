using System;

public interface ITensor
{

}

public interface ITensorVariable : ITensor
{
    int Rank { get; }
    float[] Slice1D(params int[] indices);
}

public interface IComputeNode
{
    ITensor[] Inputs { get; }
    ITensor[] Outputs { get; }
    void ForwardPass();
}


public class TensorVariable1D : ITensorVariable
{
    private float[] data;

    public TensorVariable1D(int size)
    {
        this.data = new float[size];
    }

    public int Rank => 1;

    public float[] Slice1D(params int[] indices)
    {
        if (indices.Length == 0)
            return data;
        else throw new ArgumentException("Cannot slice flat tensor with indices: " + indices.ToString<int>());
    }
}
