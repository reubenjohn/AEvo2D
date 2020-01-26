using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Substance;

public class EggPouch : MonoBehaviour
{
    private Environment environment;
    private Creature parentCreature;
    private FixedJoint2D joint;
    private Egg egg;
    private int eggCount = 0;

    public ChemicalBag parentChemicalBag;
    public GameObject eggPrefab;

    public static readonly Mixture EGG_MIX = new MixtureDictionary { [EGG] = 1f }.ToMixture();
    public static readonly Mixture BABY_MIX = new MixtureDictionary { [BABY] = 1f }.ToMixture();

    void Start()
    {
        environment = GetComponentInParent<Environment>();
        parentCreature = GetComponentInParent<Creature>();
        joint = transform.Find("Anchor").GetComponent<FixedJoint2D>();
        egg = GetComponentInChildren<Egg>();
        if (egg != null)
            TransferInheritance(egg);
    }

    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            if (egg == null)
            {
                SpawnNewEgg();
            }
            else
            {
                ChemicalBag.TransferMax(egg.ChemicalBag, parentChemicalBag, EGG_MIX);
                ChemicalBag.TransferMax(egg.ChemicalBag, parentChemicalBag, BABY_MIX);
                if (egg.ChemicalBag.ApproximateMass > parentChemicalBag.ApproximateMass * .01 && egg.ChemicalBag[Substance.BABY] > 0 && parentChemicalBag[PREGNANCY_HORMONE] == 0)
                    LayEgg();
            }
        }
    }

    private void SpawnNewEgg()
    {
        GameObject newEggGameObject = Instantiate(eggPrefab, this.transform);
        joint.connectedBody = newEggGameObject.GetComponentInChildren<Rigidbody2D>();
        this.egg = newEggGameObject.GetComponent<Egg>();
        TransferInheritance(this.egg);
    }

    public void TransferInheritance(Egg egg)
    {
        egg.transform.parent = this.transform;
        egg.blueprint = parentCreature.gameObject;
        egg.inheritedName = parentCreature.inheritedName + "[" + (eggCount++) + "]";
        egg.name = egg.inheritedName + "(Egg)";
    }

    private void LayEgg()
    {
        Debug.Log(this.egg.name + " laid");
        this.egg.transform.parent = environment.MembersTransform;
        this.egg.inWomb = false;
        SpawnNewEgg();
    }
}
