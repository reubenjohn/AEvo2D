using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RecipeBook))]
public class RecipeBookEditor : Editor
{
    Dictionary<Recipe, bool> expandedRecipes;

    public RecipeBookEditor()
    {
    }

    public override VisualElement CreateInspectorGUI()
    {
        RecipeBook recipeBook = (RecipeBook)target;
        expandedRecipes = new Dictionary<Recipe, bool>();
        SetAllRecipeExpansions(recipeBook, true);
        return base.CreateInspectorGUI();
    }
    public override void OnInspectorGUI()
    {
        RecipeBook recipeBook = (RecipeBook)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Expand All"))
            SetAllRecipeExpansions(recipeBook, true);
        if (GUILayout.Button("Collapse All"))
            SetAllRecipeExpansions(recipeBook, false);
        EditorGUILayout.EndHorizontal();

        foreach (var recipe in recipeBook.Keys)
        {
            DrawReaction(recipe, recipeBook[recipe], newRecipe => recipeBook[recipe] = newRecipe);
        }
    }

    private void SetAllRecipeExpansions(RecipeBook recipeBook, bool expand)
    {
        expandedRecipes.Clear();
        foreach (var recipe in recipeBook.Keys)
            expandedRecipes[recipe] = expand;
    }

    private void DrawReaction(Recipe recipe, Reaction reaction, Action<Reaction> updateReaction)
    {
        expandedRecipes[recipe] = EditorGUILayout.BeginFoldoutHeaderGroup(expandedRecipes[recipe], recipe.ToString());
        if (expandedRecipes[recipe])
        {
            EditorGUILayout.BeginHorizontal();
            try
            {
                DrawMixture("Ingredients", reaction.ingredients, ingredients => updateReaction(new Reaction(ingredients, reaction.effects * (ingredients.TotalMass / reaction.effects.TotalMass))));
                DrawMixture("Effects", reaction.effects, effects => updateReaction(new Reaction(reaction.ingredients, effects)));
            }
            catch (ArgumentException) { }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawMixture(string name, Mixture mixture, Action<Mixture> updateMixture)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(name, EditorStyles.boldLabel);
        foreach (var substance in mixture.Keys)
        {
            EditorGUILayout.BeginHorizontal();
            float newMass = EditorGUILayout.Slider(substance.ToString(), mixture[substance], 0f, 10000f);
            if (newMass != mixture[substance])
            {
                MixtureDictionary newMixDict = mixture.ToMixtureDictionary();
                foreach (var key in mixture.Keys)
                    newMixDict[key] = mixture[key];
                newMixDict[substance] = newMass;
                updateMixture(newMixDict.ToMixture());
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

    }
}