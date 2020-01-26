using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject blueprint;
    public ChemicalBag ChemicalBag { get; private set; }

    public string inheritedName;

    public bool inWomb = true;

    void Start()
    {
        ChemicalBag = GetComponentInChildren<ChemicalBag>();
        inheritedName = inheritedName ?? this.name;
    }

    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            if (ChemicalBag.Convert(Recipe.MATURE_EGG) == 0f)
            {
                if (!inWomb && ChemicalBag[Substance.BABY] > 0)
                    Hatch();
            }
        }
    }

    private void Hatch()
    {
        Debug.Log(this.name + " hatched");
        Transform eggBodyTransform = transform.Find("Body");
        GameObject baby = Instantiate(blueprint, eggBodyTransform.position, eggBodyTransform.rotation, transform.parent);
        baby.name = inheritedName;
        Creature babyCreature = baby.GetComponent<Creature>();
        babyCreature.inheritedName = inheritedName;
        ChemicalBag babyChemicalBag = baby.GetComponentInChildren<ChemicalBag>();

        float creatureInitialMass = babyChemicalBag.initialMass.TotalMass();
        float scaleFactor = this.ChemicalBag.ExactMass() / creatureInitialMass;
        foreach (var subtanceMass in babyChemicalBag.initialMass)
            subtanceMass.mass *= scaleFactor;

        Destroy(this.gameObject);
        transform.parent = null;
    }
}
