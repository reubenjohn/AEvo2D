﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class ChemicalBag : MonoBehaviour
{
    [SerializeField] private Transform scaleTarget;
    [SerializeField] private SubstanceMass[] initialMass;

    private Flask flask;
    private Environment environment;
    private Rigidbody2D rigidBody;


    public Boolean IsInitialized => flask != null;

    public Dictionary<Substance, float>.KeyCollection Keys => flask.Keys;
    public Dictionary<Substance, float>.ValueCollection Values => flask.Values;

    public ChemicalBag() { }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        environment = GetComponentInParent<Environment>();

        flask = new Flask(initialMass.ToDictionary(substanceMass => substanceMass.substance, substanceMass => substanceMass.mass));

        rigidBody.mass = flask.Mass();
        UpdateLocalScaleIfEnabled();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float this[Substance key]
    {
        get => flask[key];
    }

    public Mixture ToMixture() => flask.Copy();

    internal float Convert(Recipe recipe)
    {
        return flask.Convert(environment.recipeBook[recipe]);
    }

    internal static float TransferMax(ChemicalBag destination, ChemicalBag source, Mixture transferMixture)
    {
        float maxYield = Flask.MaxYield(source.flask, transferMixture);
        return Transfer(destination, source, transferMixture * maxYield);
    }

    internal static float Transfer(ChemicalBag destination, ChemicalBag source, Mixture transferMixture)
    {
        if (Flask.Transfer(destination.flask, source.flask, transferMixture))
        {
            float transferMass = transferMixture.TotalMass;

            destination.rigidBody.mass = destination.flask.Mass();
            source.rigidBody.mass = source.flask.Mass();

            destination.UpdateLocalScaleIfEnabled();
            source.UpdateLocalScaleIfEnabled();
            return transferMass;
        }
        return 0;
    }

    private void UpdateLocalScaleIfEnabled()
    {
        if (scaleTarget != null)
            scaleTarget.localScale = environment.Scale(rigidBody.mass);
    }

    public float ApproximateMass => rigidBody.mass;
}
