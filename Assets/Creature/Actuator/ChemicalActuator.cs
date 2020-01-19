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
        IEnumerable<Recipe> chosenRecipes = environment.GetVoluntaryRecipes()
        .Where((recipe, recipeI) => activations.Array[activations.Offset + recipeI] > UnityEngine.Random.Range(0, 1));
        List<float> chosenRecipeYields = chosenRecipes.Select(recipe => chemicalBag.Convert(recipe)).ToList();
        // Debug.Log(gameObject.name + " choosen recipe yields: " + chosenRecipes.Zip(chosenRecipeYields, (recipe, yield) => recipe.ToString() + "(" + yield + ")").ToString<string>());
    }

    public int InputCount => environment.GetVoluntaryRecipes().Length;

}
