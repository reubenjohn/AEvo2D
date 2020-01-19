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
    public readonly Recipe[] involuntaryRecipes = new Recipe[] {
        Recipe.AGE_SKIN,
        Recipe.PROCESS_TOXIN,
        Recipe.PROCESS_VENOM,
        Recipe.DIGESTIVE_ENZYME_INTERFERENCE,
        Recipe.JET_FORWARD,
        Recipe.TURN_LEFT,
        Recipe.TURN_RIGHT,
    };
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
    private Transform membersTransform;

    // Start is called before the first frame update
    void Start()
    {
        membersTransform = transform.Find("Members");
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

    public Recipe[] GetVoluntaryRecipes()
    {
        if (VoluntaryRecipes == null)
        {
            VoluntaryRecipes = Enum.GetValues(typeof(Recipe)).Cast<Recipe>().Where(recipe => !involuntaryRecipes.Contains(recipe)).ToArray();
            Debug.Log("Voluntary recipes: " + VoluntaryRecipes.ToString<Recipe>());
        }
        return VoluntaryRecipes;
    }

    internal Vector3 Scale(float mass) => Vector2.one * (float)Math.Sqrt(mass / this.massDensity);
}
