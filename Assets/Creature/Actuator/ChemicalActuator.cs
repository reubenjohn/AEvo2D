using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static Recipe;

public class ChemicalActuator : MonoBehaviour, IActuator
{
    private Environment environment;
    private ChemicalBag chemicalBag;

    private Recipe[] recipes = {
        GROW_SKIN,
        MAKE_VENOM,
        VEGETATION_DIGESTIVE_ENZYME,
        MEAT_DIGESTIVE_ENZYME,
        DIGEST_VEGETATION,
        DIGEST_MEAT,
        GROW_FUEL_GLAND,
        GROW_CILIA,
        MAKE_JET_FUEL,
        MAKE_LEFT_FUEL,
        MAKE_RIGHT_FUEL,
        GROW_EGG,
        GROW_BABY,
    };

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponentInParent<Environment>();
        chemicalBag = GetComponentInParent<ChemicalBag>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Act(ArraySegment<float> activations)
    {
        List<float> chosenRecipeYields = recipes
            .Select((recipe, recipeI) => chemicalBag.Convert(recipe, activations.Array[activations.Offset + recipeI].SignedToUnsignUnitFraction()))
            .ToList();
        // Debug.Log(gameObject.name + " choosen recipe yields: " + chosenRecipes.Zip(chosenRecipeYields, (recipe, yield) => recipe.ToString() + "(" + yield + ")").ToString<string>());
    }

    public int InputCount => recipes.Length;

}
