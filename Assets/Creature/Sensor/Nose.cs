using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nose : MonoBehaviour, ISensor
{
    public float saturation = .1f;

    public Color inactiveColor = Color.grey, activeColor = Color.red;
    private Environment environment;
    private SpriteRenderer spriteRenderer;

    private float[] receptors;

    void Start()
    {
        environment = GetComponentInParent<Environment>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        receptors = new float[environment.odorousSubstances.Length];
    }

    void Update()
    {
    }

    public void OnOdorDetected(OdorSource source, Mixture odors, ParticleCollisionEvent particleCollisionEvent)
    {
        // Debug.Log(transform.parent.parent.gameObject.name + "'s\t" + gameObject.name + "\tdetected\t" + odors + "\tfrom\t" + source.gameObject.name);
        Debug.Assert(odors.Keys.SequenceEqual(environment.odorousSubstances), "Found non-odorous substances in odor!");

        for (var i = 0; i < environment.odorousSubstances.Length; i++)
            receptors[i] += odors[environment.odorousSubstances[i]];

    }

    public float[] GetReceptors() => receptors;

    public void OnRefresh()
    {
        for (int i = 0; i < receptors.Length; i++)
            receptors[i] = Mathf.Clamp(receptors[i], -1f, 1f);
        spriteRenderer.color = Color.Lerp(inactiveColor, activeColor, receptors.Sum());
    }

    public void OnReset() => Array.Clear(receptors, 0, receptors.Length);

}
