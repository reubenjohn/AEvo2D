using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecurrentShallowBrain : Brain, IBrainViewable
{
    public int l1MemorySize;

    private ReccurrentNode reccurrentNode;
    private float[] inputs;
    private float[] outputs;

    public override void Build(int inputSize, int outputSize)
    {
        this.reccurrentNode = new ReccurrentNode(new TensorVariable1D(inputSize), outputSize, l1MemorySize);
        this.inputs = this.reccurrentNode.input.Slice1D();
        this.outputs = this.reccurrentNode.output.Slice1D();
    }

    public override void Compute()
    {
        this.reccurrentNode.ForwardPass();
    }

    public override float[] GetInputActivations() => inputs;

    public override float[] GetOutputActivations() => outputs;

    public void ResetHUD(GameObject brainHudGameObject)
    {
        Transform layersTransform = brainHudGameObject.transform.Find("Layers");
        LayersHUD layers = layersTransform.gameObject.GetComponent<LayersHUD>();
        layers.LayerCount = 2;

        Tensor1DHUD inputLayer = layersTransform.GetChild(0).GetComponent<Tensor1DHUD>();
        inputLayer.NeuronCount = GetInputActivations().Length;
        inputLayer.GetComponentsInChildren<NeuronHUD>().Zip(GetSensorLabels(), (neuron, label) => neuron.label = label).Count();
        GameObject outputLayer = layersTransform.GetChild(1).gameObject;
        reccurrentNode.ResetHUD(outputLayer);
        outputLayer.GetComponentsInChildren<NeuronHUD>().Zip(GetActuatorLabels(), (neuron, label) => neuron.label = label).Count();
    }

    public override bool UpdateHUD(GameObject brainHudGameObject)
    {
        if (base.UpdateHUD(brainHudGameObject))
            return false;

        float[] inputs = GetInputActivations();
        Transform layersTransform = brainHudGameObject.transform.Find("Layers");
        NeuronHUD[] neuronHUDs = layersTransform.GetChild(0).GetComponentsInChildren<NeuronHUD>();
        for (int i = 0; i < inputs.Length; i++)
            neuronHUDs[i].activation = inputs[i];
        reccurrentNode.UpdateHUD(layersTransform.GetChild(1).gameObject);

        return true;
    }
}

public class ReccurrentNode : IComputeNode, IBrainViewable
{
    public ITensor[] Inputs { get; }
    public ITensor[] Outputs { get; }
    public ITensorVariable input { get; private set; }
    public ITensorVariable output { get; }

    protected ITensorVariable memory;
    protected readonly float[,] weights;

    public ReccurrentNode(ITensorVariable input, int outputSize, int memorySize)
    {
        if (input.Rank != 1)
            throw new ArgumentException("Only 1D tensors are supported");
        this.Inputs = new ITensorVariable[] { this.input = input };
        this.Outputs = new ITensorVariable[] { this.output = new TensorVariable1D(outputSize) };
        this.memory = new TensorVariable1D(memorySize);
        this.weights = new float[outputSize, memorySize + input.Slice1D().Length];
        weights.Randomize(-1f, 1f);
    }

    public void ForwardPass()
    {
        float[] inputArr = input.Slice1D();
        float[] outputArr = output.Slice1D();
        float[] memoryArr = memory.Slice1D();

        Array.Clear(outputArr, 0, outputArr.Length);
        for (int o = 0; o < outputArr.Length; o++)
        {
            for (int m = 0, w = 0; m < memoryArr.Length; m++, w++)
                outputArr[o] += memoryArr[m] * weights[o, w];
            for (int i = 0, w = memoryArr.Length; i < inputArr.Length; i++, w++)
                outputArr[o] += inputArr[i] * weights[o, w];
            outputArr[o] = (float)Math.Tanh(outputArr[o]);
        }
        Array.Copy(outputArr, memoryArr, memoryArr.Length);
    }

    public void ResetHUD(GameObject layer)
    {
        layer.GetComponent<Tensor1DHUD>().NeuronCount = output.Slice1D().Length;
    }

    public bool UpdateHUD(GameObject layer)
    {
        float[] outputActivations = output.Slice1D();
        NeuronHUD[] neuronHUDs = layer.GetComponentsInChildren<NeuronHUD>();
        for (int i = 0; i < outputActivations.Length; i++)
        {
            neuronHUDs[i].activation = outputActivations[i];
            neuronHUDs[i].weights = weights.SliceRow(i).ToArray();
        }
        return true;
    }
}
