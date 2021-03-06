﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChemicalBagSensor : MonoBehaviour, ISensor
{
    [SerializeField] private float massSaturation = .01f;
    [SerializeField] private float saturation = .01f;
    [SerializeField] public Substance[] substanceReceptors;
    [SerializeField] public ChemicalBag chemicalBag;
    [SerializeField] private bool senseMass = true;

    private Environment environment;

    private float[] receptors;

    void Start()
    {
        environment = GetComponentInParent<Environment>();
        receptors = new float[substanceReceptors.Length + (senseMass ? 1 : 0)];
    }

    void Update()
    {
    }

    public float[] GetReceptors() => receptors;

    public IEnumerable<string> GetLabels()
    {
        IEnumerable<string> labels = senseMass ? new List<string>() { "Mass" } : new List<string>();
        labels = labels.Union(substanceReceptors.Select(substance => substance.ToString()));
        return labels;
    }

    public void OnRefresh()
    {
        int i = 0;
        if (senseMass)
            receptors[i++] = (chemicalBag.ApproximateMass * massSaturation).ClampNormal();
        foreach (Substance substance in substanceReceptors)
            receptors[i++] = (chemicalBag[substance] * saturation).ClampNormal();
    }

    public void OnReset() => Array.Clear(receptors, 0, receptors.Length);
}
