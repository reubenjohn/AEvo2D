using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Substance;

public class EggPouch : MonoBehaviour
{
    public GameObject eggBlueprint;
    public int eggCapacity = 1;

    public Rigidbody2D RigidBody { get; private set; }

    private Environment environment;
    private Creature parentCreature;
    private int allTimeEggCount = 0;

    public static readonly Mixture EGG_MIX = new MixtureDictionary { [EGG] = 1f }.ToMixture();
    public static readonly Mixture BABY_MIX = new MixtureDictionary { [BABY] = 1f }.ToMixture();

    void Start()
    {
        environment = GetComponentInParent<Environment>();
        parentCreature = GetComponentInParent<Creature>();
        RigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            Egg[] eggs = GetComponentsInChildren<Egg>();
            foreach (var egg in eggs)
            {
                ChemicalBag parentChemicalBag = parentCreature.ChemicalBag;
                ChemicalBag.TransferMax(egg.ChemicalBag, parentChemicalBag, EGG_MIX, BABY_MIX);
                if (egg.ChemicalBag.ApproximateMass > parentChemicalBag.ApproximateMass * .01 && egg.ChemicalBag[Substance.BABY] > 0 && parentChemicalBag[PREGNANCY_HORMONE] == 0)
                    LayEgg(egg);
            }
            if (eggs.Length < eggCapacity)
                SpawnNewEgg();
        }
    }

    private void SpawnNewEgg()
    {
        GameObject newEggGameObject = Instantiate(eggBlueprint, transform);
        newEggGameObject.GetComponent<Egg>().name = parentCreature.name + "." + (++allTimeEggCount);
    }

    private void LayEgg(Egg egg)
    {
        egg.transform.SetParent(environment.MembersTransform, true);
    }
}
