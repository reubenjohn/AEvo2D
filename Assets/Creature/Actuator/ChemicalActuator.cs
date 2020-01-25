using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChemicalActuator : MonoBehaviour, IActuator
{
    private Environment environment;
    private ChemicalBag chemicalBag;

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
        List<float> chosenRecipeYields = environment.GetVoluntaryRecipes()
            .Select((recipe, recipeI) => chemicalBag.Convert(recipe, activations.Array[activations.Offset + recipeI].SignedToUnsignUnitFraction()))
            .ToList();
        // Debug.Log(gameObject.name + " choosen recipe yields: " + chosenRecipes.Zip(chosenRecipeYields, (recipe, yield) => recipe.ToString() + "(" + yield + ")").ToString<string>());
    }

    public int InputCount => environment.GetVoluntaryRecipes().Length;

}
