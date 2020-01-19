using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSensor : MonoBehaviour, ISensor
{
    [SerializeField] private float activation = 1f;

    private float[] receptors;

    void Start()
    {
        receptors = new float[1];
        receptors[0] = activation;
    }

    public float[] GetReceptors() => receptors;

    public void OnRefresh() { }

    public void OnReset() { }
}
