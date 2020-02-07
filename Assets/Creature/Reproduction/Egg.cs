using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject blueprint;

    public ChemicalBag ChemicalBag { get; private set; }

    private Joint2D eggPouchJoint;

    void Start()
    {
        ChemicalBag = GetComponentInChildren<ChemicalBag>();
        eggPouchJoint = GetComponentInChildren<Joint2D>();
        UpdateEggPouchJoint();
    }

    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            if (ChemicalBag.Convert(Recipe.MATURE_EGG) == 0f)
            {
                if (eggPouchJoint.enabled)
                    UpdateEggPouchJoint();
                if (!eggPouchJoint.enabled && ChemicalBag[Substance.BABY] > 0)
                    Hatch();
            }
        }
    }

    private void UpdateEggPouchJoint()
    {
        eggPouchJoint.connectedBody = GetComponentInParent<EggPouch>()?.RigidBody;
        eggPouchJoint.enabled = eggPouchJoint.connectedBody != null;
    }

    private void Hatch()
    {
        GameObject baby = Instantiate(blueprint, transform.position, transform.rotation, transform.parent);
        baby.name = name;
        baby.GetComponentInChildren<ChemicalBag>().initialMass.ScaleTo(this.ChemicalBag.ExactMass());

        Destroy(this.gameObject);
        transform.parent = null;
    }
}
