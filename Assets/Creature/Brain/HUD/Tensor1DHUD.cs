using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tensor1DHUD : MonoBehaviour
{
    public GameObject neuron;
    public RectTransform inputTensor1DHUDTransform;

    public int NeuronCount
    {
        get => transform.childCount;
        set
        {
            if (transform.childCount != value)
            {
                transform.DestroyAndDetachChildren();
                for (int i = 0; i < value; i++)
                {
                    GameObject neuronInstance = Instantiate(neuron, transform);
                    neuronInstance.name = "Neuron " + i;
                    neuronInstance.GetComponent<NeuronHUD>().inputLayerTransform = inputTensor1DHUDTransform;
                }
            }
        }
    }
}
