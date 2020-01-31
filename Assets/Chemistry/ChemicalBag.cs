using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class ChemicalBag : MonoBehaviour
{
    [SerializeField] public Transform scaleTarget;
    [SerializeField] public SubstanceMass[] initialMass;

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

    public float this[Substance key]
    {
        get => flask[key];
    }

    public Mixture ToMixture() => flask.Copy();

    public float Convert(Recipe recipe, float convertionFactor = 1f)
    {
        return flask.Convert(environment.recipeBook[recipe], convertionFactor);
    }

    public static float TransferMax(ChemicalBag destination, ChemicalBag source, Mixture transferMixture)
    {
        float maxYield = Flask.MaxYield(source.flask, transferMixture);
        return Transfer(destination, source, transferMixture * maxYield);
    }

    public static float Transfer(ChemicalBag destination, ChemicalBag source, Mixture transferMixture)
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

    public float ExactMass() => this.flask.Mass();
}
