using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSensor : MonoBehaviour, ISensor
{
    public static IEnumerable<string> LABELS = new List<string> { "Random" };

    [SerializeField] public int nReceptors;

    private float[] receptors;

    void Start()
    {
        receptors = new float[nReceptors];
    }

    public float[] GetReceptors() => receptors;

    public void OnRefresh() => receptors.Randomize(-1f, 1f);

    public void OnReset() => Array.Clear(receptors, 0, receptors.Length);

    public IEnumerable<string> GetLabels() => LABELS;
}
