using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Substance;

public class Environment : MonoBehaviour
{
    public float fuelImpulse = 100;
    public float torqueImpulse = 10;
    public float massDensity = 100;
    public float minSkinThickness = .1f;
    public float minimumGlobuleMass = .1f;

    public RecipeBook recipeBook;
    public ChemicalBag chemicalBag;
    private Recipe[] VoluntaryRecipes;
    public Substance[] odorousSubstances = new Substance[] {
        VEGETATION_DIGESTIVE_ENZYME, MEAT_DIGESTIVE_ENZYME,
        FAT
    };
    internal Mixture OdorlessMixture = new MixtureDictionary()
    {
        [VEGETATION_DIGESTIVE_ENZYME] = 0f,
        [MEAT_DIGESTIVE_ENZYME] = 0f,
        [FAT] = 0f,
    }.ToMixture();

    public Creature[] creaturesToSpawn;
    public Transform MembersTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MembersTransform = transform.Find("Members");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            foreach (var creature in creaturesToSpawn)
            {
                // Instantiate(creature, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f), 0), Quaternion.identity, membersTransform);
            }
        }
    }

    internal Vector3 Scale(float mass) => Vector2.one * (float)Math.Sqrt(mass / this.massDensity);
}
